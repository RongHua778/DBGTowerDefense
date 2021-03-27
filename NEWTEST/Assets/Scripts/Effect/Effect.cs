using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public abstract EffectType EffectType { get; }
    public abstract void Affect(GameObject target, float value);
}

public class Strength : Effect
{
    public override EffectType EffectType { get { return EffectType.Strength; } }

    public override void Affect(GameObject target, float value)
    {
        Turret turret = target.GetComponent<Turret>();
        if (turret != null)
        {
            target.GetComponent<Turret>().AttackDamage += value;
        }
    }
}

public class SpeedUp : Effect
{
    public override EffectType EffectType { get { return EffectType.SpeedUp; } }

    public override void Affect(GameObject target, float value)
    {
        Turret turret = target.GetComponent<Turret>();
        if (turret != null)
        {
            target.GetComponent<Turret>().AttackSpeed += value;
            //float nextAttackTime = target.GetComponent<Turret>().NextAttackTime;

            //target.GetComponent<Turret>().NextAttackTime = nextAttackTime - Mathf.Abs(nextAttackTime - 1 / value);
        }
    }
}

public class LongSighted : Effect
{
    public override EffectType EffectType { get { return EffectType.LongSighted; } }

    public override void Affect(GameObject target, float value)
    {
        Turret turret = target.GetComponent<Turret>();
        if (turret != null)
        {
            target.GetComponent<Turret>().AttackRange += value;
        }
    }
}



