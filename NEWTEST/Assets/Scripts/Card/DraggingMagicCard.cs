using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DBGTD.Cells;

public class DraggingMagicCard : DraggingActions
{
    [SerializeField] private MagicCircle _magicCircle;

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
                else
                    ApplyEffect();
                break;
            case MagicType.NoTarget:
                break;
        }
        _magicCircle.Hide();
        _card.HideCard();
    }

    public override void OnStartDrag()
    {
        base.OnStartDrag();
        ShowMagicCircle();
    }

    private void ShowMagicCircle()
    {
        _magicCircle.Show();
        _magicCircle.SetCircleRange(_card.CardAsset.MagicRange);
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
        IAffectable affectable;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _card.CardAsset.MagicRange);
        foreach (var item in colliders)
        {
            affectable = item.GetComponent<IAffectable>();
            if (affectable != null)
            {
                affectable.Affect(_card.CardAsset.EffectList);
            }
        }
    }


    public override void UnsuccessfulDrag()
    {
        base.UnsuccessfulDrag();
        _magicCircle.Hide();
        
    }
}
