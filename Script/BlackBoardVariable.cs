using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT
{
    [CreateNodeMenu("MYBT/BlackboardVariable")]
    public class BlackBoardVariable : Node
    {
        public string uid;
        public string Name = string.Empty;
        

        public Blackboard Blackboard;

        protected override void Init()
        {
            Blackboard = ((TestGraph)graph).blackboard;

            //Outputs = new 
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
                TypeConstraint.None,
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