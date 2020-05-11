using XNode;

namespace Graph
{
    [CreateNodeMenu("Graph/BlackboardVariable")]
    [NodeTint("#e63946")]
    public class BlackBoardVariable : Node
    {
        public string uid;
        public string Name = string.Empty;

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

            //return null;

        }
    }
}