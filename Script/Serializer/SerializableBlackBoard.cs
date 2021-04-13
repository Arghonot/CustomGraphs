using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class StorableType : System.Attribute
    {
        public Type ReferenceType;

        public StorableType(Type reftype)
        {
            ReferenceType = reftype;
        }
    }

    public class VariableStorageRoot
    {
        public string GUID;
        public string Name;
        public object copyValue;

        public VariableStorageRoot()
        {
            GUID = Guid.NewGuid().ToString();
        }
    }

    public class VariableStorage<T> : VariableStorageRoot
    {
        public T Value;

        public void StoreValue()
        {
            Value = (T)copyValue;
        }

        public VariableStorage<T> SetAsCopy(VariableStorage<T> original)
        {
            this.Name = original.Name;
            this.Value = (T)(original.Value);

            return this;
        }

        //public override object Clone()
        //{
        //    var item = (VariableStorage<T>)MemberwiseClone();

        //    item.Name = this.Name;
        //    item.GUID = Guid.NewGuid().ToString();
        //    item.Value = this.Value;

        //    return item;
        //}



        //public VariableStorage(VariableStorage<T> original)
        //{
        //    Debug.Log("Copy constructor");
        //    this.Name = original.Name;
        //    this.Value = original.Value;
        //}
    }

    [Serializable]
    [StorableType(typeof(string))]
    public class StringVariable : VariableStorage<string> { }
    [Serializable]
    [StorableType(typeof(double))]
    public class DoubleVariable : VariableStorage<double> { }
    [Serializable]
    [StorableType(typeof(int))]
    public class IntVariable : VariableStorage<int> { }
    [Serializable]
    [StorableType(typeof(Transform))]
    public class TransformVariable : VariableStorage<Transform> { }

    [Serializable]
    [StorableType(typeof(AnimationCurve))]
    public class CurveVariable : VariableStorage<AnimationCurve> { }

    [Serializable]
    [StorableType(typeof(LibNoise.QualityMode))]
    public class QualityModeVariable : VariableStorage<LibNoise.QualityMode> { }

    [Serializable]
    // Basically do the bridge between Graph edition - Data edition in scene and
    // Scene runtime
    public class SerializableBlackBoard
    {
        // TODO Yeah terrible way of doing this, need some further
        // investigations on how to change it.
        public DoubleVariable[] Doubles;
        public IntVariable[] Ints;
        public StringVariable[] Strings;
        public TransformVariable[] Transforms;
        public CurveVariable[] AnimationCurves;
        public QualityModeVariable[] QualityModes;

        public void InitializeContent(DefaultGraph graph)
        {
            var compiledBlackBoard = graph.CompileAllBlackboard();

            var DoublesContainer = new List<DoubleVariable>();
            var IntsContainer = new List<IntVariable>();
            var StringContainer = new List<StringVariable>();
            var QualityModeContainer = new List<QualityModeVariable>();

            foreach (var item in compiledBlackBoard)
            {
                Debug.Log(item.Value.Name + " " + item.Value.TypeName);

                switch (item.Value.TypeName)
                {
                    case "double":
                        DoublesContainer.Add(new DoubleVariable()
                        {
                            Name = item.Value.Name
                        });
                        break;
                    case "int":
                        IntsContainer.Add(new IntVariable()
                        {
                            Name = item.Value.Name
                        });
                        break;
                    case "string":
                        StringContainer.Add(new StringVariable()
                        {
                            Name = item.Value.Name
                        });
                        break;
                    case "QualityMode":
                        QualityModeContainer.Add(new QualityModeVariable()
                        {
                            Name = item.Value.Name
                        });
                        break;
                    default:
                        break;
                }
                //HandleVariable(item.Value, DoublesContainer, IntsContainer, StringContainer);
            }

            Strings = StringContainer.ToArray();
            Doubles = DoublesContainer.ToArray();
            Ints = IntsContainer.ToArray();
            QualityModes = QualityModeContainer.ToArray();
        }

        //public void HandleVariable(
        //    Variable item,
        //    List<DoubleVariable> doubleContainer,
        //    List<IntVariable> intContainer,
        //    List<StringVariable> stringContainer)
        //{

        //    Debug.Log(item.Name);
        //    // TODO use type name to make it directly with the attribute instead
        //    switch (item.TypeName)
        //    {
        //        case "double":
        //            doubleContainer.Add(new DoubleVariable()
        //            {
        //                Name = item.Name
        //            });
        //            break;
        //        case "int":
        //            intContainer.Add(new IntVariable()
        //            {
        //                Name = item.Name
        //            });
        //            break;
        //        case "string":
        //            stringContainer.Add(new StringVariable()
        //            {
        //                Name = item.Name
        //            });
        //            break;
        //        default:
        //            break;
        //    }
        //}

        public GenericDicionnary GetArguments()
        {
            GenericDicionnary gd = new GenericDicionnary();

            foreach (var item in Doubles)
            {
                gd.Set<double>(item.Name, item.Value);
            }
            foreach (var item in Ints)
            {
                gd.Set<int>(item.Name, item.Value);
            }
            foreach (var item in Strings)
            {
                gd.Set<string>(item.Name, item.Value);
            }
            return null;
        }
    }
}