using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PieceCtrler : MonoBehaviour
{
    //位置情報
    public int x; //駒の現在のx座標
    public float selectY = 1.0f; //駒が選択された時のy座標(ちょっとだけ浮く)
    public static float setY = 0.566f;//選択されていない時のy座標(定位置)
    public int z; //駒の現在のz座標

    //駒の状態
    public bool isSelected = false; //駒が選択されているか否か
    public bool isCaptured = false; //駒がとられているか否か
    public bool isInhand = false; //駒が持ち駒か否か
    public bool isPromoted = false; //駒が成っているか
    public static PieceCtrler selectedPiece = null;

    //インスタンス化
    public Piece pieceData;

    void Start()
    {
        //Debug.Log("サイズ変わった？");
        if (pieceData == null)
            Debug.LogWarning($"{gameObject.name}: pieceData はまだ設定されていません（Init前）");
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
            selectedPiece.isSelected = false;
            Debug.Log($"{this.gameObject.name}:選択解除/setY：{setY}");
            this.transform.position = new Vector3(x, setY, z); // 元の高さに戻す
            return;
        }
        //別のコマが選ばれても選択を解除
        if (selectedPiece != null && selectedPiece != this)
        {
            selectedPiece.isSelected = false;
            selectedPiece.transform.position = new Vector3(x, setY, z);
            return;
        }

        // 自分を新しく選択す
        isSelected = true;
        selectedPiece = this; //前回の選択記録を破棄してからthisを代入

        if (selectedPiece != null)
            Debug.Log("選択中：" + selectedPiece);
        else
            Debug.LogError("コマが見つかりません");

        transform.position = new Vector3(x, selectY, z);

        //移動できるマスを取得して、そのマスを光らせる
        List<Vector3> moveTiles = GetCanMoveTiles();
        Debug.LogWarning($"移動できるマス：moveTiles");

        foreach(Vector3 pos in moveTiles)
        {
            string tileName = $"Tile_{pos.x}_{pos.z}";
            Debug.Log(pos + tileName);

            GameObject tileObj = GameObject.Find(tileName);
            Transform tileSurface = tileObj.transform.parent.GetChild(1);

            Debug.Log("tileObj:" + tileObj + $"tileChild:{tileSurface}");
            if (tileObj != null)
            {
                TileCtrler tileCtrler = tileSurface.GetComponent<TileCtrler>();
                Debug.Log(tileCtrler);
                if (tileCtrler != null)
                {
                    tileCtrler.HighLightTile();
                }
                else
                {
                    Debug.LogWarning("tileCtrlerがnullです");
                }
            }
        }
    }

    public virtual List<Vector3> GetCanMoveTiles()
    {
        return new List<Vector3>();
    }

    public void Move(Vector3 targetPos)
    {
        //移動前に、駒の現在地情報をnullにする。これやらないと、データ上は駒の位置情報が残ります。
        BoardManager.boardGridInfo[x, z] = null;
        if (BoardManager.boardGridInfo[x, z] == null) Debug.Log($"boardGridInfo[{x}, {z}]をnullにしました。");

        x = Mathf.RoundToInt(targetPos.x);
        z = Mathf.RoundToInt(targetPos.z);

        this.transform.position = new Vector3(x, setY, z);
    }
    //移動先の駒をとって良いかの判定
    public bool IsCanCapture(int x, int z)
    {
        Transform target = BoardManager.boardGridInfo[x, z];

        //もし移動先に駒があった場合は、それが敵か否かの判定が必要
        if (target != null)
        {
            Piece enemy = GetComponent<PieceCtrler>().pieceData;

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
            Debug.Log("移動先に駒があるので、指定したマス目に移動できません。");
            return false;
        }
    }
}