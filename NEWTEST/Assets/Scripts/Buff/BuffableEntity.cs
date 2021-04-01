using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public abstract class BuffableEntity : MonoBehaviour
{
    public Dictionary<int, Buff> _buffs = new Dictionary<int, Buff>();
    void Update()
    {
        foreach (var effect in _buffs.Values.ToList())
        {
            effect.Tick(Time.deltaTime);
            if (effect.IsFinished)
            {
                _buffs.Remove((int)effect.buffName);
            }
        }
    }

    public void AddBuff(Buff effect, bool stackable, int stacks, bool isInfinity, float duration)
    {

        if (_buffs.ContainsKey((int)effect.buffName))
        {
            Buff effectItem = _buffs[(int)effect.buffName];

            if (effect.IsStackable)
            {
                effectItem.Stacks += stacks;
            }
            if (effectItem.Duration < duration)
            {
                effectItem.Duration = duration;
            }
            effectItem.Affect(this.gameObject);
        }
        else
        {
            effect.IsInfinity = isInfinity;
            effect.IsStackable = stackable;
            effect.Stacks += stacks;
            effect.Duration += duration;
            _buffs.Add((int)effect.buffName, effect);
            effect.Affect(this.gameObject);
        }
    }

    public virtual void ApplyEffects(IEnumerable<BuffConfig> effectList)
    {

    }

    public void ClearBuffs()
    {
        _buffs.Clear();
    }

}
