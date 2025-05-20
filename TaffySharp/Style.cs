using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using global::TaffySharp.Lib;
using System.Drawing;

namespace TaffySharp
{
    /// <summary>
    /// A typed representation of the CSS style information for a single node.
    ///
    /// <para>
    /// The most important idea in flexbox is the notion of a "main" and "cross" axis, which are always perpendicular to each other.
    /// The orientation of these axes are controlled via the [`FlexDirection`] field of this struct.
    /// </para>
    /// <para>
    /// This struct follows the [CSS equivalent](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_Flexible_Box_Layout/Basic_Concepts_of_Flexbox) directly;
    /// information about the behavior on the web should transfer directly.
    /// </para>
    /// <para>
    /// Detailed information about the exact behavior of each of these fields
    /// can be found on [MDN](https://developer.mozilla.org/en-US/docs/Web/CSS) by searching for the field name.
    /// The distinction between margin, padding and border is explained well in
    /// this [introduction to the box model](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_Box_Model/Introduction_to_the_CSS_box_model).
    /// </para>
    /// <para>
    /// If the behavior does not match the flexbox layout algorithm on the web, please file a bug!
    /// </para>
    /// </summary>
    public class Style : IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// What layout strategy should be used
        /// </summary>
        // Managed state
        public Display Display { get; set; } = Display.Default;
        /// <summary>
        /// Whether a child is display:table or not. This affects children of block layouts.
        /// This should really be part of `Display`, but it is currently seperate because table layout isn't implemented
        /// </summary>
        public bool ItemIsTable { get; set; } = false;

        /// <summary>
        /// Is it a replaced element like an image or form field?
        /// <https://drafts.csswg.org/css-sizing-3/#min-content-zero>
        /// </summary>
        public bool ItemIsReplaced { get; set; } = false;

        /// <summary>
        /// Should size styles apply to the content box or the border box of the node
        /// </summary>
        public BoxSizing BoxSizing { get; set; } = BoxSizing.BorderBox;

        /// <summary>
        /// How children overflowing their container should affect layout
        /// </summary>
        public Point<Overflow> Overflow { get; set; } = new(TaffySharp.Overflow.Visible, TaffySharp.Overflow.Visible);

        /// <summary>
        /// How much space (in points) should be reserved for the scrollbars of `Overflow::Scroll` and `Overflow::Auto` nodes.
        /// </summary>
        public float ScrollbarWidth { get; set; } = 0;

        /// <summary>
        /// What should the `position` value of this struct use as a base offset?
        /// </summary>
        public Position Position { get; set; } = Position.Relative;

        /// <summary>
        /// How should the position of this element be tweaked relative to the layout defined?
        /// </summary>
        public Rect<LengthPercentageAuto> Inset { get; set; } = Rect<LengthPercentageAuto>.LengthPercentageAuto_Auto;

        /// <summary>
        /// Sets the initial size of the item
        /// </summary>
        public Size<Dimension> Size { get; set; } = Size<Dimension>.Dimension_Auto;

        /// <summary>
        /// Controls the minimum size of the item
        /// </summary>
        public Size<Dimension> MinSize { get; set; } = Size<Dimension>.Dimension_Auto;

        /// <summary>
        /// Controls the maximum size of the item
        /// </summary>
        public Size<Dimension> MaxSize { get; set; } = Size<Dimension>.Dimension_Auto;

        /// <summary>
        /// Sets the preferred aspect ratio for the item
        ///
        /// <para>The ratio is calculated as width divided by height.</para>
        /// </summary>
        public float? AspectRatio { get; set; }

        /// <summary>
        /// How large should the margin be on each side?
        /// </summary>
        public Rect<LengthPercentageAuto> Margin { get; set; } = Rect<LengthPercentageAuto>.LengthPercentageAuto_Zero;

        /// <summary>
        /// How large should the padding be on each side?
        /// </summary>
        public Rect<LengthPercentageAuto> Padding { get; set; } = Rect<LengthPercentageAuto>.LengthPercentageAuto_Zero;

        /// <summary>
        /// How large should the border be on each side?
        /// </summary>
        public Rect<LengthPercentageAuto> Border { get; set; } = Rect<LengthPercentageAuto>.LengthPercentageAuto_Zero;

        /// <summary>
        /// How this node's children aligned in the cross/block axis?
        /// </summary>
        public AlignItems? AlignItems { get; set; } = null;

        /// <summary>
        /// How this node should be aligned in the cross/block axis
        /// <para>Falls back to the parents [`AlignItems`] if not set explicitly</para>
        /// </summary>
        public AlignItems? AlignSelf { get; set; } = null;

