using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph;
using System.Reflection;
using System.Linq;

// store the guid, names and types for every stored variable and handle unity's serialization
// callback to gain some time while managing datas.
public class GraphVariableStorageHelper : ISerializationCallbackReceiver
{
    protected Dictionary<string, string> GuidToNames;// = new Dictionary<string, string>();
    protected Dictionary<string, Type> GuidToType;// = new Dictionary<string, Type>();

    [SerializeField] private string[] guids;
    [SerializeField] private string[] names;
    [SerializeField] private Type[] types;

    public GraphVariableStorageHelper()
    {
        GuidToNames = new Dictionary<string, string>();
        GuidToType = new Dictionary<string, Type>();
    }

    public void OnBeforeSerialize()
    {
        int i = 0;

        if (GuidToNames == null || GuidToType == null)
        {
            GuidToNames = new Dictionary<string, string>();
            GuidToType = new Dictionary<string, Type>();
        }

        guids = new string[GuidToNames.Count()];
        names = new string[GuidToNames.Count()];
        types = new Type[GuidToNames.Count()];

        foreach (var item in GuidToNames)
        {
            guids[i] = item.Key;
            names[i] = item.Value;
            i++;
        }

        i = 0;
        foreach (var item in GuidToType)
        {
            types[i] = item.Value;
            i++;
        }
    }

    public void OnAfterDeserialize()
    {
        GuidToNames = new Dictionary<string, string>();
        GuidToType = new Dictionary<string, Type>();

        for (int i = 0; i < guids.Length; i++)
        {
            GuidToNames.Add(guids[i], names[i]);
            GuidToType.Add(guids[i], types[i]);
        }
    }
}

[Serializable]
public partial class GraphVariableStorage : GraphVariableStorageHelper
{
    [SerializeField] public List<StringVariable> Strings;
    [SerializeField] public List<DoubleVariable> Doubles;
    [SerializeField] public List<CurveVariable> AnimationCurves;
    [SerializeField] public List<TransformVariable> Transforms;

    // to be tested
    public T GetVariableStorage<T>(string Guid) where T : VariableStorageRoot
    {
        VariableStorageRoot value = GetContainerInstance(Guid);

        if (value != null)
        {
            return (T)value;
        }

        return default(T);
    }

    // to be tested
    public T GetValue<T>(string guid)
    {
        object container = GetContainerInstance(guid);
        MethodInfo GetValueMethod = container.GetType().GetMethod("GetValue");

        return (T)GetValueMethod.Invoke(container, new object[] { });
    }

    // to be tested
    public void SetValue(string Guid, object value)
    {
        object container = GetContainerInstance(Guid);
        MethodInfo AddMethod = container.GetType().GetMethod("Set");

        AddMethod.Invoke(container, new object[]
        {
            value
        });
    }

    // to be tested
    public void SetName(string Guid, string newName)
    {
        GetContainerInstance(Guid).Name = newName;
    }

    // to be tested
    public void Remove(string Guid)
    {
        IList container = GetListOfContainer(GetContainerType(Guid));

        foreach (var item in container)
        {
            if (((VariableStorageRoot)item).GUID == Guid)
            {
                container.Remove(item);
                break;
            }
        }
    }

    // to be implemented
    public void Compile(GraphVariableStorage storageToCompile)
    {
        throw new NotImplementedException();
    }

    // to be tested
    public bool ContainsValue(string guid)
    {
        return GuidToNames.ContainsKey(guid);
    }

    // return the Variable storage type
    // to be tested
    public Type GetContainerType(string guid)
    {
        return ((object)GetContainerInstance(guid)).GetType();
    }

    // Get the type of the variable stored
    // to be tested
    public Type GetStoredType(string guid)
    {
        VariableStorageRoot container = GetContainerInstance(guid);
        StorableType containerMetadata = (StorableType)Attribute.GetCustomAttribute(container.GetType(), typeof(StorableType));

        return containerMetadata.ReferenceType;
    }

