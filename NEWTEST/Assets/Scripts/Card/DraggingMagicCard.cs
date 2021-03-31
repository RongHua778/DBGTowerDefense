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
        if (endCell == null)
            HideMagicCircle();
        else
            DrawMagicCicle();
    }

    public override void OnEndDrag()
    {
        base.OnEndDrag();
        if (endCell == null)
        {
            UnsuccessfulDrag();
            return;
        }
        if (_card.CardAsset.MagicDamage > 0)
            DealAOEDamage();
        if (_card.CardAsset.TargetEffectList.Count > 0)
            ApplyEffect();
        gameObject.HideCircle();
        _card.HideCard();
    }

    public override void OnStartDrag()
    {
        base.OnStartDrag();
    }

    private void DrawMagicCicle()
    {
        gameObject.DrawCircle(_card.CardAsset.MagicRange, 0.04f, StaticData.Instance.MagicRangeColor);

    }
    private void HideMagicCircle()
    {
        gameObject.HideCircle();
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
        TargetEffectableEntity affectable;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _card.CardAsset.MagicRange);
        foreach (var item in colliders)
        {
            affectable = item.GetComponent<TargetEffectableEntity>();
            if (affectable != null)
            {
                affectable.ApplyEffects(_card.CardAsset.TargetEffectList);
            }
        }
    }


    public override void UnsuccessfulDrag()
    {
        base.UnsuccessfulDrag();
        HideMagicCircle();
    }
}
