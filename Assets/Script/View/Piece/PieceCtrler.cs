using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;

enum FiledStatus
{
    None = 1,
    OnBoard,
    Captured
}


public class PieceCtrler : MonoBehaviour
{
    //ここから
    public Piece pieceData;    //インスタンス化
    public int thisX; //駒の現在のx座標
    public const float selectY = 1.0f; //駒が選択された時のy座標(ちょっとだけ浮く)
    public const float defaultY = 0.57f;//選択されていない時のy座標(定位置)
    public int thisZ; //駒の現在のz座標

    public virtual List<Vector2Int> GetCanMoveTiles()
    {
        return new List<Vector2Int>();
    }

    //ここまでが修正コードです


    //インスタンス化
    [Header("インスタンス化")]
    [SerializeField] BoardManager boardManager;

    //位置情報
    [Header("駒の位置情報")]


    // "金"の動き。頻出なので、使いまわせる様にここで定義
    public readonly Vector2Int[] GoldDirections = {
        new(-1,1),
        new(0,1),
        new(1,1),
        new(-1,0),
        new(1,0),
        new(0,-1)
    };


    SoundCtrler sound;

    void Start()
    {
        thisX = Mathf.RoundToInt(this.transform.position.x);
        thisZ = Mathf.RoundToInt(this.transform.position.z);
        //Debug.Log("サイズ変わった？");
        if (pieceData == null)
            Debug.LogWarning($"{gameObject.name}: pieceData はまだ設定されていません（Init前）");
        //もしsoundの中が空っぽだったら、SoundCtrlを入れる。void Start()でやった方が安全そう
        if (sound == null)
            sound = FindObjectOfType<SoundCtrler>();

        if (boardManager == null)
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

    public void Move(Vector2Int pos)
    {

    }


    public bool TryMoveTo(Vector2Int target)
    {
        return true;
    }
}