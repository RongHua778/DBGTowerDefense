using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundTargetProjectile : Projectile
{
    
    protected override void Update()
    {
        MoveProjectile(_targetGroundPos);
    }

    protected override void DealDamage()
    {
        IDamageable idamage;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explodeRange);
        foreach (var item in colliders)
        {
            idamage = item.GetComponent<IDamageable>();
            if (idamage != null)
            {
                idamage.TakeDamage(Damage);
            }
        }
        ObjectPool.Instance.UnSpawn(this.gameObject);
    }
}
