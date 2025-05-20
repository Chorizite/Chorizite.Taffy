using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffySharp
{

    /// <summary>
    /// Represents a dimension type
    /// </summary>
    public enum DimensionType
    {
        /// <summary>
        /// Auto, will get its sizing from the parent / content.
        /// </summary>
        Auto = 0,

        /// <summary>
        /// A specific unit length.
        /// </summary>
        Length = 1,

        /// <summary>
        /// A percentage of the parent element. Should be specified as 0.0 - 1.0.
        /// </summary>
        Percent = 2
    }

    /// <summary>
    /// Sets the layout used for the children of this node
    ///
    /// <para>The default values depends on on which feature flags are enabled. The order of precedence is: Flex, Grid, Block, None.</para>
    /// </summary>
    public enum Display
    {
        /// <summary>
        /// The children will follow the block layout algorithm
        /// </summary>
        Block = 0,

        /// <summary>
        /// The children will follow the flexbox layout algorithm
        /// </summary>
        Flex = 1,

        /// <summary>
        /// The children will follow the CSS Grid layout algorithm
        /// </summary>
        Grid = 2,

        /// <summary>
        /// The node is hidden, and it's children will also be hidden
        /// </summary>
        None = 3,

        /// <summary>
        /// Default, flex
        /// </summary>
        Default = Flex
    }

    /// <summary>
    /// Specifies whether size styles for this node are assigned to the node's "content box" or "border box"
    ///
    /// <list type="bullet">
    ///   <item>The "content box" is the node's inner size excluding padding, border and margin</item>
    ///   <item>The "border box" is the node's outer size including padding and border (but still excluding margin)</item>
    /// </list>
    ///
    /// <para>This property modifies the application of the following styles:</para>
    ///
    /// <list type="bullet">
    ///   <item>`size`</item>
    ///   <item>`min_size`</item>
    ///   <item>`max_size`</item>
    ///   <item>`flex_basis`</item>
    /// </list>
    ///
    /// <seealso>See https://developer.mozilla.org/en-US/docs/Web/CSS/box-sizing </seealso>
    /// </summary>
    public enum BoxSizing
    {
        /// <summary>
        /// Size styles such <see cref="Style.Size"/>, <see cref="Style.MinSize"/>, <see cref="Style.MaxSize"/>, specify the box's "border box" (the size excluding margin but including padding/border)
        /// </summary>
        BorderBox = 0,

        /// <summary>
        /// Size styles such <see cref="Style.Size"/>, <see cref="Style.MinSize"/>, <see cref="Style.MaxSize"/> specify the box's "content box" (the size excluding padding/border/margin)
        /// </summary>
        ContentBox = 1
    }

    /// <summary>
    /// How children overflowing their container should affect layout
    /// <para>
    /// In CSS the primary effect of this property is to control whether contents of a parent container that overflow that container should
    /// be displayed anyway, be clipped, or trigger the container to become a scroll container. However it also has secondary effects on layout,
    /// the main ones being:
    ///
    ///    <para>- The automatic minimum size Flexbox/CSS Grid items with non-`Visible` overflow is `0` rather than being content based </para>
    ///    <para>- <see cref="Overflow.Scroll"/> nodes have space in the layout reserved for a scrollbar (width controlled by the <see cref="Style.ScrollbarWidth"/> property) </para>
    /// </para>
    /// <para>
    /// In Taffy, we only implement the layout related secondary effects as we are not concerned with drawing/painting. The amount of space reserved for
    /// a scrollbar is controlled by the <see cref="Style.ScrollbarWidth"/> property. If this is `0` then `Scroll` behaves identically to `Hidden`.
    /// </para>
    /// <para>https://developer.mozilla.org/en-US/docs/Web/CSS/overflow</para>
    /// </summary>
    public enum Overflow
    {
        /// <summary>
        /// The automatic minimum size of this node as a flexbox/grid item should be based on the size of its content.
        /// Content that overflows this node *should* contribute to the scroll region of its parent.
        /// </summary>
        Visible = 0,

        /// <summary>
        /// The automatic minimum size of this node as a flexbox/grid item should be based on the size of its content.
        /// Content that overflows this node should *not* contribute to the scroll region of its parent.
        /// </summary>
        Clip = 1,

        /// <summary>
        /// The automatic minimum size of this node as a flexbox/grid item should be `0`.
        /// Content that overflows this node should *not* contribute to the scroll region of its parent.
        /// </summary>
        Hidden = 2,

        /// <summary>
        /// The automatic minimum size of this node as a flexbox/grid item should be `0`. Additionally, space should be reserved
        /// for a scrollbar. The amount of space reserved is controlled by the <see cref="Style.ScrollbarWidth"/> property.
        /// Content that overflows this node should *not* contribute to the scroll region of its parent.
        /// </summary>
        Scroll = 3
    }

    /// <summary>
    /// The positioning strategy for this item.
    /// <para>
    /// This controls both how the origin is determined for the [`Style::position`] field,
    /// and whether or not the item will be controlled by flexbox's layout algorithm.
    /// </para>
    /// <para>WARNING: this enum follows the behavior of [CSS's `position` property](https://developer.mozilla.org/en-US/docs/Web/CSS/position),
    /// which can be unintuitive.</para>
    ///
    /// <para><see cref="Position.Relative"/> is the default value, in contrast to the default behavior in CSS.</para>
    /// </summary>
    public enum Position
    {
        /// <summary>
        /// The offset is computed relative to the final position given by the layout algorithm.
        /// Offsets do not affect the position of any other items; they are effectively a correction factor applied at the end.
        /// </summary>
        Relative = 0,

        /// <summary>
        /// The offset is computed relative to this item's closest positioned ancestor, if any.
        /// Otherwise, it is placed relative to the origin.
        /// No space is created for the item in the page layout, and its size will not be altered.
        /// 
        ///
        /// <para>WARNING: to opt-out of layouting entirely, you must use <see cref="Display.None"/> instead on your <see cref="Style" /> object.</para>
        /// </summary>
        Absolute = 1
    }

    /// <summary>
    /// Used to control how child nodes are aligned.
    /// <para>For Flexbox it controls alignment in the cross axis</para>
    /// <para>For Grid it controls alignment in the block axis</para>
    ///
    /// <para>[MDN](https://developer.mozilla.org/en-US/docs/Web/CSS/align-items)</para>
    /// </summary>
    public enum AlignItems
    {
        /// <summary>
        /// Items are packed toward the start of the axis
        /// </summary>
        Start,

        /// <summary>
        /// Items are packed toward the end of the axis
        /// </summary>
        End,

        /// <summary>
        /// Items are packed towards the flex-relative start of the axis.
        ///
        /// <para>For flex containers with flex_direction RowReverse or ColumnReverse this is equivalent
        /// to End. In all other cases it is equivalent to Start.</para>
        /// </summary>
        FlexStart,

        /// <summary>
        /// Items are packed towards the flex-relative end of the axis.
        ///
        /// <para>For flex containers with flex_direction RowReverse or ColumnReverse this is equivalent
        /// to Start. In all other cases it is equivalent to End.</para>
        /// </summary>
        FlexEnd,

        /// <summary>
        /// Items are packed along the center of the cross axis
        /// </summary>
        Center,

        /// <summary>
        /// Items are aligned such as their baselines align
        /// </summary>
        Baseline,

        /// <summary>
        /// Stretch to fill the container
        /// </summary>
        Stretch
    }

    /// <summary>
    /// Sets the distribution of space between and around content items
    /// <para>For Flexbox it controls alignment in the cross axis</para>
    /// <para>For Grid it controls alignment in the block axis</para>
    ///
    /// <para>[MDN](https://developer.mozilla.org/en-US/docs/Web/CSS/align-content)</para>
    /// </summary>
    public enum AlignContent
    {
        /// <summary>
        /// Items are packed toward the start of the axis
        /// </summary>
        Start,

        /// <summary>
        /// Items are packed toward the end of the axis
        /// </summary>
        End,

        /// <summary>
        /// Items are packed towards the flex-relative start of the axis.
        ///
        /// <para>For flex containers with flex_direction RowReverse or ColumnReverse this is equivalent
        /// to End. In all other cases it is equivalent to Start.</para>
        /// </summary>
        FlexStart,

        /// <summary>
        /// Items are packed towards the flex-relative end of the axis.
        ///
        /// <para>For flex containers with flex_direction RowReverse or ColumnReverse this is equivalent
        /// to Start. In all other cases it is equivalent to End.</para>
        /// </summary>
        FlexEnd,

        /// <summary>
        /// Items are centered around the middle of the axis
        /// </summary>
        Center,

        /// <summary>
        /// Items are stretched to fill the container
        /// </summary>
        Stretch,

        /// <summary>
        /// The first and last items are aligned flush with the edges of the container (no gap).
        /// The gap between items is distributed evenly.
        /// </summary>
        SpaceBetween,

        /// <summary>
        /// The gap between the first and last items is exactly THE SAME as the gap between items.
        /// The gaps are distributed evenly
        /// </summary>
        SpaceEvenly,

        /// <summary>
        /// The gap between the first and last items is exactly HALF the gap between items.
        /// The gaps are distributed evenly in proportion to these ratios.
        /// </summary>
        SpaceAround,
    }

    /// <summary>
    /// Used by block layout to implement the legacy behaviour of <code>&lt;center&gt;</code> and <code>&lt;div align="left | right | center"&gt;</code>
    /// </summary>
    public enum TextAlign
    {
        /// <summary>
        /// No special legacy text align behaviour.
        /// </summary>
        Auto,

        /// <summary>
        /// Corresponds to `-webkit-left` or `-moz-left` in browsers
        /// </summary>
        LegacyLeft,

        /// <summary>
        /// Corresponds to `-webkit-right` or `-moz-right` in browsers
        /// </summary>
        LegacyRight,

        /// <summary>
        /// Corresponds to `-webkit-center` or `-moz-center` in browsers
        /// </summary>
        LegacyCenter,
    }

    /// <summary>
    /// The direction of the flexbox layout main axis.
    ///
    /// <para>
    /// There are always two perpendicular layout axes: main (or primary) and cross (or secondary).
    /// Adding items will cause them to be positioned adjacent to each other along the main axis.
    /// By varying this value throughout your tree, you can create complex axis-aligned layouts.
    /// </para>
    /// 
    /// <para>Items are always aligned relative to the cross axis, and justified relative to the main axis.</para>
    ///
    /// <para>The default behavior is <see cref="FlexDirection.Row"/> </para>
    ///
    /// <para>[Specification](https://www.w3.org/TR/css-flexbox-1/#flex-direction-property)</para>
    /// </summary>
    public enum FlexDirection
    {
        /// <summary>
        /// Defines +x as the main axis
        ///
        /// <para>Items will be added from left to right in a row.</para>
        /// </summary>
        Row,

        /// <summary>
        /// Defines +y as the main axis
        ///
        /// <para>Items will be added from top to bottom in a column.</para>
        /// </summary>
        Column,

        /// <summary>
        /// Defines -x as the main axis
        ///
        /// <para>Items will be added from right to left in a row.</para>
        /// </summary>
        RowReverse,

        /// <summary>
        /// Defines -y as the main axis
        ///
        /// <para>Items will be added from bottom to top in a column.</para>
        /// </summary>
        ColumnReverse,
    }

    /// <summary>
    /// Controls whether flex items are forced onto one line or can wrap onto multiple lines.
    ///
    /// <para>Defaults to <see cref="FlexWrap.NoWrap"/></para>
    ///
    /// <para>[Specification](https://www.w3.org/TR/css-flexbox-1/#flex-wrap-property)</para>
    /// </summary>
    public enum FlexWrap
    {
        /// <summary>
        /// Items will not wrap and stay on a single line
        /// </summary>
        NoWrap,

        /// <summary>
        /// Items will wrap according to this item's <see cref="Style.FlexDirection"/>
        /// </summary>
        Wrap,

        /// <summary>
        /// Items will wrap in the opposite direction to this item's <see cref="Style.FlexDirection"/>
        /// </summary>
        WrapReverse,
    }
}
