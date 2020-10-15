namespace Graph
{
    public class RootInt : Root
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public int Input;

        public override object Run()
        {
            return GetInputValue<int>(
                 "Input",
                 this.Input);
        }
    }
}
