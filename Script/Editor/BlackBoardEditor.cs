using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BTEditor
{
    [CustomNodeEditor(typeof(BT.Blackboard))]
    public class BlackBoardEditor : XNodeEditor.NodeEditor
    {
        int Selected = 0;
        // TODO get this array from the DefaultGenerator so everything
        // that needs to be changed is there 
        public string[] options = Variable.GetTypes();
        string UIDToDelete = string.Empty;

        // = new string[] { "Int", "Bool", "String" };

        public override int GetWidth()
        {
            return ((BT.Blackboard)target).width;
        }

        public override void OnBodyGUI()
        {
            if (target == null ||
                ((BT.Blackboard)target).container == null)
            {
                return;
            }

            BT.Blackboard blackboard = target as BT.Blackboard;

            AddVariable(blackboard);

            if (blackboard.container.Count > 0)
            {
                GUILayout.Space(20);
            }

            DisplayAll(blackboard);
        }

        void DisplayAll(BT.Blackboard blackboard)
        {
            if (blackboard.container == null ||
                blackboard.container.Count == 0)
            {
                return;
            }

            foreach (var item in blackboard.container)
            {
                DisplayLine(blackboard, item);
            }

            if (UIDToDelete != string.Empty)
            {
                blackboard.container.Remove(UIDToDelete);
            }
        }

        void DisplayLine(BT.Blackboard blackboard, KeyValuePair<string, Variable> elem)
        {
            GUILayout.BeginHorizontal("box");

            // Display name and allow change
            elem.Value.Name = EditorGUILayout.TextField(
                elem.Value.Name,
                GUILayout.Width(blackboard.TextWidth));

            // Display type and allow change
            //elem.Value.SelectedType = EditorGUILayout.Popup(
            //    elem.Value.SelectedType,
            //    options,
            //    GUILayout.Width(blackboard.TypeWidth));

            EditorGUILayout.LabelField(
                elem.Value.TypeName,
                GUILayout.Width(blackboard.TypeWidth));

            // Allow variable removal
            if (GUILayout.Button("-", GUILayout.Width(blackboard.MinusWidth)))
            {
                UIDToDelete = elem.Key;
            }

            GUILayout.EndHorizontal();
        }

        void AddVariable(BT.Blackboard blackboard)
        {
            GUILayout.BeginHorizontal();

            Selected = EditorGUILayout.Popup(Selected, options);

            if (GUILayout.Button("Add"))
            {
                var guid =
                    Guid.NewGuid().ToString();
                
                blackboard.AddVariable(guid, options[Selected]);
            }

            GUILayout.EndHorizontal();
        }
    }
}