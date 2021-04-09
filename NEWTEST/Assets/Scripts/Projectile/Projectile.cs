using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Projectile : ReusableObject
{
    public ProjectileType ProjectileType = default;
    [SerializeField] protected float _moveSpeed = default;
    [SerializeField] private float sputteringRange = default;
    [SerializeField] protected float _criticalRate = default;
    [SerializeField] protected float _damage = default;
    public float Damage { get => _damage; set => _damage = value; }
    public float SputteringRange { get => sputteringRange; set => sputteringRange = value; }

    protected readonly float minDistanceToDealDamage = .1f;

    protected Transform target;
    protected Vector2 _targetGroundPos;
    protected SpriteRenderer _spriteRenderer;
    protected Turret _turretOwner;

    protected GameObject _hitEffect = default;

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

    protected virtual void PlayHitEffect(float effectScale)
    {
        GameObject effect = Instantiate(_hitEffect, transform.position, Quaternion.identity);
        effect.transform.localScale = 0.5f * Vector3.one * Mathf.Max(1, effectScale);
        Destroy(effect, 3f);
    }

    protected void TriggerShootAttackEffect(Enemy target)
    {
        foreach (var effect in _turretOwner._cardAsset.FinalEffectList.AttackEffects)
        {
            AttackEffect attackEffect = LevelManager.Instance.GetAttackEffect(effect);
            if (attackEffect.AttackEffectTiming == AttackEffectTiming.Shoot)
                attackEffect.Affect(this, target);
        }
    }

    protected void TriggerHitAttackEffect(Enemy target)
    {
        foreach (var effect in _turretOwner._cardAsset.FinalEffectList.AttackEffects)
        {
            AttackEffect attackEffect = LevelManager.Instance.GetAttackEffect(effect);
            if (attackEffect.AttackEffectTiming == AttackEffectTiming.Hit)
                attackEffect.Affect(this, target);
        }
    }
    public void SetProjectile(Transform enemy, Turret turret)
    {
        _turretOwner = turret;
        target = enemy;
        _targetGroundPos = enemy.position;
        _spriteRenderer.sprite = turret._cardAsset.ProjectileSprite;
        _damage = turret.TurretAttack;
        _moveSpeed = turret._cardAsset.ProjectileSpeed;
        _criticalRate = turret._cardAsset.CriticalRate;
        SputteringRange = turret._cardAsset.SputteringRange;
        _hitEffect = turret._cardAsset.HitEffectPrefab;
        TriggerShootAttackEffect(enemy.GetComponent<Enemy>());

    }

    public virtual float GetTargetDistance()
    {
        return 0;
    }

    public float GetTargetAndTurretDistance()
    {
        return ((Vector2)target.position - (Vector2)_turretOwner.transform.position).magnitude;
    }

    public override void OnSpawn()
    {

    }

    public override void OnUnSpawn()
    {

    }
}
