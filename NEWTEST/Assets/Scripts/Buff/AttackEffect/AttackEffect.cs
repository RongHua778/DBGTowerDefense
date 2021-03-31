using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackEffectTiming
{
    Shoot,
    Damage
}
public enum AttackEffectType
{
    RangeBaseDamage,
    Flame
}
public abstract class AttackEffect
{
    public float KeyValue;
    public abstract AttackEffectTiming AttackEffectTiming { get; }
    public abstract AttackEffectType AttackEffectType { get; }
    public abstract void Affect(object receiver, object giver);
}

public class RangeBaseDamage : AttackEffect
{
    
    public override AttackEffectTiming AttackEffectTiming => AttackEffectTiming.Shoot;
    public override AttackEffectType AttackEffectType => AttackEffectType.RangeBaseDamage;
    public override void Affect(object receiver, object giver)
    {
        Projectile projectile = receiver as Projectile;
        Turret turret = giver as Turret;
        float distance = projectile.GetTargetDistance();
        projectile.Damage = projectile.Damage * (1 + KeyValue * distance);
    }
}