    public void Flush()
    {
        IEnumerable<object> correspondingArrayRow =
            GetAllListOfContainers();

        foreach (var item in correspondingArrayRow)
        {
            ((IList)item).Clear();
        }

        GuidToNames.Clear();
        GuidToType.Clear();
    }

    public void CreateCopy(object variableInstanceToCopy)
    {
        List<Type> containerType = new List<Type>();
        // get the field that contains val and add a copy with activator
        var typesThatCanStoreVariable = Assembly.GetAssembly(typeof(VariableStorageRoot)).
            GetTypes().
            Where(t => typeof(VariableStorageRoot).IsAssignableFrom(t)).
            ToList();

        Type valAttribute = ((StorableType)Attribute.GetCustomAttribute(
                    variableInstanceToCopy.GetType(),
                    typeof(StorableType))).ReferenceType;
        typesThatCanStoreVariable.ForEach(x =>
        {
            if (x != typeof(VariableStorageRoot) && x != typeof(VariableStorage<>))
            {
                if (((StorableType)Attribute.GetCustomAttribute(
                    x,
                    typeof(StorableType))).ReferenceType == valAttribute)
                {
                    containerType.Add(x);
                }
            }
        });

        IList correspondingArrayRow =
            (IList)this.GetType().
            GetFields().
            Select(x => x.GetValue(this)).
            Where(x => x.ToString().Contains(containerType[0].ToString())).
            First();

        Type variableContainerType = variableInstanceToCopy.GetType();
        //ConstructorInfo VariableContainerConstructor = 
        //    variableStoredInContainerType.GetConstructor(new Type[]{ variableInstanceToCopy.GetType() });
        object newVariableContainerInstance = (Activator.CreateInstance(variableInstanceToCopy.GetType()));

        MethodInfo initializationMethod = variableContainerType.GetMethod("SetAsCopy");
        object variableContainer = initializationMethod.Invoke(newVariableContainerInstance, new object[]
        {
            variableInstanceToCopy
        });
        correspondingArrayRow.Add(variableContainer);

        VariableStorageRoot variableContainerAsGenericType = (VariableStorageRoot)variableContainer;

        GuidToNames.Add(variableContainerAsGenericType.GUID, variableContainerAsGenericType.Name);
        GuidToType.Add(variableContainerAsGenericType.GUID, variableContainerAsGenericType.GetType());

        Debug.Log("Adding a value of type " + variableContainerAsGenericType.GetType() + " and name " + variableContainerAsGenericType.Name + " Guid " + variableContainerAsGenericType.GUID);
        Debug.Log(variableContainerAsGenericType.GetType());

    }

    public void Add(Type typeToAdd, string Name = "")
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

        object newvalue = Activator.CreateInstance(types[0]);
        toto.Add(newvalue);
        VariableStorageRoot newval = (VariableStorageRoot)newvalue;
        newval.Name = Name;
        GuidToNames.Add(newval.GUID, newval.Name);
        GuidToType.Add(newval.GUID, newval.GetType());
    }

    private IEnumerable<IList> GetAllListOfContainers()
    {
        return 
            this.GetType().
            GetFields().
            Select(x => x.GetValue(this) as IList);
    }

    private IList GetListOfContainer(Type type)
    {
        List<Type> containerType = new List<Type>();
        // get the field that contains val and add a copy with activator
        var typesThatCanStoreVariable = Assembly.GetAssembly(typeof(VariableStorageRoot)).
            GetTypes().
            Where(t => typeof(VariableStorageRoot).IsAssignableFrom(t)).
            ToList();

        Type valAttribute = ((StorableType)Attribute.GetCustomAttribute(
                    type,
                    typeof(StorableType))).ReferenceType;
        typesThatCanStoreVariable.ForEach(x =>
        {
            if (x != typeof(VariableStorageRoot) && x != typeof(VariableStorage<>))
            {
                if (((StorableType)Attribute.GetCustomAttribute(
                    x,
                    typeof(StorableType))).ReferenceType == valAttribute)
                {
                    containerType.Add(x);
                }
            }
        });

        return
            (IList)this.GetType().
            GetFields().
            Select(x => x.GetValue(this)).
            Where(x => x.ToString().Contains(containerType[0].ToString())).
            First();
    }

    private VariableStorageRoot GetContainerInstance(string guid)
    {
        IList container = GetListOfContainer(GuidToType[guid]);

        foreach (var row in container)
        {
            if (((VariableStorageRoot)row).GUID == guid)
            {
                return (VariableStorageRoot)row;
            }
        }

        return null;
    }
}

