//using System.Collections;
//using UnityEditor;
//using UnityEngine;

//namespace Graph
//{
//    public static   class VariableRect
//    {
//        public static float XSpacing = 130f;
//        public static float YSpacing = 2f;

//        public static float Width = 300f;
//        public static float Height = 0; // 100
//    }

//    [CustomPropertyDrawer(typeof(DoubleVariable))]
//    public class DoubleVariablePropertyDrawer : PropertyDrawer
//    {
//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            EditorGUI.BeginProperty(position, label, property);
//            var indent = EditorGUI.indentLevel;
//            EditorGUI.indentLevel = 0;
//            var NameRect = new Rect(
//                position.x,
//                position.y + VariableRect.YSpacing,
//                VariableRect.Width,
//                position.height + VariableRect.Height);


//            var ValueRect = new Rect(
//                position.x + VariableRect.XSpacing,
//                NameRect.y + VariableRect.YSpacing,
//                position.width - (position.x + VariableRect.XSpacing),
//                position.height + VariableRect.Height);

//            EditorGUI.LabelField(
//                NameRect,
//                property.FindPropertyRelative("Name").stringValue);
//            EditorGUI.PropertyField(
//                ValueRect,
//                property.FindPropertyRelative("Value"),
//                GUIContent.none);

//            //base.OnGUI(position, property, label);

//            // Set indent back to what it was
//            EditorGUI.indentLevel = indent;

//            EditorGUI.EndProperty();
//        }
//    }

//    [CustomPropertyDrawer(typeof(IntVariable))]
//    public class IntVariablePropertyDrawer : PropertyDrawer
//    {
//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            EditorGUI.BeginProperty(position, label, property);
//            var indent = EditorGUI.indentLevel;
//            EditorGUI.indentLevel = 0;
//            var NameRect = new Rect(
//                position.x,
//                position.y + VariableRect.YSpacing,
//                VariableRect.Width,
//                position.height + VariableRect.Height);
//            var ValueRect = new Rect(
//                position.x + VariableRect.XSpacing,
//                NameRect.y + VariableRect.YSpacing,
//                position.width - (position.x + VariableRect.XSpacing),
//                position.height + VariableRect.Height);

//            EditorGUI.LabelField(
//                NameRect,
//                property.FindPropertyRelative("Name").stringValue);
//            EditorGUI.PropertyField(
//                ValueRect,
//                property.FindPropertyRelative("Value"),
//                GUIContent.none);

//            //base.OnGUI(position, property, label);

//            // Set indent back to what it was
//            EditorGUI.indentLevel = indent;

//            EditorGUI.EndProperty();
//        }
//    }
//}
