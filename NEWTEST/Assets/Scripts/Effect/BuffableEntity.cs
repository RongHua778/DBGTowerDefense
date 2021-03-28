using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class BuffableEntity : MonoBehaviour
{
    public Dictionary<EffectType, Buff> _effects = new Dictionary<EffectType, Buff>();
    public BuffTargetType BuffTargetType;
    // Update is called once per frame
    void Update()
    {
        foreach (var effect in _effects.Values.ToList())
        {
            effect.Tick(Time.deltaTime);
            if (effect.IsFinished)
            {
                _effects.Remove(effect.EffectType);
            }
        }
    }

    public void AddBuff(Buff effect, int stacks, float duration)
    {
        if (_effects.ContainsKey(effect.EffectType))
        {
            Buff effectItem = _effects[effect.EffectType];

            if (effect.IsStackable)
            {
                effectItem.EffectStacks += stacks;
            }
            if (effectItem.Duration < duration)
            {
                effectItem.Duration = duration;
            }
            effectItem.Affect(this.gameObject);
        }
        else
        {
            effect.EffectStacks += stacks;
            effect.Duration += duration;
            _effects.Add(effect.EffectType, effect);
            effect.Affect(this.gameObject);
        }
    }

    public void ApplyEffects(IEnumerable<MagicBuff> effectList)
    {
        foreach (var effectItem in effectList)
        {
            Buff effect = BuffFactory.GetEffect(effectItem.EffectType);
            if (effect.BuffTargetType != this.BuffTargetType)
                return;
            AddBuff(effect, effectItem.Stacks, effectItem.Duration);
        }
    }

    public void ClearEffects()
    {
        _effects.Clear();
    }
}
