using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveProjectile : Projectile
{
    private const float rangeAdjust = 1.1f;//调整爆炸的范围略大一点
    protected override void MoveProjectile()
    {
        MoveTowards(this.transform.position);
    }

    protected override void RotateProjectile()
    {
        RotateTowards(this.transform.position);
    }
    protected override void DealDamage()
    {
        _sputteringRange = _turretOwner.AttackRange * rangeAdjust;
        base.DealDamage();
        IDamageable idamage;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f * _sputteringRange);
        foreach (var item in colliders)
        {
            idamage = item.GetComponent<IDamageable>();
            if (idamage != null)
            {
                TriggerDamageEffect(item.GetComponent<Enemy>());
                idamage.TakeDamage(Damage);
            }
        }
        ObjectPool.Instance.UnSpawn(this.gameObject);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
    }

}
