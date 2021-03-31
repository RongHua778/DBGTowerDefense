using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DBGTD.Cells;
using System;

public abstract class Turret : ReusableObject
{
    [Header("SupportField")]
    static Collider2D[] targetsBuffer = new Collider2D[1];
    protected GameObject _projectile;
    protected Cell _landedCell;
    public Enemy CurrentEnemyTarget { get; set; }
    public List<Enemy> _enemies = new List<Enemy>();
    public const int enemyLayerMask = 1 << 9;
    private float nextAttackTime;
    public CardSO _cardAsset;
    protected float _rotSpeed = 15f;

    [SerializeField] protected Transform _rotTrans;
    [SerializeField] protected Transform _projectileSpawnPos;
    [SerializeField] protected GameObject _persistCanvas;
    [SerializeField] protected Image _persistProgress;
    [SerializeField] protected TargetEffectableEntity _effectableEntity;

    protected bool _turretLanded = false;
    public bool TurretLanded
    {
        get { return _turretLanded; }
        set
        {
            _turretLanded = value;
            if (_turretLanded)
                _persistCanvas.SetActive(true);
            else
                _persistCanvas.SetActive(false);

        }
    }

    [Header("TowerAttribute")]
    [SerializeField] protected float _attackRange = 0f;
    [SerializeField] protected float _attackDamage = 0f;
    [SerializeField] protected float _attackSpeed = 0f;
    [SerializeField] protected float _persistTime = 0f;
    [SerializeField] protected float _maxPersistTime = 0f;
    [SerializeField] protected float _criticalRate = 0f;
    [SerializeField] protected float _projectileSpeed = 0f;

    public float AttackRange
    {
        get => _attackRange * (1 + RangeIntensify);
        set => _attackRange = value;
    }
    public float AttackDamage
    {
        get => _attackDamage * (1 + AttackIntensify);
        set => _attackDamage = value;
    }
    public float AttackSpeed
    {
        get => _attackSpeed * (1 + SpeedIntensify);
        set => _attackSpeed = value;
    }
    public float PersistTime
    {
        get => _persistTime;
        set
        {
            if (value > MaxPersistTime)
                MaxPersistTime = value;
            _persistTime = value;
            _persistProgress.fillAmount = _persistTime / MaxPersistTime;
            if (_persistTime <= 0)
                ObjectPool.Instance.UnSpawn(this.gameObject);
        }
    }
    public float CriticalRate
    {
        get => _criticalRate;
        set => _criticalRate = value;
    }
    public float ProjectileSpeed
    {
        get => _projectileSpeed;
        set => _projectileSpeed = value;
    }

    public float MaxPersistTime
    {
        get { return _maxPersistTime; }
        set { _maxPersistTime = value; }
    }



    [Header("Intensify")]
    [SerializeField] private float _attackIntensify = 0f;
    [SerializeField] private float _speedIntensify = 0f;
    [SerializeField] private float _rangeIntensify = 0f;

    [Header("AttackEffect")]
    public List<AttackEffect> attackEffects = new List<AttackEffect>();

    public float AttackIntensify { get => _attackIntensify; set => _attackIntensify = value; }
    public float SpeedIntensify { get => _speedIntensify; set => _speedIntensify = value; }
    public float RangeIntensify { get => _rangeIntensify; set => _rangeIntensify = value; }

    protected void Update()
    {
        if (!TurretLanded)
            return;
        if (TrackTarget() || AcquireTarget())
        {
            RotateTowardsEnemy();
            FireProjectile();
        }
        PersistimeCountDown();
    }

    public void ShowRange()
    {
        gameObject.DrawCircle(AttackRange, 0.04f, StaticData.Instance.TowerRangeColor);
    }
    public void HideRange()
    {
        gameObject.HideCircle();
    }

    public void LandTurret(Cell endCell)
    {
        TurretLanded = true;
        _landedCell = endCell;
        endCell.SetTurret(this);
    }

    protected void PersistimeCountDown()
    {
        PersistTime -= Time.deltaTime;
    }

    protected void SetCollider()
    {
        this.GetComponent<CircleCollider2D>().radius = AttackRange;
    }

    protected bool AcquireTarget()
    {
        int hits = Physics2D.OverlapCircleNonAlloc(transform.position, AttackRange, targetsBuffer, enemyLayerMask);
        if (hits > 0)
        {
            CurrentEnemyTarget = targetsBuffer[0].GetComponent<Enemy>();
            return true;
        }

        CurrentEnemyTarget = null;
        return false;
    }

    private bool TrackTarget()
    {

        if (CurrentEnemyTarget == null)
        {
            return false;
        }
        if (CurrentEnemyTarget.IsDie)
        {
            CurrentEnemyTarget = null;
            return false;
        }
        Vector2 a = transform.position;
        Vector2 b = CurrentEnemyTarget.transform.position;
        if ((a - b).magnitude > AttackRange + 0.2f)//enemyµÄscale±ØÐëÎª1
        {
            CurrentEnemyTarget = null;
            return false;
        }
        return true;
    }

    protected void RotateTowardsEnemy()
    {
        if (CurrentEnemyTarget == null)
            return;
        var dir = CurrentEnemyTarget.transform.position - _rotTrans.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        Quaternion look_Rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _rotTrans.rotation = Quaternion.Lerp(_rotTrans.rotation, look_Rotation, _rotSpeed * Time.deltaTime);
    }


    protected virtual void FireProjectile()
    {
        if (Time.time - nextAttackTime > 1 / AttackSpeed)
        {
            if (CurrentEnemyTarget != null)
            {
                Projectile newProjectile = LevelManager.Instance.SpawnProjectile(_cardAsset.ProjectileType);
                newProjectile.transform.position = _projectileSpawnPos.transform.position;
                newProjectile.GetComponent<Projectile>().SetProjectile(CurrentEnemyTarget, this);
            }
            else
            {
                return;
            }

            nextAttackTime = Time.time;
        }
    }

    public virtual void ReadCardAsset(CardSO cardSO)
    {
        _cardAsset = cardSO;
        _projectile = Resources.Load<GameObject>("Prefabs/Projectile/BasicProjectile");
        ReadBasicAttribute();
        ReadTurretEffects();
    }

    private void ReadBasicAttribute()
    {
        AttackDamage = _cardAsset.Damage;
        AttackRange = _cardAsset.Range;
        AttackSpeed = _cardAsset.Speed;
        PersistTime = _cardAsset.PersistTime;
        MaxPersistTime = _cardAsset.PersistTime;
        CriticalRate = _cardAsset.CriticalRate;
        ProjectileSpeed = _cardAsset.ProjectileSpeed;
    }

    private void ReadTurretEffects()
    {
        //attackeffect
        foreach (var attackEffect in _cardAsset.AttackEffectBuffList)
        {
            AttackEffect effect = AttackEffectFactory.GetEffect(attackEffect.AttackEffectType);
            effect.KeyValue = attackEffect.Value;
            attackEffects.Add(effect);
        }
    }


    private void OnMouseOver()
    {
        ShowRange();
    }

    private void OnMouseExit()
    {
        HideRange();
    }

    public override void OnSpawn()
    {
        TurretLanded = false;
    }

    public override void OnUnSpawn()
    {
        _enemies.Clear();
        _landedCell = null;
        CurrentEnemyTarget = null;
        nextAttackTime = 0;
        _rotTrans.localRotation = Quaternion.Euler(Vector3.zero);
        AttackIntensify = 0;
        RangeIntensify = 0;
        SpeedIntensify = 0;
        HideRange();
        _effectableEntity.ClearEffects();
        attackEffects.Clear();
    }



}
