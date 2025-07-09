using System.Collections.Generic;
using XNode;

namespace CustomGraph
{
    public static class ColorProfile
    {
        public const string Root = "#E9C46A";
        public const string Branch = "#48a9a6";
        public const string Leaf = "#457b9d";
        public const string Blackboard = "#1d3557";
        public const string Debug = "#393D3F";
        public const string Input = "#395985";
        public const string Mathematics = "#5c677d";
    }

    public interface IRoot
    {
        public bool CanRun();
        public object Run();
        IEnumerable<NodePort> Ports { get; }
        object GetValue(NodePort port);
    }

    [NodeTint(ColorProfile.Root)]
    [HideFromNodeMenu]
    public class Root<T> : NodeBase, IRoot
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public T Input;

        public bool CanRun() => GetInputPort("Input").IsConnected;
        public override object Run() => GetInputValue<T>("Input", Input);
    }
}