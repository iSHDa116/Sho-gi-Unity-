using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnCtrler : PieceCtrler
{
    public override List<Vector2Int> GetCanMoveTiles()
    {
        List<Vector2Int> moves = new();

        int dir = (pieceData.playerType == PlayerType.Sente) ? 1 : -1;

        int targetZ = thisZ + dir;

        if(targetZ >= 0 && targetZ < 9)
        {
            Board board = new Board();
            if(board.GetPiece(thisX, targetZ) == null)
            {
                moves.Add(new Vector2Int(thisX, targetZ));
            }
        }

        return moves;
    }
}
