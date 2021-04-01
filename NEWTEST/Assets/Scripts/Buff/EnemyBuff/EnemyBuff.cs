using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBuff
{
    public abstract EnemyBuffType BuffType { get; }
    public bool IsFinished { get; internal set; }
    public bool IsStackable { get; set; }
    public bool IsInfinity { get; set; }
    public float Duration;
    public int EffectStacks;
    public Enemy Target;

    public abstract void Affect(Enemy target);

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

public class SlowDown : EnemyBuff
{
    public override EnemyBuffType BuffType => EnemyBuffType.SlowDown;
    public override void Affect(Enemy target)
    {
        Target = target;
        if (target != null)
        {
            target.SlowDown = true;
        }
    }

    public override void End()
    {
        Target.SlowDown = false;
    }
}
