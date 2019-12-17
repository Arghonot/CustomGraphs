using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT
{
    /// <summary>
    /// The state of an action.
    /// </summary>
    public enum BTState
    {
        Success,
        Failure
    }

    /// <summary>
    /// Base class for any node's logic (can be a decorator, composite or leaf).
    /// </summary>
    public abstract class BTNode : Node
    {
        /// <summary>
        /// The node's childs.
        /// </summary>
        [Input] public BTState inPort;
        /// <summary>
        /// The node's parent.
        /// </summary>
        [Output] public BTState outPort;

        /// <summary>
        /// The node's data source.
        /// </summary>
        public GenericDictionary AIcontext = null;

        /// <summary>
        /// Store the node's context and call the run action if everything is 
        /// setup properly.
        /// </summary>
        /// <param name="port">The requested port</param>
        /// <returns></returns>
        public override object GetValue(NodePort port)
        {
            if (!Application.isPlaying ||
                port == null ||
                port.Connection == null)
            {
                return BTState.Failure;
            }

            BTNode parentNode = port.Connection.node as BTNode;

            if (parentNode != null)
            {
                AIcontext = parentNode.AIcontext;
            }

            return Run();
        }

        /// <summary>
        /// The behavior of the node
        /// </summary>
        /// <returns>Sucess if it's action succeeded, failure otherwise.</returns>
        public abstract BTState Run();

    }
}