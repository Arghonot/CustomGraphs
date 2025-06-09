using UnityEngine;

namespace CustomGraph
{
    [CreateNodeMenu("Graph/Misc/Comment")]
    public class CommentNode : Single
    {
        [TextArea]
        public string Comment;
    }
}