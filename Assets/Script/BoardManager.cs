using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static int width = 9, depth = 9;
    public static Transform[,] boardGridInfo = new Transform[width, depth];

    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameManager gm;
    [SerializeField] TileCtrler tileCtrler;
    // Start is called before the first frame update
    void Start()
    {
        BoardCreate();
    }

    void BoardCreate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                Transform tile = Instantiate(tilePrefab, position, Quaternion.identity).transform;
                boardGridInfo[x, y] = tile;

                tile.name = "Tile_" + x + "_" + y;
                tile.parent = transform;
            }
        }
    }

    public static bool IsTileEmpty(int x, int z)
    {
        return boardGridInfo[x, z] == null;
    }
    public static bool IsOutBoard(int x, int z)
    {
        return (x < 0 || x >= BoardManager.width || z < 0 || z >= BoardManager.depth);

    }
}
