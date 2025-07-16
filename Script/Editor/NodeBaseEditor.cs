using UnityEditor;
using XNode;
using XNodeEditor;
using System.Reflection;
using static XNode.Node;
using UnityEngine;
using System;


namespace CustomGraph
{
    [CustomNodeEditor(typeof(NodeBase))]
    public class NodeBaseEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            // Let XNode handle all ports/fields normally
            base.OnBodyGUI();
            DrawNonInstantiableTypeInputPorts(target);

            serializedObject.ApplyModifiedProperties();
        }

        public static void DrawNonInstantiableTypeInputPorts(object target)
        {
            NodeBase node = target as NodeBase;

            // Check for problematic [Input] fields
            foreach (FieldInfo field in node.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var inputAttr = field.GetCustomAttribute<InputAttribute>();
                if (inputAttr == null) continue;

                // Get field type
                Type fieldType = field.FieldType;

                // Check if abstract/interface/generic
                bool isProblematic = fieldType.IsAbstract || fieldType.IsInterface;

                if (isProblematic)
                {
                    NodePort port = node.GetPort(field.Name);
                    if (port != null)
                    {
                        NodeEditorGUILayout.PortField(new GUIContent(ObjectNames.NicifyVariableName(field.Name)), port);
                    }
                }
            }
        }
    }
}