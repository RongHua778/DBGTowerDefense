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
    }

    public override void OnEndDrag()
    {
        base.OnEndDrag();
        if (!endDragSuccessful)
            return;
        //deal damage
        switch (_card.CardAsset.MagicType)
        {
            case MagicType.Target:
                if (_card.CardAsset.MagicDamage > 0)
                    DealAOEDamage();
                if (_card.CardAsset.BuffList.Count > 0)
                    ApplyEffect();
                break;
            case MagicType.NoTarget:
                break;
        }
        gameObject.HideCircle();
        _card.HideCard();
    }

    public override void OnStartDrag()
    {
        base.OnStartDrag();
        gameObject.DrawCircle(_card.CardAsset.MagicRange, 0.04f, StaticData.Instance.MagicRangeColor);
    }



    private void DealAOEDamage()
    {
        IDamageable idamage;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _card.CardAsset.MagicRange);
        foreach (var item in colliders)
        {
            idamage = item.GetComponent<IDamageable>();
            if (idamage != null)
            {
                idamage.TakeDamage(_card.CardAsset.MagicDamage);
            }
        }
    }

    private void ApplyEffect()
    {
        BuffableEntity affectable;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _card.CardAsset.MagicRange);
        foreach (var item in colliders)
        {
            affectable = item.GetComponent<BuffableEntity>();
            if (affectable != null)
            {
                affectable.ApplyEffects(_card.CardAsset.BuffList);
            }
        }
    }


    public override void UnsuccessfulDrag()
    {
        base.UnsuccessfulDrag();
        gameObject.HideCircle();
    }
}
