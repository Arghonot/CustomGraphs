﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Graph
{
    public static class VariableRect
    {
        public static float XSpacing = 130f;
        public static float YSpacing = 2f;

        public static float Width = 300f;
        public static float Height = 0; // 100
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

    [CustomPropertyDrawer(typeof(GraphVariableStorage))]
    public class GraphVariableStoragePropertyDrawer : PropertyDrawer
    {
        private int fieldHeight = 16;
        private float fieldSize = 20;
        private float padding = 2;

        private int LabelWidth = 100;

        private static string[] controlNames = new string[] {
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
            //Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            position.height = fieldHeight;

            SerializedProperty iterator = property.Copy();
            float posx = position.x;
            // TODO, the problem here is the layout pass of the IMGUI
            // seems like it's trying to register the amount of properties
            // but it's "changing between the layout pass and the paint pass"
            // maybe the solution would be to just ask to draw for every list 
            // containing more than 0 elements and create another property drawer
            // for each type of variable.
            while (iterator.Next(true))
            {
                if (!controlNames.Contains(iterator.name))
                {
                    var props = PropertyDrawerUtils.GetVisibleChildren(iterator);

                    if (props.Count() > 1)
                    {
                        // write type's name here
                        //Debug.Log("iterator : " + iterator.name);

                        //GUILayout.Label(iterator.name);
                        //position.y += fieldHeight + (padding * 4);
                        EditorGUI.LabelField(position, iterator.name);
                        position.y += fieldHeight + (padding * 3);
                        foreach (var item in props)
                        {
                            // foreach metadata in this list container
                            if (item.name == "data")
                            {
                                var listContainer = PropertyDrawerUtils.GetVisibleChildren(item);

                                foreach (var metas in listContainer)
                                {
                                    var variable = PropertyDrawerUtils.GetVisibleChildren(metas);

                                    if (metas.name == "Name")
                                    {
                                        EditorGUI.LabelField(position, metas.stringValue);
                                        position.x -= LabelWidth;

                                        //Debug.Log("metas's name : " + metas.name + " " + metas.stringValue);
                                    }
                                    else if (metas.name == "Value")
                                    {
                                        position.width = EditorGUIUtility.currentViewWidth - position.x - 19;

                                        // TODO bad way of doing this should be changed in the future (along with the -labelwidth)
                                        // but on the other hand it allows to have Value<T> custom proptery drawer ...
                                        EditorGUI.PropertyField(position, metas);
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
            GraphVariableStorage targetObject =(GraphVariableStorage)GetTargetObjectOfProperty(property);
            int amountOfFields = targetObject.GetAmountOfGUIFields();

            return (fieldSize * amountOfFields) + (padding * amountOfFields);
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
            //while (index-- >= 0)
            //    enm.MoveNext();
            //return enm.Current;

            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }
            return enm.Current;
        }

    }
}