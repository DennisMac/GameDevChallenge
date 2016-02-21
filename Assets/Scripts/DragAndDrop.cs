using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour
{
    AudioSource audioSource;
    private bool _mouseState;
    private GameObject targetObject;
    public Vector3 screenSpace;
    public Vector3 offset;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            targetObject = GetClickedObject(out hitInfo);
            if (targetObject != null)
            {
                Debug.Log("hit target 2:" + targetObject.gameObject.tag.ToString());
                switch (targetObject.gameObject.tag)
                {
                    case "Tile":
                        Tile target = (targetObject.gameObject.GetComponent<Tile>());
                        _mouseState = true;
                        screenSpace = Camera.main.WorldToScreenPoint(target.transform.position);
                        offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
                        target.Dragging();
                        audioSource.Play();
                        break;
                    case "GridCell":
                        GridCell targetCell = (targetObject.gameObject.GetComponent<GridCell>());
                        if (Tile.selectedTile != null)
                        {
                            screenSpace = Camera.main.WorldToScreenPoint(targetCell.transform.position);
                            Tile.selectedTile.PlaceOnBoard(targetCell.transform.position);
                            Debug.Log("Fling to board vector" + targetCell.transform.position.ToString());
                        }
                        break;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _mouseState = false;
            if (targetObject != null)
            {
                switch (targetObject.gameObject.tag)
                {
                    case "Tile":
                        targetObject.gameObject.GetComponent<Tile>().PlaceOnBoard();
                        break;
                }
            }
        }

        if (_mouseState)
        {
            var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            targetObject.transform.position = curPosition;
        }
    }


    GameObject GetClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
            Debug.Log("hit target:" + target.gameObject.tag.ToString());
            if (!(target.gameObject.tag == "Tile" || target.gameObject.tag == "GridCell"))
            {
                target = null;
            }
        }
        if (target != null)
        {
            return target;
        }
        else
            return null;
    }
}



