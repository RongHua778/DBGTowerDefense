using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundProjectile : Projectile
{

    protected override void MoveProjectile()
    {
        MoveTowards(_targetGroundPos);
    }

    protected override void RotateProjectile()
    {
        RotateTowards(_targetGroundPos);
    }

    protected override void DealDamage()
    {
        base.DealDamage();
        IDamageable idamage;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _sputteringRange);
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

    public override float GetTargetDistance()
    {
        float distance = (_targetGroundPos - (Vector2)transform.position).magnitude;
        return distance;
    }

}
