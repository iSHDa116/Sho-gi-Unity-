using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCtrler : PieceCtrler
{
    Vector2Int[] diretions = {

        new Vector2Int(-1,2),
        new Vector2Int(1, 2)
    };

    public override List<Vector2Int> GetCanMoveTiles()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        int dir = (this.pieceData.playerType == PlayerType.Sente) ? 1 : -1;

        //成っているなら
        if (isPromoted)
        {
            foreach (Vector2Int gd in GoldDirections)
            {
                int nx = thisX + gd.x;
                int nz = thisZ + gd.y * dir;

                if (BoardManager.IsOutBoard(nx, nz))
                    continue;

                Transform target = BoardManager.boardGridInfo[nx, nz];
                if (target != null)
                {
                    Piece enemy = target.GetComponent<PieceCtrler>().pieceData;
                    if (enemy.playerType != this.pieceData.playerType)
                    {
                        moves.Add(new Vector2Int(nx, nz));
                    }
                }
                else
                {
                    moves.Add(new Vector2Int(nx, nz));
                }
            }
        }
        else
        {
            foreach (Vector2Int d in diretions)
            {
                int nx = thisX + d.x;
                int nz = thisZ + d.y * dir;

                if (BoardManager.IsOutBoard(nx, nz))
                    continue;

                Transform target = BoardManager.boardGridInfo[nx, nz];
                if (target != null)
                {
                    Piece enemy = target.GetComponent<PieceCtrler>().pieceData;
                    if (enemy.playerType != this.pieceData.playerType)
                    {
                        moves.Add(new Vector2Int(nx, nz));
                    }
                }
                else
                {
                    moves.Add(new Vector2Int(nx, nz));
                }
            }
        }

        return moves;
    }
}
