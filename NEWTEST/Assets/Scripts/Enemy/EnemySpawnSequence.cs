using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnSequence
{
    [SerializeField]
    EnemyFactory factory = default;

    [SerializeField]
    EnemyType type = EnemyType.normal;

    [SerializeField, Range(1, 100)]
    int amount = 1;

    [SerializeField, Range(0.1f, 30f)]
    float coolDown = 1f;


    public State Begin() => new State(this);

    [System.Serializable]
    public struct State
    {
        EnemySpawnSequence sequence;
        int count;
        float cooldown;
        public State(EnemySpawnSequence sequence)
        {
            this.sequence = sequence;
            count = 0;
            cooldown = sequence.coolDown;
        }

        public float Progress(float deltaTime)
        {
            cooldown += Time.deltaTime;
            while (cooldown >= sequence.coolDown)
            {
                cooldown -= sequence.coolDown;
                if (count >= sequence.amount)
                {
                    return cooldown;
                }
                count += 1;
                LevelManager.Instance.SpawnEnemy(sequence.factory, sequence.type);
            }
            return -1f;
        }
    }

}
