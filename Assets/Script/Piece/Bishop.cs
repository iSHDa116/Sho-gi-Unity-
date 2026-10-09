using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override List<Vector2Int> GetCanMoveTiles(Board board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        AddLineMoves(moves,board,DiagonalDirections);
        return moves;
    }
}
