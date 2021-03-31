using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public static class NoTargetEffectFactory
{
    private static Dictionary<NoTargetEffectType, Type> effectByType;
    private static bool IsInitialized => effectByType != null;

    public static void InitializeFactory()
    {
        if (IsInitialized)
            return;

        var effectTypes = Assembly.GetAssembly(typeof(NoTargetEffect)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(NoTargetEffect)));

        effectByType = new Dictionary<NoTargetEffectType, Type>();

        foreach (var type in effectTypes)
        {
            var tempEffect = Activator.CreateInstance(type) as NoTargetEffect;
            effectByType.Add(tempEffect.NoTargetEffectType, type);
        }
    }

    public static NoTargetEffect GetEffect(NoTargetEffectType effectType)
    {
        InitializeFactory();

        if (effectByType.ContainsKey(effectType))
        {
            Type type = effectByType[effectType];
            var effect = Activator.CreateInstance(type) as NoTargetEffect;
            return effect;
        }
        return null;
    }
}
