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
            storage = ((DefaultGraph)graph).originalStorage;
        }

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