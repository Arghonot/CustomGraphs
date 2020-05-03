using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using System;

namespace BT
{
    [CreateNodeMenu("MYBT/Blackboard")]
    public class Blackboard : Node
    {
        public int TextWidth = 130;
        public int TypeWidth = 100;
        public int MinusWidth = 20;

        public int width = 300;

        /// <summary>
        /// GUID - variables data
        /// </summary>
        public Dictionary<string, Variable> container;

        /// <summary>
        /// GUID current name
        /// </summary>
        public Dictionary<string, string> ContainerNames;

        private void Awake()
        {
            container = new Dictionary<string, Variable>();
        }

        // TODO make a string[] of the possible variables, so it's not recalculated every frames
        public string[] GetVariableNames()
        {
            return container.Select(x => x.Value.Name).ToArray();
        }

        public bool AddVariable(string guid, string type)
        {
            if (container.ContainsKey(name))
            {
                return false;
            }

            container.Add(guid, Variable.CreateType(type));

            return true;
        }

        public string GetTypeFromGUID(string guid)
        {
            if (!container.ContainsKey(guid))
            {
                return "int";
            }

            return Variable.GetType(container[guid].GetValueType());
        }

        public string GetName(string guid)
        {
            if (!container.ContainsKey(guid))
            {
                return null;
            }

            return container[guid].Name;
        }
    }
}