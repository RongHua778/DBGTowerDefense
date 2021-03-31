using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "DBGTD/WaveAsset")]
public class EnemyWave : ScriptableObject
{
    [SerializeField]
    EnemySpawnSequence[] spawnSequence = { new EnemySpawnSequence() };

    public State Begin() => new State(this);

    [System.Serializable]
    public struct State
    {
        EnemyWave wave;
        int index;
        EnemySpawnSequence.State sequence;
        public State (EnemyWave wave)
        {
            this.wave = wave;
            index = 0;
            sequence = wave.spawnSequence[0].Begin();
        }

        public float Progress(float deltaTime)
        {
            deltaTime = sequence.Progress(deltaTime);
            while (deltaTime >= 0f)
            {
                if (++index >= wave.spawnSequence.Length)
                {
                    return deltaTime;
                }
                sequence = wave.spawnSequence[index].Begin();
                deltaTime = sequence.Progress(deltaTime);
            }
            return -1f;
        }
    }
}
