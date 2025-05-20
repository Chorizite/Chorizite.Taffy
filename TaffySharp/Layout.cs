using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffySharp.Lib;

namespace TaffySharp
{
    /// <summary>
    /// The final result of a layout algorithm for a single node.
    /// </summary>
    public class Layout
    {
        /// <summary>
        /// The relative ordering of the node
        ///
        /// <para>Nodes with a higher order should be rendered on top of those with a lower order.
        /// This is effectively a topological sort of each tree.</para>
        /// </summary>
        public long Order { get; set; }

        /// <summary>
        /// The top-left corner of the node
        /// </summary>
        public Point<float> Location { get; set; } = new Point<float>(0, 0);

        /// <summary>
        /// The width and height of the node
        /// </summary>
        public Size<float> Size { get; set; } = new Size<float>(0, 0);

        /// <summary>
        /// The width and height of the content inside the node. This may be larger than the size of the node in the case of
        /// overflowing content and is useful for computing a "scroll width/height" for scrollable nodes
        /// </summary>
        public Size<float> ContentSize { get; set; } = new Size<float>(0, 0);

        /// <summary>
        /// The size of the scrollbars in each dimension. If there is no scrollbar then the size will be zero.
        /// </summary>
        public Size<float> ScrollbarSize { get; set; } = new Size<float>(0, 0);

        /// <summary>
        /// The size of the borders of the node
        /// </summary>
        public Rect<float> Border { get; set; } = new Rect<float>(0);

        /// <summary>
        /// The size of the padding of the node
        /// </summary>
        public Rect<float> Padding { get; set; } = new Rect<float>(0);

        /// <summary>
        /// The size of the margin of the node
        /// </summary>
        public Rect<float> Margin { get; set; } = new Rect<float>(0);

        /// <summary>
        /// Create a new layout
        /// </summary>
        internal Layout() {
        
        }

        /// <summary>
        /// Create a new layout from a C struct
        /// </summary>
        /// <param name="layout"></param>
        internal unsafe Layout(c_Layout layout)
        {
            Order = layout.order;
            Location = new Point<float>(layout.location[0], layout.location[1]);
            Size = new Size<float>(layout.size[0], layout.size[1]);
            ContentSize = new Size<float>(layout.content_size[0], layout.content_size[1]);
            ScrollbarSize = new Size<float>(layout.scrollbar_size[0], layout.scrollbar_size[1]);
            Border = new Rect<float>(layout.border[0], layout.border[1], layout.border[2], layout.border[3]);
            Padding = new Rect<float>(layout.padding[0], layout.padding[1], layout.padding[2], layout.padding[3]);
            Margin = new Rect<float>(layout.margin[0], layout.margin[1], layout.margin[2], layout.margin[3]);
        }
    }
}
