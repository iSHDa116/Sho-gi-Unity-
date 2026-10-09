using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : Piece
{
    Vector2Int[] LanceDir = { new Vector2Int(1,0) };
    int Forward { get{ return isSente ? 1 : -1; } }
    public override List<Vector2Int> GetCanMoveTiles(Board board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int frontZ = z + Forward;
        AddLineMoves(moves, board, LanceDir);
        return moves;
    }
}
