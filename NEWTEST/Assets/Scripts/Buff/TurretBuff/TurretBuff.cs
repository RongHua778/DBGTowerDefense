using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurretBuff
{
    public abstract TurretBuffType EffectType { get; }
    public bool IsFinished { get; internal set; }
    public bool IsStackable { get; set; }
    public bool IsInfinity { get; set; }
    public float Duration;
    public int EffectStacks;
    public object Target;

    public abstract void Affect(object target);

    public void Tick(float delta)
    {
        if (IsInfinity)
            return;
        Duration -= delta;
        if (Duration <= 0)
        {
            End();
            IsFinished = true;
        }
    }

    public abstract void End();
}

public class Strength : TurretBuff
{
    public override TurretBuffType EffectType { get { return TurretBuffType.Strength; } }
    public override void Affect(object target)
    {
        Turret turret = target as Turret;
        if (turret != null)
        {
            turret.AttackIntensify = 5f * EffectStacks;
        }
    }

    public override void End()
    {
        
    }
}

public class SpeedUp : TurretBuff
{
    public override TurretBuffType EffectType { get { return TurretBuffType.SpeedUp; } }
    public override void Affect(object target)
    {
        Turret turret = target as Turret;
        if (turret != null)
        {
            turret.SpeedIntensify = 5f * EffectStacks;
        }
    }

    public override void End()
    {
        
    }
}

public class LongSighted : TurretBuff
{
    public override TurretBuffType EffectType { get { return TurretBuffType.LongSighted; } }
    public override void Affect(object target)
    {
        Turret turret = target as Turret;
        if (turret != null)
        {
            turret.RangeIntensify = 5f * EffectStacks;
        }
    }

    public override void End()
    {
        
    }
}






