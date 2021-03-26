using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBGTD.Cells;

public class DraggingTowerCard : DraggingActions
{
    private GameObject GhostTurret;
    public LayerMask GridLayer;


    private void Start()
    {
        GridLayer = LayerMask.GetMask("Grid");
    }
    public override void OnDraggingInUpdate()
    {
        base.OnDraggingInUpdate();
        if (GhostTurret != null)
            GhostTurret.transform.position = transform.position;
    }

    public override void OnEndDrag()
    {
        base.OnEndDrag();
        Vector2 pos;
        if (!WheatherEndAtCell(out pos))
        {
            ObjectPool.Instance.UnSpawn(GhostTurret);
            _card.ShowCard();
        }
        else
        {
            if (MoneySystem.CanOfferCost(_card.CardAsset.CardCost))
            {
                MoneySystem.ReduceMoney(_card.CardAsset.CardCost);
                GhostTurret.transform.position = pos;
                GhostTurret.GetComponent<Collider2D>().enabled = true;
            }
            else
            {
                ObjectPool.Instance.UnSpawn(GhostTurret);
                _card.ShowCard();
            }

        }
        GhostTurret = null;
    }

    public override void OnStartDrag()
    {
        base.OnStartDrag();
        GhostTurret = CreateGhostTower(_card.CardAsset.TurretPrefab);
    }

    private GameObject CreateGhostTower(GameObject prefab)
    {
        GameObject turret = ObjectPool.Instance.Spawn(prefab);
        turret.GetComponent<Turret>().ReadCardAsset(_card.CardAsset);
        turret.GetComponent<Collider2D>().enabled = false;
        return turret;
    }

    protected override bool DragSuccessful()
    {
        return true;
    }


}
