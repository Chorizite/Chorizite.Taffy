using System.Runtime.InteropServices;
using TaffySharp.Lib;
using AutoMapper;

namespace TaffySharp;

public enum BoxSizing
{
    BorderBox = 0,
    ContentBox = 1
}

public enum Overflow
{
    Visible = 0,
    Hidden = 1,
    Scroll = 2,
    Clip = 3
}

public enum Position
{
    Relative = 0,
    Absolute = 1
}

public enum FlexWrap
{
    NoWrap = 0,
    Wrap = 1,
    WrapReverse = 2
}

public enum FlexDirection
{
    Row = 0,
    Column = 1,
    RowReverse = 2,
    ColumnReverse = 3
}

public enum AlignItems
{
    Start = 0,
    End = 1,
    FlexStart = 2,
    FlexEnd = 3,
    Center = 4,
    Baseline = 5,
    Stretch = 6
}

public enum AlignContent
{
    Start = 0,
    End = 1,
    FlexStart = 2,
    FlexEnd = 3,
    Center = 4,
    Stretch = 5,
    SpaceBetween = 6,
    SpaceEvenly = 7,
    SpaceAround = 8
}

public enum GridAutoFlow
{
    Row = 0,
    Column = 1,
    RowDense = 2,
    ColumnDense = 3
}

public enum Dimension {
    Auto = 0,
    Length = 1,
    Percent = 2
}

// Structs matching Rust FFI structs
[StructLayout(LayoutKind.Sequential)]
public struct Length
{
    public int Dim; // 0: Auto, 1: Length, 2: Percent
    public float Value;
}

[StructLayout(LayoutKind.Sequential)]
public struct Size
{
    public Length Width;
    public Length Height;
}

[StructLayout(LayoutKind.Sequential)]
public struct Rect
{
    public Length Left;
    public Length Right;
    public Length Top;
    public Length Bottom;
}

[StructLayout(LayoutKind.Sequential)]
public struct GridIndex
{
    public sbyte Kind; // 0: Auto, 1: Line, 2: Span
    public short Value;
}

[StructLayout(LayoutKind.Sequential)]
public struct GridPlacement
{
    public GridIndex Start;
    public GridIndex End;
}

[StructLayout(LayoutKind.Sequential)]
public struct GridTrackSize
{
    public Length MinSize;
    public Length MaxSize;
}

[StructLayout(LayoutKind.Sequential)]
public struct GridTrackSizing
{
    public int Repetition; // -2: Single, -1: AutoFit, 0: AutoFill, >0: Count
    public IntPtr Single; // Nullable pointer for single track
    public IntPtr Repeat; // Pointer to array
    public UIntPtr RepeatCount; // Length of repeat array
}

[StructLayout(LayoutKind.Sequential)]
public struct Layout
{
    public long Order;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public float[] Location; // x, y
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public float[] Size; // width, height
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public float[] ContentSize; // width, height
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public float[] ScrollbarSize; // width, height
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] Border; // top, right, bottom, left
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] Padding; // top, right, bottom, left
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] Margin; // top, right, bottom, left
}

public unsafe class TaffyTree : IDisposable
{
    private TaffyTree* _tree;
    private bool _disposed;

    static MapperConfiguration config = new MapperConfiguration(cfg =>
    {
    });
    static IMapper mapper = config.CreateMapper();

    public TaffyTree()
    {
        _tree = NativeMethods.taffy_new();
        Console.WriteLine($"_tree: {(ulong)_tree:X}");

        if ((IntPtr)_tree == IntPtr.Zero)
            throw new InvalidOperationException("Failed to create TaffyTree");
    }

    public void EnableRounding()
    {
        NativeMethods.taffy_enable_rounding(_tree);
    }

    public void DisableRounding()
    {
        NativeMethods.taffy_disable_rounding(_tree);
    }

    public unsafe ulong CreateNode(Style style)
    {
        return NativeMethods.taffy_node_create(_tree, style._nativeStyle);
    }

    public unsafe ulong NewWithChildren(Style style, params ulong[] children)
    {
        fixed (ulong* childrenPtr = children)
        {
            return NativeMethods.taffy_new_with_children(_tree, style._nativeStyle, childrenPtr, (nuint)children.Length);
        }
    }

    public bool AddChild(ulong parent, ulong child)
    {
        return NativeMethods.taffy_node_add_child(_tree, parent, child) == 0;
    }

    public bool DropNode(ulong node)
    {
        return NativeMethods.taffy_node_drop(_tree, node) == 0;
    }

    public void DropAllNodes()
    {
        NativeMethods.taffy_node_drop_all(_tree);
    }

    public bool ReplaceChildAtIndex(ulong parent, uint index, ulong child)
    {
        return NativeMethods.taffy_node_replace_child_at_index(_tree, parent, index, child) == 0;
    }

    public bool RemoveChild(ulong parent, ulong child)
    {
        return NativeMethods.taffy_node_remove_child(_tree, parent, child) == 0;
    }

    public bool RemoveChildAtIndex(ulong parent, uint index)
    {
        return NativeMethods.taffy_node_remove_child_at_index(_tree, parent, index) == 0;
    }

    public int Dirty(ulong node)
    {
        return NativeMethods.taffy_node_dirty(_tree, node);
    }

    public bool MarkDirty(ulong node)
    {
        return NativeMethods.taffy_node_mark_dirty(_tree, node) == 0;
    }

    public bool SetStyle(ulong node, Style style)
    {
        var mapped = mapper.Map<Style, c_Style>(style);
        return NativeMethods.taffy_node_set_style(_tree, node, &mapped) == 0;
    }

    public bool SetMeasure(ulong node, bool measure)
    {
        return NativeMethods.taffy_node_set_measure(_tree, node, measure ? 1 : 0) == 0;
    }

    public bool ComputeLayout(ulong node, c_Size availableSpace)
    {
        return NativeMethods.taffy_compute_layout(_tree, node, availableSpace) == 0;
    }

    public bool GetLayout(ulong node, out c_Layout layout)
    {
        layout = new c_Layout();
        fixed (c_Layout* layoutPtr = &layout)
        {
            var ret = NativeMethods.taffy_get_layout(_tree, node, layoutPtr);

            return ret == 0;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            if (_tree != null)
            {
                NativeMethods.taffy_free(_tree);
                _tree = null;
            }
            _disposed = true;
        }
    }

    ~TaffyTree()
    {
        Dispose();
    }
}
