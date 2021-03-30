using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnModes
{
    Fixed,
    Random
}

public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private EnemyFactory enemyFactory = default;
    [SerializeField] private GameScenario scenario = default;
    GameScenario.State activeScenario;

    [SerializeField] private SpawnModes spawnMode = SpawnModes.Fixed;
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private float delayBtwWaves = 1f;

    [Header("Fixed Delay")]
    [SerializeField] private float delayBtwSpawns;

    [Header("Random Delay")]
    [SerializeField] private float minRandomDelay;
    [SerializeField] private float maxRandomDelay;

    //private WayPoint _waypoint;

    private float _spawnTimer;
    private int _enemiesSpawned;
    private int _enemiesRemaining;


    private float _waveTimer;
    [SerializeField]
    private float _waveInterval = default;
    // Start is called before the first frame update
    void Start()
    {
        //_waypoint = GetComponent<WayPoint>();

        _enemiesRemaining = enemyCount;

        GameEvents.Instance.onEnemyDie += RecordEnemy;
        GameEvents.Instance.onEnemyReach += RecordEnemy;
        activeScenario = scenario.Begin();
    }

    // Update is called once per frame
    void Update()
    {
        activeScenario.Progress();
        //SpawnCounter();
        //WaveCounter();
    }

    private void SpawnCounter()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer < 0)
        {
            _spawnTimer = GetSpawnDelay();
            if (_enemiesSpawned < enemyCount)
            {
                _enemiesSpawned++;
                //SpawnEnemy();
            }
        }
    }

    private void WaveCounter()
    {
        _waveTimer -= Time.deltaTime;
        if (_waveTimer < 0)
        {
            _waveTimer = _waveInterval;
            StartCoroutine(NextWave());
        }
    }

    private float GetSpawnDelay()
    {
        float delay = 0;
        if (spawnMode == SpawnModes.Fixed)
        {
            delay = delayBtwSpawns;
        }
        else
        {
            delay = GetRandomDelay();
        }
        return delay;
    }
    private float GetRandomDelay()
    {
        float randomTimer = Random.Range(minRandomDelay, maxRandomDelay);
        return randomTimer;
    }


    private void RecordEnemy()
    {
        _enemiesRemaining--;
        if (_enemiesRemaining <= 0)
        {
            //StartCoroutine(NextWave());
        }
    }

    private IEnumerator NextWave()
    {
        yield return new WaitForSeconds(delayBtwWaves);
        _enemiesRemaining = enemyCount;
        _spawnTimer = 0;
        _enemiesSpawned = 0;
    }


    private void OnDisable()
    {
        GameEvents.Instance.onEnemyDie -= RecordEnemy;
        GameEvents.Instance.onEnemyReach -= RecordEnemy;
    }
}
