using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DBGTD.Cells;
using System;
using System.Linq;

public abstract class Turret : ReusableObject
{
    [Header("SupportField")]
    public CardSO _cardAsset;
    public Card _card;
    public bool ShootFirst = true;
    public Square LandedSquare;
    public Enemy CurrentEnemyTarget { get; set; }
    public const int enemyLayerMask = 1 << 9;

    protected float _rotSpeed = 15f;
    protected GameObject _projectile;

    private float nextAttackTime;

    [SerializeField] protected Transform _rotTrans;
    [SerializeField] protected Transform _projectileSpawnPos;
    [SerializeField] protected GameObject _persistCanvas;
    [SerializeField] protected Image _persistProgress;

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
    [SerializeField] protected int _attackRange = 0;
    [SerializeField] protected float _attackDamage = 0f;
    [SerializeField] protected float _attackSpeed = 0f;
    [SerializeField] protected float _persistTime = 0f;
    [SerializeField] protected float _maxPersistTime = 0f;
    [SerializeField] protected float _criticalRate = 0f;
    [SerializeField] protected float _projectileSpeed = 0f;

    public int AttackRange
    {
        get {
            if (LandedSquare != null)
                return _attackRange + LandedSquare.RangeIntensify;
            else
                return _attackRange;
        } 
        set => _attackRange = value;
    }
    public float AttackDamage
    {
        get
        {
            if (LandedSquare != null)
                return _attackDamage *(1+ LandedSquare.AttackIntensify);
            else
                return _attackDamage;
        }
        set => _attackDamage = value;
    }
    public float AttackSpeed
    {
        get
        {
            if (LandedSquare != null)
                return _attackSpeed * (1 + LandedSquare.SpeedIntensify);
            else
                return _attackSpeed;
        }
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

    //[Header("AttackEffect")]
    //public List<AttackEffect> attackEffects = new List<AttackEffect>();


    //敌人检测相关
    static Collider2D[] targetsBuffer = new Collider2D[10];
    List<Collider2D> SquareColliders = new List<Collider2D>();
    List<Collider2D> potentialEnemyies = new List<Collider2D>();
    ContactFilter2D filter = new ContactFilter2D();

    //自动检测最前方敌人间隔
    protected float autoCheckCounter;
    private const float autoCheckInterval = .5f;

    private void Start()
    {
        LayerMask mask = enemyLayerMask;
        filter.SetLayerMask(mask);
        filter.useLayerMask = true;

    }
    protected virtual void Update()
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

    public virtual void LandTurret(Square landedSquare)
    {
        LandedSquare = landedSquare;
        TurretLanded = true;
        LandedSquare.SetTurret(this);
        foreach (var square in LandedSquare.GetRangeSquares(AttackRange))
        {
            if(square.IsRoad)
                SquareColliders.Add(square.GetComponent<Collider2D>());
        }
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
        int hit = 0;
        potentialEnemyies.Clear();
        foreach (var collider in SquareColliders)
        {
            hit = Physics2D.OverlapCollider(collider, filter, targetsBuffer);
            if (hit <= 0)
                continue;
            for (int i = 0; i < hit; i++)
            {
                if (!potentialEnemyies.Contains(targetsBuffer[i]))
                    potentialEnemyies.Add(targetsBuffer[i]);
            }
        }
        if (potentialEnemyies.Count > 0)
        {
            if (ShootFirst)
            {
                return FirstAttack();
            }
            else
            {
                return RandomAttack();
            }
        }
        CurrentEnemyTarget = null;
        return false;
    }

    private bool RandomAttack()
    {
        CurrentEnemyTarget = potentialEnemyies[UnityEngine.Random.Range(0, potentialEnemyies.Count)].GetComponent<Enemy>();
        return true;
    }

    //protected bool AcquireTarget2()
    //{
    //    int hits = Physics2D.OverlapCircleNonAlloc(transform.position, AttackRange, targetsBuffer, enemyLayerMask);
    //    if (hits > 0)
    //    {
    //        if (_ShootFirst)//攻击第一个敌人
    //        {
    //            int maxIndex = -1;
    //            for (int i = 0; i < hits; i++)
    //            {
    //                Enemy enemy = targetsBuffer[i].GetComponent<Enemy>();

    //                if (enemy.CurrentWayPointIndex > maxIndex)
    //                {
    //                    maxIndex = enemy.CurrentWayPointIndex;
    //                    CurrentEnemyTarget = enemy;
    //                }
    //                else if (enemy.CurrentWayPointIndex == maxIndex)
    //                {
    //                    if (enemy.GetDistanceToNextPoint() < CurrentEnemyTarget.GetDistanceToNextPoint())
    //                        CurrentEnemyTarget = enemy;
    //                }
    //            }
    //            return true;
    //        }
    //        else
    //        {

    //            CurrentEnemyTarget = targetsBuffer[UnityEngine.Random.Range(0, hits)].GetComponent<Enemy>();
    //            return true;
    //        }
    //    }

    //    CurrentEnemyTarget = null;
    //    return false;
    //}

    private bool FirstAttack()
    {
        int maxIndex = -1;
        for (int i = 0; i < potentialEnemyies.Count; i++)
        {
            if (potentialEnemyies[i] == null)
                continue;
            Enemy enemy = potentialEnemyies[i].GetComponent<Enemy>();
            if (enemy.CurrentWayPointIndex > maxIndex)
            {
                maxIndex = enemy.CurrentWayPointIndex;
                CurrentEnemyTarget = enemy;
            }
            else if (enemy.CurrentWayPointIndex == maxIndex)
            {
                if (enemy.GetDistanceToNextPoint() < CurrentEnemyTarget.GetDistanceToNextPoint())
                    CurrentEnemyTarget = enemy;
            }
        }
        return true;
    }

    private bool TrackTarget()
    {
        if (Time.time - autoCheckCounter > autoCheckInterval)//自动重新索敌间隔
        {
            CurrentEnemyTarget = null;
            autoCheckCounter = Time.time;
            return true;
        }

        if (CurrentEnemyTarget == null)
        {
            return false;
        }
        if (CurrentEnemyTarget.IsDie)
        {
            CurrentEnemyTarget = null;
            return false;
        }
        //Vector2 a = transform.position;
        //Vector2 b = CurrentEnemyTarget.transform.position;
        //if ((a - b).magnitude > AttackRange + 0.2f)//enemy的scale必须为1
        //{
        //    CurrentEnemyTarget = null;
        //    return true;
        //}
        return true;
    }

    protected virtual void RotateTowardsEnemy()
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
                LocateTarget();
            }
            else
            {
                return;
            }

