using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;

public class PieceCtrler : MonoBehaviour
{
    //インスタンス化
    [Header("インスタンス化")]
    [SerializeField] BoardManager boardManager;

    //位置情報
    [Header("駒の位置情報")]
    public int thisX; //駒の現在のx座標
    public const float selectY = 1.0f; //駒が選択された時のy座標(ちょっとだけ浮く)
    public const float defaultY = 0.57f;//選択されていない時のy座標(定位置)
    public int thisZ; //駒の現在のz座標

    // "金"の動き。頻出なので、使いまわせる様にここで定義
    public readonly Vector2Int[] GoldDirections = {
        new(-1,1),
        new(0,1),
        new(1,1),
        new(-1,0),
        new(1,0),
        new(0,-1)
    };


    //インスタンス化
    public Piece pieceData;
    SoundCtrler sound;

    void Start()
    {
        //Debug.Log("サイズ変わった？");
        if (pieceData == null)
            Debug.LogWarning($"{gameObject.name}: pieceData はまだ設定されていません（Init前）");
        //もしsoundの中が空っぽだったら、SoundCtrlを入れる。void Start()でやった方が安全そう
        if (sound == null)
            sound = FindObjectOfType<SoundCtrler>();
        
        if(boardManager == null)
        {
            boardManager = FindObjectOfType<BoardManager>();
        }
    }
    // GameManager.csで駒のインスタンス化を行う際に使います。これをしないとエラーが起きます(原因不明です)
    public void Init(Piece data)
    {
        pieceData = data;
        thisX = Mathf.RoundToInt(this.transform.position.x);
        thisZ = Mathf.RoundToInt(this.transform.position.z);
        Debug.Log($"{gameObject.name} の Init 完了: {pieceData.pieceType} {pieceData.playerType}_{thisX}_{thisZ}");
    }

    //駒の状態
    public bool isSelected = false; //駒が選択されているか否か
    public bool isCaptured = false; //駒がとられているか否か
    public bool isInhand = false; //駒が持ち駒か否か
    public bool isPromoted = false; //駒が成っているか
    public static PieceCtrler selectedPiece;

    public void SelectPiece()
    {
        // もし、選択中の駒がなければ
        if (GameManager.selectPiece != this)
        {
            GameManager.selectPiece = this;
            // 駒を少し浮かせる
            transform.position = new Vector3(thisX, selectY, thisZ);
        }
        else
        {
            transform.position = new Vector3(thisX, defaultY, thisZ);
            GameManager.selectPiece = null;
        }

    }

    public virtual List<Vector3> GetCanMoveTiles()
    {        
        return new List<Vector3>();
    }

    public void Move(Vector3 targetPos)
    {
        //Tileの色を元の色に戻す
        boardManager.ResetHighlightedTiles(this);
        int oldX = thisX, oldZ = thisZ; //元いた場所の座標を避難させる(新旧をわかりやすくするため)
        int nx = Mathf.RoundToInt(targetPos.x); //移動先のx座標
        int nz = Mathf.RoundToInt(targetPos.z); //移動先のz座標

        if (BoardManager.IsOutBoard(nx, nz))
        {
            Debug.LogError($"x:{nx}/z:{nz}は盤外です!! (PieceCtrler.cs/133行目)");
            return;
        }

        //targetに、移動先のマスの情報を格納
        Transform target = BoardManager.boardGridInfo[nx, nz];
        //もし移動先が空じゃなければ、ifの中身を実行
        if (target != null)
        {
            //移動先の駒を取得
            PieceCtrler enemy = target.GetComponent<PieceCtrler>();
            //もし相手の駒が敵なら
            if (enemy.pieceData.playerType != this.pieceData.playerType)
            {
                //相手を非表示
                HandManager.instance.CaptureEnemy(enemy, enemy.pieceData.playerType);
                // 音を鳴らす
                sound.CaptureSound();
                GetKing(enemy.pieceData);
                
            }
            else
            {
                Debug.LogWarning("移動先のマスに味方がいるため、指定した場所には進めません");
            }
        }
        //元護摩じゃなければ、移動前に駒の現在地情報をnullにする。これやらないと、データ上は駒の位置情報が残ります。
        if(!isInhand)
        {
            BoardManager.boardGridInfo[oldX, oldZ] = null;
            if (BoardManager.boardGridInfo[oldX, oldZ] == null)
                Debug.Log($"boardGridInfo[{oldX}, {oldZ}]をnullにしました。");
        }
        //移動
        this.transform.position = new Vector3(nx, defaultY, nz);
        thisX = nx;
        thisZ = nz;
        GetComponent<AudioSource>().Play();

        //移動先に自分の駒を登録
        BoardManager.boardGridInfo[nx, nz] = this.transform;
        if (BoardManager.boardGridInfo[nx, nz] == this.transform)
        {
            Debug.Log($"boardGridInfo[{nx},{nz}]に{this}を追加しました。");
        }
        // "成る"の判定
        Promoted();

        //選択を解除する

    }

