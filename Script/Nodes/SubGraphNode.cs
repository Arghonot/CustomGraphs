using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;
using static XNodeEditor.NodeEditor;

namespace Graph
{
    [CustomNodeEditor(typeof(Graph.SubGraphMaster))]
    public class SubGraphMaster : NodeBase
    {
        public DefaultGraph targetSubGraph;

        private GraphVariableStorage targetBlackboard() => targetSubGraph.blackboard.storage;
        /*[HideInInspector] */[SerializeField] private string[] _fieldNames;
        /*[HideInInspector] */[SerializeField] private string[] _guids;

        [ContextMenu("FlushFieldNames")]
        public void FlushFieldNames()
        {
            _fieldNames = null;
        }

        public void OnDropGraph()
        {
            FlushInputs();
            CreateInputs();
        }

        public void OnRemoveGraph()
        {
            FlushInputs();
        }

        private void FlushInputs()
        {
            if (_fieldNames == null)
            {
                return;
            }

            for (int i = 0; i < _fieldNames.Length; i++)
            {
                if (GetInputPort(_fieldNames[i]) != null)
                {
                    RemoveDynamicPort(_fieldNames[i]);
                }
            }

            _guids = null;
            _fieldNames = null;
        }

        private void CreateInputs()
        {
            _guids = targetBlackboard().getAllGuids();
            _fieldNames = new string[_guids.Length];
            UnityEngine.Debug.Log("CreateInputs " + _guids.Length);

            for (int i = 0; i < _guids.Length; i++)
            {
                _fieldNames[i] = targetBlackboard().GetName(_guids[i]);
                UnityEngine.Debug.Log("in for " + _fieldNames[i]);

                AddDynamicInput(targetBlackboard().GetVariableType(_guids[i]), ConnectionType.Override, TypeConstraint.Strict, _fieldNames[i]);
            }
        }

        [ContextMenu("DebugTestFillDictionnary")]
        public void DebugTestFillDictionnary()
        {
            var copy = targetSubGraph.originalStorage.CreateDeepCopy();

            for (int i = 0; i < _guids.Length; i++)
            {
                UnityEngine.Debug.Log(copy.ContainsGuid(_guids[i]));
            }

            foreach (var item in targetSubGraph.originalStorage.getAllGuids())
            {
                UnityEngine.Debug.Log(item);
            }
        }

        public GraphVariableStorage GenerateProperStorage()
        {
            var storage = targetSubGraph.originalStorage.CreateDeepCopy();
            object value = null;
            for (int i = 0; i < _guids.Length; i++)
            {
                value = GetInputValue<object>(_fieldNames[i]);

                if (value == null)
                {
                    value = Activator.CreateInstance(storage.GetVariableType(_guids[i]));
                }

                storage.SetValue(_guids[i], value);
            }

            return storage;
        }
    }

    public class SubGraphNode<T> : SubGraphMaster
    {
        public void Awake()
        {
            if (GetOutputPort("Output") == null)
            {
                AddDynamicOutput(
                    typeof(T),
                    ConnectionType.Multiple,
                    TypeConstraint.None,
                    "Output");
            }
        }
    }
}