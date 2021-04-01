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
    public override void Affect(GameObject target)
    {
        Target = target.GetComponent<Turret>();
        if (Target != null)
        {
            Target.AttackIntensify = 5f * Stacks;
        }
    }

    public override void End()
    {
        
    }
}

public class SpeedUp : TurretBuff
{
    public override BuffName buffName => BuffName.SpeedUp;
    public override void Affect(GameObject target)
    {
        Target = target.GetComponent<Turret>();
        if (Target != null)
        {
            Target.SpeedIntensify = 5f * Stacks;
        }
    }

    public override void End()
    {
        
    }
}

public class LongSighted : TurretBuff
{
    public override BuffName buffName => BuffName.LongSighted; 
    public override void Affect(GameObject target)
    {
        Target = target.GetComponent<Turret>();
        if (Target != null)
        {
            Target.RangeIntensify = 5f * Stacks;
        }
    }

    public override void End()
    {
        
    }
}






