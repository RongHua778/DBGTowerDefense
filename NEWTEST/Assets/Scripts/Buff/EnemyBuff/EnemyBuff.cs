using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBuff : Buff
{
    public abstract EnemyBuffName enemyBuffName{get;}
    public Enemy Target;

}

public class SlowDown : EnemyBuff
{
    public override EnemyBuffName enemyBuffName => EnemyBuffName.SlowDown;
    public override bool IsStackable => false;
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
