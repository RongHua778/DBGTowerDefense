using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameScenario : ScriptableObject
{
    [SerializeField]
    EnemyWave[] waves = { };
   

    public State Begin() => new State(this);
    public struct State
    {
        GameScenario scenario;
        int index;
        EnemyWave.State wave;
        public State(GameScenario scenario)
        {
            this.scenario = scenario;
            index = 0;
            wave = scenario.waves[0].Begin();
        }

        public bool Progress()
        {
            float deltaTime = wave.Progress(Time.deltaTime);
            while (deltaTime >= 0f)
            {
                if (++index >= scenario.waves.Length)
                {
                    return true;
                }
                wave = scenario.waves[index].Begin();
                deltaTime = wave.Progress(deltaTime);
            }
            return true;
        }
    }
}
