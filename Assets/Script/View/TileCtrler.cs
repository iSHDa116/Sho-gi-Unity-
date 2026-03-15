using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCtrler : MonoBehaviour
{
    public int x;
    public float y = 0;
    int z;
    [SerializeField] Material tileColor;
    [SerializeField] Material highLightMaterial;
    Transform surface;
    // Start is called before the first frame update
    void Start()
    {
        surface = transform.Find("Surface");
        Debug.Log(surface.gameObject.name+"に入りました");
        x = Mathf.RoundToInt(this.transform.position.x);
        z = Mathf.RoundToInt(this.transform.position.z);

        this.transform.position = new Vector3(x, y, z);
    }


    public void HighLightTile()
    {
        surface.GetComponent<Renderer>().material = highLightMaterial;
    }
    public void ResetColor()
    {
        surface.GetComponent<Renderer>().material = tileColor;
    }
}
