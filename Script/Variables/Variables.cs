using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.UI.Image;
using Type = System.Type;

namespace Graph
{
    [Serializable]
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class StorableType : System.Attribute
    {
        public Type ReferenceType;

        public StorableType(Type reftype)
        {
            ReferenceType = reftype;
        }
    }

    [Serializable]
    public class VariableStorageRoot : ICloneable
    {
        [SerializeField] public string GUID;
        [SerializeField] public string Name;

        public Action<string> OnUpdateGUID;

        public void setGuid(string to)
        {
            OnUpdateGUID?.Invoke(to);

            GUID = to;
        }

        public VariableStorageRoot()
        {
            GUID = Guid.NewGuid().ToString();
        }

        public virtual string ToString()
        {
            return string.Join(" ", new string[]
            {
                GUID,
                Name
            });
        }

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        public virtual object GetValue()
        {
            throw new NotImplementedException();
        }

        public virtual void SetValue(object newValue)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class VariableStorage<T> : VariableStorageRoot
    {
        [SerializeField] public T Value;

        public VariableStorage<T> SetAsCopy(VariableStorage<T> original)
        {
            this.Name = original.Name;
            this.Value = (T)(original.Value);

            return this;
        }

        public override object GetValue()
        {
            return Value;
        }

        public override void SetValue(object newValue)
        {
            Value = (T)newValue;
        }

        public void Set(T newValue)
        {
            Value = newValue;
        }

        public override string ToString()
        {
            return string.Join(" ", new string[]
            {
                GUID,
                Name,
                Value.ToString()
            });
        }

        public override object Clone()
        {
            VariableStorage<T> newVariable = (VariableStorage<T>)Activator.CreateInstance(this.GetType());

            newVariable.Name = Name;
            newVariable.GUID = GUID;
            newVariable.Value = Value;

            return newVariable;
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

    #region C# types

    [Serializable]
    [StorableType(typeof(float))]
    public class floatVariable : VariableStorage<float> { }
    [Serializable]
    [StorableType(typeof(long))]
    public class LongVariable : VariableStorage<long> { }
    [Serializable]
    [StorableType(typeof(bool))]
    public class BoolVariable : VariableStorage<bool> { }
    [Serializable]
    [StorableType(typeof(string))]
    public class StringVariable : VariableStorage<string> { }
    [Serializable]
    [StorableType(typeof(double))]
    public class DoubleVariable : VariableStorage<double> { }
    [Serializable]
    [StorableType(typeof(int))]
    public class IntVariable : VariableStorage<int> { }

    #endregion

    #region Unity types

    [Serializable]
    [StorableType(typeof(AnimationCurve))]
    public class AnimationCurveVariable : VariableStorage<AnimationCurve> { }
    [Serializable]
    [StorableType(typeof(NavMeshAgent))]
    public class NavMeshAgentVariable : VariableStorage<NavMeshAgent> { }
    [Serializable]
    [StorableType(typeof(GameObject))]
    public class GameObjectVariable : VariableStorage<GameObject> { }
    [Serializable]
    [StorableType(typeof(Vector3))]
    public class Vector3Variable : VariableStorage<Vector3> { }
    [Serializable]
    [StorableType(typeof(Quaternion))]
    public class QuaternionVariable : VariableStorage<Quaternion> { }
    [Serializable]
    [StorableType(typeof(Transform))]
    public class TransformVariable : VariableStorage<Transform> { }

    #endregion
}