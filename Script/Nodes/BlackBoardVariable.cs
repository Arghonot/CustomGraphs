using System;
using System.Linq;
using UnityEngine;

namespace Graph
{
    [System.Serializable]
    [CreateNodeMenu("Graph/BlackboardVariable")]
    [NodeTint(ColorProfile.other1)]
    public class BlackBoardVariable : NodeBase
    {
        [SerializeField] public int VariableIndex;
        [SerializeField] public string guid = string.Empty;
        [SerializeField] public string Name = string.Empty;
        [SerializeField] public Type VariableType = null;

        public Blackboard Blackboard;

        protected override void Init()
        {
            Blackboard = ((DefaultGraph)graph).blackboard;

            ChooseFirstVariable();
        }

        public void UpdateNode()
        {
            RemoveDynamicPort(Name);
            Name = string.Empty;
            guid = string.Empty;
        }

        private void ChooseFirstVariable()
        {
            if (Ports.Count() != 0)
            {
                return;
            }

            string guid = Blackboard.storage.getAllGuids()[0];

            SetVariable(Blackboard.storage.GetName(guid), guid, 0);
        }

        public void SetVariable(string newname, string newuid, int newIndex)
        {
            if (Name != string.Empty || Name != "")
            {
                RemoveDynamicPort("Output");
            }

            this.AddDynamicOutput(
                Blackboard.GetVariableType(newuid),
                ConnectionType.Multiple,
                TypeConstraint.Strict,
                "Output");

            UpdateGUID(newuid);

            VariableIndex = newIndex;
            Name = newname;
            VariableType = Blackboard.GetVariableType(newuid);
        }

        private void UpdateGUID(string to)
        {
            // if already had a GUID stored
            if (guid != string.Empty && guid != "")
            {
                Blackboard.storage.GetContainerInstance(guid).OnUpdateGUID -= UpdateGUID;
            }

            Blackboard.storage.GetContainerInstance(to).OnUpdateGUID += UpdateGUID;
            guid = to;
        }

        public string[] GetPossibleVariables()
        {
            return ((DefaultGraph)graph).blackboard.GetVariableNames();
        }

        public override object Run()
        {
            return ((DefaultGraph)graph).runtimeStorage.Get(guid);
        }
    }
}