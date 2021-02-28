using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    // TODO create a node without input or output
    [CreateNodeMenu("Graph/Misc/Comment")]
    public class CommentNode : Single
    {
        [TextArea]
        public string Comment;
    }
}