using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public static class BuffFactory
{
    private static Dictionary<EffectType, Type> effectByType;
    private static bool IsInitialized => effectByType != null;
    public static void InitializeFactory()
    {
        if (IsInitialized)
            return;

        var effectTypes = Assembly.GetAssembly(typeof(Buff)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Buff)));

        effectByType = new Dictionary<EffectType, Type>();

        foreach(var type in effectTypes)
        {
            var tempEffect = Activator.CreateInstance(type) as Buff;
            effectByType.Add(tempEffect.EffectType, type);
        }
    }

    public static Buff GetEffect(EffectType effectType)
    {
        InitializeFactory();

        if (effectByType.ContainsKey(effectType))
        {
            Type type = effectByType[effectType];
            var effect = Activator.CreateInstance(type) as Buff;
            return effect;
        }
        return null;
    }

}
