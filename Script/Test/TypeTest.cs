using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[ExecuteInEditMode]
public class TypeTest : MonoBehaviour
{
    public bool exe;

    private void Update()
    {
        if (exe)
        {
            exe = false;

            var types = FindDerivedTypes(typeof(Variable));

            foreach (var customtype in types)
            {
                Debug.Log(customtype.Name);
            }
        }
    }
    
    public IEnumerable<Type> FindDerivedTypes(Type baseType)
    {
        return Assembly.GetAssembly(baseType).GetTypes().Where(t => baseType.IsAssignableFrom(t));
    }
}
