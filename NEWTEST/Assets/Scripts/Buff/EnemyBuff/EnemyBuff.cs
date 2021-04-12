using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBuff : Buff
{
    public abstract EnemyBuffType enemyBuffName { get; }
    public Enemy Target;
}

public class SlowDown : EnemyBuff
{
    public override EnemyBuffType enemyBuffName => EnemyBuffType.SlowDown;
    public override bool IsStackable => false;

    public override bool IsInfinity => false;

    public override float Duration { get => KeyValue; set => KeyValue = value; }

    public override void Affect(GameObject target)
    {
        Target = target.GetComponent<Enemy>();
        if (Target != null)
        {
            Target.SlowDown = true;
        }
    }

    public override void End()
    {
        Target.SlowDown = false;
    }
}
