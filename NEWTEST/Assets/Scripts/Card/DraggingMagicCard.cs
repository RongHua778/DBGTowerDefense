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
        if (_card.CardAsset.MagicDamage > 0||_card.CardAsset.TurretBuffList.Count > 0 || _card.CardAsset.EnemyBuffList.Count > 0)
            DealDamageOrEffect();
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


    private void DealDamageOrEffect()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _card.CardAsset.MagicRange);
        foreach (var item in colliders)
        {
            IDamageable idamage = item.GetComponent<IDamageable>();
            if (idamage != null)
            {
                idamage.TakeDamage(_card.CardAsset.MagicDamage);
            }
            BuffableEnemy affectable = item.GetComponent<BuffableEnemy>();
            if (affectable != null)
            {
                affectable.ApplyEffects(_card.CardAsset.EnemyBuffList);
            }
            BuffableTurret affectable2 = item.GetComponent<BuffableTurret>();
            if (affectable2 != null)
            {
                affectable2.ApplyEffects(_card.CardAsset.TurretBuffList);
            }

        }
    }

    private void ApplyEffect()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _card.CardAsset.MagicRange);
        foreach (var item in colliders)
        {
            BuffableEnemy affectable = item.GetComponent<BuffableEnemy>();
            if (affectable != null)
            {
                affectable.ApplyEffects(_card.CardAsset.EnemyBuffList);
            }
            BuffableTurret affectable2 = item.GetComponent<BuffableTurret>();
            if (affectable2 != null)
            {
                affectable2.ApplyEffects(_card.CardAsset.TurretBuffList);
            }
        }
    }


    public override void UnsuccessfulDrag()
    {
        base.UnsuccessfulDrag();
        HideMagicCircle();
    }
}
