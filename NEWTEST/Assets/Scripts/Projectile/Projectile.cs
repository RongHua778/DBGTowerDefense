using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : ReusableObject
{
    [SerializeField] private ProjectileType _projectileType;
    [SerializeField] private float _moveSpeed = 0f;
    [SerializeField] private float _sputteringRange = 0f;
    [SerializeField] private float _criticalRate = 0f;

    private readonly float minDistanceToDealDamage = .1f;
    [SerializeField] private float _damage;
    public float Damage { get => _damage; set => _damage = value; }

    [SerializeField] private Enemy _enemyTarget;
    [SerializeField] private Vector2 _targetGroundPos;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Update()
    {
        MoveProjectile();
        RotateProjectile();
    }

    private void RotateProjectile()
    {
        switch (_projectileType)
        {
            case ProjectileType.Target:
                if (_enemyTarget != null)
                {
                    RotateTowards(_enemyTarget.transform.position);
                }
                break;
            case ProjectileType.Ground:
                RotateTowards(_targetGroundPos);
                break;
        }
    }
    private void RotateTowards(Vector3 pos)
    {
        Vector3 targetPos = pos - transform.position;
        float angle = Vector3.SignedAngle(transform.up, targetPos, transform.forward);
        transform.Rotate(0f, 0f, angle);
    }

    private void MoveProjectile()
    {
        switch (_projectileType)
        {
            case ProjectileType.Target:
                if (_enemyTarget != null)
                {
                    MoveTowards(_enemyTarget.transform.position);
                }
                break;
            case ProjectileType.Ground:
                MoveTowards(_targetGroundPos);
                break;
        }
    }

    private void MoveTowards(Vector2 targetPos)
    {
        transform.position = Vector2.MoveTowards(transform.position,
            targetPos, _moveSpeed * Time.deltaTime);
        float distanceToTarget = (targetPos - (Vector2)transform.position).magnitude;
        if (distanceToTarget < minDistanceToDealDamage)
        {
            DealDamage();
        }
    }




    private void DealDamage()
    {
        IDamageable idamage;
        switch (_projectileType)
        {
            case ProjectileType.Target:
                idamage = _enemyTarget.GetComponent<IDamageable>();
                if (idamage != null)
                {
                    idamage.TakeDamage(Damage);
                }
                break;
            case ProjectileType.Ground:
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _sputteringRange);
                foreach (var item in colliders)
                {
                    idamage = item.GetComponent<IDamageable>();
                    if (idamage != null)
                    {
                        idamage.TakeDamage(Damage);
                    }
                }
                break;
            case ProjectileType.Fly:
                break;
        }
        ObjectPool.Instance.UnSpawn(this.gameObject);
    }
    public void SetProjectile(Enemy enemy,Turret turret)
    {
        _projectileType = turret._cardAsset.ProjectileType;
        _enemyTarget = enemy;
        _targetGroundPos = enemy.transform.position;
        _spriteRenderer.sprite = turret._cardAsset.ProjectileSprite;
        _damage = turret.AttackDamage;
        _moveSpeed = turret._cardAsset.ProjectileSpeed;
        _criticalRate = turret._cardAsset.CriticalRate;
        _sputteringRange = turret._cardAsset.SputteringRange;

    }

    public override void OnSpawn()
    {
       
    }

    public override void OnUnSpawn()
    {
        
    }
}
