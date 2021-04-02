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

    public Dictionary<NoTargetBuffName, NoTargetBuff> _noTargetEffects = new Dictionary<NoTargetBuffName, NoTargetBuff>();

    private TypeFactory _turretBuffFactory;
    private TypeFactory _enemyBuffFactory;
    private TypeFactory _attackEffectFactory;
    private TypeFactory _noTargetBuffFactory;

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
        _turretBuffFactory = new TurretBuffFactory();
        _turretBuffFactory.Initialize();
        _enemyBuffFactory = new EnemyBuffFactory();
        _enemyBuffFactory.Initialize();
        _attackEffectFactory = new AttackEffectFactory();
        _attackEffectFactory.Initialize();
        _noTargetBuffFactory = new NoTargetEffectFactory();
        _noTargetBuffFactory.Initialize();

        _projectileFactory.Initialize();
        activeScenario = scenario.Begin();
    }

    void Update()
    {
        activeScenario.Progress();
        NoTargetEffectCounter();
    }

    public TurretBuff GetTurretBuff(int buffID)
    {
        return _turretBuffFactory.GetType(buffID) as TurretBuff;
    }

    public EnemyBuff GetEnemyBuff(int buffID)
    {
        return _enemyBuffFactory.GetType(buffID) as EnemyBuff;
    }

    public AttackEffect GetAttackEffect(int effecID)
    {
        return _attackEffectFactory.GetType(effecID) as AttackEffect;
    }

    public NoTargetBuff GetNoTargetBuff(int noTargetBuffId)
    {
        return _noTargetBuffFactory.GetType(noTargetBuffId) as NoTargetBuff;
    }

    private void NoTargetEffectCounter()
    {
        foreach (var effect in _noTargetEffects.Values.ToList())
        {
            effect.Tick(Time.deltaTime);
            if (effect.IsFinished)
            {
                _noTargetEffects.Remove(effect.NoTargetBuffName);
            }
        }
    }

    public void AddEffect(NoTargetBuff effect, float keyValue, float duration)
    {
        if (_noTargetEffects.ContainsKey(effect.NoTargetBuffName))
        {
            NoTargetBuff effectItem = _noTargetEffects[effect.NoTargetBuffName];
            if (effect.IsStackable)
            {
                effectItem.Affect(this.gameObject);
            }
            if (effectItem.Duration < duration)
            {
                effectItem.Duration = duration;
            }
        }
        else
        {
            effect.Duration += duration;
            effect.KeyValue = keyValue;
            _noTargetEffects.Add(effect.NoTargetBuffName, effect);
            effect.Affect(this.gameObject);
        }
    }

    public void ApplyNoTargetEffects(IEnumerable<NoTargetEffectConfig> effectList)
    {
        foreach (var effectItem in effectList)
        {
            NoTargetBuff effect = GetNoTargetBuff((int)effectItem.NoTargetBuffName);
            if(effect!=null)
                AddEffect(effect, effectItem.KeyValue, effectItem.Duration);
            Debug.Assert(effect != null, "配置了错误的NotargetBuff:" + effect.NoTargetBuffName.ToString());
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
