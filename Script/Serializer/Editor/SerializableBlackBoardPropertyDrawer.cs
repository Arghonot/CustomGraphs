using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Graph;
using UnityEditor.UIElements;

namespace Graph
{
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

        //public static void DisplayChildrens(IEnumerable<SerializedProperty> listofchilds, Rect position)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    var NameRect = new Rect(
        //        position.x,
        //        position.y + VariableRect.YSpacing,
        //        VariableRect.Width,
        //        position.height + VariableRect.Height);
        //    foreach (var item in listofchilds)
        //    {
        //        if (item.name != "size")
        //        {
        //            EditorGUI.PropertyField(
        //                NameRect,
        //                item.FindPropertyRelative(item.name), true);
        //        }
        //        builder.Append(item.name);
        //        builder.Append("\n");
        //    }

        //    Debug.Log(builder.ToString());
        //}
    }

    [CustomPropertyDrawer(typeof(SerializableBlackBoard))]
    public class SerializableBlackBoardPropertyDrawer : PropertyDrawer
    {
        private int fieldAmount = 40;
        private int fieldHeight = 16;
        private float fieldSize = 20;
        private float padding = 2;

        private int LabelWidth = 100;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Using BeginProperty / EndProperty on the parent property means that
            //prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            //Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            position.height = fieldHeight;

            SerializedProperty iterator = property.Copy();

            float posx = position.x;

            while (iterator.Next(true))
            {
                if (iterator.name != "data" && iterator.name != "Array")
                {
                    var props = PropertyDrawerUtils.GetVisibleChildren(iterator);

                    if (props.Count() > 1)
                    {
                        position.y += fieldHeight + (padding * 4);
                        EditorGUILayout.Separator();
                        EditorGUI.LabelField(position, iterator.name);
                        position.y += fieldHeight + (padding * 3);

                        foreach (var item in props)
                        {
                            if (item.name != "size")
                            {
                                var realprops = PropertyDrawerUtils.GetVisibleChildren(item);
                                foreach (var p in realprops)
                                {
                                    if (p.name == "Name")
                                    {
                                        EditorGUI.LabelField(position, p.stringValue);
                                        position.x -= LabelWidth;
                                    }
                                    else if (p.name == "Value")
                                    {
                                        position.width = EditorGUIUtility.currentViewWidth - position.x - 19;

                                        // TODO bad way of doing this should be changed in the future (along with the -labelwidth)
                                        // but on the other hand it allows to have Value<T> custom proptery drawer ...
                                        EditorGUI.PropertyField(position, p);
                                        position.x = posx;
                                        position.y += fieldHeight + padding;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Set indent back to what it was
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (fieldSize * fieldAmount) + (padding * fieldAmount);
        }
    }
}
