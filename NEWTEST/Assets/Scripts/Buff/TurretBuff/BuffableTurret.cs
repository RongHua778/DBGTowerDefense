using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BuffableTurret : BuffableEntity
{

    public override void ApplyEffects(IEnumerable<BuffConfig> effectList)
    {
        foreach (var effectItem in effectList)
        {
            TurretBuff effect = LevelManager.Instance.GetTurretBuff((int)effectItem.BuffName);
            if (effect != null)
                AddBuff(effect,effectItem.Stacks, effectItem.IsInfinity, effectItem.Duration);
            Debug.Assert(effect != null, "TurretBuff÷–≈‰÷√¡À¥ÌŒÛBuff:" + effectItem.BuffName.ToString());
        }
    }



}
