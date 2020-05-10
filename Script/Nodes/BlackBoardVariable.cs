﻿using XNode;

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

        public void SetVariable(string newname, string newuid)
        {
            if (Name != string.Empty || Name != "")
            {
                RemoveDynamicPort(Name);
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

            return Blackboard.container[uid].GetDefaultValue();
        }
    }
}