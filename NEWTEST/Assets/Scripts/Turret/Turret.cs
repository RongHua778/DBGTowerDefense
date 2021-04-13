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

    //�Ѿ�ǿ�����ĵ����б�
    [HideInInspector]
    public List<Square> AlreadyIntensifyList = new List<Square>();


    [SerializeField] protected float _persistTime = 0f;
    [SerializeField] protected float _maxPersistTime = 0f;
    [SerializeField] protected Transform _rotTrans;
    [SerializeField] protected Transform _projectileSpawnPos;
    [SerializeField] protected GameObject _persistCanvas;
    [SerializeField] protected Image _persistProgress;

    //���˼�����
    static Collider2D[] targetsBuffer = new Collider2D[10];
    List<Collider2D> SquareColliders = new List<Collider2D>();
    List<Collider2D> potentialEnemyies = new List<Collider2D>();
    ContactFilter2D filter = new ContactFilter2D();

    //�Զ������ǰ�����˼��
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
        get
        {
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
                return _cardAsset.TurretAttack * (1 + LandedSquare.AttackIntensify);
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
        //������ײ�����˵Ĺ�����
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


    public virtual void LandTurret(Square landedSquare)//���·�����ʱ����
    {
        LandedSquare = landedSquare;
        TurretLanded = true;
        LandedSquare.SetTurret(this);
        GameEvents.Instance.TurretLanded(this);
        SetAttackRangeColliders();
    }

    public void UnspawnRemovePoloEffect()
    {
        LandedSquare.SquareTurret = null;
        GameEvents.Instance.TurretDemolish(this);
        //PrepareIntensify();
    }

    public void SetAttackRangeColliders()
    {
        SquareColliders.Clear();
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
        foreach (var collider in SquareColliders)//��ÿ�����ӵ�collider����������ĵ��ˣ�������potentialenemies��
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

    private bool RandomAttack()//�����������
    {
        CurrentEnemyTarget = potentialEnemyies[UnityEngine.Random.Range(0, potentialEnemyies.Count)].GetComponent<Enemy>();
        return true;
    }

    //protected bool AcquireTarget2()
    //{
    //    int hits = Physics2D.OverlapCircleNonAlloc(transform.position, AttackRange, targetsBuffer, enemyLayerMask);
    //    if (hits > 0)
    //    {
    //        if (_ShootFirst)//������һ������
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

    private bool FirstAttack()//����������ǰ������
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

    private bool TrackTarget()//�ж��Ƿ���Ҫ��������
    {
        if (Time.time - autoCheckCounter > autoCheckInterval)//�Զ��������м��
        {
            CurrentEnemyTarget = null;
            autoCheckCounter = Time.time;
            return true;
        }

        if (CurrentEnemyTarget == null)//û��Ŀ�꣬��������
        {
            return false;
        }
        if (CurrentEnemyTarget.IsDie)//Ŀ����������������
        {
            CurrentEnemyTarget = null;
            return false;
        }
        //�漰���빥����Χʱ��������Σ���Ϊ�����ü�����жϣ���ʱ����Ҫ
        //Vector2 a = transform.position;
        //Vector2 b = CurrentEnemyTarget.transform.position;
        //if ((a - b).magnitude > AttackRange + 0.2f)//enemy��scale����Ϊ1
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


    protected virtual void FireProjectile()//��������ж�
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

    protected virtual void Shoot()//�����ӵ�
    {
        Projectile newProjectile = LevelManager.Instance.SpawnProjectile(_cardAsset.ProjectileType);
        newProjectile.transform.position = _projectileSpawnPos.transform.position;
        newProjectile.GetComponent<Projectile>().SetProjectile(CurrentEnemyTarget.transform, this);
    }

    public virtual void ReadCardAsset(CardSO cardSO)//����cardasset������
    {
        _cardAsset = cardSO;
        _projectile = Resources.Load<GameObject>("Prefabs/Projectile/BasicProjectile");
        SetPersistTime();

    }

    private void SetPersistTime()//���ô���ʱ��
    {
        PersistTime = _cardAsset.PersistTime;
        MaxPersistTime = _cardAsset.PersistTime;
    }


    public override void OnSpawn()
    {
    }

    public override void OnUnSpawn()
    {
        TurretLanded = false;
        SquareColliders.Clear();
        potentialEnemyies.Clear();
        CurrentEnemyTarget = null;

        UnspawnRemovePoloEffect();

        LandedSquare = null;
        nextAttackTime = 0;
        _rotTrans.localRotation = Quaternion.Euler(Vector3.zero);
        GameEvents.Instance.AddCard(_cardAsset.original);
    }

    //private void OnDrawGizmos()//�鿴aoe��������Χ
    //{
    //    Gizmos.DrawWireSphere(this.transform.position, _cardAsset.SputteringRange);
    //}


}
