using System;

namespace Graph
{
    public class SubGraphMaster : NodeBase
    {
        public DefaultGraph SubGraph;
    }

    public class SubGraphNode<T> : SubGraphMaster
    {

        public void Awake()
        {
            if (GetOutputPort("Output") == null)
            {
                AddDynamicOutput(
                    typeof(T),
                    ConnectionType.Multiple,
                    TypeConstraint.None,
                    "Output");
            }
        }

        public override object Run()
        {
            throw new NotImplementedException();
        }
    }
}