using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{
    public bool IsFinished { get; internal set; }
    public abstract bool IsStackable { get;}
    public abstract float Duration { get; set; }
    public int Stacks;

    public float KeyValue;

    public void SetValue(float value)
    {
        if (IsStackable)
            Stacks = (int)value;
        else
            Duration = value;
    }

    public virtual void Affect(GameObject target) { }

    public void Tick(float delta)
    {
        if (IsStackable)
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
