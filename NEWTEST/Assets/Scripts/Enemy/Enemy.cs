using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : ReusableObject
{
    [HideInInspector] public WayPoint WayPoint;
    [SerializeField] protected float _initSpeed = 3f;
    [SerializeField] protected float _moveSpeed;
    public bool SlowDown = false;
    public float MoveSpeed
    {
        get { return _moveSpeed * (SlowDown ? StaticData.Instance.SlowDownRate : 1); }
        set => _moveSpeed = value;
    }

    protected int _currentWayPointIndex;
    public Vector3 CurrentPointPosition => WayPoint.GetWaypointPosition(_currentWayPointIndex);

    protected Vector3 _lastPointPosition;
    protected SpriteRenderer _spriteRenderer;
    protected const float _reachAccuracy = .1f;
    protected EnemyHealth _enemyHealth;
    public bool IsDie { get { return _enemyHealth.IsDie; } }


    void Awake()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
        InitSetUp();
        Debug.Assert(gameObject.layer == 9, "Target point on wrong layer", this);
    }

    protected void InitSetUp()
    {
        _enemyHealth.IsDie = false;
        MoveSpeed = _initSpeed;
        _currentWayPointIndex = 0;
        _lastPointPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyHealth.ResetHealth();
        GetComponent<Collider2D>().enabled = true;
    }

    void Update()
    {
        Move();
        if (CurrentPointPositionReach())
        {
            UpdateCurrentPointIndex();
        }
    }



    protected void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, CurrentPointPosition, MoveSpeed * Time.deltaTime);
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
            EndPointReach();
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
        InitSetUp();
    }

    public override void OnUnSpawn()
    {
        GetComponent<Collider2D>().enabled = false;
        SlowDown = false;
    }

}
