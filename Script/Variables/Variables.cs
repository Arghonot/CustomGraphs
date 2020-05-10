using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


#region UNITY TYPES

[BlackboardType("GameObject")]
public class Blackboard_GameObject : Variable
{
    public override string GetDefaultName()
    {
        return "GameObject";
    }

    public override Type GetValueType()
    {
        return typeof(GameObject);
    }
}

[BlackboardType("Transform")]
public class Blackboard_Transform : Variable
{
    public override string GetDefaultName()
    {
        return "Transform";
    }

    public override Type GetValueType()
    {
        return typeof(Transform);
    }
}

[BlackboardType("Vector3")]
public class Blackboard_Vector3 : Variable
{
    public override string GetDefaultName()
    {
        return "Vector3";
    }

    public override Type GetValueType()
    {
        return typeof(Vector3);
    }
}

[BlackboardType("Quaternion")]
public class Blackboard_Quaternion : Variable
{
    public override string GetDefaultName()
    {
        return "Quaternion";
    }

    public override Type GetValueType()
    {
        return typeof(Quaternion);
    }
}


#endregion

#region BUILTIN TYPES

[BlackboardType("float")]
public class Blackboard_Float : Variable
{
    public override string GetDefaultName()
    {
        return "Float";
    }

    public override object GetDefaultValue()
    {
        return (object)0f;
    }

    public override Type GetValueType()
    {
        return typeof(float);
    }
}

[BlackboardType("long")]
public class Long : Variable
{
    public override string GetDefaultName()
    {
        return "Long";
    }

    public override object GetDefaultValue()
    {
        return (object)0L;
    }

    public override Type GetValueType()
    {
        return typeof(long);
    }
}

[BlackboardType("string")]
public class Blackboard_String : Variable
{
    public override string GetDefaultName()
    {
        return "String";
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
    public override string GetDefaultName()
    {
        return "Bool";
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
    public override string GetDefaultName()
    {
        return "Int";
    }

    public override object GetDefaultValue()
    {
        return (object)0;
    }

    public override Type GetValueType() 
    {
        return typeof(int);
    }
}

#endregion

[System.Serializable]
public class Variable
{
    [SerializeField] public string Name;
    [SerializeField] public string TypeName;

    public Variable()
    {
        Name = GetDefaultName();
        TypeName = GetType(this.GetType());
    }

    #region Implemented

    public static Variable CreateCopy(Variable original)
    {
        Variable copy = CreateType(original.TypeName);

        copy.Name = original.Name;
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
        if (type == typeof(Variable)) return "Variable";

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


    #endregion

    #region To be implemented
    public virtual void DrawNodeGUI(){    }

    public virtual string GetDefaultName() { return "Variable"; }
    public virtual Type GetValueType() { return typeof(Variable); }
    public virtual object GetDefaultValue() { return null; }
    #endregion

}

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
public class BlackboardType : System.Attribute
{
    public string TypeName;

    public BlackboardType(string name, bool useString = false)
    {
        TypeName = name;
    }
}