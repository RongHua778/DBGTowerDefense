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
    public abstract void Affect(Projectile projectile, Enemy target);
}

public class RangeBaseDamage : AttackEffect
{
    
    public override AttackEffectTiming AttackEffectTiming => AttackEffectTiming.Damage;
    public override AttackEffectType AttackEffectType => AttackEffectType.RangeBaseDamage;
    public override void Affect(Projectile projectile, Enemy target)
    {
        float distance = projectile.GetTargetDistance();
        projectile.Damage = projectile.Damage * (1 + KeyValue * distance);
    }
}
