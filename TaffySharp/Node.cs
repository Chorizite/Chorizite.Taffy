using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaffySharp.Lib;

namespace TaffySharp
{
    /// <summary>
    /// A node
    /// </summary>
    public struct Node
    {
        /// <summary>
        /// The tree this node belongs to
        /// </summary>
        public TaffyTree Tree { get; }


        /// <summary>
        /// The id of this node
        /// </summary>
        public readonly ulong Id;

        /// <summary>
        /// Whether the layout of this node is dirty (outdated)
        /// </summary>
        public bool IsDirty {
            get => Tree.Dirty(this);
            set => Tree.MarkDirty(this);
        }

        internal Node(TaffyTree tree, ulong id)
        {
            Tree = tree;
            Id = id;
        }

        /// <summary>
        /// Computes the layout for this node, given the available space
        /// </summary>
        /// <param name="availableSpace"></param>
        public bool ComputeLayout(AvailableSpace availableSpace) {
            return Tree.ComputeLayout(this, availableSpace);
        }

        /// <summary>
        /// Gets the layout for this node, make sure to call <see cref="ComputeLayout"/> on either this node or one of its parents first
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        public bool GetLayout(out Layout layout) => Tree.GetLayout(this, out layout);

        /// <summary>
        /// Sets the style for this node
        /// </summary>
        /// <param name="style"></param>
        public void SetStyle(Style style) => Tree.SetStyle(this, style);

        /// <summary>
        /// Adds a child to this node
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(Node child) => Tree.AddChild(this, child);

        /// <summary>
        /// Removes a child from this node
        /// </summary>
        /// <param name="child"></param>
        public void RemoveChild(Node child) => Tree.RemoveChild(this, child);
    }
}
