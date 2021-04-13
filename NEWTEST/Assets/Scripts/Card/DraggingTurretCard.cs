using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBGTD.Cells;

public class DraggingTurretCard : DraggingActions
{
    private GameObject GhostTurret;

    public override void OnDraggingInUpdate()
    {
        base.OnDraggingInUpdate();
        if (GhostTurret == null)
            return;
        if (endSquare != null)
        {
            GhostTurret.SetActive(true);
            GhostTurret.transform.position = endSquare.GetPosofCell();
        }
        else
        {
            GhostTurret.SetActive(false);
        }
    }

    public override void OnEndDrag()
    {
        base.OnEndDrag();
        if (endSquare != null && endSquare.IsRoad)
        {
            UnsuccessfulDrag();
            return;
        }
        if (endDragSuccessful)
        {
            MoneySystem.ReduceMoney(_card.CardAsset.CardCost);
            GhostTurret.transform.position = endSquare.GetPosofCell();
            GhostTurret.GetComponent<Turret>().LandTurret(endSquare);
            GameEvents.Instance.RemoveCard(_card.CardAsset);//��ʱ�ӿ����Ƴ�������
            _card.CardAsset = null;
            ObjectPool.Instance.UnSpawn(_card.gameObject);
        }
        else
        {
            UnsuccessfulDrag();
        }
        GhostTurret = null;
        Square.PreviewingTurret = null;
    }

    public override void OnStartDrag()
    {
        base.OnStartDrag();
        GhostTurret = CreateGhostTower(_card.CardAsset.TurretPrefab);
        Square.PreviewingTurret = GhostTurret.GetComponent<Turret>();
    }

    private GameObject CreateGhostTower(GameObject prefab)
    {
        GameObject turret = ObjectPool.Instance.Spawn(prefab);
        turret.GetComponent<Turret>().ReadCardAsset(_card.CardAsset);
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
