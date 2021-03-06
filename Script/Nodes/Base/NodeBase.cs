﻿using XNode;

namespace Graph
{
    public class NodeBase : XNode.Node
    {
        public override object GetValue(NodePort port)
        {
            return Run();
        }

        public virtual object Run()
        {
            return null;
        }
    }
}