using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Piece
{
    int Forward { get { return isSente ? 1 : -1; } }
    readonly Vector2Int[] GoldDirections = {

            new Vector2Int(1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
        }; 
    public override List<Vector2Int> GetCanMoveTiles(Board board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int frontZ = z + Forward;
        AddLineMoves(moves, board, GoldDirections);
        return moves;
    }
}
