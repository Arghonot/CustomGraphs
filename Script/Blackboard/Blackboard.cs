using System.Linq;
using System.Collections.Generic;
using XNode;
using System;
using UnityEngine;

[Serializable]
public class BlackBoardDictionnary : Dictionary<string, Variable>, ISerializationCallbackReceiver
{
    [SerializeField] private List<string> keys = new List<string>();
    [SerializeField] private List<int> values = new List<int>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        //Debug.Log(this.Count()); 
        Debug.Log("OnBeforeSerialize");

        if (this.Count() == 0) return; 

        foreach (KeyValuePair<string, Variable> pair in this) 
        {
            keys.Add(pair.Key);
            values.Add((int)pair.Value.Value); 

            //values.Last().Name = pair.Value.Name;
            //values.Last().TypeName = pair.Value.TypeName;
            //values.Last().Value = pair.Value.Value;
        }
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        Debug.Log("COMPILATIONNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN"); 
        // do something
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count() == 0) return;
        Debug.Log("         OnAfterDeserialize");

        if (keys.Count != values.Count)
            throw new System.Exception("there are " + keys.Count + " keys and " + values.Count + " values after deserialization. Make sure that both key and value types are serializable.");

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], Variable.CreateType("int"));

            this[keys[i]].Value = (object)values[i]; 

            //this[keys[i]].Value = values[i].Value;
            //this[keys[i]].Name = values[i].Name;
            //Debug.Log((int)values[i].Value);

            //Debug.Log(values[i].Name + " became [" + values[i].Value + "]");
        }
    }
}

namespace BT
{
    [NodeTint(6, 24, 56)]
    public class Blackboard : Node
    {
        public int TextWidth = 130;
        public int TypeWidth = 100; 
        public int MinusWidth = 20;

        public int width = 300;

        public BlackBoardDictionnary container;

        void Awake()
        {
            container = new BlackBoardDictionnary();
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