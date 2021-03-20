using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyTargetProjectile : Projectile
{
    
    protected override void Update()
    {
        if (_enemyTarget != null)
        {
            MoveProjectile(_enemyTarget.transform.position);
        }
    }

    protected override void DealDamage()
    {
        IDamageable idamage;
        idamage = _enemyTarget.GetComponent<IDamageable>();
        if (idamage != null)
        {
            idamage.TakeDamage(Damage);
        }
        ObjectPool.Instance.UnSpawn(this.gameObject);
    }
}
