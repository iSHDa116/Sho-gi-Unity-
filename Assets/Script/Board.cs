using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public const int Size = 9;

    Piece[,] grid = new Piece[Size, Size];

    // 盤の中かどうか
    public bool IsInside(int x, int z)
    {
        return x >= 0 && x < Size && z >= 0 && z < Size;
    }

    // そのマスにいる駒を返す(いなければ null)
    public Piece GetPiece(int x, int z)
    {
        if (!IsInside(x, z)) return null;
        return grid[x, z];
    }

    // 盤の中で、かつ駒がいないマスか
    public bool IsEmpty(int x, int z)
    {
        return IsInside(x, z) && grid[x, z] == null;
    }

    // 駒を盤に置く(駒自身の x,z も更新する)
    public void PutPiece(Piece piece, int x, int z)
    {
        grid[x, z] = piece;
        piece.x = x;
        piece.z = z;
    }

    // 駒を盤から外す
    public void RemovePiece(Piece piece)
    {
        if (grid[piece.x, piece.z] == piece)
            grid[piece.x, piece.z] = null;
    }

    // 駒を別のマスへ動かす(移動先に駒がいたら上書きされる)
    public void MovePiece(Piece piece, int x, int z)
    {
        RemovePiece(piece);
        PutPiece(piece, x, z);
    }

    // 指定した色の駒を全部返す
    public List<Piece> GetPieces(bool isWhite)
    {
        List<Piece> result = new List<Piece>();
        foreach (Piece piece in grid)
        {
            if (piece != null && piece.isSente == isWhite)
                result.Add(piece);
        }
        return result;
    }

    // 指定した色のキングを探す
    public Piece FindKing(bool isWhite)
    {
        foreach (Piece piece in GetPieces(isWhite))
        {
            if (piece is King) return piece;
        }
        return null;
    }
}