        /// <summary>
        /// How this node's children should be aligned in the inline axis
        /// </summary>
        public AlignItems? JustifyItems { get; set; } = null;

        /// <summary>
        /// How this node should be aligned in the inline axis
        /// <para>Falls back to the parents [`JustifyItems`] if not set</para>
        /// </summary>
        public AlignItems? JustifySelf { get; set; } = null;

        /// <summary>
        /// How should content contained within this item be aligned in the cross/block axis
        /// </summary>
        public AlignContent? AlignContent { get; set; } = null;

        /// <summary>
        /// How should content contained within this item be aligned in the main/inline axis
        /// </summary>
        public AlignContent? JustifyContent { get; set; } = null;

        /// <summary>
        /// How large should the gaps between items in a grid or flex container be?
        /// </summary>
        public Size<LengthPercentage> Gap { get; set; } = Size<LengthPercentage>.LengthPercentage_Zero;

        /// <summary>
        /// How items elements should aligned in the inline axis
        /// </summary>
        public TextAlign TextAlign { get; set; } = TextAlign.Auto;

        /// <summary>
        /// Which direction does the main axis flow in?
        /// </summary>
        public FlexDirection FlexDirection { get; set; } = FlexDirection.Row;

        /// <summary>
        /// Should elements wrap, or stay in a single line?
        /// </summary>
        public FlexWrap FlexWrap { get; set; } = FlexWrap.NoWrap;

        /// <summary>
        /// Sets the initial main axis size of the item
        /// </summary>
        public Dimension FlexBasis { get; set; } = Dimension.Auto;

        /// <summary>
        /// The relative rate at which this item grows when it is expanding to fill space
        ///
        /// <para>0.0 is the default value, and this value must be positive.</para>
        /// </summary>
        public float FlexGrow { get; set; } = 0.0f;

        /// <summary>
        /// The relative rate at which this item shrinks when it is contracting to fit into space
        ///
        /// <para>1.0 is the default value, and this value must be positive.</para>
        /// </summary>
        public float FlexShrink { get; set; } = 1.0f;

        /// <summary>
        /// Creates a new style
        /// </summary>
        public Style()
        {
            
        }

        /// <summary>
        /// Converts this style to a C struct
        /// </summary>
        /// <returns></returns>
        internal c_Style ToCStruct()
        {
            return new c_Style
            {
                display = (int)Display,
                item_is_table = ItemIsTable ? 1 : 0,
                item_is_replaced = ItemIsReplaced ? 1 : 0,
                box_sizing = (int)BoxSizing,
                overflow_x = (int)Overflow.X,
                overflow_y = (int)Overflow.Y,
                scrollbar_width = ScrollbarWidth,
                position = (int)Position,
                inset = Inset.ToCStruct(),
                size = Size.ToCStruct(),
                min_size = MinSize.ToCStruct(),
                max_size = MaxSize.ToCStruct(),
                aspect_ratio = AspectRatio.HasValue ? AspectRatio.Value : 0,
                has_aspect_ratio = AspectRatio.HasValue ? 1 : 0,
                margin = Margin.ToCStruct(),
                padding = Padding.ToCStruct(),
                border = Border.ToCStruct(),
                align_items = AlignItems.HasValue ? (int)AlignItems.Value : 0,
                has_align_items = AlignItems.HasValue ? 1 : 0,
                align_self = AlignSelf.HasValue ? (int)AlignSelf.Value : 0,
                has_align_self = AlignSelf.HasValue ? 1 : 0,
                justify_items = JustifyItems.HasValue ? (int)JustifyItems.Value : 0,
                has_justify_items = JustifyItems.HasValue ? 1 : 0,
                justify_self = JustifySelf.HasValue ? (int)JustifySelf.Value : 0,
                has_justify_self = JustifySelf.HasValue ? 1 : 0,
                align_content = AlignContent.HasValue ? (int)AlignContent.Value : 0,
                has_align_content = AlignContent.HasValue ? 1 : 0,
                justify_content = JustifyContent.HasValue ? (int)JustifyContent.Value : 0,
                has_justify_content = JustifyContent.HasValue ? 1 : 0,
                gap = Gap.ToCStruct(),
                text_align = (int)TextAlign,
                flex_direction = (int)FlexDirection,
                flex_wrap = (int)FlexWrap,
                flex_basis = FlexBasis.ToCStruct(),
                flex_grow = FlexGrow,
                flex_shrink = FlexShrink
            };
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        ~Style()
        {
            Dispose();
        }
    }
}
