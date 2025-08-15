using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCtrler : PieceCtrler
{
    public override List<Vector3> GetCanMoveTiles()
    {
        List<Vector3> moves = new List<Vector3>();

        int dir = (this.pieceData.playerType == PlayerType.Sente) ? 1 : -1;

        foreach (Vector2Int d in GoldDirections)
        {
            int dx = x + d.x;
            int dz = z + d.y * dir;

            Debug.Log($"現在の座標：ax={dx},az={dz}");
            if (BoardManager.IsOutBoard(dx, dz))
                continue;
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
