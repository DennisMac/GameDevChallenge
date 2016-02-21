using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public static Tile selectedTile = null;
    [SerializeField]
    Renderer renderer;
    AudioSource audioSource;
    TextMesh textMesh;
    Vector3 homePosition = new Vector3(0, 15, -6);
    bool free = false;
    [SerializeField]
    float speed = 3.0f;
    Grid grid;
    GridCell gridCell = null;

    public bool WasOnBoard { get; set; }

    void Awake () {
        WasOnBoard = false;
        audioSource = GetComponent<AudioSource>();
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
            gridCell = grid.FindNearestFreeCell(this.transform.position);
            DropTile();
        }
    }

    public void PlaceOnBoard(Vector3 position)
    {
        Debug.Log("Free: " + free.ToString());
        if (!free || selectedTile == this)
        {
            free = false;
            Debug.Log("got here 2");
            gridCell = grid.FindNearestFreeCell(position);
            DropTile();
        }
    }

    private void DropTile()
    {
        if (gridCell != null)
        {
            transform.position = gridCell.transform.position + new Vector3(0f, .2f, 0f);
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            gridCell.Available = false;
            UnSelectThisTile();
            WasOnBoard = true;
        }
        else
        {
            SetFree();
            this.SelectThisTile();
        }
    }



    public void Dragging()
    {
        free = false;
        if (gridCell != null)
        {
            gridCell.Available = true;
        }
        if (selectedTile != null)
        {
            selectedTile.UnSelectThisTile();
        }
        this.SelectThisTile();
    }

    public void SetFree()
    {
        if (selectedTile != null)
        {
            selectedTile.UnSelectThisTile();
        }
        //this.SelectThisTile();
        free = true;
        if ((homePosition - transform.position).sqrMagnitude > 10 || WasOnBoard)
        {
            audioSource.Play();//swoosh back to home
        }
    }


    public void SelectThisTile()
    {
        //change color to selected
        selectedTile = this;
        renderer.material.color = new Color(.7f, .7f, .0f);
    }
    public void UnSelectThisTile()
    {
        //change color to unselected
        selectedTile = null;
        renderer.material.color = new Color(1f, 1f, 1f);
    }

}
