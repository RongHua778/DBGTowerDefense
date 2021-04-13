using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BuffableEnemy : BuffableEntity
{
    public Dictionary<int, EnemyBuff> _buffs = new Dictionary<int, EnemyBuff>();
    void Update()
    {
        foreach (var effect in _buffs.Values.ToList())
        {
            effect.Tick(Time.deltaTime);
            if (effect.IsFinished)
            {
                _buffs.Remove((int)effect.enemyBuffName);
            }
        }
    }
    public void AddBuff(EnemyBuff newBuff)
    {

        if (_buffs.ContainsKey((int)newBuff.enemyBuffName))
        {
            Buff buff = _buffs[(int)newBuff.enemyBuffName];

            if (buff.IsStackable)//敌人BUFF可叠加的如燃烧都是不限时间BUFF
            {
                buff.Stacks += (int)newBuff.KeyValue;
                buff.Affect(this.gameObject);
            }
            else if (buff.Duration < newBuff.KeyValue)//不可叠加的如减速为时间限定BUFF
            {
                buff.Duration = newBuff.KeyValue;
            }
        }
        else
        {
            if (newBuff.IsStackable)
                newBuff.Stacks = (int)newBuff.KeyValue;
            else
                newBuff.Duration = newBuff.KeyValue;
            _buffs.Add((int)newBuff.enemyBuffName, newBuff);
            newBuff.Affect(this.gameObject);
        }
    }
    public void ApplyEffects(IEnumerable<EffectConfig> configList)
    {
        foreach (var config in configList)
        {
            if (config.BaseEffectType == EffectType.EnemyBuff)
            {
                EnemyBuff buff = LevelManager.Instance.GetEnemyBuff(config);
                if (buff != null)
                    AddBuff(buff);
            }
        }
    }

    public void ClearBuffs()
    {
        _buffs.Clear();
    }


}
