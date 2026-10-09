using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public int x;
    public int z;
    public readonly float defaultY = 0.56f;
    public readonly float selectY = 1.55f;
    public bool isSente;
    public bool isCapturedPiece;
    public PieceType type;

    // 4方向・斜め4方向。子クラスで共通して使う
    protected static readonly Vector2Int[] StraightDirections =
    {
        new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1)
    };
    protected static readonly Vector2Int[] DiagonalDirections =
    {
        new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(-1, -1), new Vector2Int(1, -1)
    };

    // 生成した直後に呼ぶ初期設定(コンストラクタ設定)
    public void Initialize(PieceType type, int x, int z, bool isSente)
    {
        this.type = type;
        this.x = x;
        this.z = z;
        this.isSente = isSente;
        this.transform.position = new Vector3(this.x, defaultY, this.z);
    }

    // 駒のルール上、動けるマスを返す(王手かどうかはここでは考えない)
    // 子クラスで必ず override(上書き)する
    public abstract List<Vector2Int> GetCanMoveTiles(Board board);

    protected void AddStepMoves(List<Vector2Int> moves, Board board, Vector2Int[] directions)
    {
        foreach (Vector2Int dir in directions)
        {
            int nx = x + dir.x;
            int nz = z + dir.y;

            if (!board.IsInside(nx, nz)) continue;

            Piece other = board.GetPiece(nx, nz);
            if (other == null || other.isSente != isSente)
                moves.Add(new Vector2Int(nx, nz));
        }
    }

    protected void AddLineMoves(List<Vector2Int> moves, Board board, Vector2Int[] directions)
    {
        foreach (Vector2Int dir in directions)
        {
            int nx = x + dir.x;
            int nz = z + dir.y;

            while (board.IsInside(nx, nz))
            {
                Piece other = board.GetPiece(nx, nz);

                if (other == null)
                {
                    moves.Add(new Vector2Int(nx, nz));
                }
                else
                {
                    if (other.isSente != isSente)
                        moves.Add(new Vector2Int(nx, nz));
                    break; // 駒にぶつかったので、この方向はここまで
                }

                nx += dir.x;
                nz += dir.y;
            }
        }
    }

    // 
    public void ChangePlayer()
    {
        isSente = !isSente;
    }

    // 取られた駒の持ち主を変更している
    public void GetPiece()
    {
        isCapturedPiece = !isCapturedPiece;
    }
}
