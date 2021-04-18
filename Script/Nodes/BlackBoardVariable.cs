using UnityEngine;

namespace Graph
{
    [System.Serializable]
    [CreateNodeMenu("Graph/BlackboardVariable")]
    [NodeTint(ColorProfile.other1)]
    public class BlackBoardVariable : NodeBase
    {
        [SerializeField] public string guid;
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
            guid = string.Empty;
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

            guid = newuid;
            VariableIndex = newIndex;
            Name = newname;
        }

        public string[] GetPossibleVariables()
        {
            return ((DefaultGraph)graph).blackboard.GetVariableNames();
        }

        public override object Run()
        {
            // we try to get the key from the blackboard gd if any
            if (((DefaultGraph)graph).storage.ContainsGuid(guid))
            {
                return ((DefaultGraph)graph).storage.Get(guid);
            }
            // or we return null if it isn't even in the blackboard
            if (!Blackboard.storage.ContainsGuid(guid))
            {
                return null;
            }
            // or we return the default value, might be null
            return Blackboard.storage.Get(guid);
        }

        //public override object GetValue(NodePort port)
        //{
        //    Debug.Log("BB VARIABLE");
        //    // we try to get the key from the blackboard gd if any
        //    if (((DefaultGraph)graph).gd.ContainsKey(Name))
        //    {
        //        return ((DefaultGraph)graph).gd.Get(Name);
        //    }
        //    // or we return null if it isn't even in the blackboard
        //    if (!Blackboard.container.ContainsKey(uid))
        //    {
        //        return null;
        //    }
        //    // or we return the default value, might be null
        //    return Blackboard.container[uid].GetDefaultValue();
        //}
    }
}