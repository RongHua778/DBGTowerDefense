using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//玩家生命值计算，胜负条件判断
public class LevelManager : Singleton<LevelManager>
{
    [Header("Settings")]
    [SerializeField] private GameScenario scenario = default;
    GameScenario.State activeScenario;
    [SerializeField] private int lives = 10;
    [SerializeField] private WayPoint _wayPoint = default;
    [SerializeField] private ProjectileFactory _projectileFactory = default;
    [SerializeField] private EnemyFactory _enemyFactory = default;
    private int _maxLive = 10;
    public int TotalLives
    {
        get { return lives; }
        set
        {
            if (TotalLives <= 0)
            {
                //game over
            }
            lives = Mathf.Clamp(value, 0, _maxLive);
        }
    }


    private void Start()
    {
        activeScenario = scenario.Begin();
    }

    void Update()
    {
        activeScenario.Progress();
    }


    public void SpawnEnemy(EnemyType type)
    {
        Enemy enemy = _enemyFactory.GetEnemy(type);
        enemy.WayPoint = _wayPoint;
        enemy.transform.localPosition = _wayPoint.GetWaypointPosition(0);
    }

    public Projectile SpawnProjectile(ProjectileType type)
    {
        Projectile projectile = _projectileFactory.GetProjectile(type);
        return projectile;
    }

    private void ReduceLives(int live)
    {
        TotalLives -= live;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }
}
