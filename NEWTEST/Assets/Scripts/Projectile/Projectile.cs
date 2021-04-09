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
        if (_hitEffect != null)
        {
            GameObject effect = Instantiate(_hitEffect, transform.position, Quaternion.identity);
            effect.transform.localScale = Vector3.one * Mathf.Max(1, _sputteringRange);
            Destroy(effect, 3f);
        }

    }

    protected void TriggerDamageEffect(Enemy target)
    {
        foreach (var effect in _turretOwner._cardAsset.PlayEffectList.AttackEffects)
        {
            AttackEffect attackEffect = LevelManager.Instance.GetAttackEffect(effect);
            attackEffect.Affect(this, target);
        }
    }
    public void SetProjectile(Transform enemy, Turret turret)
    {
        _turretOwner = turret;
        ProjectileType = turret._cardAsset.ProjectileType;
        target = enemy;
        _targetGroundPos = enemy.position;
        _spriteRenderer.sprite = turret._cardAsset.ProjectileSprite;
        _damage = turret.AttackDamage;
        _moveSpeed = turret._cardAsset.ProjectileSpeed;
        _criticalRate = turret._cardAsset.CriticalRate;
        _sputteringRange = turret._cardAsset.SputteringRange;
        _hitEffect = turret._cardAsset.HitEffectPrefab;

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
