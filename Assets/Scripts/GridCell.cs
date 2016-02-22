using UnityEngine;
using System.Collections;

public class GridCell : MonoBehaviour {


    private bool isAvailable = true;
    [SerializeField]
    Renderer cellRenderer;
    private char letter;

    public Tile Tile { get; set; }

    public GridCell()
    {
        letter = '0';
    }

    public bool Available
    {
        get { return isAvailable; }
        set
        {
            isAvailable = value;
            if (isAvailable)
            {
                cellRenderer.material.color = new Color(.0f, .0f, .0f);
            }
            else
            {
                cellRenderer.material.color = new Color(.7f, .7f, .0f);
            }
        }
    }

    public void SetLetter(char letter, Tile tile)
    {
        this.letter = letter;
        this.Tile = tile;
    }

    public char GetLetter()
    {
        return letter;
    }
}
