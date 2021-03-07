using System.Collections.Generic;
using System.Linq;

namespace data_doc_api.Lib
{
    public class ParentChild<T>
    {
        public ParentChild(T parent, T child)
        {
            this.Parent = parent;
            this.Child = child;
        }

        public T Parent { get; set; }
        public T Child { get; set; }
    }

    public class TreeNode<T>
    {
        public T Current { get; set; }
        public IEnumerable<TreeNode<T>> Children { get; set; }

        public TreeNode(IEnumerable<ParentChild<T>> parentChildMapping, T node)
        {
            var mappings = parentChildMapping.Where(parentChildMapping => parentChildMapping.Parent.Equals(node));
            this.Current = node;
            this.Children = mappings.Select(mapping => new TreeNode<T>(parentChildMapping, mapping.Child));
        }
    }
}