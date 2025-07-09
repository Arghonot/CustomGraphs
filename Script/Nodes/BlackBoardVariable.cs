using System.Linq;
using UnityEngine;

namespace CustomGraph
{
    [System.Serializable]
    [CreateNodeMenu("Graph/BlackboardVariable")]
    [NodeTint(ColorProfile.Input)]
    public class BlackBoardVariable : NodeBase
    {
        [SerializeField] public int VariableIndex;
        public string guid => _guid;
        [SerializeField] private string _guid = string.Empty;
        public string name => _name;
        [SerializeField] private string _name = string.Empty;

        public Blackboard Blackboard;

        protected override void Init()
        {
            Blackboard = ((GraphBase)graph).blackboard;

            ChooseFirstVariable();
        }

        private void ChooseFirstVariable()
        {
            if (Ports.Count() != 0) return;

            string guid = Blackboard.storage.getAllGuids()[0];

            SetVariable(Blackboard.storage.GetName(guid), guid, 0);
        }

        public void SetVariable(string newname, string newuid, int newIndex)
        {
            if (_name != string.Empty || _name != "")
            {
                RemoveDynamicPort("Output");
            }

            this.AddDynamicOutput(Blackboard.GetVariableType(newuid), ConnectionType.Multiple, TypeConstraint.Strict, "Output");

            UpdateGUID(newuid);

            VariableIndex = newIndex;
            _name = newname;
        }

        private void UpdateGUID(string to)
        {
            UnregisterPreviousVariable();
            var toVariableInstance = Blackboard.storage.GetContainerInstance(to);
            toVariableInstance.OnUpdateGUID += UpdateGUID;
            toVariableInstance.OnRemoveInstance += UnregisterPreviousVariable;
            _guid = to;
        }

        private void UnregisterPreviousVariable()
        {
            if (_guid == string.Empty || _guid == "") return;
            // if already had a GUID stored
            var toVariableInstance = Blackboard.storage.GetContainerInstance(_guid);

            toVariableInstance.OnUpdateGUID -= UpdateGUID;
            toVariableInstance.OnRemoveInstance -= UnregisterPreviousVariable;
        }

        // todo we could cache the graph's variable in the graph instead of recomputing that every time
        public string[] GetPossibleVariables() => ((GraphBase)graph).blackboard.GetVariableNames();

        public override object Run()
        {
            if (((GraphBase)graph).runtimeStorage.ContainsGuid(_guid))
            {
                return ((GraphBase)graph).runtimeStorage.GetFromGUID(_guid);
            }

            return ((GraphBase)graph).originalStorage.GetFromGUID(_guid);
        }
    }
}