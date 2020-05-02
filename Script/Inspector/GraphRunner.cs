using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class GraphRunner : MonoBehaviour
{
    public BT.TestGraph graph;

    /// <summary>
    /// Organized as follow : GUID - Value's datas
    /// </summary>
    // TODO directly referencing blackboardelement ? shall we use the BT namespace here directly ?
    public Dictionary<string, BT.BlackboardElement> values = null;

    public void BuildValueDictionnary()
    {
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
            values = new Dictionary<string, BT.BlackboardElement>();
        }

        foreach (var item in graph.blackboard.container)
        {
            values.Add(item.Key, (BT.BlackboardElement)item.Value);
        }
    }
}
