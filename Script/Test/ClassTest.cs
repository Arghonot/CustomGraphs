﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[BlackboardType("string")]
public class Blackboard_String : Variable
{
    public override void DrawInspectorGUI()
    {
        EditorGUILayout.LabelField(Name);
        Value = (object)EditorGUILayout.TextField((string)Value);
    }

    public override void DrawNodeGUI()
    {
        throw new NotImplementedException();
    }

    public override object FromString(string newvalue)
    {
        Value = (object)newvalue;

        return Value;
    }

    public override string GetDefaultName()
    {
        return "DefaultString";
    }

    public override object GetDefaultValue()
    {
        return (object)"DefaultStringValue";
    }

    public override Type GetValueType()
    {
        return typeof(string);
    }

    public override string ToString()
    {
        return (string)Value;
    }
}

[BlackboardType("bool")]
public class Blackboard_Bool : Variable
{
    public override void DrawInspectorGUI()
    {
        EditorGUILayout.LabelField(Name);
        Value = (object)EditorGUILayout.Toggle((bool)Value);
    }

    public override void DrawNodeGUI()
    {
        throw new NotImplementedException();
    }

    public override object FromString(string newvalue)
    {
        if (newvalue == "True")
        {
            Value = (object)true;
        }
        else
        {
            Value = (object)false;
        }

        return Value;
    }

    public override string GetDefaultName()
    {
        return "DefaultBool";
    }

    public override object GetDefaultValue()
    {
        return (object)false;
    }

    public override Type GetValueType()
    {
        return typeof(bool);
    }

    public override string ToString()
    {
        return ((bool)Value).ToString();
    }
}

[BlackboardType("int")]
public class Blackboard_Int : Variable
{
    public override void DrawInspectorGUI()
    {
        EditorGUILayout.LabelField(Name);

        if (Value == null) return;

        Value = 
            (object)EditorGUILayout.IntField(
            (int)Value);
    }

    public override void DrawNodeGUI()
    {
        throw new NotImplementedException();
    }

    public override object FromString(string newvalue)
    {
        int testval = 0;
        if (int.TryParse(newvalue, out testval))
        {
            Value = (object)testval;
        }

        return Value;
    }

    public override string GetDefaultName()
    {
        return "DefaultInt";
    }

    public override object GetDefaultValue()
    {
        return (object)0;
    }

    public override Type GetValueType() 
    {
        return typeof(int);
    }

    public override string ToString()
    {
        return ((int)Value).ToString();
    }
}

public abstract partial class Variable
{
    public string Name;
    public object Value;
    public string TypeName;

    public Variable()
    {
        Name = GetDefaultName();
        Value = GetDefaultValue();
        TypeName = GetTypeName();
    }

    #region Implemented

    public static Variable CreateCopy(Variable original)
    {
        Variable copy = CreateType(original.TypeName);

        copy.Name = original.Name;
        copy.Value = original.Value;
        copy.TypeName = original.TypeName;

        return copy;
    }

    /// <summary>
    /// Create an instance from a type's name.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Variable CreateType(string type)
    {
        var ChildTypes = FindDerivedTypes(typeof(Variable));

        foreach (var customtype in ChildTypes)
        {
            BlackboardType bbtype = (BlackboardType)Attribute.GetCustomAttribute(
                customtype,
                typeof(BlackboardType));

            if (bbtype != null)
            {
                if (bbtype.TypeName == type)
                {
                    return (Variable)Activator.CreateInstance(customtype);
                }
            }
        }

        return null;
    }

    public static string GetType(Type type)
    {
        var ChildTypes = FindDerivedTypes(typeof(Variable));

        foreach (var item in ChildTypes)
        {
            if (item == type)
            {
                BlackboardType bbtype = (BlackboardType)Attribute.GetCustomAttribute(
                    item,
                    typeof(BlackboardType));

                return bbtype.TypeName;
            }
        }

        return string.Empty;
    }

    public static string[] GetTypes()
    {
        List<string> types = new List<string>();

        FindDerivedTypes(typeof(Variable)).ToList().ForEach(x =>
        {
            if (x != typeof(Variable))
            {
                types.Add(((BlackboardType)Attribute.GetCustomAttribute(
                    x,
                    typeof(BlackboardType))).TypeName);
            }
        });

        return types.ToArray();
    }

    private static IEnumerable<Type> FindDerivedTypes(Type baseType)
    {
        return Assembly.GetAssembly(baseType).GetTypes().
            Where(t => baseType.IsAssignableFrom(t));
    }

    public string GetTypeName()
    {
        return GetType(this.GetType());
    }

    public static explicit operator Variable(DumbVariable v)
    {
        Variable newvar = CreateType(v.TypeName);

        newvar.Name = v.Name;
        newvar.Value = new object();
        newvar.Value = v.Value;

        return newvar;
    }

    #endregion

    #region To be implemented

    public abstract void DrawInspectorGUI();
    public abstract void DrawNodeGUI();

    public abstract string GetDefaultName();
    public new abstract string ToString();
    public abstract object FromString(string newvalue);
    public abstract Type GetValueType();
    public abstract object GetDefaultValue(); // Necessary ?

    #endregion

}

[Serializable]
public class DumbVariable
{
    [SerializeField] public string Name;
    [SerializeField] public string Value;
    [SerializeField] public string TypeName;

    public DumbVariable()
    {
    }

    public DumbVariable(Variable source)
    {
        this.Name = source.Name;
        this.Value = source.ToString();
        this.TypeName = source.TypeName;
    }
}

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
public class BlackboardType : System.Attribute
{
    public string TypeName;

    public BlackboardType(string name)
    {
        TypeName = name;
    }
}