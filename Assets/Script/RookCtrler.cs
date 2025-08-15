using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookCtrler : PieceCtrler
{
    public override List<Vector3> GetCanMoveTiles()
    {
        List<Vector3> moves = new List<Vector3>();

        Vector2Int[] directions = {
            new Vector2Int(0,1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0)
        };


        foreach (Vector2Int d in directions)
        {
            int nx = x + d.x;
            int nz = z + d.y;

            while (!BoardManager.IsOutBoard(nx, nz))
            {

                Transform target = BoardManager.boardGridInfo[nx, nz];
                if (target != null)
                {
                    Piece enemy = target.GetComponent<PieceCtrler>().pieceData;
                    if (enemy.playerType != this.pieceData.playerType)
                    {
                        moves.Add(new Vector3(nx, setY, nz));
                    }
                    break;
                }

                moves.Add(new Vector3(nx, setY, nz));
                //必ず、movesに座標を入れてから + しないと、配列外が出てエラーになる
                nx += d.x;
                nz += d.y;
                continue;
            }
        }
        if (isPromoted)
        {
            Vector2Int[] promDir = {
                new Vector2Int(-1,1),
                new Vector2Int(1,1),
                new Vector2Int(-1, -1),
                new Vector2Int(1, -1)
            };

            foreach (Vector2Int p in promDir)
            {
                int px = x + p.x;
                int pz = z + p.y;

                if (BoardManager.IsOutBoard(px, pz)) continue;

                Transform target = BoardManager.boardGridInfo[px, pz];

                if (target != null)
                {
                    Piece enemy = target.GetComponent<PieceCtrler>().pieceData;
                    if (enemy.playerType != this.pieceData.playerType)
                    {
                        moves.Add(new Vector3(px, setY, pz));
                    }
                    continue;
                }
                else
                {
                    moves.Add(new Vector3(px, setY, pz));
                }
            }
        }
        
        return moves;
    }
}
