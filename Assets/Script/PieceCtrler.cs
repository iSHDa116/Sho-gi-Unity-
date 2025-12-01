using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;

public class PieceCtrler : MonoBehaviour
{
    //位置情報
    public int x; //駒の現在のx座標
    public float selectY = 1.0f; //駒が選択された時のy座標(ちょっとだけ浮く)
    public static float setY = 0.57f;//選択されていない時のy座標(定位置)
    public int z; //駒の現在のz座標
    public Vector2Int[] GoldDirections = {
        new Vector2Int(-1,1),
        new Vector2Int(0,1),
        new Vector2Int(1,1),
        new Vector2Int(-1,0),
        new Vector2Int(1,0),
        new Vector2Int(0,-1)
    };

    //駒の状態
    public bool isSelected = false; //駒が選択されているか否か
    public bool isCaptured = false; //駒がとられているか否か
    public bool isInhand = false; //駒が持ち駒か否か
    public bool isPromoted = false; //駒が成っているか
    public static PieceCtrler selectedPiece = null;

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
    }
    public void Init(Piece data)
    {
        pieceData = data;
        x = Mathf.RoundToInt(this.transform.position.x);
        z = Mathf.RoundToInt(this.transform.position.z);
        Debug.Log($"{gameObject.name} の Init 完了: {pieceData.pieceType} {pieceData.playerType}_{x}_{z}");
    }

    void OnMouseDown()
    {
        

        //もし同じ駒を押したら、選択を解除
        if (selectedPiece == this && isSelected)
        {
            //タイルの色を元の色に戻す
            selectedPiece.ResetHighlightedTiles();
            selectedPiece.isSelected = false;
            selectedPiece = null;
            Debug.Log($"{this.gameObject.name}:選択解除/setY：{setY},{isSelected}");
            this.transform.position = new Vector3(x, setY, z); // 元の高さに戻す

            return;
        }
        //別のコマが選ばれても選択を解除
        if (selectedPiece != null && selectedPiece != this)
        {
            //Tileの色を元の色に戻す
            selectedPiece.ResetHighlightedTiles();
            // 高さを元の高さに戻す
            selectedPiece.transform.position = new Vector3(selectedPiece.x, setY, selectedPiece.z);
            //選択を解除
            selectedPiece.isSelected = false;
            selectedPiece = null;
            return;
        }

        // 自分を新しく選択す
        isSelected = true;
        selectedPiece = this; //前回の選択記録を破棄してからthisを代入
        if (selectedPiece != null)
            Debug.Log("選択中：" + selectedPiece);
        else
            Debug.LogError("コマが見つかりません");
        if((GameManager.isPlayer && selectedPiece.pieceData.playerType != PlayerType.Sente))
        {
            Debug.LogWarning("今は先手です");
            selectedPiece = null;
            return;
        }
        if(!GameManager.isPlayer && selectedPiece.pieceData.playerType != PlayerType.Gote)
        {
            Debug.LogWarning("今は後手です");
            selectedPiece = null;
            return;
        }
        // 少しだけ浮かせる
        transform.position = new Vector3(x, selectY, z);

        //移動できるマスを取得して、そのマスを光らせる
        List<Vector3> moveTiles = GetCanMoveTiles();
        DebugTiles(moveTiles);

        foreach (Vector3 pos in moveTiles)
        {
            int tx = Mathf.RoundToInt(pos.x);
            int tz = Mathf.RoundToInt(pos.z);

            string tileName = $"Tile_{tx}_{tz}";
            GameObject tileObj = GameObject.Find(tileName);
            if (tileObj == null)
            {
                Debug.LogWarning($"タイルが見つかりません: {tileName}");
                continue;
            }

            // 子参照はやめてコンポーネントに頼る
            TileCtrler tile = tileObj.GetComponent<TileCtrler>();
            if (tile == null)
            {
                Debug.LogWarning($"TileCtrlerが見つかりません: {tileName}");
                continue;
            }

            tile.HighLightTile();
            // ハイライト後に戻すために記録しておく（未実装なら省略）
            // highlightedTiles.Add(tile);
        }
    }

    public virtual List<Vector3> GetCanMoveTiles()
    {        
        return new List<Vector3>();
    }

    public void Move(Vector3 targetPos)
    {
        //Tileの色を元の色に戻す
        ResetHighlightedTiles();
        int oldX = x, oldZ = z; //元いた場所の座標を避難させる(新旧をわかりやすくするため)
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
                HandManager.instance.Capture(enemy, enemy.pieceData.playerType);
                // 音を鳴らす
                sound.CaptureSound();
            }
            else
            {
                Debug.LogWarning("移動先のマスに味方がいるため、指定した場所には進めません");
            }
        }
        //移動前に、駒の現在地情報をnullにする。これやらないと、データ上は駒の位置情報が残ります。
        BoardManager.boardGridInfo[oldX, oldZ] = null;
        if (BoardManager.boardGridInfo[oldX, oldZ] == null)
            Debug.Log($"boardGridInfo[{oldX}, {oldZ}]をnullにしました。");

        //移動
        this.transform.position = new Vector3(nx, setY, nz);
        x = nx;
        z = nz;
        sound.DropSound();

        //移動先に自分の駒を登録
        BoardManager.boardGridInfo[nx, nz] = this.transform;
        if (BoardManager.boardGridInfo[nx, nz] == this.transform)
        {
            Debug.Log($"boardGridInfo[{nx},{nz}]に{this}を追加しました。");
        }

        // "成る"の判定
        Promoted();
        //選択を解除する
        selectedPiece.isSelected = false; //選択を解除
        selectedPiece = null; // 選択を解除
        GameManager.isPlayer = !GameManager.isPlayer;
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
            //もし移動先の駒が敵 または　空だったら
            if (enemy.playerType != this.pieceData.playerType || enemy == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public bool TryMoveTo(Vector3 target)
    {
        if (selectedPiece == null) return false;

        List<Vector3> moveAble = selectedPiece.GetCanMoveTiles();
        DebugTiles(moveAble);

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

    void ResetHighlightedTiles()
    {
        foreach (var pos in selectedPiece.GetCanMoveTiles())
        {
            int tileX = Mathf.RoundToInt(pos.x);
            int tileZ = Mathf.RoundToInt(pos.z);

            Transform tile = BoardManager.boardGridInfo[tileX, tileZ];
            string tileName = $"Tile_{tileX}_{tileZ}";
            GameObject tileObj = GameObject.Find(tileName);

            tileObj.GetComponent<TileCtrler>().ResetColor();
        }
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