using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BuffTargetType
{
    Turret,
    Enemy
}
public abstract class TargetEffect
{
    public abstract EffectType EffectType { get; }
    public abstract BuffTargetType BuffTargetType { get; }
    public bool IsFinished { get; internal set; }
    public abstract bool IsStackable { get; }
    public bool IsInfinity;
    public float Duration;
    public int EffectStacks;
    public GameObject Target;

    public abstract void Affect(GameObject target);

    public void Tick(float delta)
    {
        Duration -= delta;
        if (Duration <= 0)
        {
            End();
            IsFinished = true;
        }
    }

    public abstract void End();
}

public class Strength : TargetEffect
{
    public override EffectType EffectType { get { return EffectType.Strength; } }
    public override BuffTargetType BuffTargetType => BuffTargetType.Turret;
    public override bool IsStackable { get => true; }
    public override void Affect(GameObject target)
    {
        Target = target;
        Turret turret = target.GetComponent<Turret>();
        if (turret != null)
        {
            turret.AttackIntensify = 5f * EffectStacks;
        }
    }

    public override void End()
    {
        Turret turret = Target.GetComponent<Turret>();
        turret.AttackIntensify = 0;
    }
}

public class SpeedUp : TargetEffect
{
    public override EffectType EffectType { get { return EffectType.SpeedUp; } }
    public override BuffTargetType BuffTargetType => BuffTargetType.Turret;
    public override bool IsStackable { get => true; }
    public override void Affect(GameObject target)
    {
        Target = target;
        Turret turret = target.GetComponent<Turret>();
        if (turret != null)
        {
            turret.SpeedIntensify = 5f * EffectStacks;
        }
    }

    public override void End()
    {
        Turret turret = Target.GetComponent<Turret>();
        turret.SpeedIntensify = 0;
    }
}

public class LongSighted : TargetEffect
{
    public override EffectType EffectType { get { return EffectType.LongSighted; } }
    public override BuffTargetType BuffTargetType => BuffTargetType.Turret;
    public override bool IsStackable { get => true; }
    public override void Affect(GameObject target)
    {
        Target = target;
        Turret turret = target.GetComponent<Turret>();
        if (turret != null)
        {
            turret.RangeIntensify = 5f * EffectStacks;
        }
    }

    public override void End()
    {
        Turret turret = Target.GetComponent<Turret>();
        turret.RangeIntensify = 0;
    }
}

public class SlowDown : TargetEffect
{
    public override EffectType EffectType { get { return EffectType.SlowDown; } }
    public override BuffTargetType BuffTargetType => BuffTargetType.Enemy;
    public override bool IsStackable { get => false; }
    public override void Affect(GameObject target)
    {
        Target = target;
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.SlowDown = true;
        }
    }

    public override void End()
    {
        Enemy enemy = Target.GetComponent<Enemy>();
        enemy.SlowDown = false;
    }
}




