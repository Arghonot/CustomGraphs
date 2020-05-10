using System;
using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    public class GraphRunner : MonoBehaviour
    {
        public ScriptableObject So;
        public Graph.DefaultGraph graph;

        /// <summary>
        /// Organized as follow : GUID - Value's datas
        /// </summary>
        [SerializeField]
        public BlackBoardDictionnary values = null;

        public bool ContainsValue(string ValueName, Type valueType)
        {
            foreach (var item in values)
            {
                if (item.Value.Name == ValueName &&
                    item.Value.GetValueType() == valueType)
                {
                    return true;
                }
            }

            return false;
        }

        public void BuildValueDictionnary()
        {
            if (graph.blackboard == null)
            {
                graph = null;
                return;
            }
            if (values != null)
            {
                values.Clear();
            }
            else
            {
                values = new BlackBoardDictionnary();
            }

            var original = graph.CompileAllBlackboard();

            foreach (var item in original)
            {
                values.Add(item.Key, Variable.CreateCopy(item.Value));
            }
        }
    }
}