using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
盤面の見た目を管理するスクリプト

CreateBoard() 盤面を生成

CreatePiece() 盤面上にコマを生成する。

*/

public class BoardView : MonoBehaviour
{
    //↓↓↓↓マスに使う変数↓↓↓↓
    [SerializeField] GameObject spacePrefab;
    //マス目の操作時に使用
    SpaceCtrler[,] spaces = new SpaceCtrler[Board.Size, Board.Size];
    // ハイライトにするマスを格納
    List<SpaceCtrler> highlightedSpaces = new List<SpaceCtrler>();

    // Start is called before the first frame update
    public void CreateBoard()
    {
        for (int z = 0; z < Board.Size; z++)
        {
            for (int x = 0; x < Board.Size; x++)
            {
                GameObject obj = Instantiate(spacePrefab, new Vector3(x, 0, z), Quaternion.identity);
                obj.name = $"space_{x}_{z}";

                SpaceCtrler space = obj.GetComponent<SpaceCtrler>();
                if (space == null) Debug.LogError("SpaceCtrlerの取得に失敗しました");
                space.Init(x, z);
                spaces[x, z] = space;
            }
        }
    }

    
    // 動けるマスをハイライト
    public void ShowHighlights(List<Vector2Int> tiles)
    {
        ClearHighlights();

        foreach (Vector2Int pos in tiles)
        {
            SpaceCtrler space = spaces[pos.x, pos.y];
            space.HighLightSpace();
            highlightedSpaces.Add(space);
        }
    }

    // ハイライトを消す
    public void ClearHighlights()
    {
        foreach (SpaceCtrler space in highlightedSpaces)
            space.ResetColor();

        highlightedSpaces.Clear();
    }
}