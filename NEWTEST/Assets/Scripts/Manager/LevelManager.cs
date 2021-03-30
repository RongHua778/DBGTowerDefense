using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//玩家生命值计算，胜负条件判断
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private int lives = 10;
    [SerializeField] private WayPoint _wayPoint = default;

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

    private void ReduceLives(int live)
    {
        TotalLives -= live;
    }

    public void SpawnEnemy(EnemyFactory factory, EnemyType type)
    {
        Enemy enemy = factory.GetEnemy(type);
        enemy.WayPoint = _wayPoint;
        enemy.transform.localPosition = _wayPoint.GetWaypointPosition(0);
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }
}
