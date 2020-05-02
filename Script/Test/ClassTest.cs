using System;
using UnityEngine;

[BlackboardType("String")]
public class Blackboard_String : Variable
{
    public static string    TypeName()
    {
        return "welsh";
    }

    public override void DrawUI()
    {
        throw new System.NotImplementedException();
    }

    public override string GetDefaultName()
    {
        return "StringVariable";
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

[BlackboardType("Bool")]
public class Blackboard_Bool : Variable
{

    public override void DrawUI()
    {
        throw new System.NotImplementedException();
    }

    public override string GetDefaultName()
    {
        throw new System.NotImplementedException();
    }

    public override object GetDefaultValue()
    {
        throw new System.NotImplementedException();
    }

    public override Type GetValueType()
    {
        throw new NotImplementedException();
    }
}

[BlackboardType("Int")]
public class Blackboard_Int : Variable
{
    public Blackboard_Int()
    {
        Debug.Log("Blackboard_Int");
        Debug.Log(Name + "  " + (int)Value);
    }

    public override void DrawUI()
    {
        throw new System.NotImplementedException();
    }

    // TODO is public necessary ?
    public override string GetDefaultName()
    {
        return "IntVariable";
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

    public Variable()
    {
        Name = GetDefaultName();
        Value = GetDefaultValue();

        Debug.Log("Variable");
    }

    public abstract void DrawUI();
    public abstract string GetDefaultName();
    public abstract Type GetValueType();
    public abstract object GetDefaultValue(); // Necessary ?
}

[System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct)
]
public class BlackboardType : System.Attribute
{
    private string TypeName;

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
