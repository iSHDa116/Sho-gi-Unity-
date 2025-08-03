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

    public void Init(Piece data)
    {
        pieceData = data;
        Debug.Log($"{gameObject.name} の Init 完了: {pieceData.pieceType} {pieceData.playerType}");

    }

    void Start()
    {
        //pieceData = new Piece();
        this.GetComponent<BoxCollider>().size = new Vector3(0.027f, 0.04f, 0.03f);
    }

    void OnMouseDown()
    {
        x = Mathf.RoundToInt(this.transform.position.x);
        z = Mathf.RoundToInt(this.transform.position.z);

        //もし同じ駒を押したら、選択を解除
        if (selectedPiece == this && isSelected)
        {
            selectedPiece.isSelected = false;
            Debug.Log($"{this.gameObject.name}:選択解除/setY：{setY}");
            this.transform.position = new Vector3(x, setY, z); // 元の高さに戻す
            Debug.Log($"{gameObject.name}: 選択解除/setY={setY}");
            return;
        }
        //別のコマが選ばれても選択を解除
        if (selectedPiece != this && selectedPiece == null)
        {
            selectedPiece.isSelected = false;
            selectedPiece.transform.position = new Vector3(x, setY, z);
        }

        // 自分を新しく選択す
        isSelected = true;
        selectedPiece = this; //前回の選択記録を破棄してからthisを代入

        transform.position = new Vector3(x, selectY, z);
        Debug.Log($"{gameObject.name}: 選択");

        //移動でいるますを取得して、そのマスを光らせる
        List<Vector3> moveTiles = GetCanMoveTiles();
        Debug.LogWarning(moveTiles);

        for (int i = 0; i < moveTiles.Count; i++)
        {
            Vector3 pos = moveTiles[i];
            string tileName = $"Tile_{pos.x}_{pos.z}";
            Debug.LogWarning(pos + tileName);

            GameObject tileObj = GameObject.Find(tileName);
            Transform tileChild = tileObj.transform.parent.GetChild(1);

            Debug.Log("tileObj:" + tileObj);
            if (tileObj != null)
            {
                TileCtrler tileCtrler = tileChild.GetComponent<TileCtrler>();
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
        x = (int)targetPos.x;
        z = (int)targetPos.z;

        this.transform.position = new Vector3(x, setY, z);
    }
    //移動先の駒をとって良いかの判定
    public bool IsCanCapture(int x, int z)
    {
        Transform target = BoardManager.boardGridInfo[x, z];

        //もし移動先に駒があった場合は、それが敵か否かの判定が必要
        if (target != null)
        {
            Piece enemy = target.GetComponent<Piece>();

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