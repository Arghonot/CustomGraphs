using System.Linq;

namespace CustomGraph
{
    public class IntGraph : GraphBase
    {
        public RootInt Root;

        public new int Run(GraphVariables newStorage = null)
        {
            if (newStorage != null)
            {
                this.originalStorage = newStorage;
            }

            return (int)Root.GetValue(Root.Ports.First());
        }
    }
}