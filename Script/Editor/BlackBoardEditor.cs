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
        public string[] options = Variable.GetTypes();
        string UIDToDelete = string.Empty;

        public override int GetWidth()
        {
            return ((Graph.Blackboard)target).width;
        }

        public override void OnBodyGUI()
        {
            Graph.Blackboard blackboard = target as Graph.Blackboard;

            AddVariable(blackboard);

            if (blackboard.container.Count > 0)
            {
                GUILayout.Space(4);
            }

            DisplayAll(blackboard);


            // -- load system
            LoadSubgraphBlackboard();
        }

        private void LoadSubgraphBlackboard()
        {
            Graph.Blackboard blackboard = target as Graph.Blackboard;

            // TODO How to handle a variable being renamed ? it will keep the same GUID so it will never 
            // TODO handle a recursivity problem if a subgraph contain itself as a subgraph
            // TODO add a boolean on graph add node instead of a linq request
            if (blackboard.graph.nodes.Where(x => x.GetType().IsSubclassOf(typeof(Graph.SubGraphNode))).Count() != 0)
            {
                GUILayout.Space(10);

                if (GUILayout.Button("load subgraphs blackboard"))
                {
                    ((Graph.DefaultGraph)blackboard.graph).UpdateDictionnary(
                        ((Graph.DefaultGraph)blackboard.graph).CompileAllBlackboard());
                }
            }
        }

        void DisplayAll(Graph.Blackboard blackboard)
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
                blackboard.RemoveVariable(UIDToDelete);
            }
        }

        void DisplayLine(Graph.Blackboard blackboard, KeyValuePair<string, Variable> elem)
        {
            GUILayout.BeginHorizontal("box");

            // Display name and allow change
            elem.Value.Name = EditorGUILayout.TextField(
                elem.Value.Name,
                GUILayout.Width(blackboard.TextWidth));

            // Display type
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

        void AddVariable(Graph.Blackboard blackboard)
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