using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public static class EffectFactory
{
    private static Dictionary<EffectType, Effect> effectByType;
    private static bool IsInitialized => effectByType != null;


    public static void InitializeFactory()
    {
        if (IsInitialized)
            return;

        var effectTypes = Assembly.GetAssembly(typeof(Effect)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Effect)));

        effectByType = new Dictionary<EffectType, Effect>();

        foreach(var type in effectTypes)
        {
            var tempEffect = Activator.CreateInstance(type) as Effect;
            effectByType.Add(tempEffect.EffectType, tempEffect);
        }
    }

    public static Effect GetEffect(EffectType effectType)
    {
        InitializeFactory();

        if (effectByType.ContainsKey(effectType))
        {
            Effect effect = effectByType[effectType];
            return effect;
        }
        return null;
    }

}
