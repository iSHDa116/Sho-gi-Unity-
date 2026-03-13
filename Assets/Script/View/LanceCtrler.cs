using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceCtrler : PieceCtrler
{
    public override List<Vector3> GetCanMoveTiles()
    {
        List<Vector3> moves = new List<Vector3>();

        int dir = (this.pieceData.playerType == PlayerType.Sente) ? 1 : -1;

        if (!isPromoted)
        {
            int nz = thisZ + dir;
            while (!BoardManager.IsOutBoard(thisX, nz))
            {
                Transform target = BoardManager.boardGridInfo[thisX, nz];

                if (target != null)
                {
                    Piece enemy = target.GetComponent<PieceCtrler>().pieceData;
                    if (enemy != null && enemy.playerType != this.pieceData.playerType)
                    {
                        moves.Add(new Vector3(thisX, defaultY, nz));
                    }
                    break;
                }

                moves.Add(new Vector3(thisX, defaultY, nz));
                nz += dir;

            }
        }
        else
        {
            foreach (Vector2Int d in GoldDirections)
            {
                int dx = thisX + d.x;
                int dz = thisZ + d.y * dir;

                if (BoardManager.IsOutBoard(dx, dz))
                    continue;

                Transform target = BoardManager.boardGridInfo[dz, dz];
                if (target != null)
                {
                    Piece enemy = target.GetComponent<PieceCtrler>().pieceData;

                    if (enemy.playerType != this.pieceData.playerType)
                    {
                        moves.Add(new Vector3(dx, defaultY, dz));
                    }
                }
                else
                {
                    moves.Add(new Vector3(dx, defaultY, dz));
                }
            }
        }

        return moves;
    }
}
