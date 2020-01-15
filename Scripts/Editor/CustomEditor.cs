using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static XNodeEditor.NodeEditor;
using XNode;
using XNodeEditor;
using System;

namespace BT
{
    [CustomNodeEditor(typeof(BT.BTNode))]
    public class BTNodeEditor : NodeEditor
    {
        private BTNode Node;

        public override void OnHeaderGUI()
        {
            GUILayout.Label(
                string.Join(
                    "\t",
                    new string[]
                    {
                        GetNumberInHierarchy(target).ToString(),
                        target.name
                    }),
                NodeEditorResources.styles.nodeHeader,
                GUILayout.Height(30));
        }

        private int GetNumberInHierarchy(Node target)
        {
            List<NodePort> parent = ((Node)target).Outputs.First().GetConnections();

            if (parent.Count == 0)
            {
                return 0;
            }

            List<NodePort> childs = parent.First().node.Inputs.First().GetConnections();
            int index = 0;

            foreach (var port in childs)
            {
                if (port.node == target)
                {
                    return index;
                }

                index++;
            }

            return 0;

        }
    }
}