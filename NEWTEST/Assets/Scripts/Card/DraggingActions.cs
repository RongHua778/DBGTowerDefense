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
    public virtual void OnStartDrag()
    {
        _card.HideCard();
    }

    public virtual void OnEndDrag()
    {
        LevelManager.Instance.GameSpeedControl(1);
    }

    public virtual void OnDraggingInUpdate()
    {
        LevelManager.Instance.GameSpeedControl(0);
    }

    public virtual bool CanDrag
    {
        get 
        {
            if (MoneySystem.CanOfferCost(_card.CardAsset.CardCost))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    protected abstract bool DragSuccessful();

    protected virtual void Awake()
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
        else
        {
            Debug.LogWarning("Not Enough Money");
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
