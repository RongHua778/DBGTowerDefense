using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public class NoTargetEffectFactory:TypeFactory
{
    public override Type baseType => typeof(NoTargetBuff);


    public override void GenerateDIC(IEnumerable<Type> types)
    {
        foreach (Type type in types)
        {
            var buff = Activator.CreateInstance(type) as NoTargetBuff;
            TypeDic.Add((int)buff.NoTargetBuffName, type);
        }
    }
}
