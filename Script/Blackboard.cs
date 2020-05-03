using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using System;

namespace BT
{

    //public enum BlackBoardType
    //{
    //    Int,
    //    Bool,
    //    String,
    //    GameObject
    //}

    //// TODO move it to another file
    //public class BlackboardElement
    //{
    //    public string ValueName;
    //    public BT.BlackBoardType ValueType;
    //    public object value;
    //}

    ///// <summary>
    ///// This class is supposed to be used ONLY in Xnode, it contains additional datas
    ///// that aren't needed in unity's inspector or ingame.
    ///// </summary>
    //public class BlackBoardGraphElement : BlackboardElement
    //{
    //    private int selectedType;
    //    public int SelectedType
    //    {
    //        get { return selectedType; }
    //        set
    //        {
    //            ValueType = (BT.BlackBoardType)value;
    //            this.value = DefaultGenerator.CreateDefaultValue(ValueType);
    //            selectedType = value;
    //        }
    //    }
    //}

    //public static class DefaultGenerator
    //{
    //    public static object CreateDefaultValue(BT.BlackBoardType type)
    //    {
    //        switch (type)
    //        {
    //            case BT.BlackBoardType.Int:
    //                return (object)0;
    //            case BT.BlackBoardType.Bool:
    //                return (object)true;
    //            case BT.BlackBoardType.String:
    //                return (object)"DefaultString";
    //            default:
    //                return (object)0;
    //        }
    //    }

    //    public static string CreateDefaultName(BT.BlackBoardType type)
    //    {
    //        switch (type)
    //        {
    //            case BT.BlackBoardType.Int:
    //                return "Int";
    //            case BT.BlackBoardType.Bool:
    //                return "Bool";
    //            case BT.BlackBoardType.String:
    //                return "String";
    //            default:
    //                return "UnknownDefault";
    //        }
    //    }
    //}

    [CreateNodeMenu("MYBT/Blackboard")]
    public class Blackboard : Node
    {
        public int TextWidth = 130;
        public int TypeWidth = 100;
        public int MinusWidth = 20;

        public int width = 300;

        /// <summary>
        /// GUID - variables data
        /// </summary>
        public Dictionary<string, Variable> container;

        /// <summary>
        /// GUID current name
        /// </summary>
        public Dictionary<string, string> ContainerNames;

        private void Awake()
        {
            container = new Dictionary<string, Variable>();
        }

        // TODO make a string[] of the possible variables, so it's not recalculated every frames
        public string[] GetVariableNames()
        {
            return container.Select(x => x.Value.Name).ToArray();
        }

        public bool AddVariable(string guid, string type)
        {
            if (container.ContainsKey(name))
            {
                return false;
            }

            container.Add(guid, Variable.CreateType(type)); //GetBlackBoardElement(type));

            return true;
        }

        //BlackBoardGraphElement GetBlackBoardElement(BlackBoardType type)
        //{
        //    return new BlackBoardGraphElement() {
        //        ValueName = DefaultGenerator.CreateDefaultName(type),
        //        ValueType = BT.BlackBoardType.Int,
        //        value = DefaultGenerator.CreateDefaultValue(type),
        //        SelectedType = (int)type
        //        };
        //}

        //public bool AddVariable(string guid, string index)
        //{
        //    //BlackBoardType type = GetTypeFromIndex(index);

        //    return AddVariable(guid, type);
        //}

        // TODO remove this one
        //public BlackBoardType GetTypeFromName(string name)
        //{
        //    if (!container.ContainsKey(name))
        //    {
        //        return BlackBoardType.Int;
        //    }

        //    return container[name].get;
        //}

        //public BlackBoardType   GetTypeFromIndex(int index)
        //{
        //    if (index > (int)Enum.GetValues(typeof(BlackBoardType)).Cast<BlackBoardType>().Max())
        //    {
        //        return Enum.GetValues(typeof(BlackBoardType)).Cast<BlackBoardType>().Max();
        //    }

        //    if (index == 0)
        //    {
        //        return BlackBoardType.Int;
        //    }

        //    return (BT.BlackBoardType)index;
        //}

        public string GetTypeFromGUID(string guid)
        {
            if (!container.ContainsKey(guid))
            {
                return "int";
            }

            return Variable.GetType(container[guid].GetValueType());
        }

        public string GetName(string guid)
        {
            if (!container.ContainsKey(guid))
            {
                return null;
            }

            return container[guid].Name;
        }
    }
}