using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    public class GenericDicionnary : Dictionary<string, object>
    {
        public T Get<T>(string name)
        {
            return (T)this[name];
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
    }
}