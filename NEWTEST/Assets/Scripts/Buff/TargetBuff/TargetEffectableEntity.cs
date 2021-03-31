using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class TargetEffectableEntity : MonoBehaviour
{
    public Dictionary<EffectType, TargetEffect> _effects = new Dictionary<EffectType, TargetEffect>();
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

    public void AddBuff(TargetEffect effect, int stacks, float duration)
    {
        if (_effects.ContainsKey(effect.EffectType))
        {
            TargetEffect effectItem = _effects[effect.EffectType];

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

    public void ApplyEffects(IEnumerable<TargetEffectConfig> effectList)
    {
        foreach (var effectItem in effectList)
        {
            TargetEffect effect = TargetEffectFactory.GetEffect(effectItem.EffectType);
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
