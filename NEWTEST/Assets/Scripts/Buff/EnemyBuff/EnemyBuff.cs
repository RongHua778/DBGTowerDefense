using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBuff:Buff
{
    public Enemy Target;

}

public class SlowDown : EnemyBuff
{
    public override BuffName buffName => BuffName.SlowDown;
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
