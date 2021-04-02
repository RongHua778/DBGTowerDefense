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

    public override bool IsStackable => false;

    public override void Affect(GameObject target)
    {
        base.Affect(target);
        StaticData.Instance.NodeSpawnInterval /= KeyValue;
        //StaticData.Instance.NodeSpeed *= KeyValue;
    }

    public override void End()
    {
        StaticData.Instance.NodeSpawnInterval *= KeyValue;
        //StaticData.Instance.NodeSpeed /= KeyValue;
    }
}

public class Inverstment : NoTargetBuff
{
    public override NoTargetBuffName NoTargetBuffName => NoTargetBuffName.Investment;

    public override BuffName buffName => BuffName.None;

    public override bool IsStackable => false;

    public override void Affect(GameObject target)
    {
        base.Affect(target);
        //StaticData.Instance.BasicIncomeInterval /= KeyValue;
    }

    public override void End()
    {
        MoneySystem.CurrentMoney += (int)KeyValue;
        //StaticData.Instance.BasicIncomeInterval *= KeyValue;
    }
}

public class MagicMaster : NoTargetBuff
{
    public override NoTargetBuffName NoTargetBuffName => NoTargetBuffName.MagicMaster;

    public override BuffName buffName => BuffName.None;

    public override bool IsStackable => false;

    public override void Affect(GameObject target)
    {
        base.Affect(target);
        StaticData.Instance.MagicRangeIntensify += KeyValue;
    }

    public override void End()
    {
        StaticData.Instance.MagicRangeIntensify -= KeyValue;
    }
}
