using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    [SerializeField]
    RisingLetter risingLetterPrefab;
    public static Tile selectedTile = null;
    [SerializeField]
    Renderer tileRenderer = null;
    AudioSource audioSource;
    TextMesh textMesh;
    Vector3 homePosition = new Vector3(0, 15, -6);
    bool free = false;
    [SerializeField]
    float speed = 3.0f;
    [SerializeField]
    Explosion explosion = null;
    Grid grid;
    GridCell gridCell = null;
    private bool bouncing = false;
    private float TimeToBounce = 0.2f;
    private float bounceTimeElapsed = 0f;
    private Vector3 boardPosition;
    private Quaternion boardRotation;



    #region properties

    private static Tile lastTilePlaced;
    public static Tile LastTilePlaced
    {
        get { return lastTilePlaced; }
        set
        {
            lastTilePlaced = value;
            Debug.Log("last tile placed" + lastTilePlaced.textMesh.text);
        }
    }
    #endregion
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
            transform.position = Vector3.Lerp(transform.position, homePosition, Time.deltaTime * speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90f, 0f, 0f), Time.deltaTime * speed);
        }
        else
        {
            if (bouncing)
            {
                bounceTimeElapsed += Time.deltaTime;

                if (bounceTimeElapsed > TimeToBounce)
                {
                    transform.position = boardPosition;
                    transform.rotation = boardRotation;
                    bouncing = false;
                }
                else
                {
                    if (bounceTimeElapsed < TimeToBounce / 2f)
                    {
                        transform.position += Vector3.up * Time.deltaTime*4;
                    }
                    else
                    {
                        transform.position -= Vector3.up * Time.deltaTime*4;
                    }
                }
            }
        }
    }

    public void Bounce()
    {
        boardPosition = transform.position;
        boardRotation = transform.rotation;
        bounceTimeElapsed = 0f;
        bouncing = true;
        Instantiate(explosion, transform.position, Quaternion.identity);
        RisingLetter  risingLetter = Instantiate(risingLetterPrefab, transform.position + new Vector3(-.2f,1f,.5f), Quaternion.Euler(90, 0, 0)) as RisingLetter;
        risingLetter.SetText(textMesh.text);
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
        if (!free || selectedTile == this)
        {
            free = false;
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
            gridCell.SetLetter(this.textMesh.text[0], this);
            UnSelectThisTile();
            grid.CheckForSpelledWords();
            WasOnBoard = true;
             
        }
        else
        {
            SetFree();
            grid.CheckForSpelledWords();
            this.SelectThisTile();
        }
    }



    public void Dragging()
    {
        free = false;
        if (gridCell != null)
        {
            gridCell.Available = true;
            gridCell.SetLetter('0', null);
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
        LastTilePlaced = this;
        tileRenderer.material.color = new Color(.7f, .7f, .0f);
    }
    public void UnSelectThisTile()
    {
        //change color to unselected
        selectedTile = null;
        tileRenderer.material.color = new Color(1f, 1f, 1f);
    }

}
