using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCtrler : PieceCtrler
{
    public override List<Vector3> GetCanMoveTiles()
    {
        List<Vector3> moves = new List<Vector3>();

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
            int dx = x + d.x;
            int dz = z + d.y;

            if (BoardManager.IsOutBoard(dx, dz)) continue;

            Transform target = BoardManager.boardGridInfo[dx, dz];

            if (target != null)
            {
                Piece enemy = target.GetComponent<PieceCtrler>().pieceData;

                if (enemy.playerType != this.pieceData.playerType)
                {
                    moves.Add(new Vector3(dx, setY, dz));
                }
            }
            else
            {
                moves.Add(new Vector3(dx, setY, dz));
            }
        }

        return moves;

    }
}
