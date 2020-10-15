using System.Linq;

namespace Graph
{
    [NodeTint(ColorProfile.Branch)]
    public class Branch<T> : NodeBase
    {
        public void Awake()
        {
            if (GetOutputPort("Output") == null)
            {
                AddDynamicOutput(
                    typeof(T),
                    ConnectionType.Multiple,
                    TypeConstraint.Strict,
                    "Output");
            }
        }
    }
}