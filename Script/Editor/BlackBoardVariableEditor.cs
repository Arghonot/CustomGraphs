using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using BT;
using UnityEditor;
using XNodeEditor;

namespace BTEditor
{
    [CustomNodeEditor(typeof(BT.BlackBoardVariable))]
    public class BlackBoardVariableEditor : XNodeEditor.NodeEditor
    {
        int selected = 0;

        public override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
        }

        public override void OnBodyGUI()
        {
            BlackBoardVariable variable = (BlackBoardVariable)target;

            DisplayPorts(variable);
            DisplayNameSelection(variable);

            serializedObject.ApplyModifiedProperties();

        }

        void DisplayPorts(BlackBoardVariable variable)
        {
            //if (variable == null ||
            //    variable.Name == null ||
            //    variable.Name == string.Empty)
            //{
            //    return;
            //}

            //string[] excludes = { "m_Script", "graph", "position", "ports" };

            //// Iterate through serialized properties and draw them like the Inspector (But with ports)
            //SerializedProperty iterator = serializedObject.GetIterator();
            //bool enterChildren = true;

            //while (iterator.NextVisible(enterChildren))
            //{
            //    enterChildren = false;
            //    if (excludes.Contains(iterator.name)) continue;

            //    NodeEditorGUILayout.PropertyField(iterator, true);

            //    Debug.Log(iterator.name);

            //    // Only draw if it is an ouput
            //    //if (iterator.name.Contains("value"))
            //    //{
            //    //    NodeEditorGUILayout.PropertyField(iterator, true);
            //    //}
            //}

            // Iterate through dynamic ports and draw them in the order in which they are serialized
            foreach (XNode.NodePort dynamicPort in target.DynamicPorts)
            {
                if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
                NodeEditorGUILayout.PortField(dynamicPort);
            }
        }

        void DisplayNameSelection(BlackBoardVariable variablenode)
        {
            string[] GUIDs = variablenode.Blackboard.GetGUIDS();
            string[] names = variablenode.Blackboard.GetVariableNames(GUIDs);

            int prevSelected = selected;
            selected = EditorGUILayout.Popup(selected, variablenode.GetPossibleVariables());

            if (variablenode.GetPossibleVariables().Length > 0)
            {
                if (prevSelected != selected)
                {
                    variablenode.SetVariable(names[selected], GUIDs[selected]);
                }
                else if (variablenode.Name == null)
                {
                    variablenode.SetVariable(names[0], GUIDs[0]);
                }
            }
        }
    }
}