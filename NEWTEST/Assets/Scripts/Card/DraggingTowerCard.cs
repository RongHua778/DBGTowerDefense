using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBGTD.Cells;

public class DraggingTowerCard : DraggingActions
{
    private GameObject GhostTurret;
    public LayerMask GridLayer;

    public override void OnDraggingInUpdate()
    {
        Time.timeScale = 0;
        if (GhostTurret != null)
            GhostTurret.transform.position = transform.position;
    }

    public override void OnEndDrag()
    {
        Vector2 pos;
        if (!WheatherEndAtCell(out pos))
        {
            ObjectPool.Instance.UnSpawn(GhostTurret);
            _card.ShowCard();
        }
        else
        {
            GhostTurret.transform.position = pos;
            GhostTurret.GetComponent<Collider2D>().enabled = true;
        }
        GhostTurret = null;
        Time.timeScale = 1;
    }

    public override void OnStartDrag()
    {
    
        GhostTurret = CreateGhostTower(_card.CardAsset.TurretPrefab);
        _card.HideCard();
    }

    private GameObject CreateGhostTower(GameObject prefab)
    {
        GameObject turret = ObjectPool.Instance.Spawn(prefab);
        turret.GetComponent<Turret>().SetAttribute(_card.CardAsset);
        turret.GetComponent<Collider2D>().enabled = false;
        return turret;
    }

    protected override bool DragSuccessful()
    {
        return true;
    }


}
