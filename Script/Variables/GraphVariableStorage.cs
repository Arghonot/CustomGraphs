using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;

namespace Graph
{
    // TODO make the load subgraph work
    public class GraphVariableStorageHelper
    {
        [HideInInspector] protected Action OnFinishedSerializationProcess;
        [HideInInspector] public Dictionary<string, string> GuidToNames;
        [HideInInspector] public Dictionary<string, Type> GuidToType;
        [HideInInspector] public Dictionary<string, object> GuidToValue;
        [HideInInspector] public Dictionary<string, VariableStorageRoot> GuidToStorage;

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

        [SerializeField] protected string[] guids;
        [SerializeField] protected string[] names;
        [SerializeField] protected Type[] types;
        // This is basically a backup in case a compilation occured since the last serialization
        // pass, if so 'types' will always be null, so we can still recover it's value by storing its 
        // stored type's name and execute a less performance effective alogrythm to find the type.
        [SerializeField] protected string[] typeString;

        public GraphVariableStorageHelper()
        {
            GuidToNames = new Dictionary<string, string>();
            GuidToType = new Dictionary<string, Type>();
            GuidToStorage = new Dictionary<string, VariableStorageRoot>();
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
    public partial class GraphVariableStorage : GraphVariableStorageHelper, ISerializationCallbackReceiver
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
            ReinitObjectDictionnary();
            OnFinishedSerializationProcess += ReinitObjectDictionnary;

            GuidToValue = new Dictionary<string, object>();

            FillSelfStorageMetadatas();
        }

        ~GraphVariableStorage()
        {
            OnFinishedSerializationProcess -= ReinitObjectDictionnary;
        }


        // we save the GUIDs as a fast way to restore the object.
        // later we just have to retrieve the object instance from the list and add its
        // GUID in the dictionnary.
        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            GuidToNames.Clear();
            GuidToType.Clear();
            GuidToValue.Clear();
            GuidToStorage.Clear();

            ReinitObjectDictionnary();

            //if (types == null)
            //{
            //    SetTypes();
            //}

            //for (int i = 0; i < guids.Length; i++)
            //{
            //    GuidToNames.Add(guids[i], names[i]);
            //    GuidToType.Add(guids[i], types[i]);
            //}

            //OnFinishedSerializationProcess?.Invoke();
        }