            nextAttackTime = Time.time;
        }
    }

    protected virtual void LocateTarget()
    {
        Projectile newProjectile = LevelManager.Instance.SpawnProjectile(_cardAsset.ProjectileType);
        newProjectile.transform.position = _projectileSpawnPos.transform.position;
        newProjectile.GetComponent<Projectile>().SetProjectile(CurrentEnemyTarget.transform, this);
    }

    public virtual void ReadCardAsset(Card card)
    {
        _card = card;
        _cardAsset = _card.CardAsset;
        _projectile = Resources.Load<GameObject>("Prefabs/Projectile/BasicProjectile");
        ReadBasicAttribute();
        //ReadTurretEffects();
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

    //private void ReadTurretEffects()
    //{
    //    //attackeffect
    //    foreach (var attackEffect in _cardAsset.PlayEffectList.AttackEffects)
    //    {
    //        AttackEffect effect = LevelManager.Instance.GetAttackEffect((int)attackEffect.ChangeAttackEffect);
    //        effect.KeyValue = attackEffect.Value;
    //        attackEffects.Add(effect);
    //    }
    //}



    public override void OnSpawn()
    {
        TurretLanded = false;
    }

    public override void OnUnSpawn()
    {
        SquareColliders.Clear();
        potentialEnemyies.Clear();
        CurrentEnemyTarget = null;
        nextAttackTime = 0;
        _rotTrans.localRotation = Quaternion.Euler(Vector3.zero);
        HideRange();
        //attackEffects.Clear();
        GameEvents.Instance.AddCard(_cardAsset);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, _cardAsset.SputteringRange);
    }


}
