using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GraphRunner : MonoBehaviour
{
    public BT.TestGraph graph;

    /// <summary>
    /// Organized as follow : GUID - Value's datas
    /// </summary>
    // TODO directly referencing blackboardelement ? shall we use the BT namespace here directly ?
    public Dictionary<string, Variable> values = null;

    public void BuildValueDictionnary()
    {
        print("BUILDING");
        // if un initialized we don't want to do anything
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

        foreach (var item in graph.blackboard.container)
        {
            values.Add(item.Key, Variable.CreateCopy(item.Value));
        }
        //values = new Dictionary<string, Variable>(graph.blackboard.container);
    }
}
