using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopCtrler : PieceCtrler
{
    Vector2Int[] directions = {
        new Vector2Int(-1, 1),
        new Vector2Int(1, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(1, -1)
    };

    Vector2Int[] promDir = {
        new Vector2Int(0,1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1)
    };

    public override List<Vector2Int> GetCanMoveTiles()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        foreach (Vector2Int d in directions)
        {
            int nx = thisX + d.x;
            int nz = thisZ + d.y;

            while (!Board.IsOutBoard(nx, nz))
            {
                PieceCtrler target = Board.boardPieceInfo[nx, nz];

                if (target != null)
                {
                    Piece enemy = target.pieceData;
                    if (enemy.playerType != this.pieceData.playerType)
                    {
                        moves.Add(new Vector2Int(nx, nz));
                    }
                    break;
                }

                moves.Add(new Vector2Int(nx, nz));
                nx += d.x;
                nz += d.y;
                continue;
            }
        }
        if (isPromoted)
        {
            foreach (Vector2Int p in promDir)
            {
                int px = thisX + p.x;
                int pz = thisZ + p.y;

                if (Board.IsOutBoard(px, pz)) continue;

                PieceCtrler target = Board.boardPieceInfo[px, pz];

                if (target != null)
                {
                    Piece enemy = target.pieceData;
                    if (enemy.playerType != this.pieceData.playerType)
                    {
                        moves.Add(new Vector2Int(px, pz));
                    }
                    continue;
                }
                else
                {
                    moves.Add(new Vector2Int(px, pz));
                }
            }
        }

        return moves;
    }
}
