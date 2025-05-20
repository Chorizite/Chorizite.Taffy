using System.Runtime.InteropServices;
using TaffySharp.Lib;

namespace TaffySharp;

/// <summary>
/// A TaffyTree
/// </summary>
public unsafe class TaffyTree : IDisposable
{
    private UIntPtr _tree;
    private bool _disposed;

    /// <summary>
    /// Creates a new <see cref="TaffyTree"/> that can store <paramref name="initializeCapacity"/> nodes before reallocation
    /// </summary>
    /// <param name="initializeCapacity"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public TaffyTree(uint initializeCapacity = 16)
    {
        _tree = NativeMethods.taffytree_with_capacity((nuint)initializeCapacity);

        if (_tree == UIntPtr.Zero)
            throw new InvalidOperationException("Failed to create TaffyTree");
    }

    /// <summary>
    /// Enable rounding of layout values. Rounding is enabled by default.
    /// </summary>
    public void EnableRounding()
    {
        NativeMethods.taffytree_enable_rounding(_tree);
    }

    /// <summary>
    /// Disable rounding of layout values. Rounding is enabled by default.
    /// </summary>
    public void DisableRounding()
    {
        NativeMethods.taffytree_disable_rounding(_tree);
    }


    /// <summary>
    /// Creates and adds a new unattached leaf node to the tree, and returns the node of the new node
    /// </summary>
    /// <param name="style"></param>
    /// <returns></returns>
    public Node NewLeaf(Style style)
    {
        var c_Style = style.ToCStruct();
        var nodeId = NativeMethods.taffytree_new_leaf(_tree, &c_Style);

        return new Node(this, nodeId);
    }

    /// <summary>
    /// Creates and adds a new node, which may have any number of `children`
    /// </summary>
    /// <param name="style"></param>
    /// <param name="children"></param>
    /// <returns></returns>
    public unsafe Node NewWithChildren(Style style, params Node[] children)
    {
        var childrenIds = children.Select(x => x.Id).ToArray();
        fixed (ulong* childrenPtr = childrenIds)
        {
            var c_Style = style.ToCStruct();
            var nodeId = NativeMethods.taffytree_new_with_children(_tree, &c_Style, childrenPtr, (nuint)children.Length);
            return new Node(this, nodeId);
        }
    }

    /// <summary>
    /// Adds a `child` node under the supplied `parent`
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <returns></returns>
    public bool AddChild(Node parent, Node child)
    {
        return NativeMethods.taffytree_add_child(_tree, parent.Id, child.Id) == 0;
    }

    /// <summary>
    /// Remove a specific node from the tree and drop it
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public bool Remove(Node node)
    {
        return NativeMethods.taffytree_remove(_tree, node.Id) == 0;
    }

    /// <summary>
    /// Drops all nodes in the tree
    /// </summary>
    public void Clear()
    {
        NativeMethods.taffytree_clear(_tree);
    }

    public bool ReplaceChildAtIndex(Node parent, uint index, Node child)
    {
        return NativeMethods.taffytree_replace_child_at_index(_tree, parent.Id, index, child.Id) == 0;
    }

    public bool RemoveChild(Node parent, Node child)
    {
        return NativeMethods.taffytree_remove_child(_tree, parent.Id, child.Id) == 0;
    }

    public bool RemoveChildAtIndex(Node parent, uint index)
    {
        return NativeMethods.taffytree_remove_child_at_index(_tree, parent.Id, index) == 0;
    }

    /// <summary>
    /// Indicates whether the layout of this node needs to be recomputed
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public bool Dirty(Node node)
    {
        return NativeMethods.taffytree_dirty(_tree, node.Id) == 0 ? false : true;
    }

    /// <summary>
    /// Marks the layout of this node and its ancestors as outdated
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public bool MarkDirty(Node node)
    {
        return NativeMethods.taffytree_mark_dirty(_tree, node.Id) == 0;
    }

    public bool SetStyle(Node node, Style style)
    {
        var c_Style = style.ToCStruct();
        return NativeMethods.taffytree_set_style(_tree, node.Id, &c_Style) == 0;
    }

    public bool ComputeLayout(Node node, AvailableSpace availableSpace)
    {
        return NativeMethods.taffytree_compute_layout(_tree, node.Id, availableSpace.ToCStruct()) == 0;
    }

    public bool GetLayout(Node node, out Layout layout)
    {
        IntPtr layoutPtr = Marshal.AllocHGlobal(Marshal.SizeOf<c_Layout>());
        try
        {
            var ret = NativeMethods.taffytree_layout(_tree, node.Id, (c_Layout*)layoutPtr);
            if (ret == 0)
            {
                c_Layout c_layout = Marshal.PtrToStructure<c_Layout>(layoutPtr);
                layout = new Layout(c_layout);
                return true;
            }
            layout = new Layout();
            return false;
        }
        finally
        {
            Marshal.FreeHGlobal(layoutPtr);
        }
    }

    /// <summary>
    /// Disposes the <see cref="TaffyTree"/>, freeing its memory
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            if (_tree != UIntPtr.Zero)
            {
                NativeMethods.taffytree_free(_tree);
                _tree = UIntPtr.Zero;
            }
            _disposed = true;
        }
    }

    ~TaffyTree()
    {
        Dispose();
    }
}
