using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DBGTD.Cells;
using System;

public abstract class Turret : ReusableObject, IAffectable
{

    protected CardSO _cardAsset;
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
    public Enemy CurrentEnemyTarget { get; set; }
    public List<Enemy> _enemies = new List<Enemy>();

    private float nextAttackTime;
    public float NextAttackTime { get => nextAttackTime; set => nextAttackTime = value; }

    protected GameObject _projectile;
    protected Cell _landedCell;


    [SerializeField] protected float _rotSpeed = 5f;
    [SerializeField] protected Transform _rotTrans;
    [SerializeField] protected Transform _projectileSpawnPos;
    [SerializeField] protected GameObject _persistCanvas;
    [SerializeField] protected AttackCollider _attackCollider;

    public void LandTurret(Cell endCell)
    {
        TurretLanded = true;
        _landedCell = endCell;
        endCell.SetTurret(this);
    }

    [SerializeField] protected Image _persistProgress;


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
        get { return _attackRange; }
        set
        {
            _attackRange = value;
            _cardAsset.Range = value;
            _attackCollider.SetAttackRange(value);
        }
    }
    public float AttackDamage
    {
        get { return _attackDamage; }
        set
        {
            _attackDamage = value;
            _cardAsset.Damage = value;
        }
    }
    public float AttackSpeed
    {
        get => _attackSpeed;
        set
        {
            _attackSpeed = value;
            _cardAsset.Speed = value;
        }
    }
    public float PersistTime
    {
        get => _persistTime;
        set
        {
            if (value > MaxPersistTime)
                MaxPersistTime = value;
            _persistTime = value;
            //_persistTime = Mathf.Min(MaxPersistTime, value);
            _persistProgress.fillAmount = _persistTime / MaxPersistTime;
            if (_persistTime <= 0)
                ObjectPool.Instance.UnSpawn(this.gameObject);
        }
    }
    public float CriticalRate
    {
        get => _criticalRate;
        set
        {
            _criticalRate = value;
            _cardAsset.CriticalRate = value;
        }
    }
    public float ProjectileSpeed
    {
        get => _projectileSpeed;
        set
        {
            _projectileSpeed = value;
            _cardAsset.ProjectileSpeed = value;
        }
    }

    public float MaxPersistTime
    {
        // get { return StaticData.Instance.MaxPersistime; }
        get { return _maxPersistTime; }
        set { _maxPersistTime = value; }
    }



    // Start is called before the first frame update


    protected void Update()
    {
        GetCurrentEnemyTarget();
        RotateTowardsEnemy();
        FireProjectile();
        PersistimeCountDown();
    }

    protected void PersistimeCountDown()
    {
        if (TurretLanded)
            PersistTime -= Time.deltaTime;
    }

    protected void SetCollider()
    {
        this.GetComponent<CircleCollider2D>().radius = AttackRange;
    }

    protected void GetCurrentEnemyTarget()
    {
        if (_enemies.Count <= 0)
        {
            CurrentEnemyTarget = null;
            return;
        }
        if (_enemies[0].IsDie)
        {
            _enemies.RemoveAt(0);
        }
        else
        {
            CurrentEnemyTarget = _enemies[0];
        }
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
        if (Time.time - NextAttackTime > 1 / AttackSpeed)
        {
            if (CurrentEnemyTarget != null)
            {
                Vector3 dirToTarget = CurrentEnemyTarget.transform.position - transform.position;
                GameObject newProjectile = ObjectPool.Instance.Spawn(_projectile);
                newProjectile.transform.position = _projectileSpawnPos.transform.position;
                newProjectile.GetComponent<Projectile>().SetProjectile(CurrentEnemyTarget, _cardAsset);
            }
            else
            {
                return;
            }

            NextAttackTime = Time.time;
        }
    }

    public virtual void ReadCardAsset(CardSO cardSO)
    {
        _cardAsset = cardSO;
        _projectile = Resources.Load<GameObject>("Prefabs/Projectile/BasicProjectile");
        AttackDamage = _cardAsset.Damage;
        AttackRange = _cardAsset.Range;
        AttackSpeed = _cardAsset.Speed;
        PersistTime = _cardAsset.PersistTime;
        MaxPersistTime = _cardAsset.PersistTime;
        CriticalRate = _cardAsset.CriticalRate;
        ProjectileSpeed = _cardAsset.ProjectileSpeed;
        _attackCollider.SetAttackRange(AttackRange);
    }


    // Update is called once per frame

    public override void OnSpawn()
    {
        TurretLanded = false;
    }

    public override void OnUnSpawn()
    {
        _enemies.Clear();
        _landedCell = null;
        CurrentEnemyTarget = null;
        NextAttackTime = 0;
        _rotTrans.localRotation = Quaternion.Euler(Vector3.zero);
    }


    public virtual void Affect(IEnumerable<MagicEffect> effectList)
    {
        foreach (var effectItem in effectList)
        {
            Effect effect = EffectFactory.GetEffect(effectItem.EffectType);
            effect.Affect(this.gameObject, effectItem.Value);
        }
    }
}
