using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is interesting if you need to set variables for your 
/// graph instance and easily share it between scenes
/// </summary>
[CreateAssetMenu(fileName = "GraphDatas", menuName = "Graphs/GraphData", order = 1)]
public class GraphInterpretor : ScriptableObject
{
    // TODO find a way to set values at runtime and keep them in the inspector
    // like any regular ScriptableObject.
    public BT.TestGraph graph;

    /// <summary>
    /// Organized as follow : GUID - Value's datas
    /// </summary>
    public BlackBoardDictionnary values = null;

    public void BuildValueDictionnary()
    {

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
