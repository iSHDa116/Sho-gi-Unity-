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
            int ax = x + d.x;
            int az = z + d.y * dir;

            Debug.Log($"現在の座標：ax={ax},az={az}");
            if (BoardManager.IsOutBoard(ax, az))
                continue;

            moves.Add(DirectionMove(ax, az));
        }
        return moves;
    }
}
