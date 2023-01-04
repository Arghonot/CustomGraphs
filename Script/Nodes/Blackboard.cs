using System.Linq;
using XNode;
using System;
using UnityEngine;

namespace Graph
{
    [NodeTint(ColorProfile.Blackboard)]
    [Serializable]
    public class Blackboard : Node
    {
        public int TextWidth = 130;
        public int TypeWidth = 100;
        public int MinusWidth = 20;

        public int width = 300;

        [SerializeField]
        public GraphVariableStorage storage;

        public string[] GetVariableNames()
        {
            return storage.GetAllNames();
        }

        public void InitializeBlackboard()
        {
            Debug.Log("InitializeBlackboard");
            storage = ((DefaultGraph)graph).originalStorage;
        }

        //public bool AddVariable(string guid, string type)
        //{
        //    if (storage.ContainName(name))
        //    {
        //        return false;
        //    }
            
        //    storage.Add(guid, Variable.CreateType(type));

        //    return true;
        //}

        //public bool RemoveVariable(string guid)
        //{
        //    if (!storage.ContainsKey(guid))
        //    {
        //        return false;
        //    }

        //    storage.Remove(guid);
        //    ((DefaultGraph)graph).OnDeleteVariable(guid);

        //    return true;
        //}

        //public Type GetVariableType(string guid)
        //{
        //    return storage[guid].GetValueType();
        //}

        //public string GetTypeFromGUID(string guid)
        //{
        //    if (!storage.ContainsKey(guid))
        //    {
        //        return "int";
        //    }

        //    return Variable.GetType(storage[guid].GetValueType());
        //}

        //public string GetName(string guid)
        //{
        //    if (!storage.ContainsKey(guid))
        //    {
        //        return null;
        //    }

        //    return storage[guid].Name;
        //}

        public Type GetVariableType(string guid)
        {
            return storage.GetContainedType(guid);
        }

        // TODO decide if storage should be exposed
        public string[] GetGUIDS()
        {
            return storage.getAllGuids();
        }

        public string[] GetAllNames()
        {
            return storage.GetAllNames();
        }

        // TODO decide if storage should be exposed
        public string[] GetVariableNames(string[] guids)
        {
            return storage.GetNames(guids);
        }

        public void PrintStorageGUID(GraphVariableStorage otherstorage)
        {
            storage.CompareDictionnaries(otherstorage);
        }
    }
}