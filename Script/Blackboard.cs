using System.Linq;
using System.Collections.Generic;
using XNode;
using System;

namespace BT
{
    [NodeTint(6, 24, 56)]
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

        private void Awake()
        {
            container = new Dictionary<string, Variable>();
        }

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

        public Type GetVariableType(string guid)
        {
            return container[guid].GetValueType();
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

        public string[] GetGUIDS()
        {
            int index = 0;
            string[] guids = new string[container.Count];

            foreach (var item in container)
            {
                guids[index++] = item.Key;
            }

            return guids;
        }

        public string[] GetVariableNames(string[] uids)
        {
            string[] names = new string[uids.Count()];

            for (int i = 0; i < uids.Length; i++)
            {
                if (container.ContainsKey(uids[i]))
                {
                    names[i] = container[uids[i]].Name;
                }
                else
                {
                    names[i] = "UNKNOWN";
                }
            }

            return names;
        }
    }
}