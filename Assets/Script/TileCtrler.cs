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
        Debug.Log(surface.gameObject.name+"に入りました");
        x = Mathf.RoundToInt(this.transform.position.x);
        z = Mathf.RoundToInt(this.transform.position.z);

        this.transform.position = new Vector3(x, y, z);
    }



    // Update is called once per frame
    void OnMouseDown()
    {
        //Debug.Log(this.gameObject);
        if (BoardManager.boardGridInfo[x, z] != null)
        {
            Debug.Log($"{this.gameObject.name}: {BoardManager.boardGridInfo[x, z].transform}");
        }
        else
        {
            Debug.Log($"{this.gameObject.name}: Null");
        }

        //駒が何も選択されていなければ、処理を中断。
        if (PieceCtrler.selectedPiece == null) return;

        PieceCtrler piece = GetComponent<PieceCtrler>();
        if (PieceCtrler.selectedPiece.isInhand)
        {
            if (HandManager.instance.TryDrop(PieceCtrler.selectedPiece, x, z))
            {
                //GameManager.Instance.TurnChange(piece);
                piece.isSelected = false;
                PieceCtrler.selectedPiece = null;
                return;
            }
            else
            {
                Debug.LogWarning("このマスには打てません");
            }
        }

        // 選択した駒を浮かす
        Vector3 target = new Vector3(x, PieceCtrler.setY, z);
        // クリックしたますが移動可能なら
        if (PieceCtrler.selectedPiece.TryMoveTo(target))
        {
            //GameManager.Instance.TurnChange(piece);
            PieceCtrler.selectedPiece.Move(new Vector3(x, PieceCtrler.setY, z)); //クリックしたマスに移動
            UI.instance.TurnChangetext();
            return;
        }
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
