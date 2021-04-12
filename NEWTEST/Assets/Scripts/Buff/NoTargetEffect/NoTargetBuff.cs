using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class NoTargetBuff : Buff
{
    public abstract NoTargetBuffType NoTargetBuffType { get; }

    public GameObject Target;
    public override void Affect(GameObject target)
    {
        Target = target;
    }

}

public class Overload : NoTargetBuff
{
    public override NoTargetBuffType NoTargetBuffType => NoTargetBuffType.Overload;
    public override bool IsStackable => false;

    public override bool IsInfinity => false;

    public override float Duration { get => KeyValue; set => KeyValue = value; }

    public override void Affect(GameObject target)
    {
        base.Affect(target);
        StaticData.Instance.NodeSpawnInterval /= KeyValue;

    }

    public override void End()
    {
        StaticData.Instance.NodeSpawnInterval *= KeyValue;
    }
}

public class Inverstment : NoTargetBuff
{
    public override NoTargetBuffType NoTargetBuffType => NoTargetBuffType.Investment;

    public override bool IsStackable => false;

    public override bool IsInfinity => false;

    public override float Duration { get => KeyValue; set => KeyValue = value; }

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
    public override NoTargetBuffType NoTargetBuffType => NoTargetBuffType.MagicMaster;


    public override bool IsStackable => false;

    public override bool IsInfinity => false;

    public override float Duration { get => KeyValue; set => KeyValue = value; }

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

public class FastConveyor : NoTargetBuff
{
    public override NoTargetBuffType NoTargetBuffType => NoTargetBuffType.FastConveyor;


    public override bool IsStackable => false;

    public override bool IsInfinity => false;

    public override float Duration { get => KeyValue; set => KeyValue = value; }

    public override void Affect(GameObject target)
    {
        base.Affect(target);
        StaticData.Instance.NodeSpeed += KeyValue;
    }

    public override void End()
    {
        StaticData.Instance.NodeSpeed -= KeyValue;

    }
}
