using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

namespace GraphEditor
{
    [CustomNodeEditor(typeof(CustomGraph.Blackboard))]
    public class BlackBoardEditor : XNodeEditor.NodeEditor
    {
        int Selected = 0;
        public string[] options = CustomGraph.GraphVariableStorage.GetPossibleTypesName();
        public Type[] optionsType = CustomGraph.GraphVariableStorage.getPossibleTypes();
        string UIDToDelete = string.Empty;

        public override int GetWidth()
        {
            return ((CustomGraph.Blackboard)target).width;
        }

        public override void OnBodyGUI()
        {
            CustomGraph.Blackboard blackboard = target as CustomGraph.Blackboard;

            AddVariable(blackboard);

            if (blackboard.storage.Count() > 0)
            {
                GUILayout.Space(4);
            }

            DebugGraph(blackboard);
            DisplayAll(blackboard);
        }

        private void DebugGraph(CustomGraph.Blackboard blackboard)
        {
            if (GUILayout.Button("debug blackboard"))
            {
                blackboard.storage.DebugDictionnaries();
            }
        }

        void DisplayAll(CustomGraph.Blackboard blackboard)
        {
            if (blackboard.storage == null ||
                blackboard.storage.Count() == 0)
            {
                return;
            }

            string[] Guids = blackboard.storage.getAllGuids();

            for (int i = 0; i < Guids.Length; i++)
            {
                DisplayLine(blackboard, blackboard.storage.GetContainerInstance(Guids[i]));
            }

            if (UIDToDelete != string.Empty)
            {
                blackboard.storage.Remove(UIDToDelete);
                UIDToDelete = string.Empty;
            }
        }

        void DisplayLine(CustomGraph.Blackboard blackboard, CustomGraph.VariableStorageRoot elem)
        {
            GUILayout.BeginHorizontal("box");

            // Display name and allow change
            string newName = EditorGUILayout.TextField(elem.Name, GUILayout.Width(blackboard.TextWidth));
            blackboard.storage.SetName(elem.GUID, newName);

            // Display type
            EditorGUILayout.LabelField(blackboard.storage.GetContainedType(elem.GUID).Name, GUILayout.Width(blackboard.TypeWidth));

            // Allow variable removal
            if (GUILayout.Button("-", GUILayout.Width(blackboard.MinusWidth)))
            {
                UIDToDelete = elem.GUID;
            }

            GUILayout.EndHorizontal();
        }

        void AddVariable(CustomGraph.Blackboard blackboard)
        {
            GUILayout.BeginHorizontal();

            Selected = EditorGUILayout.Popup(Selected, options);
            if (GUILayout.Button("Add"))
            {
                blackboard.storage.Add(optionsType[Selected]);
            }

            GUILayout.EndHorizontal();
        }
    }
}