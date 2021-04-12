using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttributeType
{
    Cost,
    TurretAttack,
    TurretSpeed,
    TurretRange,
    SputterRange,
    CritcalRate,
    PersistTime,
    MagicRange,
    MagicAttack
}
public enum AttackEffectType
{
    RangeBaseDamage,
    Flame,
    SlowProjectile,
    RangeBaseSputtering
}

public enum EnemyBuffType
{
    SlowDown,
    OverLoad
}

[System.Serializable]
public struct AttributeConfig
{
    public AttributeType ChangeAttribute;
    public float Value;
}

[System.Serializable]
public struct AttackEffectConfig
{
    public AttackEffectType AttackEffectType;
    public float Value;
}

[System.Serializable]
public struct EnemyBuffConfig
{
    public EnemyBuffType EnemyBuffName;
    public float Value;

    public EnemyBuffConfig(EnemyBuffType name,float value)
    {
        this.EnemyBuffName = name;
        this.Value = value;
    }
}

public enum NoTargetBuffType
{
    Overload,
    Investment,
    MagicMaster,
    FastConveyor
}



[System.Serializable]
public struct NoTargetEffectConfig
{
    public NoTargetBuffType NoTargetBuffType;
    public float Duration;
    public float KeyValue;

    public NoTargetEffectConfig(NoTargetBuffType name, float duration, float keyValue)
    {
        this.NoTargetBuffType = name;
        this.Duration = duration;
        this.KeyValue = keyValue;
    }
}


[System.Serializable]
public struct CardEffectConfig
{
    public List<AttributeConfig> AttributeEffects;
    public List<AttackEffectConfig> AttackEffects;
    public List<EnemyBuffConfig> EnemyBuffs;
    public List<NoTargetEffectConfig> NoTargetEffects;

}

