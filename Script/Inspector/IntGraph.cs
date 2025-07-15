using System;

namespace CustomGraph
{
    public class IntGraph : GraphBase
    {
        public override Type GetRootNodeType() => typeof(int);
    }
}