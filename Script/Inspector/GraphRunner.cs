using System.Collections.Generic;
using UnityEngine;

public class GraphRunner : MonoBehaviour
{
    public BT.TestGraph graph;

    /// <summary>
    /// Organized as follow : GUID - Value's datas
    /// </summary>
    public Dictionary<string, Variable> values = null;

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
            values = new Dictionary<string, Variable>();
        }

        var original = graph.CompileAllBlackboard();

        foreach (var item in original)
        {
            values.Add(item.Key, Variable.CreateCopy(item.Value));
        }
    }
}
