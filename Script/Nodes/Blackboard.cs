using XNode;
using System;
using UnityEngine;

namespace Graph
{
    [NodeTint(ColorProfile.Blackboard)]
    [Serializable]
    [HideFromNodeMenu]
    public class Blackboard : Node
    {
        public int TextWidth = 130;
        public int TypeWidth = 100;
        public int MinusWidth = 20;

        public int width = 300;

        [SerializeField] public GraphVariableStorage storage = new GraphVariableStorage();
        public event Action<string> OnStorageDataAddedOrRemoved;

        [ContextMenu("Register")]
        private void Awake()
        {
            storage.OnDataAddedOrRemoved += OnStorageDataAddedOrRemoved;
        }

        private void OnDestroy()
        {
            storage.OnDataAddedOrRemoved -= OnStorageDataAddedOrRemoved;
        }

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

        public string[] GetGUIDS()
        {
            return storage.getAllGuids();
        }

        public string[] GetAllNames()
        {
            return storage.GetAllNames();
        }

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