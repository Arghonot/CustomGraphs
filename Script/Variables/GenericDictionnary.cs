using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    public class GenericDicionnary : Dictionary<string, object>
    {
        public T Get<T>(string name)
        {
            if (!Contains(name))
            {
                Debug.LogError("THE KEY [" + name + "] DOES NOT EXISTS.");
            }

            return (T)this[name];
        }

        public object TryGet(string name)
        {
            if (!Contains(name))
            {
                return null;
            }

            return this[name];
        }

        public object Get(string name)
        {
            return this[name];
        }

        public bool Set<T>(string name, T value)
        {
            if (this.ContainsKey(name))
            {
                this.Remove(name);
            }

            this.Add(name, (object)value);

            return true;
        }

        public bool Contains(string key)
        {
            return ContainsKey(key);
        }
    }
}