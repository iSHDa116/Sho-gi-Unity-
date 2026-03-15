using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCtrler : PieceCtrler
{
    public override List<Vector2Int> GetCanMoveTiles()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        Vector2Int[] Directions = {
            new Vector2Int(-1,1),
            new Vector2Int(0,1),
            new Vector2Int(1,1),
            new Vector2Int(-1,0),
            new Vector2Int(1,0),
            new Vector2Int(0,-1),
            new Vector2Int(1,-1),
            new Vector2Int(-1,-1)

        };

        foreach (Vector2Int d in Directions)
        {
            int dx = thisX + d.x;
            int dz = thisZ + d.y;

            if (BoardManager.IsOutBoard(dx, dz)) continue;

            Transform target = BoardManager.boardGridInfo[dx, dz];

            if (target != null)
            {
                Piece enemy = target.GetComponent<PieceCtrler>().pieceData;

                if (enemy.playerType != this.pieceData.playerType)
                {
                    moves.Add(new Vector2Int(dx, dz));
                }
            }
            else
            {
                moves.Add(new Vector2Int(dx, dz));
            }
        }

        return moves;

    }
}
