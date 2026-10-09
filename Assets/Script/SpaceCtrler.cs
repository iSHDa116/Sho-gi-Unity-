using UnityEngine;

public class SpaceCtrler : MonoBehaviour 
{
    public int x, z;

    Renderer rend;
    [SerializeField] Material normalMaterial; 
    [SerializeField] Material highlightMaterial;

    public void Init(int x, int z)
    {
        this.x = x;
        this.z = z;
        rend = GetComponent<Renderer>();
    }

    public void HighLightSpace()
    {
        rend.material = highlightMaterial;
    }

    public void ResetColor()
    {
        rend.material = normalMaterial;
    }
}