using System;
using UnityEngine;

namespace Graph
{
    public class BlackboardableVariableAttribute : PropertyAttribute
    {
        public Type VariableType { get; private set; }

        public BlackboardableVariableAttribute(Type variableType)
        {
            VariableType = variableType;
        }
    }
}