using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCtrler : MonoBehaviour
{
    int x;
    float y = 0;
    int z;
    [SerializeField] Material tileColor;
    [SerializeField] Material highLightMaterial;
    Transform surface;
    // Start is called before the first frame update
    void Start()
    {
        surface = transform.Find("Surface");
        Debug.Log(surface.gameObject.name+"入りました");
        x = Mathf.RoundToInt(this.transform.position.x);
        z = Mathf.RoundToInt(this.transform.position.z);

        this.transform.position = new Vector3(x, y, z);
    }



    // Update is called once per frame
    void OnMouseDown()
    {
        Debug.Log(this.gameObject.name);
        if (PieceCtrler.selectedPiece != null)
        {
            Vector3 target = new Vector3(x, PieceCtrler.setY, z);

            if (PieceCtrler.selectedPiece.TryMoveTo(target))
            {
                PieceCtrler.selectedPiece.Move(new Vector3(x, PieceCtrler.setY, z)); //マスに移動
            }

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
