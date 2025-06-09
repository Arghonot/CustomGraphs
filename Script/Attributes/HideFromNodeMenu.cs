using System;
using UnityEngine;

namespace Graph
{
    /// <summary>
    /// Use that attribute if you don't want a class to be visible in the node creation ui.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HideFromNodeMenu : PropertyAttribute { }
}

