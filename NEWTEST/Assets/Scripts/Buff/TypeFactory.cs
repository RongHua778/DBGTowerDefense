using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public abstract class TypeFactory
{
    public abstract Type baseType { get; }
    public Dictionary<int, Type> TypeDic = new Dictionary<int, Type>();
    
    public void Initialize()
    {
        var types = Assembly.GetAssembly(baseType).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(baseType));
        GenerateDIC(types);
    }

    public abstract void GenerateDIC(IEnumerable<Type> types);

    public object GetType(int id)
    {
        if (TypeDic.ContainsKey(id))
        {
            Type type = TypeDic[id];
            var o = Activator.CreateInstance(type);
            return o;
        }
        return null;
    }


}
