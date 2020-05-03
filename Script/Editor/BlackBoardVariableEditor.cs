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
        }

        void DisplayPorts(BlackBoardVariable variable)
        {
            if (variable == null ||
                variable.Name == null ||
                variable.Name == string.Empty)
            {
                return;
            }

            string[] excludes = { "m_Script", "graph", "position", "ports" };

            // Iterate through serialized properties and draw them like the Inspector (But with ports)
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (excludes.Contains(iterator.name)) continue;

                // Only draw if it is an ouput
                if (iterator.name.Contains("Output"))
                {
                    // with the type corresponding to the variable
                    if (iterator.name.Contains(
                        variable.Blackboard.GetTypeFromGUID(variable.uid)))
                    {
                        NodeEditorGUILayout.PropertyField(iterator, true);
                    }
                }
            }
        }

        void DisplayNameSelection(BlackBoardVariable variable)
        {
            string[] GUIDs = variable.GetGUIDs();
            string[] names = variable.GetNames(GUIDs);

            int prevSelected = selected;
            selected = EditorGUILayout.Popup(selected, variable.GetPossibleVariables());

            if (variable.GetPossibleVariables().Length > 0)
            {
                if (prevSelected != selected)
                {
                    variable.Name = names[selected];//variable.GetPossibleVariables()[selected];
                    variable.uid = GUIDs[selected];
                }
                else if (variable.Name == null)
                {
                    variable.Name = names[0];//variable.GetPossibleVariables()[selected];
                    variable.uid = GUIDs[0];
                    //variable.Name = variable.GetPossibleVariables()[0];
                }
            }
        }
    }
}