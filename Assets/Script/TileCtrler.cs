using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCtrler : MonoBehaviour
{
    int x;
    int y;
    int z;
    [SerializeField] Material tileColor;
    [SerializeField] Material highLightMaterial;
    Transform surface;
    // Start is called before the first frame update
    void Start()
    {
        surface = transform.Find("Surface");
        x = Mathf.RoundToInt(this.transform.position.x);
        z = Mathf.RoundToInt(this.transform.position.z);
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        Debug.Log(this.gameObject.name);
        if (PieceCtrler.selectedPiece != null)
        {
            PieceCtrler.selectedPiece.Move(new Vector3(x, PieceCtrler.setY, z)); //マスに移動
            PieceCtrler.selectedPiece.isSelected = false; //選択を解除
            PieceCtrler.selectedPiece = null; // 選択を解除
        }
    }

    public void HighLightTile()
    {
        surface.GetComponent<Renderer>().material = highLightMaterial;
    }
    public void ResetTileColor()
    {
        surface.GetComponent<Renderer>().material = tileColor;
    }
}
