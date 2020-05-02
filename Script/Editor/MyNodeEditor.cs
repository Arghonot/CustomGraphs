using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BTEditor
{
    [CustomNodeEditor(typeof(BT.MyNode))]
    public class MyNodeEditor : XNodeEditor.NodeEditor
    {
        public int selected = 0;
        public string[] options = new string[] { "Cube", "Sphere", "Plane" };

        public override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
        }

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            selected = EditorGUILayout.Popup(selected, options);
        }
    }
}
