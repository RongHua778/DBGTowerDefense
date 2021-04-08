using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurretBuff:Buff
{
    public Turret Target;

}

public class Strength : TurretBuff
{
    public override BuffName buffName => BuffName.Strength;
    public override bool IsStackable => true;
    public override void Affect(GameObject target)
    {
        Target = target.GetComponent<Turret>();
        if (Target != null)
        {
            Target.AttackIntensify = 0.1f * Stacks;
        }
    }

    public override void End()
    {
        if (Target != null)
        {
            Target.AttackIntensify = 0f;
        }
    }
}

public class SpeedUp : TurretBuff
{
    public override BuffName buffName => BuffName.SpeedUp;

    public override bool IsStackable => true;
    public override void Affect(GameObject target)
    {
        Target = target.GetComponent<Turret>();
        if (Target != null)
        {
            Target.SpeedIntensify = 0.1f * Stacks;
        }
    }

    public override void End()
    {
        
    }
}

public class LongSighted : TurretBuff
{
    public override BuffName buffName => BuffName.LongSighted;
    public override bool IsStackable => true;
    public override void Affect(GameObject target)
    {
        Target = target.GetComponent<Turret>();
        if (Target != null)
        {
            Target.RangeIntensify = 0.1f * Stacks;
        }
    }

    public override void End()
    {
        if (Target != null)
        {
            Target.RangeIntensify = 0f;
        }
    }
}

public class Persist : TurretBuff
{
    public override BuffName buffName => BuffName.Persist;
    public override bool IsStackable => false;
    public override void Affect(GameObject target)
    {
        Target = target.GetComponent<Turret>();
        if (Target != null)
        {
            Target.PersistTime += 10f;
        }
    }

    public override void End()
    {

    }
}






