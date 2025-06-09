using XNode;

namespace CustomGraph
{
    [HideFromNodeMenu]
    public class NodeBase : Node
    {
        public override object GetValue(NodePort port) => Run();
        public virtual object Run() => null;
    }
}