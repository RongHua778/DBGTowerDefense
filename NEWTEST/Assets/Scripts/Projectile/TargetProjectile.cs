using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProjectile : Projectile
{
    protected override void MoveProjectile()
    {
        if (_enemyTarget != null)
        {
            MoveTowards(_enemyTarget.transform.position);
        }
    }

    protected override void RotateProjectile()
    {
        if (_enemyTarget != null)
        {
            RotateTowards(_enemyTarget.transform.position);
        }
    }

    protected override void DealDamage()
    {
        IDamageable idamage;
        idamage = _enemyTarget.GetComponent<IDamageable>();
        if (idamage != null)
        {
            TriggerDamageEffect(_enemyTarget);
            idamage.TakeDamage(Damage);
        }
        ObjectPool.Instance.UnSpawn(this.gameObject);
    }

    public override float GetTargetDistance()
    {
        float distance = ((Vector2)_enemyTarget.transform.position - (Vector2)transform.position).magnitude;
        return distance;
    }
}
