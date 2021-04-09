using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class NoTargetBuff : Buff
{
    public abstract NoTargetBuffName NoTargetBuffName { get; }

    public GameObject Target;
    public override void Affect(GameObject target)
    {
        Target = target;
    }

}

public class Overload : NoTargetBuff
{
    public override NoTargetBuffName NoTargetBuffName => NoTargetBuffName.Overload;
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
    public override NoTargetBuffName NoTargetBuffName => NoTargetBuffName.Investment;

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
    public override NoTargetBuffName NoTargetBuffName => NoTargetBuffName.MagicMaster;


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
    public override NoTargetBuffName NoTargetBuffName => NoTargetBuffName.FastConveyor;


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
