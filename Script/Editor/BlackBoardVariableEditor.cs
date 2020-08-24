using Graph;
using UnityEditor;
using XNodeEditor;

namespace GraphEditor
{
    [CustomNodeEditor(typeof(Graph.BlackBoardVariable))]
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
            string[] names = variablenode.Blackboard.GetVariableNames(GUIDs);

            if (variablenode.VariableIndex < GUIDs.Length)
            {
                selected = variablenode.VariableIndex;
            }

            int prevSelected = selected;
            selected = EditorGUILayout.Popup(selected, variablenode.GetPossibleVariables());

            if (variablenode.GetPossibleVariables().Length > 0)
            {
                if (prevSelected != selected)
                {
                    variablenode.SetVariable(names[selected], GUIDs[selected], selected);
                }
                else if (variablenode.Name == string.Empty)
                {
                    variablenode.SetVariable(names[0], GUIDs[0], 0);
                }
            }
        }
    }
}