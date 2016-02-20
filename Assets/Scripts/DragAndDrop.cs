using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour
{

    private bool _mouseState;
    private Tile target;
    private Tile targetTile;
    public Vector3 screenSpace;
    public Vector3 offset;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            target = GetClickedObject(out hitInfo);
            if (target != null)
            {
                _mouseState = true;
                screenSpace = Camera.main.WorldToScreenPoint(target.transform.position);
                offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
                target.Dragging();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _mouseState = false;
            if(target != null)
            {//release the tile
                target.PlaceOnBoard();
            }
        }

        if (_mouseState)
        {
            var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            target.transform.position = curPosition;
        }
    }


    Tile GetClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
            if (target.gameObject.tag != "Tile")
                target = null;
        }
        if (target != null)
            return (target.gameObject.GetComponent<Tile>());
        else
            return null;
    }
}



