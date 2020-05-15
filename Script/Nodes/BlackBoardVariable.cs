using UnityEngine;
using XNode;

namespace Graph
{
    [System.Serializable]
    [CreateNodeMenu("Graph/BlackboardVariable")]
    [NodeTint(ColorProfile.other1)]
    public class BlackBoardVariable : Node
    {
        [SerializeField] public string uid;
        [SerializeField] public int VariableIndex;
        [SerializeField] public string Name = string.Empty;

        public Blackboard Blackboard;

        protected override void Init()
        {
            Blackboard = ((DefaultGraph)graph).blackboard;
        }

        public void UpdateNode()
        {
            RemoveDynamicPort(Name);
            Name = string.Empty;
            uid = string.Empty;
        }

        public void SetVariable(string newname, string newuid, int newIndex)
        {
            if (Name != string.Empty || Name != "")
            {
                RemoveDynamicPort("");
            }

            this.AddDynamicOutput(
                Blackboard.GetVariableType(newuid),
                ConnectionType.Multiple,
                TypeConstraint.Strict,
                "");

            uid = newuid;
            VariableIndex = newIndex;
            Name = newname;
        }

        public string[] GetPossibleVariables()
        {
            return ((DefaultGraph)graph).blackboard.GetVariableNames();
        }

        public override object GetValue(NodePort port)
        {
            if (((DefaultGraph)graph).gd.ContainsKey(Name))
            {
                return ((DefaultGraph)graph).gd.Get(Name);
            }

            if (!Blackboard.container.ContainsKey(uid))
            {
                return null;
            }

            return Blackboard.container[uid].GetDefaultValue();
        }
    }
}