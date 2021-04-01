using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{
    public abstract BuffName buffName { get; }
    public bool IsFinished { get; internal set; }
    public bool IsStackable { get; set; }
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
    public abstract void End();
}
