using System;
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
}

[BlackboardType("int")]
public class Blackboard_Int : Variable
{
    public override void DrawInspectorGUI()
    {
        EditorGUILayout.LabelField(Name);
        Value = (object)EditorGUILayout.IntField((int)Value);
    }

    public override void DrawNodeGUI()
    {
        throw new NotImplementedException();
    }

    // TODO is public necessary ?
    public override string GetDefaultName()
    {
        return "DefaultInt";
    }

    // TODO is public necessary ?
    public override object GetDefaultValue()
    {
        return (object)0;
    }

    public override Type GetValueType()
    {
        return typeof(int);
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
        //Debug.Log("1");
        List<string> types = new List<string>();

        //Debug.Log("2");
        FindDerivedTypes(typeof(Variable)).ToList().ForEach(x =>
        {
            //Debug.Log("3");
            if (x != typeof(Variable))
            {
                types.Add(((BlackboardType)Attribute.GetCustomAttribute(
                    x,
                    typeof(BlackboardType))).TypeName);
            }
        });

        //Debug.Log("4");

        return types.ToArray();

        //var childtypes = FindDerivedTypes(typeof(Variable));
        //List<string> types = new List<string>();

        //foreach (var item in childtypes)
        //{
        //    BlackboardType bbtype = (BlackboardType)Attribute.GetCustomAttribute(
        //        item,
        //        typeof(BlackboardType));

        //    types.Add(bbtype.TypeName);
        //}

        //return types.ToArray();
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

    #endregion

    #region To be implemented

    public abstract void DrawInspectorGUI();
    public abstract void DrawNodeGUI();

    public abstract string GetDefaultName();
    public abstract Type GetValueType();
    public abstract object GetDefaultValue(); // Necessary ?

    #endregion

}

[System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct)
]
public class BlackboardType : System.Attribute
{
    public string TypeName;

    public BlackboardType(string name)
    {
        TypeName = name;
    }
}

//public class Myvar : Toto<int>
//{
//    public override void DrawUI()
//    {
//        Debug.Log("Myvar");

//        // display int UI
//    }
//}

//public class Myvarr : Toto<double>
//{
//    public override void DrawUI()
//    {
//        Debug.Log("Myvarr");

//        // display int UI
//    }
//}

//public abstract class Toto<T>
//{
//    public string name;

//    public virtual void DrawUI()
//    {
//        Debug.Log("Toto");
//        // display Generic object
//    }
//}

//public class Container
//{
//    public object test;
//}

////Toto<int> x;
////(Myvar) x.DisplayUI();

//[ExecuteInEditMode]
//public class ClassTest : MonoBehaviour
//{
//    Dictionary<string, Container> Testt = new Dictionary<string, Container>();
//    public bool Exec;

//    void Update()
//    {
//       if (Exec)
//       {
//            Toto<double> Welsh = new Myvarr();

//            Welsh.DrawUI();

//            Exec = false;
//       }
//    }
//}
