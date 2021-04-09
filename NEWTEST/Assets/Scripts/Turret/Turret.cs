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
    public bool ShootFirst = true;
    public Square LandedSquare;
    public Enemy CurrentEnemyTarget;
    public const int enemyLayerMask = 1 << 9;

    protected float _rotSpeed = 15f;
    protected GameObject _projectile;
    private float nextAttackTime;

    [SerializeField] protected float _persistTime = 0f;
    [SerializeField] protected float _maxPersistTime = 0f;
    [SerializeField] protected Transform _rotTrans;
    [SerializeField] protected Transform _projectileSpawnPos;
    [SerializeField] protected GameObject _persistCanvas;
    [SerializeField] protected Image _persistProgress;

    //敌人检测相关
    static Collider2D[] targetsBuffer = new Collider2D[10];
    List<Collider2D> SquareColliders = new List<Collider2D>();
    List<Collider2D> potentialEnemyies = new List<Collider2D>();
    ContactFilter2D filter = new ContactFilter2D();

    //自动检测最前方敌人间隔
    protected float autoCheckCounter;
    private const float autoCheckInterval = .5f;

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


    public int TurretRange
    {
        get {
            if (LandedSquare != null)
                return _cardAsset.TurretRange + LandedSquare.RangeIntensify;
            else
                return _cardAsset.TurretRange;
        } 
    }
    public float TurretAttack
    {
        get
        {
            if (LandedSquare != null)
                return _cardAsset.TurretAttack *(1+ LandedSquare.AttackIntensify);
            else
                return _cardAsset.TurretAttack;
        }
    }
    public float TurretSpeed
    {
        get
        {
            if (LandedSquare != null)
                return _cardAsset.TurretSpeed * (1 + LandedSquare.SpeedIntensify);
            else
                return _cardAsset.TurretSpeed;
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
            _persistProgress.fillAmount = _persistTime / MaxPersistTime;
            if (_persistTime <= 0)
                ObjectPool.Instance.UnSpawn(this.gameObject);
        }
    }
    public float CriticalRate
    {
        get => _cardAsset.CriticalRate;
    }
    public float ProjectileSpeed
    {
        get => _cardAsset.ProjectileSpeed;
    }

    public float MaxPersistTime
    {
        get { return _maxPersistTime; }
        set { _maxPersistTime = value; }
    }


    private void Start()
    {
        //设置碰撞检测敌人的过滤器
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


    public virtual void LandTurret(Square landedSquare)//放下防御塔时触发
    {
        LandedSquare = landedSquare;
        TurretLanded = true;
        LandedSquare.SetTurret(this);
        SetAttackRangeColliders();
    }

    public void SetAttackRangeColliders()
    {
        foreach (var square in LandedSquare.GetRangeSquares(TurretRange))
        {
            if (square.IsRoad)
                SquareColliders.Add(square.GetComponent<Collider2D>());
        }
    }

    protected void PersistimeCountDown()
    {
        PersistTime -= Time.deltaTime;
    }


    protected bool AcquireTarget()
    {
        int hit = 0;
        potentialEnemyies.Clear();
        foreach (var collider in SquareColliders)//将每个格子的collider逐个检测上面的敌人，整合在potentialenemies中
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

    private bool RandomAttack()//攻击随机敌人
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

    private bool FirstAttack()//攻击走在最前方敌人
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

    private bool TrackTarget()//判断是否需要重新索敌
    {
        if (Time.time - autoCheckCounter > autoCheckInterval)//自动重新索敌间隔
        {
            CurrentEnemyTarget = null;
            autoCheckCounter = Time.time;
            return true;
        }

        if (CurrentEnemyTarget == null)//没有目标，重新索敌
        {
            return false;
        }
        if (CurrentEnemyTarget.IsDie)//目标已死，重新索敌
        {
            CurrentEnemyTarget = null;
            return false;
        }
        //涉及脱离攻击范围时用下面这段，因为现在用检测间隔判断，暂时不需要
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


    protected virtual void FireProjectile()//攻击间隔判断
    {
        if (Time.time - nextAttackTime > 1 / TurretSpeed)
        {
            if (CurrentEnemyTarget != null)
            {
                Shoot();
            }
            else
            {
                return;
            }

            nextAttackTime = Time.time;
        }
    }

    protected virtual void Shoot()//发出子弹
    {
        Projectile newProjectile = LevelManager.Instance.SpawnProjectile(_cardAsset.ProjectileType);
        newProjectile.transform.position = _projectileSpawnPos.transform.position;
        newProjectile.GetComponent<Projectile>().SetProjectile(CurrentEnemyTarget.transform, this);
    }

    public virtual void ReadCardAsset(CardSO cardSO)//保存cardasset到本塔
    {
        _cardAsset = cardSO;
        _projectile = Resources.Load<GameObject>("Prefabs/Projectile/BasicProjectile");
        SetPersistTime();

    }

    private void SetPersistTime()//设置存续时间
    {
        PersistTime = _cardAsset.PersistTime;
        MaxPersistTime = _cardAsset.PersistTime;
    }


    public override void OnSpawn()
    {
        TurretLanded = false;
    }

    public override void OnUnSpawn()
    {
        SquareColliders.Clear();
        potentialEnemyies.Clear();
        CurrentEnemyTarget = null;
        if (LandedSquare != null)
            LandedSquare.SquareTurret = null;
        LandedSquare = null;
        nextAttackTime = 0;
        _rotTrans.localRotation = Quaternion.Euler(Vector3.zero);
        GameEvents.Instance.AddCard(_cardAsset.original);
    }

    private void OnDrawGizmos()//查看aoe塔攻击范围
    {
        Gizmos.DrawWireSphere(this.transform.position, _cardAsset.SputteringRange);
    }


}
