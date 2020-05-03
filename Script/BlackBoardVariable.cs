using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT
{
    [CreateNodeMenu("MYBT/BlackboardVariable")]
    public class BlackBoardVariable : Node
    {
        #region PORTS

        [Output]
        public int _int;
        [Output]
        public bool _bool;
        [Output]
        public string _string;

        #endregion

        public string uid;
        public string Name;

        public Blackboard Blackboard;

        protected override void Init()
        {
            Blackboard = ((TestGraph)graph).blackboard;
        }

        public string[] GetPossibleVariables()
        {
            return ((TestGraph)graph).blackboard.GetVariableNames();
        }

        // TODO premake this array instead
        public string[] GetGUIDs()
        {
            if (Blackboard.container == null) // shouldn't happen right ?
            {
                return null;
            }

            return Blackboard.container.Select(x => x.Key).ToArray();
        }

        public string[] GetNames(string[] guids)
        {
            string[] names = new string[guids.Length];

            if (guids == null ||
                guids.Length == 0)
            {
                return new string[0];
            }

            for (int i = 0; i < guids.Length; i++)
            {
                names[i] = Blackboard.GetName(guids[i]);
            }

            return names;
        }
    }
}
