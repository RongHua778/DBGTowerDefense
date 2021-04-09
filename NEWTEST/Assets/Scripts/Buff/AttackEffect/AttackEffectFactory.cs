using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;

public class AttackEffectFactory:TypeFactory
{
    public override Type baseType => typeof(AttackEffect);

    public override void GenerateDIC(IEnumerable<Type> types)
    {
        foreach (Type type in types)
        {
            var buff = Activator.CreateInstance(type) as AttackEffect;
            TypeDic.Add((int)buff.AttackEffectName, type);
        }
    }

}
