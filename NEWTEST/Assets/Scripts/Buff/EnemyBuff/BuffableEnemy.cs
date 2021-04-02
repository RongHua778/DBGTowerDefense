using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BuffableEnemy : BuffableEntity
{


    public override void ApplyEffects(IEnumerable<BuffConfig> effectList)
    {
        foreach (var effectItem in effectList)
        {
            EnemyBuff effect = LevelManager.Instance.GetEnemyBuff((int)effectItem.BuffName);
            if (effect != null)
                AddBuff(effect, effectItem.Stacks, effectItem.IsInfinity, effectItem.Duration);
            Debug.Assert(effect != null, "EnemyBuff�������˴���Buff:" + effectItem.BuffName.ToString());
        }
    }


}
