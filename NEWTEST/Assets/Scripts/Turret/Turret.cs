using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : ReusableObject
{
    [SerializeField] protected float _rotSpeed = 5f;
    [SerializeField] protected Transform _rotTrans;
    [SerializeField] protected Transform _projectileSpawnPos;
    

    [Header("TowerAttribute")]
    [SerializeField] protected float _attackRange = 0f;
    [SerializeField] protected float _attackDamage = 0f;
    [SerializeField] protected float _attackSpeed = 0f;
    [SerializeField] protected float _persistTime = 0f;
    [SerializeField] protected float _criticalRate = 0f;
    [SerializeField] protected float _projectileSpeed = 0f;
   
    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }
    public float AttackDamage { get { return _attackDamage; } set { _attackDamage = value; } }
    public float AttackSpeed { get => _attackSpeed; set => _attackSpeed = value; }
    public float PersistTime { get => _persistTime; set => _persistTime = value; }
    public float CriticalRate { get => _criticalRate; set => _criticalRate = value; }
    public float ProjectileSpeed { get => _projectileSpeed; set => _projectileSpeed = value; }


    public Enemy CurrentEnemyTarget { get; set; }
    protected List<Enemy> _enemies = new List<Enemy>();
    protected float _nextAttackTime;
    protected CardSO _cardAsset;
    protected GameObject _projectile;
    // Start is called before the first frame update

    protected void Update()
    {
        GetCurrentEnemyTarget();
        RotateTowardsEnemy();
        FireProjectile();
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
        CurrentEnemyTarget = _enemies[0];
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
        if (Time.time > _nextAttackTime)
        {
            if (CurrentEnemyTarget != null)
            {
                Vector3 dirToTarget = CurrentEnemyTarget.transform.position - transform.position;
                GameObject newProjectile = ObjectPool.Instance.Spawn(_projectile);
                newProjectile.transform.position = _projectileSpawnPos.transform.position;
                newProjectile.GetComponent<Projectile>().SetProjectile(CurrentEnemyTarget,_cardAsset);
            }
            else
            {
                return;
            }

            _nextAttackTime = Time.time + 1 / AttackSpeed;
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
        CriticalRate = _cardAsset.CriticalRate;
        ProjectileSpeed = _cardAsset.ProjectileSpeed;
        this.GetComponent<CircleCollider2D>().radius = AttackRange;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy newEnemy = other.GetComponent<Enemy>();
            _enemies.Add(newEnemy);

        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (_enemies.Contains(enemy))
            {
                _enemies.Remove(enemy);
            }
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
    // Update is called once per frame

    public override void OnSpawn()
    {
       
    }

    public override void OnUnSpawn()
    {
    }
}
