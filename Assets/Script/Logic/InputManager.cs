using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    BoardView bm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // クリックされた場所を取得する
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //駒がクリックされた時の処理
            ClickedPiece(ray);
        }
    }

    void ClickedPiece(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            PieceCtrler piece = hit.collider.GetComponent<PieceCtrler>();
            if (piece != null)
            {
                string pieceInfo = $"{piece.gameObject.name},PieceType:{piece.pieceData.pieceType},Playertype:{piece.pieceData.playerType}";
                Debug.Log(pieceInfo);

                piece.SelectPiece();
                List<Vector2Int> moves = piece.GetCanMoveTiles();
                bm.HighLightTiles(moves);
            }
            else
            {
                Debug.LogError("piece is null");
            }
        }
    }
}
