using Graph;
using UnityEngine;

// This is a test class for a SerializedBlackBoard test
[CreateAssetMenu(fileName = "test", menuName = "Test/ContainerTest", order = 1)]
public class SerializerContainer : ScriptableObject
{
    public IntGraph graph;

    //[DisplayScriptableObjectPropertiesAttribute]
    public Graph.SerializableBlackBoard sbb;

    private void Awake()
    {
        if (sbb == null) sbb = new Graph.SerializableBlackBoard();
    }
}
