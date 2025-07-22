using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CustomGraph
{
    // NOTE: This class is functional but in early-stage design. 
    // It will be progressively refactored post-release (V2+) for performance, clarity, and encapsulation.
    [Serializable]
    public partial class GraphVariables : ISerializationCallbackReceiver, IEnumerable
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

        [HideInInspector] public Dictionary<string, string> GuidToNames;
        [HideInInspector] public Dictionary<string, Type> GuidToType;
        [HideInInspector] public Dictionary<string, VariableStorageRoot> GuidToStorage;
        private static Dictionary<Type, Type> StorageTypesPerRealType;
        private Dictionary<Type, IList> ListPerRealType;
        [HideInInspector]
        public Dictionary<string, string> PublicGUIDsToNames
        {
            get
            {
                return GuidToNames;
            }
        }
        [HideInInspector]
        public Dictionary<string, Type> PublicGUIDsToType
        {
            get
            {
                return GuidToType;
            }
        }

        protected Dictionary<KeyValuePair<string, Type>, string> NameTypeToGuid;

        public event Action<string> OnDataAddedOrRemoved;

        #region Inner storage management

        public GraphVariables()
        {
            FillSelfStorageMetadatas();
            ReinitObjectDictionnary();
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            ReinitObjectDictionnary();
        }

        [ContextMenu("ReinitObjectDictionnary")]
        private void ReinitObjectDictionnary()
        {
            GuidToNames = new Dictionary<string, string>();
            GuidToType = new Dictionary<string, Type>();
            GuidToStorage = new Dictionary<string, VariableStorageRoot>();

            FillSelfStorageMetadatas();

            foreach (var list in ListPerRealType)
            {
                foreach (var item in list.Value)
                {
                    GuidToStorage.Add(((VariableStorageRoot)item).GUID, (VariableStorageRoot)item);
                    GuidToNames.Add(((VariableStorageRoot)item).GUID, ((VariableStorageRoot)item).Name);
                    GuidToType.Add(((VariableStorageRoot)item).GUID, GetVariableTypeInContainer((VariableStorageRoot)item));
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
            var storedTypes = GetAllStorableTypes();

            storedTypes.ForEach(x =>
            {
                possibleTypes.Add(((StorableType)Attribute.GetCustomAttribute(x, typeof(StorableType))).ReferenceType);
            });

            return possibleTypes.ToArray();
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

        public string[] GetGUIDsForType(Type type)
        {
            return (GetListOfContainer(GetContainerContaningType(type)).Cast<VariableStorageRoot>()).Select(x => (x).GUID).ToArray();
        }

        // TODO store this in a static list on GraphVariableStorage's constructor
        public static List<Type> GetAllStorableTypes()
        {
            return Assembly.GetAssembly(typeof(VariableStorageRoot)).GetTypes().Where(t => typeof(VariableStorageRoot).IsAssignableFrom(t) && t != typeof(VariableStorageRoot) && t != typeof(VariableStorage<>)).ToList();
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
                    ((VariableStorageRoot)item).OnDestroy();
                    container.Remove(item);
                    break;
                }
            }

            GuidToNames.Remove(Guid);
            GuidToType.Remove(Guid);
            GuidToStorage.Remove(Guid);
            OnDataAddedOrRemoved?.Invoke(Guid);
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
            GuidToStorage.Clear();
        }

        #endregion

        #region Add

        public string CreateCopy(object variableInstanceToCopy, string optionalGUID = "")
        {
            List<Type> containerType = new List<Type>();
            // get the field that contains val and add a copy with activator
            var typesThatCanStoreVariable = GetAllStorableTypes();

            Type valAttribute = ((StorableType)Attribute.GetCustomAttribute(variableInstanceToCopy.GetType(),typeof(StorableType))).ReferenceType;
            typesThatCanStoreVariable.ForEach(x =>
            {
                if (((StorableType)Attribute.GetCustomAttribute(x, typeof(StorableType))).ReferenceType == valAttribute) containerType.Add(x);
            });

            IList correspondingArrayRow = (IList)this.GetType().GetFields().
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

            OnDataAddedOrRemoved?.Invoke(variableContainerAsGenericType.GUID);
            return variableContainerAsGenericType.GUID;
        }

        public string Add(VariableStorageRoot newVal, Type type)
        {
            var vals = FindCorrespondingList(type);

            vals.Add(newVal);
            GuidToNames.Add(newVal.GUID, newVal.Name);
            GuidToType.Add(newVal.GUID, GetVariableTypeInContainer(newVal));
            GuidToStorage.Add(newVal.GUID, newVal);
            OnDataAddedOrRemoved?.Invoke(newVal.GUID);

            return newVal.GUID;
        }

        public string Add(Type typeToAdd, string Name = "", string guid = "")
        {
#if UNITY_EDITOR
            if (ListPerRealType == null || StorageTypesPerRealType == null)
            {
                FillSelfStorageMetadatas();
            }

            if (ListPerRealType.Where(x => x.Value == null).Count() >= 0)
            {
                FillSelfStorageMetadatas();
            }
#endif

            List<Type> types = new List<Type>();
            IList listContainingType = ListPerRealType[typeToAdd];
            VariableStorageRoot newVariableStorage = (VariableStorageRoot)Activator.CreateInstance(StorageTypesPerRealType[typeToAdd]);

            listContainingType.Add(newVariableStorage);
            newVariableStorage.Name = Name;

            if (guid != "")
            {
                newVariableStorage.GUID = guid;
            }

            GuidToStorage.Add(newVariableStorage.GUID, newVariableStorage);
            GuidToNames.Add(newVariableStorage.GUID, newVariableStorage.Name);
            GuidToType.Add(newVariableStorage.GUID, GetVariableTypeInContainer(newVariableStorage));

            return newVariableStorage.GUID;
        }

        private IList FindCorrespondingList(Type type)
        {
            var ilists = this.GetType().GetFields().Where(x => x.FieldType.IsGenericType && x.FieldType.GetGenericTypeDefinition() == typeof(List<>));

            foreach (var ilist in ilists)
            {
                var listInnerType = (ilist.GetValue(this)).GetType().GenericTypeArguments[0];

                if (listInnerType == StorageTypesPerRealType[type])
                {
                    return (IList)ilist.GetValue(this);
                }
            }

            return null;
        }

        #endregion

        #region Getters

        public IEnumerator<VariableStorageRoot> GetEnumerator()
        {
            foreach (var list in ListPerRealType.Values)
            {
                foreach (VariableStorageRoot item in list)
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object TryGet(string name)
        {
            string guid = string.Empty;

            if (!GuidToNames.ContainsValue(name)) return null;
            guid = GetGUIDFromName(name);
            if (!GuidToStorage.ContainsKey(guid)) return null;

            return GuidToStorage[guid].GetValue();
        }

        public T Get<T>(string name)
        {
            return (T)GetFromGUID(GetGUIDFromName(name));
        }

        public string GetGUIDFromName(string name)
        {
            return GuidToNames.Where(x => x.Value == name).FirstOrDefault().Key;
        }

        public bool ContainsName(string name)
        {
            return GuidToNames.ContainsValue(name);
        }

        public Type GetVariableType(string guid)
        {
            return GuidToType[guid];
        }

        public int Count()
        {
            return GuidToNames.Count();
        }

        public object GetFromGUID(string Guid)
        {
            return GuidToStorage[Guid].GetValue();
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

        public VariableStorageRoot getContainerFromListFromID(string guid)
        {
            IList list = GetListOfContainer(StorageTypesPerRealType[GuidToType[guid]]);

            foreach (var item in list)
            {
                if (((VariableStorageRoot)item).GUID == guid)
                {
                    return (VariableStorageRoot)item;
                }
            }

            return null;
        }

        public T GetValue<T>(string guid)
        {
            return (T)GuidToStorage[guid].GetValue();
        }

        public VariableStorageRoot GetContainerInstance(string guid)
        {
            try
            {
                return GuidToStorage[guid];
            }
            catch (Exception e)
            {
                Debug.LogError($"Error getting container instance for GUID !!!{guid}: {e.Message} \n {e.StackTrace}");
                return GuidToStorage.First().Value;
            }
        }

        private Type GetContainerContaningType(Type type)
        {
            IEnumerable<IList> containers = GetAllListOfContainers();

            foreach (var item in containers)
            {
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
            return this.GetType().GetFields().Select(x => x.GetValue(this) as IList);
        }

        private IList GetListOfContainer(Type type)
        {
            List<Type> containerType = new List<Type>();
            // get the field that contains val and add a copy with activator
            var variableStorageTypes = GetAllStorableTypes();

            Type valAttribute = ((StorableType)Attribute.GetCustomAttribute(type, typeof(StorableType))).ReferenceType;
            variableStorageTypes.ForEach(x =>
            {
                if (((StorableType)Attribute.GetCustomAttribute(x, typeof(StorableType))).ReferenceType == valAttribute) containerType.Add(x);
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

        private void FillSelfStorageMetadatas()
        {
            var types = FindAllStorableRealTypes();

            FillStorageTypesPerRealType(types);
            FillListPerRealType(types);
        }

        private void FillListPerRealType(List<Type> types)
        {
            ListPerRealType = new Dictionary<Type, IList>();

            foreach (var item in types)
            {
                ListPerRealType.Add(item, FindCorrespondingList(item));
            }
        }

        private void FillStorageTypesPerRealType(List<Type> types)
        {
            StorageTypesPerRealType = new Dictionary<Type, Type>();

            foreach (var item in types)
            {
                StorageTypesPerRealType.Add(item, GetContainerContaningType(item));
            }
        }

        private List<Type> FindAllStorableRealTypes()
        {
            List<Type> vals = new List<Type>();
            List<Type> types = new List<Type>();

            vals = GetAllStorableTypes();
            vals.ForEach(x =>
            {
                    types.Add(GetAttributeFromContainerType(x));
            });

            return types;
        }

        public string GetGUIDFromNameAndType(string name, Type type)
        {
            var possibleCandidate = GuidToNames.Where(x => x.Value == name); 
            
            return possibleCandidate.Where(x => GuidToType[x.Key] == type).First().Key;
        }

        #endregion

        #region Setters

        public string SetWithGUID<T>(string guid, object obj)
        {
            if (!GuidToStorage.ContainsKey(guid)) guid = Add(typeof(T), "", guid);
            SetValue(guid, obj);

            Debug.Log($"setting with GUID {GetName(guid)} to {((T)obj).ToString()}");

            return guid;
        }

        public string Set<T>(string name, object obj)
        {
            string guid = GetGUIDFromName(name);

            if (guid == null)
                guid = Add(typeof(T), name);
            SetValue(guid, obj);

            return guid;
        }

        public void SetGUID(string from, string to)
        {
            VariableStorageRoot container = GetContainerInstance(from);
            Type containedType = GuidToType[from];

            container.setGuid(to);

            GuidToNames.Remove(from);
            GuidToType.Remove(from);
            GuidToStorage.Remove(from);

            GuidToNames.Add(to, container.Name);
            GuidToType.Add(to, containedType);
            GuidToStorage.Add(to, container);
        }

        public void SetName(string guid, string name)
        {
            GuidToNames[guid] = name;
            GetContainerInstance(guid).Name = name;
        }

        public void SetValue(string Guid, object value)
        {
            GetContainerInstance(Guid).SetValue(value);
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

        public void CompareDictionnaries(GraphVariables otherStorage)
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

                Debug.Log($"{(GuidToNames.ContainsKey(item) ? $"guid for {GuidToNames[item]} do match at least" : "guid don't event match")}");
            }

        }

        public void DebugDictionnary()
        {
            Debug.Log("DebugDictionnaries = " + ListPerRealType[typeof(int)].Count);
            foreach (var item in GuidToNames)
            {
                Debug.Log("[" + item.Key + "] [" + item.Value + "] [" + GuidToType[item.Key] + "]");
            }
        }


        public new string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("DebugDictionnaries = " + ListPerRealType[typeof(int)].Count);
            builder.AppendLine();
            foreach (var item in GuidToNames)
            {
                builder.Append(string.Join("", new string[]
                {
                    "[GUID][",
                    item.Key,
                    "] [Name][",
                    item.Value,
                    "] [Type][",
                    GuidToType[item.Key].ToString(),
                    "] [Value] [",
                GuidToStorage[item.Key].GetValue().ToString(),
                "]\n"
                }));
            }

            return builder.ToString();
        }


        public void DebugDictionnaryInDepth()
        {
            foreach (var item in ListPerRealType)
            {
                if (item.Value.Count > 0)
                {
                    foreach (var elem in item.Value)
                    {
                        UnityEngine.Debug.Log(((VariableStorageRoot)elem).ToString());
                    }
                }
            }
        }

        public void DebugList(Type type)
        {
            var vals = FindCorrespondingList(type);

            Debug.Log(vals);
            Debug.Log(vals.Count);

            foreach (var item in vals)
            {
                ((VariableStorageRoot)item).ToString();
            }
        }

        public GraphVariables CreateDeepCopy()
        {
            GraphVariables newStorage = new GraphVariables();

            foreach (var pair in ListPerRealType)
            {
                AddRow(newStorage, pair.Key, pair.Value);
            }

            return newStorage;
        }

        private void AddRow(GraphVariables newStorage, Type type, IList list)
        {
            VariableStorageRoot tmp;

            foreach (var item in list)
            {
                tmp = (VariableStorageRoot)((VariableStorageRoot)item).Clone();
                newStorage.Add(tmp, type);
            }
        }

        #endregion
    }
}