using UnityEngine;

[CreateAssetMenu(fileName = "Test", menuName = "ScriptableObjects/TestObject", order = 1)]
public class ScriptableObjectExemple : ScriptableObject
{
    public int TestInt;
    public string TestString;
    public bool TestBool;
}
