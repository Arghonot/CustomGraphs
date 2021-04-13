using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph;
using System.Reflection;
using System.Linq;

public partial class GraphVariableStorage
{
    [SerializeField] public List<StringVariable> Strings;
}

[Serializable]
public partial class GraphVariableStorage
{    
    [SerializeField] public List<DoubleVariable> Doubles;
    [SerializeField] public List<CurveVariable> AnimationCurves;
    [SerializeField] public List<TransformVariable> Transforms;

    public void CreateCopy(object typeToCopy)
    {
        List<Type> types = new List<Type>();
        // get the field that contains val and add a copy with activator
        var vals = Assembly.GetAssembly(typeof(VariableStorageRoot)).
            GetTypes().
            Where(t => typeof(VariableStorageRoot).
            IsAssignableFrom(t)).
            ToList();

        Type valAttribute = ((StorableType)Attribute.GetCustomAttribute(
                    typeToCopy.GetType(),
                    typeof(StorableType))).ReferenceType;
        vals.ForEach(x =>
        {
            if (x != typeof(VariableStorageRoot) && x != typeof(VariableStorage<>))
            {
                if (((StorableType)Attribute.GetCustomAttribute(
                    x,
                    typeof(StorableType))).ReferenceType == valAttribute)
                {
                    types.Add(x);
                }
            }
        });

        IList correspondingArrayRow =
            (IList)this.GetType().
            GetFields().
            Select(x => x.GetValue(this)).
            Where(x => x.ToString().Contains(types[0].ToString())).
            First();

        Type FinalVariableType = typeToCopy.GetType();
        ConstructorInfo FinalVariableConstructor = 
            FinalVariableType.GetConstructor(new Type[]{ typeToCopy.GetType() });
        var finalVariableObject = (Activator.CreateInstance(typeToCopy.GetType()));

        MethodInfo initializationMethod = FinalVariableType.GetMethod("SetAsCopy");
        object FinalVariable = initializationMethod.Invoke(finalVariableObject, new object[]
        {
            typeToCopy
        });
        correspondingArrayRow.Add(FinalVariable);
    }

    public void Add(Type typeToAdd)
    {
        List<Type> types = new List<Type>();

        var vals = Assembly.GetAssembly(typeof(VariableStorageRoot)).
            GetTypes().
            Where(t => typeof(VariableStorageRoot).
            IsAssignableFrom(t)).
            ToList();

        vals.ForEach(x =>
        {
            if (x != typeof(VariableStorageRoot) && x != typeof(VariableStorage<>))
            {
                if (((StorableType)Attribute.GetCustomAttribute(
                    x,
                    typeof(StorableType))).ReferenceType == typeToAdd)
                {
                    types.Add(x);
                }
            }
        });

        IList toto =
            (IList)this.GetType().
            GetFields().
            Select(x => x.GetValue(this)).
            Where(x => x.ToString().Contains(types[0].ToString())).
            First();

        toto.Add(Activator.CreateInstance(types[0]));
    }
}

public class DictionnaryV2 : MonoBehaviour
{
    public string NameOftypeToAdd;

    [SerializeField] public GraphVariableStorage storage;

    [ContextMenu("Add double")]
    public void AddTypeFromString()
    {
        storage.Add(typeof(double));
    }

    [ContextMenu("Add Transform from type")]
    public void AddTransform()
    {
        storage.Add(typeof(Transform));
    }

    [ContextMenu("Add AnimationCurve from type")]
    public void AddAnimationCurve()
    {
        storage.Add(typeof(AnimationCurve));
    }

    [ContextMenu("Add string from type")]
    public void AddString()
    {
        storage.Add(typeof(string));
    }

    [ContextMenu("CreateCopy string")]
    public void CreateCopyString()
    {
        storage.CreateCopy(storage.Strings.First());
    }

    [ContextMenu("CreateCopy double")]
    public void CreateCopyDouble()
    {
        storage.CreateCopy(storage.Doubles.First());
    }

    [ContextMenu("CreateCopy Transform")]
    public void CreateCopyTransform()
    {
        storage.CreateCopy(storage.Transforms.First());
    }

    [ContextMenu("CreateCopy animationCurve")]
    public void CreateCopyAnimationCurve()
    {
        storage.CreateCopy(storage.AnimationCurves.First());
    }
}
