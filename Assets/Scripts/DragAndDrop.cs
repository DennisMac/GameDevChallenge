using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour
{
    AudioSource audioSource;
    private bool _mouseState;
    private GameObject targetObject;
    public Vector3 screenSpace;
    public Vector3 offset;
    public static bool dontAllowClicking = false;
    bool touchDown = false;
    bool touchUp = false;
    bool dragging = false;
    #region for iOs
    private Vector2 touchOrigin = -Vector2.one;
    #endregion

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        touchDown = false;
        touchUp = false;
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];
            touchOrigin = myTouch.position;
            if (myTouch.phase == TouchPhase.Began)
            {
                touchDown = true;
                dragging = true;
            }
            else if (myTouch.phase == TouchPhase.Ended)
            {
                dragging = false;
                touchUp = true;
            }
        }



        if (Input.GetMouseButtonDown(0) || touchDown)
        {
            RaycastHit hitInfo;
            targetObject = GetClickedObject(out hitInfo);
            if (targetObject != null)
            {
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
                        }
                        break;
                }
            }
        }

        if (Input.GetMouseButtonUp(0) || touchUp)
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
            Vector3 dragPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            if (dragging) dragPosition = new Vector3(touchOrigin.x, touchOrigin.y, screenSpace.z);

            var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            targetObject.transform.position = curPosition;
        }
    }


    GameObject GetClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Vector3 inputPosition;
        if (touchDown)
            inputPosition = new Vector3(touchOrigin.x, touchOrigin.y, 0);
        else
            inputPosition = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
            if (!(target.gameObject.tag == "Tile" || target.gameObject.tag == "GridCell"))
            {
                target = null;
            }
        }

        if (dontAllowClicking) return null;
        if (target != null)
        {
            return target;
        }
        else
            return null;
    }
}



