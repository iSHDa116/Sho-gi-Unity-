using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnCtrler : PieceCtrler
{

    public override List<Vector3> GetCanMoveTiles()
    {
        List<Vector3> moves = new List<Vector3>();

        if(pieceData == null)
            Debug.LogError("pieceDataがnullです");

        int dir = (pieceData.playerType == PlayerType.Sente) ? 1 : -1;

        if (!BoardManager.IsOutBoard(x, z + dir) && BoardManager.IsTileEmpty(x, z + dir) && !isPromoted)
        {
            Vector3 forwardMove = new Vector3(x, selectY, z + dir);
            moves.Add(forwardMove);
        }
        else if (!BoardManager.IsOutBoard(x, z + dir) && BoardManager.IsTileEmpty(x, z + dir) && isPromoted)
        {
            // 成った後の動き
            for (int i = -1; i <= 1; i++)
            {
                if (i == 0) continue; // 前進はすでに追加されている
                if (!BoardManager.IsOutBoard(x + i, z + dir) && BoardManager.IsTileEmpty(x + i, z + dir))
                {
                    Vector3 sideMove = new Vector3(x + i, selectY, z + dir);
                    moves.Add(sideMove);
                }
            }
        }

        Vector3 attackdir = new Vector3(dir, selectY, dir);

        int ax = x + (int)attackdir.x;
        int az = z + (int)attackdir.y;

        if (!BoardManager.IsOutBoard(ax, az))
        {
            Transform target = BoardManager.boardGridInfo[ax, az];
            // int ay = Mathf.RoundToInt(y);

            //攻撃範囲の取得
            if (IsCanCapture(ax, az))
            {
                moves.Add(new Vector3(ax, setY, az));
            }
        }

        return moves;
    }
}
