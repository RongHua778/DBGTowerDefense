using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//玩家生命值计算，胜负条件判断
public class LevelManager : Singleton<LevelManager>
{
    [Header("Settings")]
    [SerializeField] private int lives = 10;
    [SerializeField] private WayPoint _wayPoint = default;

    [Header("Factories")]
    [SerializeField] private GameScenario scenario = default;
    GameScenario.State activeScenario;
    [SerializeField] private ProjectileFactory _projectileFactory = default;
    [SerializeField] private EnemyFactory _enemyFactory = default;

    public Dictionary<NoTargetEffectType, NoTargetEffect> _noTargetEffects = new Dictionary<NoTargetEffectType, NoTargetEffect>();

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
        _projectileFactory.Initialize();
        activeScenario = scenario.Begin();
    }

    void Update()
    {
        activeScenario.Progress();
        NoTargetEffectCounter();
    }

    private void NoTargetEffectCounter()
    {
        foreach (var effect in _noTargetEffects.Values.ToList())
        {
            effect.Tick(Time.deltaTime);
            if (effect.IsFinished)
            {
                _noTargetEffects.Remove(effect.NoTargetEffectType);
            }
        }
    }

    public void AddEffect(NoTargetEffect effect, float keyValue, float duration)
    {
        if (_noTargetEffects.ContainsKey(effect.NoTargetEffectType))
        {
            NoTargetEffect effectItem = _noTargetEffects[effect.NoTargetEffectType];
            if (effect.IsStackable)
            {

            }
            if (effectItem.Duration < duration)
            {
                effectItem.Duration = duration;
            }
            effectItem.Affect();
        }
        else
        {
            effect.Duration += duration;
            effect.KeyValue = keyValue;
            _noTargetEffects.Add(effect.NoTargetEffectType, effect);
            effect.Affect();
        }
    }

    public void ApplyNoTargetEffects(IEnumerable<NoTargetEffectConfig> effectList)
    {
        foreach (var effectItem in effectList)
        {
            NoTargetEffect effect = NoTargetEffectFactory.GetEffect(effectItem.NoTargetEffectType);
            AddEffect(effect, effectItem.KeyValue, effectItem.Duration);
        }
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
