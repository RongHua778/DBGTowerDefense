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

            if (buff.IsStackable)
            {
                buff.Stacks += newBuff.Stacks;
            }
            if (buff.Duration < newBuff.Duration)
            {
                buff.Duration = newBuff.Duration;
            }
            buff.Affect(this.gameObject);
        }
        else
        {
            _buffs.Add((int)newBuff.enemyBuffName, newBuff);
            newBuff.Affect(this.gameObject);
        }
    }
    public void ApplyEffects(IEnumerable<EnemyBuffConfig> effectList)
    {
        foreach (var buffConfig in effectList)
        {
            EnemyBuff buff = LevelManager.Instance.GetEnemyBuff(buffConfig);
            AddBuff(buff);
            Debug.Assert(buff != null, "EnemyBuff÷–≈‰÷√¡À¥ÌŒÛBuff:" + buffConfig.EnemyBuffName.ToString());
        }
    }

    public void ClearBuffs()
    {
        _buffs.Clear();
    }


}
