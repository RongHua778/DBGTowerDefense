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
            TypeDic.Add((int)buff.AttackEffectType, type);
        }
    }
    //private static Dictionary<AttackEffectType, Type> attackEffectByType;
    //private static bool IsInitialized => attackEffectByType != null;

    //public static void InitializeFactory()
    //{
    //    if (IsInitialized)
    //        return;
    //    var attackEffectTypes= Assembly.GetAssembly(typeof(AttackEffect)).GetTypes().
    //        Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(AttackEffect)));
    //    attackEffectByType = new Dictionary<AttackEffectType, Type>();
    //    foreach (var type in attackEffectTypes)
    //    {
    //        var tempEffect = Activator.CreateInstance(type) as AttackEffect;
    //        attackEffectByType.Add(tempEffect.AttackEffectType, type);
    //    }

    //}

    //public static AttackEffect GetEffect(AttackEffectType attackEffectType)
    //{
    //    InitializeFactory();

    //    if (attackEffectByType.ContainsKey(attackEffectType))
    //    {
    //        Type type = attackEffectByType[attackEffectType];
    //        var effect = Activator.CreateInstance(type) as AttackEffect;
    //        return effect;
    //    }
    //    return null;
    //}
}
