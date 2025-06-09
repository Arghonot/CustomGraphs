using XNode;

namespace Graph
{
    [HideFromNodeMenu]
    public class NodeBase : Node
    {
        public override object GetValue(NodePort port) => Run();
        public virtual object Run() => null;
    }
}