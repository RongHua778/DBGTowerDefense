using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BuffableTurret : BuffableEntity
{
    public Dictionary<TurretBuffType, TurretBuff> _effects = new Dictionary<TurretBuffType, TurretBuff>();
    Turret _turret;

    private void Start()
    {
        _turret = this.GetComponent<Turret>();
    }
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

    public void AddBuff(TurretBuff effect, bool stackable, int stacks, bool isInfinity, float duration)
    {
        if (_effects.ContainsKey(effect.EffectType))
        {
            TurretBuff effectItem = _effects[effect.EffectType];

            if (effect.IsStackable)
            {
                effectItem.EffectStacks += stacks;
            }
            if (effectItem.Duration < duration)
            {
                effectItem.Duration = duration;
            }
            effectItem.Affect(_turret);
        }
        else
        {
            effect.IsInfinity = isInfinity;
            effect.IsStackable = stackable;
            effect.EffectStacks += stacks;
            effect.Duration += duration;
            _effects.Add(effect.EffectType, effect);
            effect.Affect(_turret);
        }
    }

    public void ApplyEffects(IEnumerable<TurretBuffConfig> effectList)
    {
        foreach (var effectItem in effectList)
        {
            TurretBuff effect = TurretBuffFactory.GetEffect(effectItem.EffectType);
            AddBuff(effect, effectItem.Stackable, effectItem.Stacks, effectItem.IsInfinity, effectItem.Duration);
        }
    }

    public void ClearEffects()
    {
        _effects.Clear();
    }


}
