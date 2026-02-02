using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isPlayer = true;

    [Header("駒")]
    [SerializeField] GameObject[] piecePrefab = new GameObject[9]; // 0-7 for pieces, 8 for King Gote

    [Header("インスタンス化")]
    [SerializeField] BoardManager bm;

    public static GameManager Instance;
    [Header("その他")]
    [SerializeField] Camera cam;
    [SerializeField] Transform pieceManager;

    //シングルトンパターン・・・ゲーム全体で唯一存在するクラスを作る、デザインパターン。
    //どこからでもアクセスできる
    //変数Instanceに自分自身を代入することで、代入したクラス(今回はGameManager)を探さず、直接呼び出せる
    //クラスをインスタンス化しなくても使える(staticはMonobehaviourを継承できないから、Unityにおいてはこちらを使うことが多い)
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Instanceはすでに存在します");
        }
    }
    void Start()
    {
        bm.BoardCreate();
        SpawnAllPiece();
    }
    
    public void TurnChange()
    {
        isPlayer = !isPlayer;
        UI.instance.TurnChangetext();
        cam.transform.Rotate(0,0,180);
    }

    void SpawnAllPiece()
    {
        SpawnAllPawn();
        SpawnRookAndBishop();
        SpawnBackPiece();
    }

    void SpawnAllPawn()
    {
        for (int x = 0; x < BoardManager.width; x++)
        {
            SpawnPiece(PieceType.Pawn, PlayerType.Sente, x, 2);
            SpawnPiece(PieceType.Pawn, PlayerType.Gote, x, 6);
        }
    }

    void SpawnRookAndBishop()
    {
        SpawnPiece(PieceType.Bishop, PlayerType.Sente, 1, 1);
        SpawnPiece(PieceType.Rook, PlayerType.Sente, 7, 1);
        SpawnPiece(PieceType.Bishop, PlayerType.Gote, 7, 7);
        SpawnPiece(PieceType.Rook, PlayerType.Gote, 1, 7);
    }

    void SpawnBackPiece()
    {
        PieceType[] backRow = new PieceType[]
        {
            PieceType.Lance, PieceType.Knight, PieceType.SilverGeneral, PieceType.GoldGeneral,
            PieceType.King, PieceType.GoldGeneral, PieceType.SilverGeneral, PieceType.Knight, PieceType.Lance
        };

        for (int x = 0; x < BoardManager.width; x++)
        {
            SpawnPiece(backRow[x], PlayerType.Sente, x, 0);
            SpawnPiece(backRow[x], PlayerType.Gote, x, 8);
        }
    }

    void SpawnPiece(PieceType type, PlayerType player, int x, int z)
    {
        int prefabIndex = (int)type;

        if (type == PieceType.King && player == PlayerType.Gote)
        {
            prefabIndex = 8;
        }

        if (piecePrefab[prefabIndex] == null)
        {
            Debug.LogError($"piecePrefab[{prefabIndex}] が null です！");
            return;
        }

        GameObject piece = Instantiate(piecePrefab[prefabIndex], new Vector3(x, PieceCtrler.defaultY, z), Quaternion.identity);
        piece.name = $"{player}.{type}_{x}_{z}";
        piece.transform.SetParent(pieceManager);

        if (player == PlayerType.Gote) piece.transform.Rotate(0, 180, 0);

        Piece pieceData = new Piece(type, player);
        piece.GetComponent<PieceCtrler>().Init(pieceData);
        piece.GetComponent<BoxCollider>().size = new Vector3(0.027f, 0.04f, 0.03f);


        BoardManager.boardGridInfo[x, z] = piece.transform;
    }
}
