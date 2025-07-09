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

    [NodeTint(ColorProfile.Root)]
    [HideFromNodeMenu]
    public abstract class Root : NodeBase
    {
        public abstract bool CanRun();// => throw new System.NotImplementedException();
    }
}