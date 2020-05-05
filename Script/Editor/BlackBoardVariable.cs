using XNode;

namespace BT
{
    [CreateNodeMenu("MYBT/BlackboardVariable")]
    [NodeTint(189, 17, 66)]
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
    }
}