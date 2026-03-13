using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    static int width = 9;
    static int depth = 9;
    public PieceCtrler[,] gridPieceInfo = new PieceCtrler[width, depth];
    public void SetUpInitPos()
    {
        // TODO:初期はいち
    }

    public PieceCtrler GetPiece(int x, int z)
    {
        return gridPieceInfo[x, z];
    }

    public void Move(PieceCtrler piece, Vector2Int target)
    {
        gridPieceInfo[piece.thisX, piece.thisZ] = null;

        piece.thisX = target.x;
        piece.thisZ = target.y;

        gridPieceInfo[target.x, target.y] = piece;
    }
}
