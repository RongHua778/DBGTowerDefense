using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : ReusableObject
{
    [HideInInspector] public WayPoint WayPoint;
    [SerializeField] protected float _initSpeed = default;
    [SerializeField] protected float _moveSpeed = default;
    protected Vector3 _lastPointPosition;
    protected SpriteRenderer _spriteRenderer;
    protected const float _reachAccuracy = .1f;
    protected EnemyHealth _enemyHealth;
    protected int _currentWayPointIndex;
    public Vector3 CurrentPointPosition => WayPoint.GetWaypointPosition(_currentWayPointIndex);

    public float MoveSpeed
    {
        get { return _moveSpeed * (SlowDown ? StaticData.Instance.SlowDownRate : 1); }
        set => _moveSpeed = value;
    }

    public bool IsDie { get { return _enemyHealth.IsDie; } }

    [Header("Debuff")]
    public bool SlowDown = false;//ÊÇ·ñ±»¼õËÙBUFF

    public void InitSetUp(float speed,float health)
    {
        _enemyHealth = GetComponent<EnemyHealth>();

        _initSpeed = speed;
        _enemyHealth._initialHealth = health;
        _enemyHealth.MaxHealth = health;
        ResetEnemy();
    }

    protected void ResetEnemy()
    {
        _enemyHealth.IsDie = false;

        MoveSpeed = _initSpeed;
        _currentWayPointIndex = 0;
        _lastPointPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyHealth.ResetHealth();
        GetComponent<Collider2D>().enabled = true;
        Debug.Assert(gameObject.layer == 9, "Target point on wrong layer", this);
    }

    void Update()
    {
        Move();
    }



    protected void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, CurrentPointPosition, MoveSpeed * Time.deltaTime);
        if (CurrentPointPositionReach())
        {
            UpdateCurrentPointIndex();
        }
    }

    protected bool CurrentPointPositionReach()
    {
        float distanceToNextPointPosition = (transform.position - CurrentPointPosition).magnitude;
        if (distanceToNextPointPosition < _reachAccuracy)
        {
            _lastPointPosition = transform.position;
            return true;
        }
        return false;
    }

    protected void UpdateCurrentPointIndex()
    {
        int lastWaypointIndex = WayPoint.Points.Length - 1;
        if (_currentWayPointIndex < lastWaypointIndex)
        {
            _currentWayPointIndex++;
        }
        else
        {
            _currentWayPointIndex = 0;
            //EndPointReach();
        }
    }

    protected void EndPointReach()
    {
        GameEvents.Instance.EnemyReach();
        UnspawnThisEnemy();
    }

    public void StopMovement()
    {
        MoveSpeed = 0;
    }

    public void ResumeMovement()
    {
        MoveSpeed = _initSpeed;
    }

    public void UnspawnThisEnemy()
    {
        ObjectPool.Instance.UnSpawn(this.gameObject);

    }

    public override void OnSpawn()
    {
        
    }

    public override void OnUnSpawn()
    {
        ResetEnemy();
        GetComponent<Collider2D>().enabled = false;
        SlowDown = false;
    }

}
