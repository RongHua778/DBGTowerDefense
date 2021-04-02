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
        if (GhostTurret == null)
            return;
        if (endCell!=null)
        {
            GhostTurret.SetActive(true);
            GhostTurret.transform.position = endCell.GetPosofCell();
            GhostTurret.GetComponent<Turret>().ShowRange();
        }
        else
        {
            GhostTurret.SetActive(false);
        }
    }

    public override void OnEndDrag()
    {
        base.OnEndDrag();
        if (endCell.IsRoad)
        {
            UnsuccessfulDrag();
            return;
        }
        if (endDragSuccessful)
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
