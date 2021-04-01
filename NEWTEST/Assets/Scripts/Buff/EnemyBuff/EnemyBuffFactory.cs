using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public class EnemyBuffFactory : MonoBehaviour
{
    private static Dictionary<EnemyBuffType, Type> effectByType;
    private static bool IsInitialized => effectByType != null;
    public static void InitializeFactory()
    {
        if (IsInitialized)
            return;

        var effectTypes = Assembly.GetAssembly(typeof(EnemyBuff)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(EnemyBuff)));

        effectByType = new Dictionary<EnemyBuffType, Type>();

        foreach (var type in effectTypes)
        {
            var tempEffect = Activator.CreateInstance(type) as EnemyBuff;
            effectByType.Add(tempEffect.BuffType, type);
        }
    }

    public static EnemyBuff GetEffect(EnemyBuffType effectType)
    {
        InitializeFactory();

        if (effectByType.ContainsKey(effectType))
        {
            Type type = effectByType[effectType];
            var effect = Activator.CreateInstance(type) as EnemyBuff;
            return effect;
        }
        return null;
    }
}
