using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public static class TurretBuffFactory
{
    private static Dictionary<TurretBuffType, Type> effectByType;
    private static bool IsInitialized => effectByType != null;
    public static void InitializeFactory()
    {
        if (IsInitialized)
            return;

        var effectTypes = Assembly.GetAssembly(typeof(TurretBuff)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(TurretBuff)));

        effectByType = new Dictionary<TurretBuffType, Type>();

        foreach(var type in effectTypes)
        {
            var tempEffect = Activator.CreateInstance(type) as TurretBuff;
            effectByType.Add(tempEffect.EffectType, type);
        }
    }

    public static TurretBuff GetEffect(TurretBuffType effectType)
    {
        InitializeFactory();

        if (effectByType.ContainsKey(effectType))
        {
            Type type = effectByType[effectType];
            var effect = Activator.CreateInstance(type) as TurretBuff;
            return effect;
        }
        return null;
    }

}
