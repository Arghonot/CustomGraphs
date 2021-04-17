using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph;
using System.Reflection;
using System.Linq;

public class DictionnaryV2 : MonoBehaviour
{
    public string NameOftypeToAdd;
    public string GUIDToGet;
    public string ValueToApply;
    public string NameToApply;

    [SerializeField] public GraphVariableStorage storage;

    [ContextMenu("FLUSH DEBUG")]
    public void DebugFlush()
    {
        storage.Flush();
    }

    [ContextMenu("DEBUG LOG DICTIONNARIES")]
    public void DebugLogDictionnaries()
    {
        storage.DebugDictionnaries();
    }

    [ContextMenu("Add string from type")]
    public void AddString()
    {
        storage.Add(typeof(string));
    }

    [ContextMenu("Add string from type with name")]
    public void AddStringWithName()
    {
        storage.Add(typeof(string), NameToApply);
    }

    [ContextMenu("CreateCopy string")]
    public void CreateCopyString()
    {
        storage.CreateCopy(storage.Strings.First());
    }





    [ContextMenu("Test get string")]
    public void TestGetString()
    {
        storage.GetVariableStorage<StringVariable>(GUIDToGet).ToString();
    }

    [ContextMenu("Test get string value")]
    public void TestGetStringValue()
    {
        Debug.Log("TestGetStringValue->" + storage.GetValue<string>(GUIDToGet));
    }

    [ContextMenu("Test set string value")]
    public void TestSetStringValue()
    {
        storage.SetValue(GUIDToGet, ValueToApply);
        storage.GetVariableStorage<StringVariable>(GUIDToGet).ToString();
    }

    //[ContextMenu("Test get string Name")]
    //public void TestSetStringName()
    //{
    //    storage.na(GUIDToGet, NameToApply);
    //    storage.GetVariableStorage<StringVariable>(GUIDToGet).ToString();
    //}

    [ContextMenu("Test remove string")]
    public void TestRemoveString()
    {
        storage.Remove(GUIDToGet);
    }

    [ContextMenu("Test contain string")]
    public void TestContainString()
    {
        Debug.Log("TestContainString->" + storage.ContainsValue(GUIDToGet));
    }

    [ContextMenu("Test get container type")]
    public void TestGetContainerType()
    {
        Debug.Log("TestGetContainerType->" + storage.GetContainerType(GUIDToGet));
    }

    [ContextMenu("Test get stored type")]
    public void TestGetStoredType()
    {
        Debug.Log("GetStoredType->" + storage.GetStoredType(GUIDToGet));
    }


    //[ContextMenu("Add double")]
    //public void AddTypeFromString()
    //{
    //    storage.Add(typeof(double));
    //}

    //[ContextMenu("Add Transform from type")]
    //public void AddTransform()
    //{
    //    storage.Add(typeof(Transform));
    //}

    //[ContextMenu("Add AnimationCurve from type")]
    //public void AddAnimationCurve()
    //{
    //    storage.Add(typeof(AnimationCurve));
    //}
    //[ContextMenu("CreateCopy double")]
    //public void CreateCopyDouble()
    //{
    //    storage.CreateCopy(storage.Doubles.First());
    //}

    //[ContextMenu("CreateCopy Transform")]
    //public void CreateCopyTransform()
    //{
    //    storage.CreateCopy(storage.Transforms.First());
    //}

    //[ContextMenu("CreateCopy animationCurve")]
    //public void CreateCopyAnimationCurve()
    //{
    //    storage.CreateCopy(storage.AnimationCurves.First());
    //}

    [ContextMenu("Get Value")]
    public void GetValue()
    {
        Debug.Log(storage.GetVariableStorage<StringVariable>(GUIDToGet).Value);
    }
}