        [ContextMenu("ReinitObjectDictionnary")]
        private void ReinitObjectDictionnary()
        {
            GuidToValue = new Dictionary<string, object>();
            GuidToStorage = new Dictionary<string, VariableStorageRoot>();
            FillSelfStorageMetadatas();

            foreach (var list in ListPerRealType)
            {
                foreach (var item in list.Value)
                {
                    GuidToStorage.Add(((VariableStorageRoot)item).GUID, (VariableStorageRoot)item);
                    GuidToNames.Add(((VariableStorageRoot)item).GUID, ((VariableStorageRoot)item).Name);
                    GuidToType.Add(((VariableStorageRoot)item).GUID, GetVariableTypeInContainer((VariableStorageRoot)item));
                    GuidToValue.Add(((VariableStorageRoot)item).GUID, ((VariableStorageRoot)item).GetValue());
                }
            }

            //foreach (var item in GuidToNames)
            //{
            //    if (!GuidToStorage.ContainsKey(item.Key))
            //    {
            //        GuidToStorage.Add(item.Key, getContainerFromListFromID(item.Key));
            //    }
            //    if (!GuidToValue.ContainsKey(item.Key))
            //    {
            //        GuidToValue.Add(item.Key, GetValue<object>(item.Key));
            //    }
            //}

            Debug.Log(GuidToStorage.Count());
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


            foreach (var item in storageToCompile.PublicGUIDsToNames)
            {
                // if contains the same type and name
                if (!ContainsGuid(item.Key) && ContainsName(storageToCompile.GetName(item.Key)) &&
                    storageToCompile.GetContainedType(item.Key) == GetContainedType(GetGUIDFromName(item.Value)))
                {
                    // prepare to change an already existing item that we don't really need to re create
                    GuidFromToList.Add(new KeyValuePair<string, string>(
                        GetGUIDFromNameAndType(item.Value, storageToCompile.GetContainedType(item.Key)),
                        item.Key));
                }
                // if doesn't contains it
                else if (!ContainsGuid(item.Key))
                {
                    // brand new 
                    CreateCopy(storageToCompile.GetContainerInstance(item.Key), item.Key);

                    //GuidToType.Add(item.Key, item.Value);
                    //GuidToNames.Add(item.Key, storageToCompile.GetName(item.Key));
                }
            }

            // we rename the ones that need renaming
            for (int i = 0; i < GuidFromToList.Count; i++)
            {
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

        public string Add(VariableStorageRoot newVal, Type type)
        {
            var vals = FindCorrespondingList(type);

            vals.Add(newVal);
            GuidToNames.Add(newVal.GUID, newVal.Name);
            GuidToType.Add(newVal.GUID, GetVariableTypeInContainer(newVal));
            GuidToValue.Add(newVal.GUID, GetValue<object>(newVal.GUID));

            return newVal.GUID;
        }

        public string Add(Type typeToAdd, string Name = "", string guid = "")
        {
            List<Type> types = new List<Type>();

            IList toto = ListPerRealType[typeToAdd];
            object newvalue = Activator.CreateInstance(StorageTypesPerRealType[typeToAdd]);
            toto.Add(newvalue);
            VariableStorageRoot newval = (VariableStorageRoot)newvalue;
            newval.Name = Name;

            // TODO shouldn't it be == instead of !=
            if (guid != "")
            {
                newval.GUID = guid;
            }

            GuidToStorage.Add(newval.GUID, newval);
            GuidToNames.Add(newval.GUID, newval.Name);
            GuidToType.Add(newval.GUID, GetVariableTypeInContainer(newval));
            GuidToValue.Add(newval.GUID, GetValue<object>(newval.GUID));

            return newval.GUID;
        }

        private IList FindCorrespondingList(Type type)
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
                        typeof(StorableType))).ReferenceType == type)
                    {
                        types.Add(x);

                    }
                }
            });

            return
                (IList)this.GetType().
                GetFields().
                Select(x => x.GetValue(this)).
                Where(x => x.ToString().Contains(types[0].ToString())).
                First();
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
            object container = GetContainerInstance(guid);

            return (T)((VariableStorageRoot)container).GetValue();

        }

        public VariableStorageRoot GetContainerInstance(string guid)
        {
            return GuidToStorage[guid];
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

        Dictionary<Type, Type> RealTypeToContainerType;
        Dictionary<Type, IList> RealTypeToList;

        public void StressTestContainer(int Amount)
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            StringBuilder builder = new StringBuilder();
            List<Type> vals = new List<Type>();
            List<Type> types = new List<Type>();
            Add(typeof(int), "");
            watch.Start();

            for (int i = 0; i < Amount; i++)
            {
                GetVariableTypeInContainer(Ints[0]);
            }
            builder.Append("\nFind container " + watch.ElapsedMilliseconds);

            for (int i = 0; i < Amount; i++)
            {
                vals = Assembly.GetAssembly(typeof(VariableStorageRoot)).
                GetTypes().
                Where(t => typeof(VariableStorageRoot).
                IsAssignableFrom(t)).
                ToList();
            }
            builder.Append("\nGet assembly " + watch.ElapsedMilliseconds);
            for (int i = 0; i < Amount; i++)
            {

                vals.ForEach(x =>
                {
                    // TODO add a break if adding to types
                    if (x != typeof(VariableStorageRoot) && x != typeof(VariableStorage<>))
                    {
                        if (((StorableType)Attribute.GetCustomAttribute(
                            x,
                            typeof(StorableType))).ReferenceType == typeof(int))
                        {
                            types.Add(x);
                        }
                    }
                });
            }
            builder.Append("\nFind types" + watch.ElapsedMilliseconds);
            IList toto = null;
            for (int i = 0; i < Amount; i++)
            {
                toto = (IList)this.GetType().
                GetFields().
                Select(x => x.GetValue(this)).
                Where(x => x.ToString().Contains(types[0].ToString())).
                First();
            }
            builder.Append("\nFindLists" + watch.ElapsedMilliseconds);
            object newvalue = null;

            for (int i = 0; i < Amount; i++)
            {
                newvalue = Activator.CreateInstance(types[0]);
                toto.Add(newvalue);
            }
            builder.Append("\nCreate variable instance " + watch.ElapsedMilliseconds);

            for (int i = 0; i < Amount; i++)
            {
                var guid = Guid.NewGuid().ToString();
                ((VariableStorageRoot)newvalue).GUID = guid;
                GuidToNames.Add(((VariableStorageRoot)newvalue).GUID, "");
                GuidToType.Add(((VariableStorageRoot)newvalue).GUID, GetVariableTypeInContainer((VariableStorageRoot)newvalue));
                GuidToValue.Add(((VariableStorageRoot)newvalue).GUID, GetValue<object>(((VariableStorageRoot)newvalue).GUID));
            }
            builder.Append("\nStore metadatas " + watch.ElapsedMilliseconds);
            var prevValue = 0l;
            for (int i = 0; i < Amount; i++)
            {
                GetValue<object>(((VariableStorageRoot)newvalue).GUID);
            }
            builder.Append("\nCall to getValue " + (watch.ElapsedMilliseconds));

            prevValue = 0l;
            for (int i = 0; i < Amount; i++)
            {
                var guid = Guid.NewGuid().ToString();
            }
            builder.Append("\nGenerating GUIDs " + (watch.ElapsedMilliseconds));
            var id = ((VariableStorageRoot)newvalue).GUID;
            for (int i = 0; i < Amount; i++)
            {
                GetContainerInstance(id);
            }
            builder.Append("\nGenerating GUIDs " + (watch.ElapsedMilliseconds));
            UnityEngine.Debug.Log(builder.ToString());
        }

        private static Dictionary<Type, Type> StorageTypesPerRealType;
        private static Dictionary<Type, IList> ListPerRealType;

        private void FillSelfStorageMetadatas()
        {
            var types = FindAllStorableRealTypes();

            if (StorageTypesPerRealType == null)
            {
                FillStorageTypesPerRealType(types);
            }
            if (ListPerRealType == null)
            {
                FillListPerRealType(types);
            }
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

        private List<Type> FindAllStorageTypes()
        {
            return Assembly.GetAssembly(typeof(VariableStorageRoot)).
                GetTypes().
                Where(t => typeof(VariableStorageRoot).
                IsAssignableFrom(t)).
                ToList();
        }

        private List<Type> FindAllStorableRealTypes()
        {
            List<Type> vals = new List<Type>();
            List<Type> types = new List<Type>();

            vals = FindAllStorageTypes();
            vals.ForEach(x =>
            {
                // TODO add a break if adding to types
                if (x != typeof(VariableStorageRoot) && x != typeof(VariableStorage<>))
                {
                    types.Add(GetAttributeFromContainerType(x));
                }
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

        private List<Type> GetStorableTypes()
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
                    types.Add(((StorableType)Attribute.GetCustomAttribute(
                        x,
                        typeof(StorableType))).ReferenceType);
                }
            });

            return types;
        }

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
            Debug.Log("DebugDictionnaries = " + ListPerRealType[typeof(int)].Count);
            foreach (var item in GuidToNames)
            {
                Debug.Log("[" + item.Key + "] [" + item.Value + "] [" + GuidToType[item.Key] + "]");
            }
        }

        public void DebugDictionnaryInDepth()
        {
            DebugList(typeof(float));
            DebugList(typeof(int));
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

        public GraphVariableStorage CreateDeepCopy()
        {
            List<Type> types = GetStorableTypes();
            GraphVariableStorage newStorage = new GraphVariableStorage();
            // will HAVE to iterate through all storages
            foreach (var item in types)
            {
                AddRow<object>(newStorage, item);
            }

            return newStorage;
        }

        private void AddRow<T>(GraphVariableStorage newStorage, Type type)
        {
            var vals = FindCorrespondingList(type);
            VariableStorageRoot tmp;

            foreach (var item in vals)
            {
                tmp = (VariableStorageRoot)((VariableStorageRoot)item).Clone();
                newStorage.Add(tmp, type);
            }
        }

        #endregion
    }
}