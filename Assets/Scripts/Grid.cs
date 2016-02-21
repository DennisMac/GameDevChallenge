using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{

    AudioSource audioSource;
    private int width = 10;
    private int height = 10;
    private float cellSpacing = 1.0f;
    [SerializeField]
    private GridCell gridCell;
    private GridCell[] cells;
    [SerializeField]
    private float minRange = 99999999f;

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
            Debug.Log(xCoord.ToString() + ", " + zCoord.ToString());
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
}




