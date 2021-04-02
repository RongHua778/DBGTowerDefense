using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyFactory", menuName = "DBGTD/Factory")]
public class EnemyFactory : ScriptableObject
{

    private Dictionary<int, EnemySO> OrcDic = new Dictionary<int, EnemySO>();
    private Dictionary<int, EnemySO> UndeadDic = new Dictionary<int, EnemySO>();

    [Header("Orc")]
    [SerializeField] EnemySO[] OrcEnemies;

    [Header("Undead")]
    [SerializeField] EnemySO[] UndeadEnemies;

    public void Initialize()
    {
        foreach(var enemySO in OrcEnemies)
        {
            OrcDic.Add(enemySO.Level, enemySO);
        }
        foreach (var enemySO in UndeadEnemies)
        {
            UndeadDic.Add(enemySO.Level, enemySO);
        }
    }

    public Dictionary<int,EnemySO> GetEnemyDIC(EnemyRace race)
    {
        switch (race)
        {
            case EnemyRace.Orc:
                return OrcDic;
            case EnemyRace.Undead:
                return UndeadDic;
        }
        Debug.LogError("没有对应的种族敌人");
        return null;
    }

    public Enemy GetEnemy(EnemyRace race,int level)
    {
        EnemySO enemySO = GetEnemyDIC(race)[level];
        Enemy enemyInstance = ObjectPool.Instance.Spawn(enemySO.Prefab).GetComponent<Enemy>();
        enemyInstance.InitSetUp(enemySO);
        return enemyInstance;
    }
}
