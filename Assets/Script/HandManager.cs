using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager instance;
    [Header("手駒台")] //ヒエラルキーウィンドウ上で表示されるテキストを変える(動作に影響はなし)
    public Transform senteTray, goteTray;

    // リストを使って駒を並べる
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
    public void CaptureEnemy(PieceCtrler enemy, PlayerType captor)
    {
        //もし、とった駒がnullなら、処理を終わらせる
        if (enemy == null) return;
        // 盤外処理
        if (BoardManager.IsOutBoard(enemy.thisX, enemy.thisZ)) return;

        // 敵の駒があった場所をnullにする
        BoardManager.boardGridInfo[enemy.thisX, enemy.thisZ] = null;

        //pieceDataの情報を更新
        enemy.isCaptured = true; //持ち駒フラグをtrueにする
        if(enemy.isPromoted == true) //もし取った駒が成っていたら
        {
            enemy.transform.Rotate(0, 0, 180); //駒を表にする
            enemy.isPromoted = false; //成る判定を解除する
        }
        
        // 持ち主を変更(もし取った駒が先手なら、取った駒にGoteを代入する)
        enemy.pieceData.playerType = (captor == PlayerType.Sente) ? PlayerType.Gote : PlayerType.Sente; 
        enemy.gameObject.name = captor.ToString() + enemy.pieceData.pieceType;

        //見た目変更
        enemy.transform.Rotate(0, 180, 0); //向きを反転
        Transform setTray = (captor == PlayerType.Sente) ? goteTray : senteTray; //どっちの持ち駒台に置くかを判定
        enemy.transform.SetParent(setTray); //持ち駒台に駒を登録


        // 手駒のリストに追加
        var list = (captor == PlayerType.Sente) ? goteHand : senteHand;
        enemy.pieceData.playerType = (captor == PlayerType.Sente) ? PlayerType.Gote : PlayerType.Sente;
        list.Add(enemy); //リストに持ち駒を登録
        ReflowTray(list, enemy.transform.parent); //横並びに整列

        enemy.thisX = Mathf.RoundToInt(transform.position.x); 
        enemy.thisZ = Mathf.RoundToInt(transform.position.z);

        enemy.isInhand = true;
        ///enemy.gameObject.SetActive(true);

    }
    // 指定したマスに に置けるかの判定
    public bool TryDrop(PieceCtrler piece, int dropX, int dropZ)
    {
        // 例外を弾く
        if (piece == null || !piece.isInhand) return false;
        if (BoardManager.IsOutBoard(dropX, dropZ)) return false;
        if (BoardManager.boardGridInfo[dropX, dropZ] == null) return false;

        //合法手じゃないなら、置けなくする
        if (IsLegalDrop(piece, dropX, dropZ)) 
        {
            return true;
        }
        else
        {
            Debug.LogWarning("このマスに駒を置くことはできません");
            return false;
        }
    }

    void DropTo(PieceCtrler piece, int dropX, int dropZ)
    {
        // 持ち駒状態を解除
        piece.isInhand = false;

        // ToDo: 親をBoardにする必要あり
        piece.transform.SetParent(null); // 駒台との親子関係を解除
        piece.transform.position = new Vector3(dropX, PieceCtrler.defaultY, dropZ);
        piece.thisX = dropX; 
        piece.thisZ = dropZ;

        BoardManager.boardGridInfo[dropX, dropZ] = piece.transform;

        piece.GetComponent<AudioSource>().Play();

        // 手駒リストから削除&整列
        var list = (piece.pieceData.playerType == PlayerType.Sente) ? senteHand : goteHand;
        list.Remove(piece);
        ReflowTray(list, (piece.pieceData.playerType == PlayerType.Sente) ? senteTray : goteTray);

    }
    //手駒を並べる
    void ReflowTray(List<PieceCtrler> list, Transform tray)
    {
        const float gapX = 0.6f;
        //float gapY = 0.5f;
        for (int i = 0; i < list.Count; i++)
        {
            list[i].transform.position = tray.position + new Vector3(i * gapX, 0, 0);
        }
    }

    //持ち駒のおける場所と置けない場所の判定処理を行う関数
    public bool IsLegalDrop(PieceCtrler piece, int x, int z)
    {
        Piece handPiece = piece.pieceData;
        //int dir = (handPiece.playerType == PlayerType.Sente) ? 1 : -1;

        //駒ごとの打てない段を確認
        switch (handPiece.pieceType)
        {
            // 歩を置きたいマスの縦列に自分の歩があった場合、その列に歩を置く事はできない(二歩)
            // 歩を相手陣地の奥に置くことはできない
            case PieceType.Pawn:
                //盤面の最奥には置く事ができない
                if ((handPiece.playerType == PlayerType.Sente && z == BoardManager.depth - 1) || (handPiece.playerType == PlayerType.Gote && z == 0))
                {
                    return false;
                }

                Transform tileInfo = BoardManager.boardGridInfo[x, z];
                if(tileInfo == null) Debug.LogError("tileInfo is NULL");

                PieceCtrler tileInfoPiece = tileInfo.GetComponent<PieceCtrler>();
                if (tileInfoPiece.pieceData.playerType != handPiece.playerType || piece == null) 
                    return false; //駒が相手の駒だった場合、飛ばす

                // 盤面の縦に居るオブジェクトが歩dで、かつまだ成っていない場合
                if (tileInfoPiece.pieceData.pieceType == PieceType.Pawn && !tileInfoPiece.isPromoted)
                    return false;//二歩
                
                return true;

            //香車と桂馬は相手陣地の奥には置けない
            case PieceType.Lance:

                if ((handPiece.playerType == PlayerType.Sente && z == BoardManager.depth - 1) ||
                (handPiece.playerType == PlayerType.Gote && z == 0))
                {
                    return false;
                }
                break;

            case PieceType.Knight:

                if ((handPiece.playerType == PlayerType.Sente && z == BoardManager.depth - 1) ||
                (handPiece.playerType == PlayerType.Gote && z == 0))
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

    internal bool IsLegalDrop(PieceType pieceType, int boardX, int boardZ)
    {
        throw new NotImplementedException();
    }
}
