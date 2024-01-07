using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Graph.BlackboardableVariableAttribute))]
public class BlackboardableVariableDrawer : PropertyDrawer
{
    private static Graph.GraphVariableStorage GetStorage(Graph.NodeBase node) => ((Graph.DefaultGraph)node.graph).blackboard.storage;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        object parentObject = GetParentObjectOfProperty(property.propertyPath, property.serializedObject.targetObject);

        Graph.NodeBase myNode = parentObject as Graph.NodeBase;
        Graph.BlackboardableVariableAttribute attr = attribute as Graph.BlackboardableVariableAttribute;
        if (attr == null)
        {
            EditorGUI.LabelField(position, label.text, "Attribute not found.");
            return;
        }

        // Get the type specified in the attribute
        Type variableType = attr.VariableType;
        if (myNode != null) DrawDropdownForType(myNode, variableType, property);
    }

    private object GetParentObjectOfProperty(string path, object obj)
    {
        var fields = path.Split('.');
        foreach (var field in fields.Take(fields.Length - 1))
        {
            obj = obj.GetType().GetField(field, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(obj);
        }
        return obj;
    }

    public static void DrawDropdownForType(Graph.NodeBase node, Type type, SerializedProperty property)
    {
        if (!GetStorage(node).ContainsGuid(property.stringValue))
        {
            var possibleGuids = GetStorage(node).GetGUIDsForType(type);
            if (possibleGuids.Length == 0) return;
            property.stringValue = possibleGuids[0];
            property.serializedObject.ApplyModifiedProperties();
        }

        GUILayout.Label("Variable to set :");
        if (GUILayout.Button(GetStorage(node).GetName(property.stringValue)))
        {
            GenericMenu menu = new GenericMenu();
            SetupMenu(node, type, menu, property);
            menu.ShowAsContext();
        }
        GUILayout.Space(12);
    }

    private static void SetupMenu(Graph.NodeBase node, Type type, GenericMenu menu, SerializedProperty property)
    {
        string[] guids = GetStorage(node).GetGUIDsForType(type);
        string[] names = GetStorage(node).GetNames(guids);

        for (int i = 0; i < guids.Length; i++)
        {
            string guid = guids[i];
            menu.AddItem(new GUIContent(names[i]), guid == property.stringValue, () => { DoChange(property, guid); });
        }
    }

    private static void DoChange(SerializedProperty property, string newGUID)
    {
        property.stringValue = newGUID;
        property.serializedObject.ApplyModifiedProperties();
    }
}
