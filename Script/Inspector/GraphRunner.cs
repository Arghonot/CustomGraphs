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

        // TODO Change this because it'll not work for multiple entities use
        values = graph.blackboard.container;
    }
}
