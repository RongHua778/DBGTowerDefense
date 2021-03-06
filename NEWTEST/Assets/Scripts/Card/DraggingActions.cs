using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBGTD.Cells;

public abstract class DraggingActions : MonoBehaviour
{
    public LayerMask GridLayer;
    private bool dragging = false;
    private Vector3 pointerOffset;
    private float zDisplacement;
    protected Square endSquare;
    protected bool endDragSuccessful = true;
    protected Camera _mainCam;

    protected Card _card;

    private static DraggingActions _draggingThis;

    public static DraggingActions DraggingThis { get => _draggingThis; }

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
    protected virtual void Awake()
    {
        _mainCam = Camera.main;
        _card = GetComponent<Card>();
    }


    public virtual void OnStartDrag()
    {
        _card.HideCard();
    }

    public virtual void OnEndDrag()
    {
        StaticData.Instance.GameSpeedResume();
        if (endSquare == null)
        {
            UnsuccessfulDrag();
        }
        else if (!MoneySystem.CanOfferCost(_card.CardAsset.CardCost))
        {
            UnsuccessfulDrag();
        }
        else
        {
            endDragSuccessful = true;
        }
    }

    public virtual void OnDraggingInUpdate()
    {
        StaticData.Instance.GameSlowDown();
        WheatherEndAtCell(out endSquare);
        if (endSquare == null)
            _card.ShowCard();
        else
            _card.HideCard();
    }


    public virtual void UnsuccessfulDrag()
    {
        dragging = false;
        StaticData.Instance.GameSpeedResume();
        _card.ShowCard();
        endDragSuccessful = false;
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

    protected Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        return _mainCam.ScreenToWorldPoint(screenMousePos);
    }

    protected bool WheatherEndAtCell(out Square inSquare)
    {
        inSquare = null;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(MouseInWorldCoords(), Vector3.forward, Mathf.Infinity, GridLayer);
        if (hit.collider == null)
            return false;
        Square square = hit.collider.GetComponent<Square>();
        if (square != null)
        {
            inSquare = square;
            return true;
        }
        return false;

        //inSquare = null;
        //if (Cell.HighLightedCell != null)
        //{
        //    inSquare = Cell.HighLightedCell as Square;
        //    return true;
        //}
        //return false;
    }


}
