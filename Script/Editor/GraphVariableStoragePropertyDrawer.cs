using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CustomGraph
{
    public static class VariableRect
    {
        public static float XSpacing = 130f;
        public static float YSpacing = 2f;

        public static float Width = 300f;
        public static float Height = 0;
    }

    public static class PropertyDrawerUtils
    {
        /// <summary>
        /// Gets visible children of `SerializedProperty` at 1 level depth.
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetVisibleChildren(this SerializedProperty serializedProperty)
        {
            SerializedProperty currentProperty = serializedProperty.Copy();
            SerializedProperty nextSiblingProperty = serializedProperty.Copy();
            {
                nextSiblingProperty.NextVisible(false);
            }

            if (currentProperty.NextVisible(true))
            {
                do
                {
                    if (SerializedProperty.EqualContents(currentProperty, nextSiblingProperty))
                        break;

                    yield return currentProperty;
                }
                while (currentProperty.NextVisible(false));
            }
        }

        public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty property)
        {
            property = property.Copy();
            var nextElement = property.Copy();
            bool hasNextElement = nextElement.NextVisible(false);
            if (!hasNextElement)
            {
                nextElement = null;
            }

            property.NextVisible(true);
            while (true)
            {
                if ((SerializedProperty.EqualContents(property, nextElement)))
                {
                    yield break;
                }

                yield return property;

                bool hasNext = property.NextVisible(false);
                if (!hasNext)
                {
                    break;
                }
            }
        }
    }

    [CustomPropertyDrawer(typeof(GraphVariables))]
    public class GraphVariableStoragePropertyDrawer : PropertyDrawer
    {
        private int fieldHeight = 20;
        private float fieldSize = 20;
        private float padding = 2;

        private static readonly string[] controlNames = new string[] {
            "names",
            "size",
            "data",
            "Array",
            "guids",
            "GUID",
            "Name",
            "typeString",
            "Value",
            "graph",
            "m_FileID",
            "m_PathID",
            "tex"
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Using BeginProperty / EndProperty on the parent property means that
            //prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);
            GUIStyle boldFoldout = new GUIStyle(EditorStyles.foldout);
            boldFoldout.fontStyle = FontStyle.Bold;
            bool isFolded = EditorPrefs.GetBool("GraphVariableFoldout", false);
            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            isFolded = EditorGUI.Foldout(foldoutRect, isFolded, "Graph Variables", true, boldFoldout);
            EditorPrefs.SetBool("GraphVariableFoldout", isFolded);

            if (isFolded)
            {
                // Indent content and move it below the foldout
                Rect contentPos = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + padding, position.width, position.height);

                DrawVariables(contentPos, property);
            }
            EditorGUI.EndProperty();
        }

        private void DrawVariables(Rect position, SerializedProperty property)
        {
            //Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            //EditorGUI.indentLevel = 0;
            position.height = fieldHeight;

            SerializedProperty iterator = property.Copy();
            float posx = position.x;
            // the problem here is the layout pass of the IMGUI
            // seems like it's trying to register the amount of properties
            // but it's "changing between the layout pass and the paint pass"
            // maybe the solution would be to just ask to draw for every list 
            // containing more than 0 elements and create another property drawer
            // for each type of variable.

            while (iterator.Next(true))
            {
                if (!controlNames.Contains(iterator.name)) DrawTypeData(iterator, ref position, posx);
            }

            //Set indent back to what it was
            EditorGUI.indentLevel = indent;
        }

        private void DrawTypeData(SerializedProperty iterator, ref Rect position, float posX)
        {
            var props = PropertyDrawerUtils.GetVisibleChildren(iterator);

            if (props.Count() > 1)
            {
                GUIStyle boldLabel = new GUIStyle(EditorStyles.boldLabel);
                EditorGUI.LabelField(position, iterator.displayName, boldLabel);
                position.y += fieldHeight + (padding * 3);

                foreach (var item in props)
                {
                    if (item.name == "data")
                    {
                        var listContainer = PropertyDrawerUtils.GetVisibleChildren(item);

                        var name = "";
                        foreach (var metas in listContainer)
                        {
                            DrawTypeInstanceData(metas, ref name, ref position, posX);
                        }
                    }
                }
            }
        }

        private void DrawTypeInstanceData(SerializedProperty metas, ref string name, ref Rect position, float posX)
        {
            var variable = PropertyDrawerUtils.GetVisibleChildren(metas);
            if (metas.name == "Name")
            {
                name = metas.stringValue;
            }
            else if (metas.name == "Value")
            {
                position.width = EditorGUIUtility.currentViewWidth - position.x - 19;
                EditorGUI.PropertyField(position, metas, new GUIContent(name));
                position.x = posX;
                position.y += fieldHeight + padding;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            bool isFolded = EditorPrefs.GetBool("GraphVariableFoldout", false);
            if (!isFolded)
                return EditorGUIUtility.singleLineHeight;

            GraphVariables targetObject = (GraphVariables)GetTargetObjectOfProperty(property);
            int fieldCount = targetObject.GetAmountOfGUIFields();
            return EditorGUIUtility.singleLineHeight + padding + (fieldCount * fieldSize) + (padding * fieldCount);
        }

        public static object GetTargetObjectOfProperty(SerializedProperty prop)
        {
            if (prop == null) return null;

            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = GetValue_Imp(obj, element);
                }
            }
            return obj;
        }

        private static object GetValue_Imp(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();

            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(source);

                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);

                type = type.BaseType;
            }
            return null;
        }

        private static object GetValue_Imp(object source, string name, int index)
        {
            var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator();

            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }
            return enm.Current;
        }
    }
}