using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public abstract class BuffableEntity : MonoBehaviour
{


    //public void AddBuff(Buff newBuff)
    //{

    //    if (_buffs.ContainsKey((int)newBuff.buffName))
    //    {
    //        Buff buff = _buffs[(int)newBuff.buffName];

    //        if (buff.IsStackable)
    //        {
    //            buff.Stacks += newBuff.Stacks;
    //        }
    //        if (buff.Duration < newBuff.Duration)
    //        {
    //            buff.Duration = newBuff.Duration;
    //        }
    //        buff.Affect(this.gameObject);
    //    }
    //    else
    //    {
    //        _buffs.Add((int)newBuff.buffName, newBuff);
    //        newBuff.Affect(this.gameObject);
    //    }
    //}

    //public virtual void ApplyEffects(IEnumerable<object> effectList)
    //{

    //}

    

}
