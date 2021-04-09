using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProjectile : Projectile
{
    protected override void MoveProjectile()
    {
        if (target != null)
        {
            MoveTowards(target.position);
        }
    }

    protected override void RotateProjectile()
    {
        if (target != null)
        {
            RotateTowards(target.position);
        }
    }

    protected override void DealDamage()
    {
        base.DealDamage();
        IDamageable idamage;
        idamage = target.GetComponent<IDamageable>();
        if (idamage != null)
        {
            TriggerDamageEffect(target.GetComponent<Enemy>());
            idamage.TakeDamage(Damage);
        }
        ObjectPool.Instance.UnSpawn(this.gameObject);
    }

    public override float GetTargetDistance()
    {
        float distance = ((Vector2)target.position - (Vector2)transform.position).magnitude;
        return distance;
    }
}