    //移動先の駒をとって良いかの判定
    public bool IsCanCapture(Vector3 vec)
    {
        int vecX = Mathf.RoundToInt(vec.x);
        int vecZ = Mathf.RoundToInt(vec.z);
        //移動先の位置情報を取得
        Transform target = BoardManager.boardGridInfo[vecX, vecZ];
        //もし移動先に駒があった場合は、それが敵か否かの判定が必要
        if (target != null)
        {
            Piece enemy = target.GetComponent<PieceCtrler>().pieceData;
            //もし移動先の駒が敵 またはマスが空だったら
            if (enemy.playerType != this.pieceData.playerType)
            {
                // 移動許可
                return true;
            }
            else
            {
                //移動不可
                return false;
            }
        }
        else
        {
            //移動許可
            return true;
        }
    }

    public bool TryMoveTo(Vector3 target)
    {
        if (selectedPiece == null) return false;

        List<Vector3> moveAble = selectedPiece.GetCanMoveTiles();
        //DebugTiles(moveAble);

        if (moveAble.Contains(target) && IsCanCapture(target))
        {
            Debug.Log($"x: {target.x} z:{target.z} に移動します");
            return true;
        }
        else
        {
            Debug.Log($"移動範囲外です");
            return false;
        }
    }

    void DebugTiles(List<Vector3> move)
    {
        var listStr = (move == null || move.Count == 0) ? "なし" : string.Join(", ", move.Select(m => $"({m.x}, {m.y}, {m.z})"));
        Debug.Log($"移動できるマス({move?.Count ?? 0})：{listStr}");
    }


    void Promoted()
    {
        if (!this.isPromoted &&
        this.pieceData.pieceType != PieceType.King &&
        this.pieceData.pieceType != PieceType.GoldGeneral &&
        this.pieceData.pieceType != PieceType.SilverGeneral)
        {
            if (this.transform.position.z >= BoardManager.depth - 3 && this.pieceData.playerType == PlayerType.Sente)
            {
                this.isPromoted = true;
                this.transform.Rotate(0, 0, 180);
            }
            else if (this.transform.position.z <= BoardManager.depth - 7 && this.pieceData.playerType == PlayerType.Gote)
            {
                this.isPromoted = true;
                this.transform.Rotate(0, 0, 180);
            }
        }
    }

    void GetKing(Piece piece)
    {
        if(piece.pieceType == PieceType.King)
        {
            Debug.Log("終了");
        }
    }

    /*Vector3 DirectionMove(int dx, int dz)
    {
        Transform target = BoardManager.boardGridInfo[dx, dz];

        if (target != null)
        {
            Piece enemy = target.GetComponent<PieceCtrler>().pieceData;
            if (enemy.playerType != this.pieceData.playerType)
            {
                return new Vector3(dx, setY, dz);
            }
        }
        else
        {
            return new Vector3(dx, setY, dz);
        }

        //return ;
    }*/
}