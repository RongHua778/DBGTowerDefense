using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DBGTD.Cells;


//玩家生命值计算，胜负条件判断
public class LevelManager : Singleton<LevelManager>
{
    [Header("Settings")]
    public CellGrid CellGrid;
    [SerializeField] private int lives = 10;
    [SerializeField] private WayPoint _wayPoint = default;

    [Header("Factories")]
    [SerializeField] private GameScenario scenario = default;
    GameScenario.State activeScenario;
    [SerializeField] private ProjectileFactory _projectileFactory = default;
    [SerializeField] private EnemyFactory _enemyFactory = default;

    public Dictionary<NoTargetBuffType, NoTargetBuff> _noTargetEffects = new Dictionary<NoTargetBuffType, NoTargetBuff>();

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
        _enemyBuffFactory = new EnemyBuffFactory();
        _enemyBuffFactory.Initialize();
        _attackEffectFactory = new AttackEffectFactory();
        _attackEffectFactory.Initialize();
        _noTargetBuffFactory = new NoTargetEffectFactory();
        _noTargetBuffFactory.Initialize();

        _enemyFactory.Initialize();
        _projectileFactory.Initialize();
        activeScenario = scenario.Begin();

        GameEvents.Instance.onTurretLanded += ApplyNoTargetEffects;
        GameEvents.Instance.onTurretDemolish += RemoveNotargetBuff;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.onTurretLanded -= ApplyNoTargetEffects;
        GameEvents.Instance.onTurretDemolish -= RemoveNotargetBuff;
    }

    void Update()
    {
        activeScenario.Progress();
        NoTargetEffectCounter();
    }


    public EnemyBuff GetEnemyBuff(EffectConfig config)
    {
        EnemyBuff buff = _enemyBuffFactory.GetType((int)config.EnemyBuffType) as EnemyBuff;
        buff.SetValue(config.KeyValue);
        return buff;
    }

    public AttackEffect GetAttackEffect(EffectConfig config)
    {
        AttackEffect attackEffect = _attackEffectFactory.GetType((int)config.AttackEffectType) as AttackEffect;
        attackEffect.SetValue(config.KeyValue);
        return attackEffect;
    }

    public NoTargetBuff GetNoTargetBuff(EffectConfig config)
    {
        NoTargetBuff noTargetBuff = _noTargetBuffFactory.GetType((int)config.NoTargetBuffType) as NoTargetBuff;
        noTargetBuff.SetValue(config.KeyValue);
        return noTargetBuff;
    }

    private void NoTargetEffectCounter()
    {
        foreach (var effect in _noTargetEffects.Values.ToList())
        {
            effect.Tick(Time.deltaTime);
            if (effect.IsFinished)
            {
                _noTargetEffects.Remove(effect.NoTargetBuffType);
            }
        }
    }

    public void AddEffect(NoTargetBuff effect)
    {
        if (_noTargetEffects.ContainsKey(effect.NoTargetBuffType))
        {
            NoTargetBuff effectItem = _noTargetEffects[effect.NoTargetBuffType];
            if (effect.IsStackable)//层数类BUFF
            {
                effectItem.Stacks += effect.Stacks;
                effectItem.Affect(this.gameObject);
            }
            else if (effectItem.Duration < effect.Duration)//时间类BUFF
            {
                effectItem.Duration = effect.Duration;
            }

        }
        else
        {
            _noTargetEffects.Add(effect.NoTargetBuffType, effect);
            effect.Affect(this.gameObject);
        }
    }

    public void RemoveNotargetBuff(Turret turret)
    {

        foreach(EffectConfig config in turret._cardAsset.NotargetEffectList)
        {
            if (config.BaseEffectType != EffectType.NoTargetBuff)
                continue;
            NoTargetBuff effect = GetNoTargetBuff(config);
            if (effect.IsStackable)
            {
                _noTargetEffects[config.NoTargetBuffType].Stacks -= effect.Stacks;//只有stackable的buff可以被移除
                _noTargetEffects[config.NoTargetBuffType].IsFinished = true;
                _noTargetEffects[config.NoTargetBuffType].End();
                _noTargetEffects.Remove(config.NoTargetBuffType);
            }
        }
    }

    public void ApplyNoTargetEffects(Turret turret)
    {
        //if (turret._cardAsset.NotargetEffectList.Count <= 0)
        //    return;
        foreach (var config in turret._cardAsset.NotargetEffectList)
        {
            NoTargetBuff effect = GetNoTargetBuff(config);
            if (effect != null)
                AddEffect(effect);
        }
    }

    public void SpawnEnemy(EnemyRace race,int level)
    {
        Enemy enemy = _enemyFactory.GetEnemy(race,level);
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
