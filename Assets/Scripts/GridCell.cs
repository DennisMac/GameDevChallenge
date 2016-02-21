using UnityEngine;
using System.Collections;

public class GridCell : MonoBehaviour {


    private bool isAvailable = true;
    [SerializeField]
    int row = 0;
    [SerializeField]
    public int column = 0;
    [SerializeField]
    Renderer renderer;

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
            if (isAvailable)
            {
                renderer.material.color = new Color(.0f, .0f, .0f);
            }
            else
            {
                renderer.material.color = new Color(.7f, .7f, .0f);
            }
        }
    }

}
