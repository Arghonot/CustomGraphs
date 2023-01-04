using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

namespace GraphEditor
{
    [CustomNodeEditor(typeof(Graph.Blackboard))]
    public class BlackBoardEditor : XNodeEditor.NodeEditor
    {
        int Selected = 0;
        public string[] options = Graph.GraphVariableStorage.GetPossibleTypesName();
        public Type[] optionsType = Graph.GraphVariableStorage.getPossibleTypes();
        string UIDToDelete = string.Empty;

        public override int GetWidth()
        {
            return ((Graph.Blackboard)target).width;
        }

        public override void OnBodyGUI()
        {
            Graph.Blackboard blackboard = target as Graph.Blackboard;

            AddVariable(blackboard);

            if (blackboard.storage.Count() > 0)
            {
                GUILayout.Space(4);
            }

            DebugGraph(blackboard);
            DisplayAll(blackboard);
        }

        private void DebugGraph(Graph.Blackboard blackboard)
        {
            if (GUILayout.Button("debug blackboard"))
            {
                blackboard.storage.DebugDictionnaries();
            }
        }

        void DisplayAll(Graph.Blackboard blackboard)
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

        void DisplayLine(Graph.Blackboard blackboard, Graph.VariableStorageRoot elem)
        {
            GUILayout.BeginHorizontal("box");

            // Display name and allow change
            string newName = EditorGUILayout.TextField(
                elem.Name,
                GUILayout.Width(blackboard.TextWidth));
            blackboard.storage.SetName(elem.GUID, newName);

            // Display type
            EditorGUILayout.LabelField(
                blackboard.storage.GetContainedType(elem.GUID).Name,
                GUILayout.Width(blackboard.TypeWidth));

            // Allow variable removal
            if (GUILayout.Button("-", GUILayout.Width(blackboard.MinusWidth)))
            {
                UIDToDelete = elem.GUID;
            }

            GUILayout.EndHorizontal();
        }

        void AddVariable(Graph.Blackboard blackboard)
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