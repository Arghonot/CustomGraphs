using UnityEngine;
using XNode;

namespace Graph
{
    [CreateNodeMenu("MYBT/BlackboardVariable")]
    [NodeTint("#e63946")]
    public class BlackBoardVariable : Node
    {
        public string uid;
        public string Name = string.Empty;

        public Blackboard Blackboard;

        protected override void Init()
        {
            Blackboard = ((TestGraph)graph).blackboard;
        }

        public void UpdateNode()
        {
            RemoveDynamicPort(Name);
            Name = string.Empty;
            uid = string.Empty;
            Debug.Log("UpdatePorts");
        }

        public void SetVariable(string newname, string newuid)
        {
            Debug.Log("[" + newname + "][" + newuid + "]");

            if (Name != string.Empty || Name != "")
            {
                RemoveDynamicPort(Name);
            }

            if (newname == "" && newuid == "")
            {
                Debug.Log("yeyeyeyey");
            }

            this.AddDynamicOutput(
                Blackboard.GetVariableType(newuid),
                ConnectionType.Multiple,
                TypeConstraint.Strict,
                newname);

            uid = newuid;
            Name = newname;
        }

        public string[] GetPossibleVariables()
        {
            return ((TestGraph)graph).blackboard.GetVariableNames();
        }

        public override object GetValue(NodePort port)
        {
            if (((TestGraph)graph).gd.ContainsKey(Name))
            {
                return ((TestGraph)graph).gd.Get(Name);
            }

            if (!Blackboard.container.ContainsKey(uid))
            {
                return null;
            }

            return Blackboard.container[uid].GetDefaultValue();

            //return null;

        }
    }
}