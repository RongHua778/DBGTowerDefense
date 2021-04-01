using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public class TurretBuffFactory : TypeFactory
{
    public override Type baseType => typeof(TurretBuff);

    public override void GenerateDIC(IEnumerable<Type> types)
    {
        foreach (Type type in types)
        {
            var buff = Activator.CreateInstance(type) as TurretBuff;
            TypeDic.Add((int)buff.buffName, type);
        }
    }

}
