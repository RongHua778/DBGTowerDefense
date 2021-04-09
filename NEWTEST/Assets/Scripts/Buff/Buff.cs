using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{
    public bool IsFinished { get; internal set; }
    public abstract bool IsStackable { get;}
    public int Stacks;
    public bool IsInfinity { get; set; }
    public float Duration;

    public virtual void Affect(GameObject target) { }

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

    public void SetBuff(EnemyBuffConfig config)
    {
        this.Stacks = config.Stacks;
        this.IsInfinity = config.IsInfinity;
        this.Duration = config.Duration;
    }
    public abstract void End();
}
