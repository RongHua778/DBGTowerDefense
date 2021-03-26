using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBGTD.Cells;

public class DraggingMagicCard : DraggingActions
{
    private MagicCircle _magicCircle;
    public LayerMask GridLayer;

    protected override void Awake()
    {
        base.Awake();
        GameObject magicCicleObj = Instantiate(Resources.Load<GameObject>("Prefabs/MagicCircle"));
        magicCicleObj.transform.SetParent(this.transform);
    }

    public override void OnDraggingInUpdate()
    {
        base.OnDraggingInUpdate();
    }

    public override void OnEndDrag()
    {
        base.OnEndDrag();
        Vector2 pos;
        _magicCircle.Hide();
        //if (!WheatherEndAtCell(out pos))
        //{
        //    ObjectPool.Instance.UnSpawn(GhostTurret);
        //    _card.ShowCard();
        //}
        //else
        //{
        //    if (MoneySystem.CanOfferCost(_card.CardAsset.CardCost))
        //    {
        //        MoneySystem.ReduceMoney(_card.CardAsset.CardCost);
        //        GhostTurret.transform.position = pos;
        //        GhostTurret.GetComponent<Collider2D>().enabled = true;
        //    }
        //    else
        //    {
        //        ObjectPool.Instance.UnSpawn(GhostTurret);
        //        _card.ShowCard();
        //    }

        //}
        //GhostTurret = null;
    }

    public override void OnStartDrag()
    {
        base.OnStartDrag();
        ShowMagicCircle();
    }

    private void ShowMagicCircle()
    {
        _magicCircle.Show();
    }

    protected override bool DragSuccessful()
    {
        return true;
    }


}
