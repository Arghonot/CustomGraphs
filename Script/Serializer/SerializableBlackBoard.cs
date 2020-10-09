using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    [Serializable]
    public class DoubleVariable
    {
        public string Name;
        public double Value;
    }

    [Serializable]
    public class IntVariable
    {
        public string Name;
        public int Value;
    }

    [Serializable]
    //Basically do the bridge between Graph edition - Data edition in scene and Scene runtime
    public class SerializableBlackBoard /*: ScriptableObject*/
    {
        // TODO Yeah terrible way of doing this, need some further investigations on how to change it.
        public DoubleVariable[] Doubles;
        public IntVariable[] Ints;

        public SerializableBlackBoard()
        {
        }

        public void InitializeContent(DefaultGraph graph)
        {
            var compiledBlackBoard = graph.CompileAllBlackboard();

            var DoublesContainer = new List<DoubleVariable>();
            var IntsContainer = new List<IntVariable>();
            //Names = new List<string>();

            foreach (var item in compiledBlackBoard)
            {
                HandleVariable(item.Value, DoublesContainer, IntsContainer);
            }

            Doubles = DoublesContainer.ToArray();
            Ints = IntsContainer.ToArray();
        }

        public void HandleVariable(
            Variable item,
            List<DoubleVariable> doubleContainer,
            List<IntVariable> intContainer)
        {
            // TODO use type name to make it directly with the attribute instead
            switch (item.TypeName)
            {
                case "double":
                    doubleContainer.Add(new DoubleVariable()
                    {
                        Name = item.Name
                    });
                    break;
                case "int":
                    intContainer.Add(new IntVariable()
                    {
                        Name = item.Name
                    });
                    break;
                default:
                    break;
            }
        }

        public GenericDicionnary GetArguments()
        {
            return null;
        }
    }
}