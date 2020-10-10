using Graph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a test class for a SerializedBlackBoard test
[CreateAssetMenu(fileName = "test", menuName = "Test/ContainerTest", order = 1)]
public class SerializerContainer : ScriptableObject
{
    //public NoiseGraph.LibnoiseGraph graph;

    //[DisplayScriptableObjectPropertiesAttribute]
    public Graph.SerializableBlackBoard sbb;

    private void Awake()
    {
        if (sbb == null) sbb = new Graph.SerializableBlackBoard();
    }
}
