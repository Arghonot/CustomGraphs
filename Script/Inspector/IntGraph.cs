using System.Linq;

namespace CustomGraph
{
    public class IntGraph : DefaultGraph
    {
        public RootInt Root;

        public new int Run(GraphVariableStorage newStorage = null)
        {
            if (newStorage != null)
            {
                this.originalStorage = newStorage;
            }

            return (int)Root.GetValue(Root.Ports.First());
        }
    }
}