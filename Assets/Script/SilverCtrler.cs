using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverCtrler : PieceCtrler
{
    public override List<Vector3> GetCanMoveTiles()
    {
        List<Vector3> moves = new List<Vector3>();

        int dir = (this.pieceData.playerType == PlayerType.Sente) ? 1 : -1;

        Vector2Int[] directions = {

            new Vector2Int(1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, -1),
        };

        foreach (Vector2Int d in directions)
        {
            int dx = x + d.x;
            int dz = z + d.y * dir;

            if (BoardManager.boardGridInfo[dx, dz])
                continue;

            moves.Add(DirectionMove(dx, dz));
        }

        return moves;
    }
}
