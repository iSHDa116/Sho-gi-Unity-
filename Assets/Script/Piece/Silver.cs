using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silver : Piece
{
    int Front { get { return isSente ? 1 : -1; } }
    readonly Vector2Int[] SilverDirections = {

            new Vector2Int(1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, -1),
        };
    public override List<Vector2Int> GetCanMoveTiles(Board board)
    {
        List<Vector2Int> moves = new();
        int frontZ = z + Front;
        AddLineMoves(moves,board,SilverDirections);
        return moves;
    }
}
