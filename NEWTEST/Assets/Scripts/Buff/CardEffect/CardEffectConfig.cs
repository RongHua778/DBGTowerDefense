using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attribute
{
    Attack,
    Speed,
    Range,
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
    SlowProjectile
}

public enum EnemyBuffName
{
    SlowDown,
    OverLoad
}

[System.Serializable]
public struct AttributeEffectConfig
{
    public Attribute ChangeAttribute;
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
    public EnemyBuffName EnemyBuffName;
    public int Stacks;
    public bool IsInfinity;
    public float Duration;
    public float Value;

    public EnemyBuffConfig(EnemyBuffName name, int stack, bool isInfinity, float duration, float value)
    {
        this.EnemyBuffName = name;
        this.Stacks = stack;
        this.IsInfinity = isInfinity;
        this.Duration = duration;
        this.Value = value;
    }
}

public enum NoTargetBuffName
{
    Overload,
    Investment,
    MagicMaster,
    FastConveyor
}



[System.Serializable]
public struct NoTargetEffectConfig
{
    public NoTargetBuffName NoTargetBuffName;
    public float Duration;
    public float KeyValue;

    public NoTargetEffectConfig(NoTargetBuffName name, float duration, float keyValue)
    {
        this.NoTargetBuffName = name;
        this.Duration = duration;
        this.KeyValue = keyValue;
    }
}


[System.Serializable]
public struct CardEffectConfig
{
    public List<AttributeEffectConfig> AttributeEffects;
    public List<AttackEffectConfig> AttackEffects;
    public List<EnemyBuffConfig> EnemyBuffs;
    public List<NoTargetEffectConfig> NoTargetEffects;

}

