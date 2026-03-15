using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    public static int width = 9, depth = 9;
    public GameObject[,] boardTileObj = new GameObject[width, depth];
    [SerializeField] GameObject tilePrefab;
    // "次の順に入れる→ 0=歩、1=飛車、2=角行、3=香車、4=桂馬、 5=銀、6=金、7=王, 8=玉"
    [SerializeField] GameObject[] piecePrafab;
    [SerializeField] GameManager gm;

    //[SerializeField] TileCtrler tileCtrler;
    // Start is called before the first frame update
    void Start()
    {
        //BoardCreate();
    }

    public void SetBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                Vector3 position = new Vector3(x, 0, y);

                // 盤面生成
                CreateBoard(position);
                // 駒を生成
                SpawnPiece(position);
            }
        }
    }

    void CreateBoard(Vector3 pos)
    {
        Transform tile = Instantiate(tilePrefab, pos, Quaternion.identity).transform;

        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.y);

        tile.name = "Tile_" + x + "_" + y;
        tile.parent = transform;
        boardTileObj[x, y] = tile.gameObject;
    }

    void SpawnPiece(Vector3 pos)
    {
        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.y);

        int pieceType = Board.boardInfo[x, y] % 10;
        int player = Board.boardInfo[x, y] / 10;

        if (pieceType == 0) return;

        pos.y = 0.55f;

        GameObject pieceObj = Instantiate(piecePrafab[pieceType - 1], pos, Quaternion.Euler(0, player * 180, 0));
        PlayerType playerType = (Board.boardInfo[x, y] > 10) ? PlayerType.Gote : PlayerType.Sente;

        pieceObj.GetComponent<PieceCtrler>().pieceData = new Piece((PieceType)pieceType, playerType);
        pieceObj.transform.SetParent(this.transform);
    }

    public void ResetHighlightedTiles(PieceCtrler piece)
    {
        foreach (var tile in piece.GetCanMoveTiles())
        {
            int tileX = Mathf.RoundToInt(tile.x);
            int tileZ = Mathf.RoundToInt(tile.y);

            //Transform tile = BoardManager.boardGridInfo[tileX, tileZ];
            GameObject tileObj = boardTileObj[tileX, tileZ];
            if (tileObj == null) Debug.LogError("tileObj is Null");

            tileObj.GetComponent<TileCtrler>().ResetColor();
        }
    }
    //駒をおけるマスを検索する関数
    public void HighLightTiles(List<Vector2Int> moveTiles)
    {
        foreach (Vector2Int pos in moveTiles)
        {
            int tx = Mathf.RoundToInt(pos.x);
            int tz = Mathf.RoundToInt(pos.y);

            GameObject tileObj = boardTileObj[tx, tz];
            if (tileObj == null)
            {
                Debug.LogWarning($"タイルが見つかりません: {tileObj}");
                continue;
            }

            TileCtrler tile = tileObj.GetComponent<TileCtrler>();
            if (tile == null)
            {
                Debug.LogWarning($"TileCtrlerが見つかりません: {tileObj.name}");
                continue;
            }
            tile.HighLightTile();
            // ハイライト後に戻すために記録しておく（未実装なら省略）
            // highlightedTiles.Add(tile);
        }
    }
}
