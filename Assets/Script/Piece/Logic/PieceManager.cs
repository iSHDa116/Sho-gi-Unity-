using UnityEngine;

public class PieceManager : MonoBehaviour
{
    //↓↓↓↓駒に使う↓↓↓↓
    [Header("歩、角、飛、香、桂、銀、金、王、玉の順番で並べる")]
    [SerializeField] GameObject[] piecePrefab = new GameObject[9];
    Board board = new Board();
    public void SetupPieces()
    {
        PieceType[] midiumRow =
        {
            PieceType.Bishop, PieceType.Rook
        };
        PieceType[] backRow =
        {
            PieceType.Lance,PieceType.Knight,PieceType.Silver,PieceType.Gold,
            PieceType.King,PieceType.Gold, PieceType.Silver, PieceType.Knight,PieceType.Lance
        };

        for (int x = 0; x < Board.Size; x++)
        {
            SpawnPiece(PieceType.Pawn, true, x, 2);
            SpawnPiece(PieceType.Pawn, false, x, 6);
            if(x == 1 ) SpawnPiece(PieceType.Bishop, true, x, 1);
            if (x == 1) SpawnPiece(PieceType.Bishop, false, x, 7);
            if (x == 7) SpawnPiece(PieceType.Rook, true, x, 1);
            if (x == 7) SpawnPiece(PieceType.Rook, false, x, 7);
            SpawnPiece(backRow[x], true, x, 0);
            SpawnPiece(backRow[x], false, x,8);
        }
    }

    // 見た目を作って、盤のデータにも登録する
    Piece SpawnPiece(PieceType type, bool isWhite, int x, int z)
    {
        Piece piece = CreatePiece(type, isWhite, x, z);
        board.PutPiece(piece, x, z);
        return piece;
    }

    public Piece CreatePiece(PieceType pieceType, bool isSente, int x, int z)
    {
        GameObject obj;
        Quaternion dir = (isSente) ? Quaternion.identity : Quaternion.Euler(2, 180, 0);

        //受け取ったisSenteがfalse(後手)だった場合、王の駒を玉にする
        bool isGyoku = pieceType == PieceType.King && !isSente;
        if (isGyoku)
        {
            obj = Instantiate(piecePrefab[8], new Vector3(x, 0.5f, z), dir);
        }
        else
        {   
            // 王以外は普通に生成
            obj = Instantiate(piecePrefab[(int)pieceType], new Vector3(x, 0.5f, z), dir);
        }
        obj.name = $"{(isSente ? "Sente" : "Gote")}:{pieceType}_{x}_{z}";

        Piece piece = obj.GetComponent<Piece>();
        piece.Initialize(pieceType, x, z, isSente);
        return piece;
    }
}