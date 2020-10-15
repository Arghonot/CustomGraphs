using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test", menuName = "ScriptableObjects/TestObject", order = 1)]
public class ScriptableObjectExemple : ScriptableObject
{
    public int TestInt;
    public string TestString;
    public bool TestBool;
}
