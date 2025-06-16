using XNode;
using System;
using UnityEngine;

namespace CustomGraph
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

        [SerializeField] public GraphVariables storage = new GraphVariables();
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
            storage = ((GraphBase)graph).originalStorage;
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

        public void PrintStorageGUID(GraphVariables otherstorage)
        {
            storage.CompareDictionnaries(otherstorage);
        }
    }
}