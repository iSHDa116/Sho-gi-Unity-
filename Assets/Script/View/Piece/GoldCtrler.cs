using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCtrler : PieceCtrler
{
    public override List<Vector2Int> GetCanMoveTiles()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        int dir = (this.pieceData.playerType == PlayerType.Sente) ? 1 : -1;

        foreach (Vector2Int d in GoldDirections)
        {
            int dirX = thisX + d.x;
            int dirZ = thisZ + d.y * dir;

            Debug.Log($"現在の座標：ax={dirX},az={dirZ}");
            if (BoardManager.IsOutBoard(dirX, dirZ))
                continue;
            Transform target = BoardManager.boardGridInfo[dirX, dirZ];

            if (target != null)
            {
                Piece enemy = target.GetComponent<PieceCtrler>().pieceData;
                if (enemy.playerType != this.pieceData.playerType)
                {
                    moves.Add(new Vector2Int(dirX, dirZ));
                }
            }
            else
            {
                moves.Add(new Vector2Int(dirX, dirZ));
            }
        }
        return moves;
    }
}
