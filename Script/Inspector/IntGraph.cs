using System.Linq;

namespace Graph
{
    public class IntGraph : DefaultGraph
    {
        public RootInt Root;

        public int Run(GraphVariableStorage newStorage = null)
        {
            if (newStorage != null)
            {
                this.originalStorage = newStorage;
            }

            return (int)Root.GetValue(Root.Ports.First());
        }
    }
}