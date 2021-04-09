using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackEffect
{
    public float KeyValue;
    public abstract AttackEffectType AttackEffectType { get; }
    public abstract void Affect(Projectile projectile, Enemy target);
    public void SetValue(AttackEffectConfig config)
    {
        KeyValue = config.Value;
    }
}

public class RangeBaseDamage : AttackEffect
{

    public override AttackEffectType AttackEffectType => AttackEffectType.RangeBaseDamage;
    public override void Affect(Projectile projectile, Enemy target)
    {
        float distance = projectile.GetTargetAndTurretDistance();
        projectile.Damage = projectile.Damage * (1 + KeyValue * distance);
    }
}

public class SlowProjectile : AttackEffect
{

    public override AttackEffectType AttackEffectType => AttackEffectType.SlowProjectile;
    public override void Affect(Projectile projectile, Enemy target)
    {
        EnemyBuffConfig config = new EnemyBuffConfig(EnemyBuffName.SlowDown, 1, false, 3f, 0.5f);
        EnemyBuff buff = LevelManager.Instance.GetEnemyBuff(config);
        target.GetComponent<BuffableEnemy>().AddBuff(buff);
    }
}
