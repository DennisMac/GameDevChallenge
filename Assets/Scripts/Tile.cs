using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    TextMesh textMesh;
    Vector3 homePosition = new Vector3(0, 15, 0);
    bool free = false;
    float speed = 3.0f;
    public Grid grid;

    void Awake () {
        textMesh = GetComponentInChildren<TextMesh>();
        grid = FindObjectOfType<Grid>(); //dmc todo: make grid a singleton
        transform.position = homePosition;
	}
	
	void Update ()
    {
        if (free)
        {
            transform.position = Vector3.Lerp(transform.position, homePosition, Time.deltaTime* speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90f,0f,0f), Time.deltaTime * speed);
        }
    }

    public void SetLetter(char letter)
    {
        textMesh.text = letter.ToString();
    }


    public void SetHomePosition(Vector3 pos)
    {
        homePosition = pos;
    }

    public void PlaceOnBoard()
    {
        if (!free)
        {
            GridCell cell  = grid.FindNearestFreeCell(this.transform.position);
            if (cell != null)
            {
                transform.position = cell.transform.position + new Vector3(0f, .2f, 0f);
                transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            }
            else
                free = true;
        }
    }

    public void Dragging()
    {
        free = false;
    }

    public void SetFree()
    {
        free = true;

    }
}
