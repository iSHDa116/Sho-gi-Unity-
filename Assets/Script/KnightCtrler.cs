using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCtrler : PieceCtrler
{
    Vector2Int[] diretions = {

        new Vector2Int(-1,2),
        new Vector2Int(1, 2)
    };

    public override List<Vector3> GetCanMoveTiles()
    {
        List<Vector3> moves = new List<Vector3>();

        int dir = (this.pieceData.playerType == PlayerType.Sente) ? 1 : -1;

        if (!isPromoted)
        {
            foreach (Vector2Int d in diretions)
            {
                int nx = x + d.x;
                int nz = z + d.y * dir;

                if (BoardManager.IsOutBoard(nx, nz))
                    continue;

                Transform target = BoardManager.boardGridInfo[nx, nz];
                if (target != null)
                {
                    Piece enemy = target.GetComponent<PieceCtrler>().pieceData;
                    if (enemy.playerType != this.pieceData.playerType)
                    {
                        moves.Add(new Vector3(nx, setY, nz));
                    }
                }
                else
                {
                    moves.Add(new Vector3(nx, setY, nz));
                }
            }
        }
        else
        {
            foreach (Vector2Int gd in GoldDirections)
            {
                int nx = x + gd.x;
                int nz = z + gd.y * dir;

                if (BoardManager.IsOutBoard(nx, nz))
                    continue;

                Transform target = BoardManager.boardGridInfo[nx, nz];
                if (target != null)
                {
                    Piece enemy = target.GetComponent<PieceCtrler>().pieceData;
                    if (enemy.playerType != this.pieceData.playerType)
                    {
                        moves.Add(new Vector3(nx, setY, nz));
                    }
                }
                else
                {
                    moves.Add(new Vector3(nx, setY, nz));
                }
            }
        }

        return moves;
    }
}
