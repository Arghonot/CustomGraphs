namespace CustomGraph
{
    [CreateNodeMenu("Graph/Constants/StringNode")]
    [NodeTint(ColorProfile.Input)]
    public class StringNode : Leaf<string>
    {
        public string value;
    }
}