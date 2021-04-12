using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBGTD.Cells;

public class DraggingMagicCard : DraggingActions
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnDraggingInUpdate()
    {
        base.OnDraggingInUpdate();
        if (endSquare == null)
            HideMagicCircle();
        else
            DrawMagicCicle();
    }

    public override void OnEndDrag()
    {
        base.OnEndDrag();
        if (endDragSuccessful)
        {
            MoneySystem.ReduceMoney(_card.CardAsset.CardCost);
            if (_card.CardAsset.MagicAttack > 0 || _card.CardAsset.FinalEffectList.Count > 0)
                DealDamageAndBuff();
            ObjectPool.Instance.UnSpawn(_card.HandleNode);
        }
        else
        {
            UnsuccessfulDrag();
        }
        gameObject.HideCircle();
    }

    public override void OnStartDrag()
    {
        base.OnStartDrag();
    }

    private void DrawMagicCicle()
    {
        gameObject.DrawCircle(_card.CardAsset.MagicRange + StaticData.Instance.MagicRangeIntensify, 0.04f, StaticData.Instance.MagicRangeColor);

    }
    private void HideMagicCircle()
    {
        gameObject.HideCircle();
    }


    private void DealDamageAndBuff()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _card.CardAsset.MagicRange + StaticData.Instance.MagicRangeIntensify);
        foreach (var item in colliders)
        {
            IDamageable idamage = item.GetComponent<IDamageable>();
            if (idamage != null)
            {
                idamage.TakeDamage(_card.CardAsset.MagicAttack);
            }
            BuffableEnemy affectable = item.GetComponent<BuffableEnemy>();
            if (affectable != null)
            {
                affectable.ApplyEffects(_card.CardAsset.FinalEffectList);
            }


        }
    }



    public override void UnsuccessfulDrag()
    {
        base.UnsuccessfulDrag();
        HideMagicCircle();
    }
}
