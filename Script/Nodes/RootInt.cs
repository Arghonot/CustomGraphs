using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Graph
{
    [CustomNodeEditor(typeof(Graph.RootInt))]
    public class RootIntEditor : XNodeEditor.NodeEditor
    {
        public override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
        }

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
        }
    }

    public class RootInt : Root<int> {    }
}
