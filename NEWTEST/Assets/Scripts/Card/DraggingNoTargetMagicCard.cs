using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingNoTargetMagicCard : DraggingActions
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnDraggingInUpdate()
    {
        base.OnDraggingInUpdate();
        if (endCell == null)
            StaticData.Instance.NoTargetEffect.SetActive(false);
        else
            StaticData.Instance.NoTargetEffect.SetActive(true);
    }

    public override void OnEndDrag()
    {
        base.OnEndDrag();
        StaticData.Instance.NoTargetEffect.SetActive(false);
        if (endDragSuccessful)
        {
            MoneySystem.ReduceMoney(_card.CardAsset.CardCost);
            LevelManager.Instance.ApplyNoTargetEffects(_card.CardAsset.NoTargetEffectList);
        }
        else
        {
            UnsuccessfulDrag();
        }

    }

    public override void OnStartDrag()
    {
        base.OnStartDrag();
    }

    public override void UnsuccessfulDrag()
    {
        base.UnsuccessfulDrag();
    }

}
