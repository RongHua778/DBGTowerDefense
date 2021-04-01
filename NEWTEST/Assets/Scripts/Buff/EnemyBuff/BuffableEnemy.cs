using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BuffableEnemy : BuffableEntity
{
    public Dictionary<EnemyBuffType, EnemyBuff> _effects = new Dictionary<EnemyBuffType, EnemyBuff>();
    Enemy _enemy;
    void Start()
    {
        _enemy = this.GetComponent<Enemy>();
    }

    void Update()
    {
        foreach (var effect in _effects.Values.ToList())
        {
            effect.Tick(Time.deltaTime);
            if (effect.IsFinished)
            {
                _effects.Remove(effect.BuffType);
            }
        }
    }

    public void AddBuff(EnemyBuff effect, bool stackable, int stacks, bool isInfinity, float duration)
    {
        if (_effects.ContainsKey(effect.BuffType))
        {
            EnemyBuff effectItem = _effects[effect.BuffType];

            if (effect.IsStackable)
            {
                effectItem.EffectStacks += stacks;
            }
            if (effectItem.Duration < duration)
            {
                effectItem.Duration = duration;
            }
            effectItem.Affect(_enemy);
        }
        else
        {
            effect.IsInfinity = isInfinity;
            effect.IsStackable = stackable;
            effect.EffectStacks += stacks;
            effect.Duration += duration;
            _effects.Add(effect.BuffType, effect);
            effect.Affect(_enemy);
        }
    }

    public void ApplyEffects(IEnumerable<EnemyBuffConfig> effectList)
    {
        foreach (var effectItem in effectList)
        {
            EnemyBuff effect = EnemyBuffFactory.GetEffect(effectItem.EffectType);
            AddBuff(effect, effectItem.Stackable, effectItem.Stacks, effectItem.IsInfinity, effectItem.Duration);
        }
    }

    public void ClearEffects()
    {
        _effects.Clear();
    }
}
