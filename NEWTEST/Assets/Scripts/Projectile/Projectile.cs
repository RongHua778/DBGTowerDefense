using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Projectile : ReusableObject
{
    public ProjectileType ProjectileType = default;
    [SerializeField] protected float _moveSpeed = default;
    [SerializeField] protected float _sputteringRange = default;
    [SerializeField] protected float _criticalRate = default;
    [SerializeField] protected float _damage = default;
    public float Damage { get => _damage; set => _damage = value; }

    protected readonly float minDistanceToDealDamage = .1f;

    protected Enemy _enemyTarget;
    protected Vector2 _targetGroundPos;
    protected SpriteRenderer _spriteRenderer;
    protected Turret _turretOwner;

    private void Awake()
    {
        _spriteRenderer = this.transform.Find("GFX").GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        MoveProjectile();
        RotateProjectile();
    }

    protected virtual void RotateProjectile()
    {
        
    }
    protected void RotateTowards(Vector3 pos)
    {
        Vector3 targetPos = pos - transform.position;
        float angle = Vector3.SignedAngle(transform.up, targetPos, transform.forward);
        transform.Rotate(0f, 0f, angle);
    }

    protected virtual void MoveProjectile()
    {
        
    }

    protected void MoveTowards(Vector2 targetPos)
    {
        transform.position = Vector2.MoveTowards(transform.position,
            targetPos, _moveSpeed * Time.deltaTime);
        float distanceToTarget = GetTargetDistance();
        if (distanceToTarget < minDistanceToDealDamage)
        {
            DealDamage();
        }
    }




    protected virtual void DealDamage()
    {
       
    }

    //protected void TriggerShootEffect()
    //{
    //    foreach (var effect in _turretOwner.attackEffects)
    //    {
    //        if (effect.AttackEffectTiming == AttackEffectTiming.Shoot)
    //            effect.Affect(this, _turretOwner);
    //    }
    //}
    protected void TriggerDamageEffect(Enemy target)
    {
        foreach (var effect in _turretOwner.attackEffects)
        {
            if (effect.AttackEffectTiming == AttackEffectTiming.Damage)
                effect.Affect(this, target);
        }
    }
    public void SetProjectile(Enemy enemy, Turret turret)
    {
        _turretOwner = turret;
        ProjectileType = turret._cardAsset.ProjectileType;
        _enemyTarget = enemy;
        _targetGroundPos = enemy.transform.position;
        _spriteRenderer.sprite = turret._cardAsset.ProjectileSprite;
        _damage = turret.AttackDamage;
        _moveSpeed = turret._cardAsset.ProjectileSpeed;
        _criticalRate = turret._cardAsset.CriticalRate;
        _sputteringRange = turret._cardAsset.SputteringRange;
        //TriggerShootEffect();

    }

    public virtual float GetTargetDistance()
    {
        return 0;
    }

    public override void OnSpawn()
    {

    }

    public override void OnUnSpawn()
    {

    }
}
