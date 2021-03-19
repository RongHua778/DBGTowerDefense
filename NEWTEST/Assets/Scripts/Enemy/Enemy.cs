using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : ReusableObject
{
    [SerializeField] protected float _initSpeed = 3f;
    protected float _moveSpeed;

    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

    [HideInInspector]
    public WayPoint WayPoint;

    protected int _currentWayPointIndex;
    public Vector3 CurrentPointPosition => WayPoint.GetWaypointPosition(_currentWayPointIndex);

    protected Vector3 _lastPointPosition;
    protected SpriteRenderer _spriteRenderer;

    protected const float _reachAccuracy = .1f;

    protected EnemyHealth _enemyHealth;



    // Start is called before the first frame update
    void Awake()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
        InitSetUp();
    }

    protected void InitSetUp()
    {
        _enemyHealth.IsDie = false;
        MoveSpeed = _initSpeed;
        _currentWayPointIndex = 0;
        _lastPointPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
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
       
    }
}
