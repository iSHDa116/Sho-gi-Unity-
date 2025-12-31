using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager instance;
    [Header("手駒を置くアンカー")]
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

    void SetPos()
    {
        int oldX = Mathf.RoundToInt(transform.position.x);
        int oldZ = Mathf.RoundToInt(transform.position.z);
    }
    // 取った駒を持ち駒化する処理
    public void CaptureEnemy(PieceCtrler enemy, PlayerType captor)
    {
        //もし、とった駒がnullなら、処理を終わらせる
        if (enemy == null) return;

        // 盤外処理
        /*if (!BoardManager.IsOutBoard(enemy.x, enemy.z))
            // 盤外じゃなければ空にする
            BoardManager.boardGridInfo[enemy.x, enemy.z] = null;
        */

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
        enemy.transform.SetParent(captor == PlayerType.Sente ? goteTray : senteTray); //持ち駒台に駒を登録


        // 手駒のリストに
        var list = (captor == PlayerType.Sente) ? goteHand : senteHand;
        enemy.pieceData.playerType = (captor == PlayerType.Sente) ? PlayerType.Gote : PlayerType.Sente;
        list.Add(enemy); //リストに持ち駒を登録
        ReflowTray(list, enemy.transform.parent); //横並びに整列

        enemy.x = Mathf.RoundToInt(transform.position.x); 
        enemy.z = Mathf.RoundToInt(transform.position.z);

        enemy.isInhand = true;
        ///enemy.gameObject.SetActive(true);

    }
    // 指定したマスに に置けるかの判定
    public bool TryDrop(PieceCtrler piece, int dropX, int dropZ)
    {
        if (piece == null || !piece.isInhand) return false;

        if (!BoardManager.IsOutBoard(dropX, dropZ) && BoardManager.boardGridInfo[dropX, dropZ] == null)
        {
            //List<Vector3> dropTiles = ;
            //SearchDropTiles();
            //合法手じゃないなら、置けなくする
            if (!IsLegalDrop(piece, dropX, dropZ)) return false;

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

        piece.GetComponent<AudioSource>().Play();

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

    void SearchDropTiles(List<Vector3> moveTiles)
    {
        foreach (Vector3 pos in moveTiles)
        {
            int tx = Mathf.RoundToInt(pos.x);
            int tz = Mathf.RoundToInt(pos.z);

            string tileName = $"Tile_{tx}_{tz}";
            GameObject tileObj = GameObject.Find(tileName);
            if (tileObj == null)
            {
                Debug.LogWarning($"タイルが見つかりません: {tileName}");
                continue;
            }

            // 子参照はやめてコンポーネントに頼る
            TileCtrler tile = tileObj.GetComponent<TileCtrler>();
            if (tile == null)
            {
                Debug.LogWarning($"TileCtrlerが見つかりません: {tileName}");
                continue;
            }

            tile.HighLightTile();
            // ハイライト後に戻すために記録しておく（未実装なら省略）
            // highlightedTiles.Add(tile);
        }
    }
    //持ち駒のおける場所と置けない場所の判定処理を行う関数
    bool IsLegalDrop(PieceCtrler piece, int x, int z)
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
                if ((handPiece.playerType == PlayerType.Sente && z == BoardManager.depth - 1) ||
                (handPiece.playerType == PlayerType.Gote && z == 0))
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
                    if (p2.pieceData.playerType != handPiece.playerType || piece == null) continue; //駒が相手の駒だった場合、飛ばす

                    // 盤面の縦に居るオブジェクトが歩dで、かつまだ成っていない場合
                    if (p2.pieceData.pieceType == PieceType.Pawn && !p2.isPromoted)
                        return false;//二歩
                }
                break;

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


}
