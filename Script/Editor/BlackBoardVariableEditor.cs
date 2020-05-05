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