using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Graph
{
    public static   class VariableRect
    {
        public static float XSpacing = 130f;
        public static float YSpacing = 2f;

        public static float Width = 300f;
        public static float Height = 0; // 100
    }

    [CustomPropertyDrawer(typeof(DoubleVariable))]
    public class DoubleVariablePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var NameRect = new Rect(
                position.x,
                position.y + VariableRect.YSpacing,
                VariableRect.Width,
                position.height + VariableRect.Height);


            var ValueRect = new Rect(
                position.x + VariableRect.XSpacing,
                NameRect.y + VariableRect.YSpacing,
                position.width - (position.x + VariableRect.XSpacing),
                position.height + VariableRect.Height);

            EditorGUI.LabelField(
                NameRect,
                property.FindPropertyRelative("Name").stringValue);
            EditorGUI.PropertyField(
                ValueRect,
                property.FindPropertyRelative("Value"),
                GUIContent.none);

            //base.OnGUI(position, property, label);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(IntVariable))]
    public class IntVariablePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var NameRect = new Rect(
                position.x,
                position.y + VariableRect.YSpacing,
                VariableRect.Width,
                position.height + VariableRect.Height);
            var ValueRect = new Rect(
                position.x + VariableRect.XSpacing,
                NameRect.y + VariableRect.YSpacing,
                position.width - (position.x + VariableRect.XSpacing),
                position.height + VariableRect.Height);

            EditorGUI.LabelField(
                NameRect,
                property.FindPropertyRelative("Name").stringValue);
            EditorGUI.PropertyField(
                ValueRect,
                property.FindPropertyRelative("Value"),
                GUIContent.none);

            //base.OnGUI(position, property, label);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }

    //[CustomPropertyDrawer(typeof(SerializableBlackBoard))]
    //public class SerializableBlackBoardPropertyDrawer : PropertyDrawer
    //{
    //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //    {
    //            //Using BeginProperty / EndProperty on the parent property means that
    //            //prefab override logic works on the entire property.
    //            EditorGUI.BeginProperty(position, label, property);

    //        //Don't make child fields be indented
    //            var indent = EditorGUI.indentLevel;
    //        EditorGUI.indentLevel = 0;

    //        var NameRect = new Rect(
    //            position.x,
    //            position.y + VariableRect.YSpacing,
    //            VariableRect.Width,
    //            position.height + VariableRect.Height);

    //        EditorGUI.PropertyField(
    //            NameRect,
    //            property.FindPropertyRelative("Doubles"), true);

    //            //Rect rect = new Rect(
    //            //    NameRect.x + VariableRect.XSpacing,
    //            //    NameRect.y + VariableRect.YSpacing,
    //            //    NameRect.width + VariableRect.Width,
    //            //    NameRect.height + VariableRect.Height);


    //    //        for (int i = 0; i<property.FindPropertyRelative("Doubles").arraySize; i++)
    //    //        {
    //    //            EditorGUI.PropertyField(
    //    //                NameRect,
    //    //                property.FindPropertyRelative("Doubles").GetArrayElementAtIndex(i));

    //    //            rect = new Rect(
    //    //                rect.x + VariableRect.XSpacing,
    //    //                rect.y + VariableRect.YSpacing,
    //    //                rect.width + VariableRect.Width,
    //    //                rect.height + VariableRect.Height);
    //    //}

    //    //Set indent back to what it was
    //    EditorGUI.indentLevel = indent;

    //    EditorGUI.EndProperty();
    //    }

    //    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //    {
    //        return 50.0f;
    //    }
    //}
}
