using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public static class TargetEffectFactory
{
    private static Dictionary<EffectType, Type> effectByType;
    private static bool IsInitialized => effectByType != null;
    public static void InitializeFactory()
    {
        if (IsInitialized)
            return;

        var effectTypes = Assembly.GetAssembly(typeof(TargetEffect)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(TargetEffect)));

        effectByType = new Dictionary<EffectType, Type>();

        foreach(var type in effectTypes)
        {
            var tempEffect = Activator.CreateInstance(type) as TargetEffect;
            effectByType.Add(tempEffect.EffectType, type);
        }
    }

    public static TargetEffect GetEffect(EffectType effectType)
    {
        InitializeFactory();

        if (effectByType.ContainsKey(effectType))
        {
            Type type = effectByType[effectType];
            var effect = Activator.CreateInstance(type) as TargetEffect;
            return effect;
        }
        return null;
    }

}
