using System;
using UnityEngine;

namespace CustomGraph
{
    /// <summary>
    /// Use that attribute if you don't want a class to be visible in the node creation ui.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HideFromNodeMenu : PropertyAttribute { }
}

