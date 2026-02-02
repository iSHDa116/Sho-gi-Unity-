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

        // もし選択した歩が持ち駒じゃないなら
        if (!isInhand)
        {
            // 成っていなければ
            if (!isPromoted)
            {
                // 移動範囲の定義
                Vector3 forwardMove = new Vector3(thisX, defaultY, thisZ + dir);
                // 移動範囲の追加
                moves.Add(forwardMove);
                Debug.Log($"{forwardMove}を追加しました");
            }
            else
            {
                // 成った後の動き
                foreach (Vector2Int d in GoldDirections)
                {
                    int dx = thisX + d.x;
                    int dz = thisZ + d.y * dir;

                     // Debug.Log($"現在の座標：ax={ax},az={az}");
                    if (BoardManager.IsOutBoard(dx, dz))
                        continue;
                    
                    Transform target = BoardManager.boardGridInfo[dx, dz];
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
        }
        //持ち駒なら
        else
        {
            /*Vector3 dropPos = new Vector3();
            for(int boardX = 0; boardX <= BoardManager.width; boardX++)
            {
                for(int boardZ = 0; boardZ <= BoardManager.depth; boardZ++)
                {
                    if(BoardManager.IsOutBoard(boardX, boardZ))
                        continue;

                    // マスの情報を取得
                    Transform tileInfo = BoardManager.boardGridInfo[boardX, boardZ];

                    // マスに駒がなければ追加
                    if(tileInfo != null && HandManager.instance.IsLegalDrop(this, boardX, boardZ))
                    {
                        PieceCtrler enemy = tileInfo.GetComponent<PieceCtrler>();
                        if(enemy.pieceData.playerType == this.pieceData.playerType)
                        {
                            dropPos = new Vector3(boardX,setY,boardZ);
                            moves.Add(dropPos);
                        }
                    }
                    else
                    {
                        dropPos = new Vector3(boardX, setY, boardZ);
                        moves.Add(dropPos);
                    }
                }
            }
            return moves;*/
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
