using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    [SerializeField]
    Manager manager;
    AudioSource audioSource;
    private int width = 10;
    private int height = 10;
    private float cellSpacing = 1.0f;
    [SerializeField]
    private GridCell gridCell;
    private GridCell[] cells;
    [SerializeField]
    private float minRange = 99999999f;

    List<Tile> tilesToHighLight = new List<Tile>();

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void Start()
    {
        //Place the cells in a grid pattern
        cells = new GridCell[width * height];
        float xOffset = ((float)width - 1) * cellSpacing / 2f;
        float zOffset = ((float)height - 1) * cellSpacing / 2f;

        for (int j = 0; j < width; j++)
        {
            for (int i = 0; i < height; i++)
            {
                cells[(i * (height)) + j] = Instantiate(gridCell, new Vector3(i * cellSpacing - xOffset, 0, j * cellSpacing - zOffset), Quaternion.EulerAngles(Mathf.PI / 2f, 0, 0)) as GridCell;
                cells[(i * (height)) + j].Available = true;
            }
        }
    }



    /// <summary>
    /// Find the cell directly under the mouse. If that cell is not available do a more costly method to find the nearest one.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public GridCell FindNearestFreeCell(Vector3 pos)
    {
        int i = 0;
        GridCell nearestCell = null;
        float minDistance = 9999999f;

        float xCoord = pos.x + (width / 2f);
        float zCoord = pos.z + (height / 2f);

        xCoord = Mathf.Round(xCoord - 0.5f);
        zCoord = Mathf.Round(zCoord - 0.5f);

        if (xCoord >= 0 && zCoord >= 0 && xCoord < width && zCoord < height)
        {
            nearestCell = cells[(int)(xCoord * (width) + zCoord)];
            if (nearestCell.Available)
            {
                audioSource.Play();
                return nearestCell;
            }
        }

        //not positioned directly over a cell so do it the inefficient way
        foreach (GridCell c in cells)//dmc todo: do this proper
        {
            if (c.Available)
            {
                float distance = (pos - c.transform.position).sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCell = c;
                }
            }
        }

        if (minDistance < minRange)
        {
            audioSource.Play();
            return nearestCell;
        }
        else
        {   //Only provide the nearest cell if reasonably close to it.
            return null;
        }
    }



    
    public void CheckForSpelledWords()
    {
        List<string> foundWords = new List<string>();
        tilesToHighLight.Clear();
        foreach (string s in manager.Words())
        {
            if (FindWord(s))
            {
                foundWords.Add(s);
                foreach (Tile t in tilesToHighLight)
                {
                    t.Bounce();
                }
            }
            else
            {
                tilesToHighLight.Clear();
            }
        }
        manager.FillWordsSpelled(foundWords);
    }


    /// <summary>
    /// Find the first letter then call FindRestOfWord() in each direction
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    private bool FindWord(string word)
    {
        Tile firstLetterTile = null;
        bool found = false;
        for (int j = 0; j < width; j++)
        {
            for (int i = 0; i < height; i++)
            {
                if (word[0] == cells[(i * (height)) + j].GetLetter())
                {
                    firstLetterTile = cells[(i * (height)) + j].Tile;
                    tilesToHighLight.Add(firstLetterTile);
                    for (int dir = 0; dir < 8; dir++)
                    {
                        found = FindRestOfWord(word, i, j, (Direction)dir, 1);
                        if (found)
                            return true;
                    }
                    tilesToHighLight.Remove(firstLetterTile);
                }
            }
        }
        
        return false;
    }


    enum Direction { n, ne, e, se, s, sw, w, nw};

    private bool FindRestOfWord(string word, int i, int j, Direction dir, int index)
    {
        
        //if we get here, the first letter of word was found at i, j 
        //So start at i,j and look around it for the second letter.
        switch (dir)
        {
            case Direction.n:
                j += 1; if (j > height) return false;
                break;
            case Direction.ne:
                j += 1; if (j > height) return false;
                i += 1; if (i > width) return false;
                break;
            case Direction.e:
                i += 1; if (i > width) return false;
                break;
            case Direction.se:
                j -= 1; if (j < 0) return false;
                i += 1; if (i > width) return false;
                break;
            case Direction.s:
                j -= 1; if (j < 0) return false;
                break;
            case Direction.sw:
                i -= 1; if (i < 0) return false;
                j -= 1; if (j < 0) return false;
                break;
            case Direction.w:
                i -= 1; if (i < 0) return false;
                break;
            case Direction.nw:
                i -= 1; if (i < 0) return false;
                j += 1; if (j > height) return false;
                break;
        }

        //were on the board so look
        {
            if (word[index] != cells[(i * (height)) + j].GetLetter())
            {
                return false;
            }
            else
            {
                tilesToHighLight.Add(cells[(i * (height)) + j].Tile);
                if (index >= word.Length - 1)
                {
                    return true;
                }
                else
                {
                    return FindRestOfWord(word, i, j, (Direction)dir, index + 1);
                }
            }
        }
        return false;
    }

}




