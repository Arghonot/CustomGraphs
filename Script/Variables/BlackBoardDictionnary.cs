﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graph
{
    [Serializable]
    public class BlackBoardDictionnary : Dictionary<string, Variable>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<string> keys = new List<string>();
        [SerializeField] private List<Variable> values = new List<Variable>();

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            if (this.Count() == 0) return;

            foreach (KeyValuePair<string, Variable> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count() == 0) return;

            if (keys.Count != values.Count)
                throw new System.Exception(" there are " + keys.Count + " keys and " + values.Count + " values after deserialization. Make sure that both key and value types are serializable.");

            for (int i = 0; i < keys.Count; i++)
            {
                this.Add(keys[i], values[i]);
            }
        }
    }

}