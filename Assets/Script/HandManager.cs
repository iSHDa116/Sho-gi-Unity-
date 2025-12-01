using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager instance;
    [Header("手駒を置くアンカー")]
    public Transform senteTray, goteTray;

    //リストを使って駒を並べる
    readonly List<PieceCtrler> senteHand = new();
    readonly List<PieceCtrler> goteHand = new();

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    }
    // 取った駒を持ち駒化する処理
    public void Capture(PieceCtrler enemy, PlayerType captor)
    {
        if (enemy == null) return;

        if (!BoardManager.IsOutBoard(enemy.x, enemy.z))
            BoardManager.boardGridInfo[enemy.x, enemy.z] = null;

        //pieceDataの情報を更新
        enemy.isCaptured = true;
        enemy.isCaptured = true;
        enemy.isPromoted = false;
        enemy.pieceData.playerType = captor;

        //見た目変更
        enemy.transform.localRotation = Quaternion.identity;
        enemy.transform.Rotate(0, 180, 0);
        enemy.transform.SetParent(captor == PlayerType.Sente ? goteTray : senteTray);


        // 手駒のリストに
            var list = (captor == PlayerType.Sente) ? goteHand : senteHand;
        list.Add(enemy);
        ReflowTray(list, enemy.transform.parent);

        enemy.x = -1; enemy.z = -1;

        enemy.gameObject.SetActive(true);

    }
    // 2) 打つ：持ち駒( piece ) を 指定したマスに に置けるかの判定
    public bool TryDrop(PieceCtrler piece, int dropX, int dropZ)
    {
        if (piece == null || !piece.isInhand) return false;
        if (!BoardManager.IsOutBoard(dropX, dropZ) && BoardManager.boardGridInfo[dropX, dropZ] == null)
        {
            if (IsLegalDrop(piece, dropX, dropZ)) return false;

            //盤に配置
            DropTo(piece, dropX, dropZ);
            return true;
        }
        return false;
    }

    void DropTo(PieceCtrler piece, int dropX, int dropZ)
    {
        // 持ち駒状態を解除
        piece.isInhand = false;

        // 後で親をBoardにする必要あり
        piece.transform.SetParent(null);
        piece.transform.position = new Vector3(dropX, PieceCtrler.setY, dropZ);
        piece.x = dropX; piece.z = dropZ;
        BoardManager.boardGridInfo[dropX, dropZ] = piece.transform;

        // 手駒リストから削除&整列
        var list = (piece.pieceData.playerType == PlayerType.Sente) ? senteHand : goteHand;
        list.Remove(piece);
        ReflowTray(list, (piece.pieceData.playerType == PlayerType.Sente) ? senteTray : goteTray);

    }
    //手駒を並べる
    void ReflowTray(List<PieceCtrler> list, Transform tray)
    {
        float gapX = 0.6f;
        //float gapY = 0.5f;
        for (int i = 0; i < list.Count; i++)
        {
            list[i].transform.position = tray.position + new Vector3(i * gapX, 0, 0);
        }
    }
    bool IsLegalDrop(PieceCtrler piece, int x, int z)
    {
        Piece p = piece.pieceData;
        int dir = (p.playerType == PlayerType.Sente) ? 1 : -1;

        //駒ごとの打てない段を確認
        switch (p.pieceType)
        {
            // 歩を置きたいマスの縦列に自分の歩があった場合、その列に歩を置く事はできない(二歩)
            // 歩を相手陣地の奥に置くことはできない
            case PieceType.Pawn:
                //盤面の最奥には置く事ができない
                if ((p.playerType == PlayerType.Sente && z == BoardManager.depth - 1) ||
                (p.playerType == PlayerType.Gote && z == 0))
                {
                    return false;
                }

                //二歩のチェック
                for (int zz = 0; zz < BoardManager.depth; zz++)
                {
                    // 縦に駒がいないか探す
                    Transform t = BoardManager.boardGridInfo[x, zz];
                    if (t == null) continue; //駒がなければ飛ばす
                    PieceCtrler p2 = t.GetComponent<PieceCtrler>();
                    if (p2.pieceData.playerType != p.playerType || piece == null) continue; //駒が相手の駒だった場合、飛ばす

                    // 盤面の縦に居るオブジェクトが歩dで、かつまだ成っていない場合
                    if (p2.pieceData.pieceType == PieceType.Pawn && !p2.isPromoted)
                        return false;//二歩
                }
                break;

            //香車と桂馬は相手陣地の奥には置けない
            case PieceType.Lance:

                if ((p.playerType == PlayerType.Sente && z == BoardManager.depth - 1) ||
                (p.playerType == PlayerType.Gote && z == 0))
                {
                    return false;
                }
                break;

            case PieceType.Knight:

                if ((p.playerType == PlayerType.Sente && z == BoardManager.depth - 1) ||
                (p.playerType == PlayerType.Gote && z == 0))
                {
                    return false;
                }
                break;
            //その他(飛車・角行・金・銀・王はどこでも置ける)
            default:
                break;
        }

        return true;
    }


}
