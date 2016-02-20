using UnityEngine;
using System.Collections;

public class GridCell : MonoBehaviour {


    private bool isAvailable = false;
    public int row = 0;
    public int column = 0;
    public Renderer renderer;

    public GridCell(int i, int j)
    {
        row = 1;
        column = j;
    }

    public bool Available
    {
        get { return isAvailable; }
        set
        {
            isAvailable = value;
            //Material mat = this.gameObject.GetComponent<Material>();
            if (isAvailable)
            {
                renderer.material.color = new Color(.7f,.7f,.0f);
            }
            else
            {
                renderer.material.color = new Color(.25f, .25f, .5f);
            }
        }
    }

}
