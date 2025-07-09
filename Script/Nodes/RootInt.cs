namespace CustomGraph
{
    [HideFromNodeMenu]
    public class RootInt : Root
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public int Input;

        public override bool CanRun() => GetInputPort("Input").IsConnected;
        public override object Run() => GetInputValue<int>("Input", this.Input);
    }
}
