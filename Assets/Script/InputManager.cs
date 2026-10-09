using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]Camera mainCamera;
    public void HandleClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, 1000f)) return;

        Piece clickedPiece = hit.collider.GetComponent<Piece>();
        if (clickedPiece != null )
        {
            Debug.Log(clickedPiece.gameObject.name);
            return;
        }
    }
}
