using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    easy, normal, hard
}
[CreateAssetMenu]
public class EnemyFactory : ScriptableObject
{
    [System.Serializable]
    class EnemyConfig
    {
        public GameObject prefab = default;

        [Range(0.5f, 3f)]
        public float speed = default;
        [Range(1, 100)]
        public int health = default;

    }

    [SerializeField]
    EnemyConfig easy = default, normal = default, hard = default;

    EnemyConfig GetConfig(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.easy:return easy;
            case EnemyType.normal:return normal;
            case EnemyType.hard:return hard;
        }
        return null;
    }

    public Enemy GetEnemy(EnemyType type=EnemyType.normal)
    {
        EnemyConfig config = GetConfig(type);
        Enemy instance = ObjectPool.Instance.Spawn(config.prefab).GetComponent<Enemy>();
        instance.InitSetUp(config.speed,config.health);
        return instance;
    }
}
