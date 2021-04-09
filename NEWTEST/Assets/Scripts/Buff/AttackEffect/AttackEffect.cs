using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackEffectTiming
{
    Shoot,
    Hit
}

public abstract class AttackEffect
{
    public float KeyValue;
    public abstract AttackEffectType AttackEffectName { get; }
    public abstract AttackEffectTiming AttackEffectTiming { get; }
    public abstract void Affect(Projectile projectile, Enemy target);
    public void SetValue(AttackEffectConfig config)
    {
        KeyValue = config.Value;
    }
}

public class RangeBaseSputtering : AttackEffect
{

    public override AttackEffectType AttackEffectName => AttackEffectType.RangeBaseSputtering;

    public override AttackEffectTiming AttackEffectTiming => AttackEffectTiming.Shoot;

    public override void Affect(Projectile projectile, Enemy target)
    {
        float distance = projectile.GetTargetDistance();
        projectile.SputteringRange = projectile.SputteringRange * (1 + KeyValue * distance);
    }
}

public class RangeBaseDamage : AttackEffect
{

    public override AttackEffectType AttackEffectName => AttackEffectType.RangeBaseDamage;

    public override AttackEffectTiming AttackEffectTiming => AttackEffectTiming.Shoot;

    public override void Affect(Projectile projectile, Enemy target)
    {
        float distance = projectile.GetTargetDistance();
        projectile.Damage = projectile.Damage * (1 + KeyValue * distance);
    }
}

public class SlowProjectile : AttackEffect
{

    public override AttackEffectType AttackEffectName => AttackEffectType.SlowProjectile;

    public override AttackEffectTiming AttackEffectTiming => AttackEffectTiming.Hit;

    public override void Affect(Projectile projectile, Enemy target)
    {
        EnemyBuffConfig config = new EnemyBuffConfig(EnemyBuffName.SlowDown, 1f);
        EnemyBuff buff = LevelManager.Instance.GetEnemyBuff(config);
        target.GetComponent<BuffableEnemy>().AddBuff(buff);
    }
}
