namespace CustomGraph
{
    [NodeTint(ColorProfile.Branch)]
    [HideFromNodeMenu]
    public class Branch<T> : NodeBase
    {
        public void Awake()
        {
            if (GetOutputPort("Output") == null)
            {
                AddDynamicOutput(typeof(T), ConnectionType.Multiple, TypeConstraint.Strict, "Output");
            }
        }
    }
}