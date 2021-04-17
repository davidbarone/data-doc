using System.Collections.Generic;
using System.Linq;
using System;

namespace data_doc_api.Lib
{
    /// <summary>
    /// Represents a parent-child relationship
    /// </summary>
    /// <typeparam name="T">The type of each of the parent and child objects</typeparam>
    public class ParentChild<T>
    {
        /// <summary>
        /// Constructor for the ParentChild class
        /// </summary>
        /// <param name="parent">The parent object</param>
        /// <param name="child">The child object</param>
        public ParentChild(T parent, T child)
        {
            this.Parent = parent;
            this.Child = child;
        }

        /// <summary>
        /// The parent object
        /// </summary>
        public T Parent { get; set; }
        /// <summary>
        /// The child object
        /// </summary>
        public T Child { get; set; }
    }

    /// <summary>
    /// A tree node. Each tree node contains the current object nod, and its parent and children nodes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNode<T>
    {
        /// <summary>
        /// Optional compare function
        /// </summary>
        private Func<T, T, bool> EqualityComparer { get; set; }

        /// <summary>
        /// The current node of the tree.
        /// </summary>
        public T Current { get; set; }

        /// <summary>
        /// The parent node
        /// </summary>
        public TreeNode<T> Parent { get; set; }

        /// <summary>
        /// This children of the node.
        /// </summary>
        public IEnumerable<TreeNode<T>> Children { get; set; }

        /// <summary>
        /// Constructor. Builds a Tree off a node, using a flag list of parent-child relationships.
        /// </summary>
        /// <param name="parentChildMapping">A list of parent-child objects to build the tree node from</param>
        /// <param name="node">The current node</param>
        /// <param name="parent">The parent of the current node</param>
        /// <param name="equalityComparer"></param>
        public TreeNode(IEnumerable<ParentChild<T>> parentChildMapping, T node, TreeNode<T> parent = null, Func<T, T, bool> equalityComparer = null)
        {
            this.EqualityComparer = equalityComparer;
            IEnumerable<ParentChild<T>> mappings = null;

            if (this.EqualityComparer != null)
            {
                mappings = parentChildMapping
                    .Where(parentChildMapping => equalityComparer(parentChildMapping.Parent, node))
                    .Where(parentChildMapping => !IsAncestorOf(parent, node));
            }
            else
            {
                mappings = parentChildMapping
                    .Where(parentChildMapping => parentChildMapping.Parent.Equals(node))
                    .Where(parentChildMapping => !IsAncestorOf(parent, node));
            }

            this.Current = node;
            this.Parent = parent;
            this.Children = mappings.Select(mapping => new TreeNode<T>(parentChildMapping, mapping.Child, this, this.EqualityComparer));
        }

        /// <summary>
        /// returns true of node is equal to parent, or any ancestor thereof.
        /// </summary>
        /// <param name="parent">The parent node</param>
        /// <param name="node">The current node</param>
        /// <returns></returns>
        public bool IsAncestorOf(TreeNode<T> parent, T node)
        {
            while (parent != null)
            {
                if (this.EqualityComparer != null)
                {
                    if (this.EqualityComparer(parent.Current, node))
                    {
                        return true;
                    }
                }
                else
                {
                    if (parent.Current.Equals(node))
                    {
                        return true;
                    }
                }
                parent = parent.Parent;
            }
            return false;
        }
    }
}