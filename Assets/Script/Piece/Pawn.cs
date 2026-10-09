using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{    // 先手の場合、zが +1,後手の場合は-1される
    int Forward { get { return isSente ? 1 : -1; } }
    public override List<Vector2Int> GetCanMoveTiles(Board board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int frontZ = z + Forward;
        return moves;
    }
}
