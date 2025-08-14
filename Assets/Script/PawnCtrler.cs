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
        Debug.Log("pieceData."+this.pieceData.pieceType);

        int dir = (this.pieceData.playerType == PlayerType.Sente) ? 1 : -1;
        Debug.Log("dir == " + dir);
        //Debug.Log($"現在地: ({x}, {z}) dir: {dir} => 移動先: {z + dir}");

        if (!BoardManager.IsOutBoard(x, z+dir))
        {

            if (!isPromoted)
                {
                    Vector3 forwardMove = new Vector3(x, setY, z + dir);
                    moves.Add(forwardMove);
                    Debug.Log($"{forwardMove}を追加しました");
                }
                else
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

        }
        else
        {
            Debug.LogError("PawnCtrler(18).リストにマスを追加できませんでした");
        }

        /*Vector3 attackdir = new Vector3(dir, selectY, dir);

        int ax = x + Mathf.RoundToInt(attackdir.x);
        int az = z + Mathf.RoundToInt(attackdir.z);

        if (!BoardManager.IsOutBoard(ax, az))
        {
            Transform target = BoardManager.boardGridInfo[ax, az];
            // int ay = Mathf.RoundToInt(y);

            //攻撃範囲の取得
            if (IsCanCapture(ax, az))
            {
                moves.Add(new Vector3(ax, setY, az));
            }
        }*/
        return moves;
    }
}
