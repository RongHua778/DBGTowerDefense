using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ProjectileType
//{
//    TargetEnemy,
//    TargetGround
//}
public abstract class Projectile : ReusableObject
{
   // public ProjectileType projectileType = ProjectileType.TargetEnemy;

    [SerializeField] protected float moveSpeed = 10f;

    protected float explodeRange = 2f;

    protected float minDistanceToDealDamage = .1f;

    protected float damage;
    protected float Damage { get => damage; set => damage = value; }

    protected Enemy _enemyTarget;

    protected Vector2 _targetGroundPos;

    protected virtual void Update()
    {
        
    }

    protected virtual void MoveProjectile(Vector2 targetPos)
    {
        transform.position = Vector2.MoveTowards(transform.position,
            targetPos, moveSpeed * Time.deltaTime);
        float distanceToTarget = (targetPos - (Vector2)transform.position).magnitude;
        if (distanceToTarget < minDistanceToDealDamage)
        {
            DealDamage();
        }
    }


    protected virtual void DealDamage()
    {

    }
    public void SetProjectile(Enemy enemy,float damage)
    {
        _enemyTarget = enemy;
        _targetGroundPos = enemy.transform.position;
        this.Damage = damage;
    }


}
