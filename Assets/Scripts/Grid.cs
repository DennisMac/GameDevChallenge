using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

    public int width = 10;
    public int height = 10;
    public float cellSpacing = 1.0f;
    public GridCell gridCell;
    private GridCell[] cells;
    public float minRange = 20f;


    void Start () {
        cells = new GridCell[width * height];
        float xOffset = ((float)width-1) * cellSpacing / 2f; 
        float zOffset = ((float)height-1) * cellSpacing / 2f;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                cells[(i * (height)) + j] = Instantiate(gridCell, new Vector3(i * cellSpacing - xOffset, 0, j * cellSpacing - zOffset), Quaternion.EulerAngles(Mathf.PI / 2f, 0, 0)) as GridCell;
            }
        }

        cells[44].Available = true;
	}


    public GridCell FindNearestFreeCell(Vector3 pos)
    {
        int i = 0;
        GridCell nearestCell = null;
        float minDistance = 9999999f;





        foreach (GridCell c in cells)//dmc todo: do this proper
        {
            Debug.Log("GridCell" + i++.ToString());
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

        
        return (minDistance < minRange) ? nearestCell : null;
    }


}

