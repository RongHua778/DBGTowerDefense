using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBGTD.Cells;

public class DraggingTowerCard : DraggingActions
{
    private GameObject GhostTurret;

    public override void OnDraggingInUpdate()
    {
        base.OnDraggingInUpdate();
        if (GhostTurret != null)
            GhostTurret.transform.position = transform.position;
    }

    public override void OnEndDrag()
    {
        base.OnEndDrag();
        if (!endDragSuccessful)
            return;
        if (endCell.IsRoad)
        {
            UnsuccessfulDrag();
            return;
        }
        if (MoneySystem.CanOfferCost(_card.CardAsset.CardCost))
        {
            MoneySystem.ReduceMoney(_card.CardAsset.CardCost);
            GhostTurret.transform.position = endCell.GetPosofCell();
            GhostTurret.GetComponent<Collider2D>().enabled = true;
            Turret turret = GhostTurret.GetComponent<Turret>();
            turret.LandTurret(endCell);
        }
        else
        {
            UnsuccessfulDrag();
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
        CardSO tempCardAsset = Instantiate(_card.CardAsset);
        turret.GetComponent<Turret>().ReadCardAsset(tempCardAsset);
        turret.GetComponent<Collider2D>().enabled = false;
        return turret;
    }


    public override void UnsuccessfulDrag()
    {
        base.UnsuccessfulDrag();
        if (GhostTurret != null)
            ObjectPool.Instance.UnSpawn(GhostTurret);
       
    }
}
