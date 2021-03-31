using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class NoTargetEffect
{
    public abstract NoTargetEffectType NoTargetEffectType { get; }
    public abstract bool IsStackable { get; }
    public float Duration;
    public float KeyValue;
    public bool IsFinished { get; internal set; }

    public void Tick(float delta)
    {
        Duration -= delta;
        if (Duration <= 0)
        {
            End();
            IsFinished = true;
        }
    }
    public abstract void Affect();
    public abstract void End();

}

public class Overload : NoTargetEffect
{
    public override NoTargetEffectType NoTargetEffectType => NoTargetEffectType.Overload;

    public override bool IsStackable => false;

    public override void Affect()
    {
        StaticData.Instance.NodeSpawnInterval -= KeyValue;
    }

    public override void End()
    {
        StaticData.Instance.NodeSpawnInterval += KeyValue;
    }
}
