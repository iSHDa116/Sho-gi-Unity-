using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static int width = 9, depth = 9;
    public static Transform[,] boardGridInfo = new Transform[width, depth];
    public GameObject[,] boardTileObj = new GameObject[width,depth];

    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameManager gm;
    //[SerializeField] TileCtrler tileCtrler;
    // Start is called before the first frame update
    void Start()
    {
        //BoardCreate();
    }

    public void BoardCreate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                Transform tile = Instantiate(tilePrefab, position, Quaternion.identity).transform;

                tile.name = "Tile_" + x + "_" + y;
                tile.parent = transform;
                boardTileObj[x,y] = tile.gameObject;
            }
        }
    }

    public static bool IsTileEmpty(int x, int z)
    {
        return boardGridInfo[x, z] == null;
    }
    public static bool IsOutBoard(int x, int z)
    {
        // 
        return (x < 0 || x >= BoardManager.width || z < 0 || z >= BoardManager.depth);

    }

    public void ResetHighlightedTiles(PieceCtrler piece)
    {
        foreach (var tile in piece.GetCanMoveTiles())
        {
            int tileX = Mathf.RoundToInt(tile.x);
            int tileZ = Mathf.RoundToInt(tile.z);

            //Transform tile = BoardManager.boardGridInfo[tileX, tileZ];
            GameObject tileObj = boardTileObj[tileX, tileZ];
            if(tileObj==null) Debug.LogError("tileObj is Null");

            tileObj.GetComponent<TileCtrler>().ResetColor();
        }
    }
    //駒をおけるマスを検索する関数
    public void SearchTiles(List<Vector3> moveTiles)
    {
        foreach (Vector3 pos in moveTiles)
        {
            int tx = Mathf.RoundToInt(pos.x);
            int tz = Mathf.RoundToInt(pos.z);

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
