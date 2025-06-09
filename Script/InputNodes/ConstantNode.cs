namespace Graph
{

    [HideFromNodeMenu]
    public class ConstantNode<T> : Leaf<T>
    {
        public T value;
        public override object Run() => value;
    }
}