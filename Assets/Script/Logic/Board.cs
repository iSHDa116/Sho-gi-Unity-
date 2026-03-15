using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    // 盤面の初期情報を設定
    public static int[,] boardInfo =
    {
        {4, 0, 1, 0, 0, 0, 11, 0, 14},
        {5, 2, 1, 0, 0, 0, 11, 13, 15},
        {6, 0, 1, 0, 0, 0, 11, 0, 16},
        {7, 0, 1, 0, 0, 0, 11, 0, 17},
        {8, 0, 1, 0, 0, 0, 11, 0, 19},
        {7, 0, 1, 0, 0, 0, 11, 0, 17},
        {6, 0, 1, 0, 0, 0, 11, 0, 16},
        {5, 3, 1, 0, 0, 0, 11, 12, 15},
        {4, 0, 1, 0, 0, 0, 11, 0, 14},

    };
    static int width = 9;
    static int depth = 9;
    public static PieceCtrler[,] boardPieceInfo = new PieceCtrler[width, depth];
    public void SetUpInitPos()
    {
        // TODO:初期はいち
    }

    public PieceCtrler GetPiece(int x, int z)
    {
        return boardPieceInfo[x, z];
    }

    public void Move(PieceCtrler piece, Vector2Int target)
    {
        boardPieceInfo[piece.thisX, piece.thisZ] = null;

        piece.thisX = target.x;
        piece.thisZ = target.y;

        boardPieceInfo[target.x, target.y] = piece;
    }
    public static bool IsTileEmpty(int x, int z)
    {
        return boardPieceInfo[x, z] == null;
    }
    public static bool IsOutBoard(int x, int z)
    {
        return (x < 0 || x >= width || z < 0 || z >= depth);
    }
}
