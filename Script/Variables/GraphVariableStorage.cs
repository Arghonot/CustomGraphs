using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Graph
{
    // TODO make the load subgraph work
    public class GraphVariableStorageHelper : ISerializationCallbackReceiver
    {
        [HideInInspector] protected Action OnFinishedSerializationProcess;
        [HideInInspector] public Dictionary<string, string> GuidToNames;
        [HideInInspector] public Dictionary<string, Type> GuidToType;
        [HideInInspector] public Dictionary<string, object> GuidToValue;
        [HideInInspector] public Dictionary<string, string> PublicGUIDsToNames
        {
            get
            {
                return GuidToNames;
            }
        }
        [HideInInspector] public Dictionary<string, Type> PublicGUIDsToType
        {
            get
            {
                return GuidToType;
            }
        }


        // to be implemented
        protected Dictionary<KeyValuePair<string, Type>, string> NameTypeToGuid;

        [SerializeField] private string[] guids;
        [SerializeField] private string[] names;
        [SerializeField] private Type[] types;
        // This is basically a backup in case a compilation occured since the last serialization
        // pass, if so 'types' will always be null, so we can still recover it's value by storing its 
        // stored type's name and execute a less performance effective alogrythm to find the type.
        [SerializeField] private string[] typeString;

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
            typeString = new string[GuidToNames.Count()];

            foreach (var item in GuidToNames)
            {
                guids[i] = item.Key;
                names[i] = item.Value;
                types[i] = GuidToType[item.Key];
                typeString[i] = types[i].AssemblyQualifiedName;
                i++;
            }
        }

        public void OnAfterDeserialize()
        {
            GuidToNames.Clear();
            GuidToType.Clear();
            GuidToValue.Clear();

            if (types == null)
            {
                SetTypes();
            }

            for (int i = 0; i < guids.Length; i++)
            {
                GuidToNames.Add(guids[i], names[i]);
                GuidToType.Add(guids[i], types[i]);
            }

            OnFinishedSerializationProcess?.Invoke();
        }

        private void SetTypes()
        {
            types = new Type[typeString.Length];

            for (int i = 0; i < typeString.Length; i++)
            {
                types[i] = Type.GetType(typeString[i]);
            }
        }
    }

    // TODO use the dictionnaries as much as possible for every classes
    // TODO decide what should be public and what should be private
    // TODO do a system of event for variable modification so we don't get their value every frames
    // TODO Should all the set and get handled directly by this class or the class that contains this class ?
    [Serializable]
    public partial class GraphVariableStorage : GraphVariableStorageHelper
    {
        public string GUID = Guid.NewGuid().ToString();
         
        // C# types
        [SerializeField] public List<floatVariable> Floats = new List<floatVariable>();
        [SerializeField] public List<LongVariable> Longs = new List<LongVariable>();
        [SerializeField] public List<BoolVariable> Bools = new List<BoolVariable>();
        [SerializeField] public List<IntVariable> Ints = new List<IntVariable>();
        [SerializeField] public List<DoubleVariable> Doubles = new List<DoubleVariable>();
        [SerializeField] public List<StringVariable> Strings = new List<StringVariable>();

        // Unity types
        [SerializeField] public List<AnimationCurveVariable> AnimationCurves = new List<AnimationCurveVariable>();
        [SerializeField] public List<TransformVariable> Transforms = new List<TransformVariable>();
        [SerializeField] public List<NavMeshAgentVariable> NavmeshAgents = new List<NavMeshAgentVariable>();
        [SerializeField] public List<GameObjectVariable> GameOjbects = new List<GameObjectVariable>();
        [SerializeField] public List<Vector3Variable> Vector3s = new List<Vector3Variable>();
        [SerializeField] public List<QuaternionVariable> Quaternions = new List<QuaternionVariable>();

        #region Inner storage management


        public GraphVariableStorage()
        {
            OnFinishedSerializationProcess += ReinitObjectDictionnary;
            ReinitObjectDictionnary();

            GuidToValue = new Dictionary<string, object>();
        }

        ~GraphVariableStorage()
        {
            OnFinishedSerializationProcess -= ReinitObjectDictionnary;
        }

        [ContextMenu("ReinitObjectDictionnary")]
        private void ReinitObjectDictionnary()
        {
            GuidToValue = new Dictionary<string, object>();

            foreach (var item in GuidToNames)
            {
                if (GetContainerInstance(item.Key) == null)
                {
                }
                else
                {
                    GuidToValue.Add(item.Key, GetValue<object>(item.Key));
                }
            }
        }
           
        #endregion

        #region Functions for UI management

        public int GetAmountOfGUIFields()
        {
            return GuidToNames.Count + GuidToType.Count;
        }

        public string[] getAllGuids()
        {
            return GuidToNames.Select(x => x.Key).ToArray();
        }

        public static Type[] getPossibleTypes()
        {
            List<Type> possibleTypes = new List<Type>();
            var vals = Assembly.GetAssembly(typeof(VariableStorageRoot)).
                GetTypes().
                Where(t => typeof(VariableStorageRoot).
                IsAssignableFrom(t)).
                ToList();

            vals.ForEach(x =>
            {
                if (x != typeof(VariableStorageRoot) && x != typeof(VariableStorage<>))
                {
                    possibleTypes.Add(((StorableType)Attribute.GetCustomAttribute(x, typeof(StorableType))).ReferenceType);
                }
            });

            return possibleTypes.ToArray();
        }

        public static string[] GetPossibleTypesName()
        {
            List<string> PossibleTypeNames = new List<string>();
            var vals = Assembly.GetAssembly(typeof(VariableStorageRoot)).
                GetTypes().
                Where(t => typeof(VariableStorageRoot).
                IsAssignableFrom(t)).
                ToList();

            vals.ForEach(x =>
            {
                if (x != typeof(VariableStorageRoot) && x != typeof(VariableStorage<>))
                {
                    PossibleTypeNames.Add(((StorableType)Attribute.GetCustomAttribute(x, typeof(StorableType))).ReferenceType.Name);
                }
            });

            return PossibleTypeNames.ToArray();
        }

        public string GetName(string guid)
        {
            return GuidToNames[guid];
        }

        public string[] GetAllNames()
        {
            return GuidToNames.Select(x => x.Value).ToArray();
        }

        public string[] GetNames(string[] guids)
        {
            return GuidToNames.Where(x => guids.Contains(x.Key)).Select(x => x.Value).ToArray();
        }

        #endregion

        #region Variable storage management

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

            GuidToNames.Remove(Guid);
            GuidToType.Remove(Guid);
            GuidToValue.Remove(Guid);
        }

        public GraphVariableStorage Merge(GraphVariableStorage storageToCompile)
        {
            // Because we can't modify a collection we are iterating on
            List<KeyValuePair<string, string>> GuidFromToList = new List<KeyValuePair<string, string>>();

            Debug.Log(this.GUID);

            foreach (var item in storageToCompile.PublicGUIDsToNames)
            {
                // if contains the same type and name
                if (!ContainsGuid(item.Key) && ContainsName(storageToCompile.GetName(item.Key)) &&
                    storageToCompile.GetContainedType(item.Key) == GetContainedType(GetGUIDFromName(item.Value)))
                {
                    Debug.Log("was contained already " + item.Value);
                    // prepare to change an already existing item that we don't really need to re create
                    GuidFromToList.Add(new KeyValuePair<string, string>(
                        GetGUIDFromNameAndType(item.Value, storageToCompile.GetContainedType(item.Key)),
                        item.Key));
                }
                // if doesn't contains it
                else if (!ContainsGuid(item.Key))
                {
                    Debug.Log("wasn't contained already " + item.Value);
                    // brand new 
                    CreateCopy(storageToCompile.GetContainerInstance(item.Key), item.Key);

                    //GuidToType.Add(item.Key, item.Value);
                    //GuidToNames.Add(item.Key, storageToCompile.GetName(item.Key));
                }
            }

            // we rename the ones that need renaming
            for (int i = 0; i < GuidFromToList.Count; i++)
            {
                Debug.Log("Setting guid from : |" + GuidFromToList[i].Key + "| to |" + GuidFromToList[i].Value + "|");

                SetGUID(GuidFromToList[i].Key, GuidFromToList[i].Value);
            }

            return this;
        }

        public void Flush()
        {
            IEnumerable<object> correspondingArrayRow =
                GetAllListOfContainers();

            foreach (var item in correspondingArrayRow)
            {
                if (item != null)
                {
                    ((IList)item).Clear();
                }
            }

            GuidToNames.Clear();
            GuidToType.Clear();
            GuidToValue.Clear();
        }

        #endregion

        #region Add

        public string CreateCopy(object variableInstanceToCopy, string optionalGUID = "")
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
            object newVariableContainerInstance = (Activator.CreateInstance(variableInstanceToCopy.GetType()));

            MethodInfo initializationMethod = variableContainerType.GetMethod("SetAsCopy");
            object variableContainer = initializationMethod.Invoke(newVariableContainerInstance, new object[]
            {
            variableInstanceToCopy
            });
            correspondingArrayRow.Add(variableContainer);

            VariableStorageRoot variableContainerAsGenericType = (VariableStorageRoot)variableContainer;

            if (optionalGUID != "")
            {
                variableContainerAsGenericType.setGuid(optionalGUID);
            }

            GuidToNames.Add(variableContainerAsGenericType.GUID, variableContainerAsGenericType.Name);
            GuidToType.Add(variableContainerAsGenericType.GUID, GetVariableTypeInContainer(variableContainerAsGenericType));
            GuidToValue.Add(variableContainerAsGenericType.GUID, GetValue<object>(variableContainerAsGenericType.GUID));

            return variableContainerAsGenericType.GUID;
        }

        public string Add(Type typeToAdd, string Name = "", string guid = "")
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

            if (guid != "")
            {
                newval.GUID = guid;
            }

            GuidToNames.Add(newval.GUID, newval.Name);
            GuidToType.Add(newval.GUID, GetVariableTypeInContainer(newval));
            GuidToValue.Add(newval.GUID, GetValue<object>(newval.GUID));

            return newval.GUID;
        }

        #endregion

        #region Getters

        public string GetGUIDFromName(string name)
        {
            return GuidToNames.Where(x => x.Value == name).First().Key;
        }

        public bool ContainsName(string name)
        {
            return GuidToNames.ContainsValue(name);
        }

        public int Count()
        {
            return GuidToNames.Count();
        }

        public object Get(string Guid)
        {
            return GuidToValue[Guid];
        }

        public T GetVariableStorage<T>(string Guid) where T : VariableStorageRoot
        {
            VariableStorageRoot value = GetContainerInstance(Guid);

            if (value != null)
            {
                return (T)value;
            }

            return default(T);
        }

        public T GetValue<T>(string guid)
        {
            object container = GetContainerInstance(guid);
            MethodInfo GetValueMethod = container.GetType().GetMethod("GetValue");

            return (T)GetValueMethod.Invoke(container, new object[] { });
        }

        public VariableStorageRoot GetContainerInstance(string guid)
        {
            // we get the list containing this instance
            Type TypeOfVariable = GetContainerContaningType(GuidToType[guid]);

            // couldn't find an appropriate type for this guid
            if (TypeOfVariable == typeof(VariableStorageRoot))
            {
                return null;
            }

            IList container = GetListOfContainer(TypeOfVariable);

            foreach (var row in container)
            {
                if (((VariableStorageRoot)row).GUID == guid)
                {
                    return (VariableStorageRoot)row;
                }
            }

            return null;
        }

        private Type GetContainerContaningType(Type type)
        {
            IEnumerable<IList> containers = GetAllListOfContainers();

            foreach (var item in containers)
            {
                // we might have debug variables that are not variable storage
                if (item != null)
                {
                    var storedType = item.GetType().GetGenericArguments().Single();

                    if (GetAttributeFromContainerType(storedType) == type)
                    {
                        return storedType;
                    }
                }
            }

            return typeof(VariableStorageRoot);
        }

        private Type GetAttributeFromContainerType(Type containerType)
        {
            return ((StorableType)Attribute.GetCustomAttribute(containerType, typeof(StorableType))).ReferenceType;
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

        // return the Variable storage type
        public Type GetContainerType(string guid)
        {
            return ((object)GetContainerInstance(guid)).GetType();
        }

        // Get the type of the variable stored
        public Type GetContainedType(string guid)
        {
            VariableStorageRoot container = GetContainerInstance(guid);

            return GetVariableTypeInContainer(container);
        }

        private Type GetVariableTypeInContainer(VariableStorageRoot container)
        {
            StorableType containerMetadata = (StorableType)Attribute.GetCustomAttribute(container.GetType(), typeof(StorableType));

            return containerMetadata.ReferenceType;
        }

        public string GetGUIDFromNameAndType(string name, Type type)
        {
            Debug.Log(name + " " + type);

            var possibleCandidate = GuidToNames.Where(x => x.Value == name); 
            
            return possibleCandidate.Where(x => GuidToType[x.Key] == type).First().Key;
        }

        #endregion

        #region Setters

        public void SetGUID(string from, string to)
        {
            VariableStorageRoot container = GetContainerInstance(from);
            object containedValue = GetValue<object>(from);
            Type containedType = GuidToType[from];

            container.setGuid(to);

            GuidToNames.Remove(from);
            GuidToValue.Remove(from);
            GuidToType.Remove(from);

            GuidToNames.Add(to, container.Name);
            GuidToValue.Add(to, containedValue);
            GuidToType.Add(to, containedType);
        }

        public void SetName(string guid, string name)
        {
            GuidToNames[guid] = name;
            GetContainerInstance(guid).Name = name;
        }

        public void SetValue(string Guid, object value)
        {
            object container = GetContainerInstance(Guid);
            MethodInfo AddMethod = container.GetType().GetMethod("Set");

            AddMethod.Invoke(container, new object[]
            {
            value
            });
        }

        #endregion

        #region Check

        public bool ContainName(string name)
        {
            return GuidToNames.ContainsValue(name);
        }

        public bool ContainsGuid(string guid)
        {
            return GuidToNames.ContainsKey(guid);
        }

        #endregion

        #region DEBUG

        public void CompareDictionnaries(GraphVariableStorage otherStorage)
        {
            IEnumerable<IList> list = otherStorage.GetAllListOfContainers();
            var Containers = GetAllListOfContainers();

            Debug.Log("Containers");
            foreach (var item in list)
            {
                if (Containers.Contains(item))
                {
                    Debug.Log("True");
                }
                else
                {
                    Debug.Log("False container");
                }
            }

            Debug.Log("GUIDs");
            foreach (var item in otherStorage.getAllGuids())
            {
                if (GuidToNames.ContainsKey(item))
                {
                    Debug.Log("True");
                }
                else
                {
                    Debug.Log("GUIDs False : " + item);
                }
            }

            Debug.Log("Names");
            foreach (var item in otherStorage.GetAllNames())
            {
                if (GuidToNames.ContainsKey(item))
                {
                    Debug.Log("True");
                }
                else
                {
                    Debug.Log("Names False : " + item);
                }
            }

            foreach (var item in otherStorage.getAllGuids())
            {
                if (otherStorage.GetContainerInstance(item) != GetContainerInstance(item))
                {
                    Debug.Log("Container not the same : " + item);
                }
                else
                {
                    Debug.Log("container equivalent : " + item);
                }
                if (otherStorage.GetContainerInstance(item) == otherStorage.GetContainerInstance(item))
                {
                    Debug.Log("container equal itself.");
                }
            }

        }

        public void DebugDictionnaries()
        {
            foreach (var item in GuidToNames)
            {
                Debug.Log("[" + item.Key + "] [" + item.Value + "] [" + GuidToType[item.Key] + "]");
            }
        }

        #endregion
    }
}