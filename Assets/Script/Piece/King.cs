using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override List<Vector2Int> GetCanMoveTiles(Board board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        AddStepMoves(moves, board, StraightDirections);
        AddStepMoves(moves, board, DiagonalDirections);
        return moves;
    }
}
