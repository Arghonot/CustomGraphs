using CustomGraph;
using UnityEditor;
using XNodeEditor;

namespace GraphEditor
{
    [CustomNodeEditor(typeof(CustomGraph.BlackBoardVariable))]
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
            // Iterate through dynamic ports and draw them in the order in which they are serialized
            foreach (XNode.NodePort dynamicPort in target.DynamicPorts)
            {
                if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
                NodeEditorGUILayout.PortField(dynamicPort);
            }
        }

        void DisplayNameSelection(BlackBoardVariable variablenode)
        {
            if (variablenode.Blackboard == null) return;
            string[] GUIDs = variablenode.Blackboard.GetGUIDS();
            string[] names = variablenode.Blackboard.GetAllNames();

            if (variablenode.VariableIndex < GUIDs.Length)
            {
                selected = variablenode.VariableIndex;
            }

            if (names.Length > 0)
            {
                selected = EditorGUILayout.Popup(selected, variablenode.GetPossibleVariables());

                if (selected != variablenode.VariableIndex)
                {
                    variablenode.SetVariable(
                        variablenode.Blackboard.storage.GetName(GUIDs[selected]),
                        GUIDs[selected],
                        selected);
                }
            }
            else
            {
                EditorGUILayout.LabelField("Use the blackboard first.");
            }


        }
    }
}