using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBGTD.Cells;

public abstract class DraggingActions : MonoBehaviour
{
    private bool dragging = false;
    private Vector3 pointerOffset;
    private float zDisplacement;

    protected Camera _mainCam;

    protected Card _card;

    private static DraggingActions _draggingThis;

    public static DraggingActions DraggingThis { get => _draggingThis; }
    public abstract void OnStartDrag();

    public abstract void OnEndDrag();

    public abstract void OnDraggingInUpdate();

    public virtual bool CanDrag
    {
        get { return true; }
    }

    protected abstract bool DragSuccessful();

    private void Awake()
    {
        _mainCam = Camera.main;
        _card = GetComponent<Card>();
    }

    private void OnMouseDown()
    {
        if (CanDrag)
        {
            dragging = true;
            HoverPreview.PreviewsAllowed = false;
            _draggingThis = this;
            OnStartDrag();
            zDisplacement = -_mainCam.transform.position.z + transform.position.z;
        }
    }

    private void Update()
    {
        if (dragging)
        {
            Vector3 mousePos = MouseInWorldCoords();
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z) + pointerOffset;
            OnDraggingInUpdate();
        }
        else
        {
            if (_card.HandleNode != null)
            {
                transform.position = _card.HandleNode.transform.position;
            }
        }
    }

    private void OnMouseUp()
    {
        if (dragging)
        {
            dragging = false;
            HoverPreview.PreviewsAllowed = true;
            _draggingThis = null;
            OnEndDrag();
        }
    }
    protected Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        screenMousePos.z = zDisplacement;
        return _mainCam.ScreenToWorldPoint(screenMousePos);
    }

    protected bool WheatherEndAtCell(out Vector2 pos)
    {
        pos = Vector2.zero;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(MouseInWorldCoords(), Vector2.zero, Mathf.Infinity);
        if (hit.collider == null)
            return false;
        Cell cell = hit.collider.GetComponent<Cell>();
        if (cell != null)
        {
            pos = cell.GetPosofCell();
            return true;
        }
        return false;
    }


}