public class DictionnaryV2 : MonoBehaviour
{
    public string NameOftypeToAdd;
    public string GUIDToGet;
    public string ValueToApply;
    public string NameToApply;

    [SerializeField] public GraphVariableStorage storage;

    [ContextMenu("FLUSH DEBUG")]
    public void DebugFlush()
    {
        storage.Flush();
    }

    [ContextMenu("Add string from type")]
    public void AddString()
    {
        storage.Add(typeof(string));
    }

    [ContextMenu("Add string from type with name")]
    public void AddStringWithName()
    {
        storage.Add(typeof(string), NameToApply);
    }

    [ContextMenu("CreateCopy string")]
    public void CreateCopyString()
    {
        storage.CreateCopy(storage.Strings.First());
    }





    [ContextMenu("Test get string")]
    public void TestGetString()
    {
        storage.GetVariableStorage<StringVariable>(GUIDToGet).ToString();
    }

    [ContextMenu("Test get string value")]
    public void TestGetStringValue()
    {
        Debug.Log("TestGetStringValue->" + storage.GetValue<string>(GUIDToGet));
    }

    [ContextMenu("Test set string value")]
    public void TestSetStringValue()
    {
        storage.SetValue(GUIDToGet, ValueToApply);
        storage.GetVariableStorage<StringVariable>(GUIDToGet).ToString();
    }

    //[ContextMenu("Test get string Name")]
    //public void TestSetStringName()
    //{
    //    storage.na(GUIDToGet, NameToApply);
    //    storage.GetVariableStorage<StringVariable>(GUIDToGet).ToString();
    //}

    [ContextMenu("Test remove string")]
    public void TestRemoveString()
    {
        storage.Remove(GUIDToGet);
    }

    [ContextMenu("Test contain string")]
    public void TestContainString()
    {
        Debug.Log("TestContainString->" + storage.ContainsValue(GUIDToGet));
    }

    [ContextMenu("Test get container type")]
    public void TestGetContainerType()
    {
        Debug.Log("TestGetContainerType->" + storage.GetContainerType(GUIDToGet));
    }

    [ContextMenu("Test get stored type")]
    public void TestGetStoredType()
    {
        Debug.Log("GetStoredType->" + storage.GetStoredType(GUIDToGet));
    }


    //[ContextMenu("Add double")]
    //public void AddTypeFromString()
    //{
    //    storage.Add(typeof(double));
    //}

    //[ContextMenu("Add Transform from type")]
    //public void AddTransform()
    //{
    //    storage.Add(typeof(Transform));
    //}

    //[ContextMenu("Add AnimationCurve from type")]
    //public void AddAnimationCurve()
    //{
    //    storage.Add(typeof(AnimationCurve));
    //}
    //[ContextMenu("CreateCopy double")]
    //public void CreateCopyDouble()
    //{
    //    storage.CreateCopy(storage.Doubles.First());
    //}

    //[ContextMenu("CreateCopy Transform")]
    //public void CreateCopyTransform()
    //{
    //    storage.CreateCopy(storage.Transforms.First());
    //}

    //[ContextMenu("CreateCopy animationCurve")]
    //public void CreateCopyAnimationCurve()
    //{
    //    storage.CreateCopy(storage.AnimationCurves.First());
    //}

    [ContextMenu("Get Value")]
    public void GetValue()
    {
        Debug.Log(storage.GetVariableStorage<StringVariable>(GUIDToGet).Value);
    }
}
