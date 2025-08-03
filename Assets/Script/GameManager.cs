using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] GameObject[] piecePrefab = new GameObject[8]; // 0-7 for pieces, 8 for King Gote
    [SerializeField] BoardManager bm;

    public static GameManager Instance;

    //シングルトンパターン・・・ゲーム全体で唯一存在するクラスを作る、デザインパターン。
    //どこからでもアクセスできる
    //変数Instanceに自分自身を代入することで、駄隠喩したクラス(今回はGameManager)を探さず、直接呼び出せる
    //クラスをstaticみたいに使える(staticはMonobehaviourを継承できないから、Unityにおいてはこちらを使うことが多い)
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
        SpawnAllPiece();
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
            SpawnPiece(PieceType.Pawn, PlayerType.Sente, x, 3);
            SpawnPiece(PieceType.Pawn, PlayerType.Gote, x, 5);
        }
    }

    void SpawnRookAndBishop()
    {
        SpawnPiece(PieceType.Rook, PlayerType.Sente, 1, 1);
        SpawnPiece(PieceType.Bishop, PlayerType.Sente, 7, 1);
        SpawnPiece(PieceType.Rook, PlayerType.Gote, 7, 7);
        SpawnPiece(PieceType.Bishop, PlayerType.Gote, 1, 7);
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

        GameObject piece = Instantiate(piecePrefab[prefabIndex], new Vector3(x, PieceCtrler.setY, z), Quaternion.identity);
        piece.name = $"{player}.{type}_{x}_{z}";
        piece.transform.parent = transform;

        if (player == PlayerType.Gote) piece.transform.Rotate(0, 180, 0);

        Piece piceData = new Piece(type, player);
        piece.GetComponent<PieceCtrler>().Init(piceData);

        BoardManager.boardGridInfo[x, z] = piece.transform;

    }

    public PieceCtrler selectPiece;
    public void SelectPiece(PieceCtrler pieceCtrler)
    {
        Debug.Log($"SelectPiece呼び出し: {pieceCtrler.gameObject.name}");
        Debug.Log($"pieceData: {pieceCtrler.pieceData}");
        selectPiece = pieceCtrler;

        for (int i = 0; i < selectPiece.GetCanMoveTiles().Count; i++)
        {
            var pos = selectPiece.GetCanMoveTiles()[i];
            string tileName = $"Tile_{pos.x}_{pos.z}";
            GameObject tileObject = GameObject.Find(tileName);

            if (tileObject != null)
            {
                TileCtrler tile = tileObject.GetComponent<TileCtrler>();

                if (tile != null)
                {
                    tile.HighLightTile();
                }
            }

            else
            {
                Debug.LogWarning($"Tile {tileName} not found.");
            }
        }
    }
}
