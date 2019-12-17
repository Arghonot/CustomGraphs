using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT
{
    public class GenericDictionary
    {
        public class Container
        {
            public object content;
        }

        Dictionary<string, object> dic;

        public GenericDictionary()
        {
            dic = new Dictionary<string, object>();
        }

        public GenericDictionary Set<T>(string name, T newcontent)
        {
            if (!dic.ContainsKey(name))
            {
                dic.Add(name, new Container()
                {
                    content = newcontent
                });
            }
            else
            {
                ((Container)dic[name]).content = newcontent;
            }

            return this;
        }

        public T Get<T>(string name)
        {
            if (!dic.ContainsKey(name))
            {
                return default(T);
            }

            return (T)((Container)dic[name]).content;
        }
    }

    /// <summary>
    /// The actual BT graph containning all it's nodes
    /// </summary>
    [CreateAssetMenu]
    public class BTGraph : NodeGraph
    {
        Node Root = null;

        public void Init()
        {
            StoreRoot();
        }

        public BTState Run(GenericDictionary gd)
        {
            if (Root == null)
            {
                StoreRoot();
            }

            ((BTNode)Root).AIcontext = gd;

            return ((BTNode)Root).Run();
        }

        void StoreRoot()
        {
            Root = nodes.Where(x => x.name.Contains("Root")).First();
        }
    }
}