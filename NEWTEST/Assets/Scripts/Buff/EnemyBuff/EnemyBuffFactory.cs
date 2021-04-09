using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public class EnemyBuffFactory : TypeFactory
{
    public override Type baseType => typeof(EnemyBuff);

    public override void GenerateDIC(IEnumerable<Type> types)
    {
        foreach (Type type in types)
        {
            var buff = Activator.CreateInstance(type) as EnemyBuff;
            TypeDic.Add((int)buff.enemyBuffName, type);
        }
    }
}
