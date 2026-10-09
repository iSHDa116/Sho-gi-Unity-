using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    readonly Vector2Int[] JumpDirections =
    {
        new Vector2Int(1, 2),
        new Vector2Int(2, -1)
    };

    public override List<Vector2Int> GetCanMoveTiles(Board board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        AddStepMoves(moves, board, JumpDirections);
        return moves;
    }
}
