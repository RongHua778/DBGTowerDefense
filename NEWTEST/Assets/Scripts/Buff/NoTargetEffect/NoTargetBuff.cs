using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class NoTargetBuff : Buff
{
    public abstract NoTargetBuffName NoTargetBuffName { get; }

    public float KeyValue;
    public GameObject Target;
    public override void Affect(GameObject target)
    {
        Target = target;
    }

}

public class Overload : NoTargetBuff
{
    public override NoTargetBuffName NoTargetBuffName => NoTargetBuffName.Overload;

    public override BuffName buffName => BuffName.None;

    public override void Affect(GameObject target)
    {
        base.Affect(target);
        StaticData.Instance.NodeSpawnInterval -= KeyValue;
    }

    public override void End()
    {
        StaticData.Instance.NodeSpawnInterval += KeyValue;
    }
}
