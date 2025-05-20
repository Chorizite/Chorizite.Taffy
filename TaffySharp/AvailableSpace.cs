using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffySharp.Lib;

namespace TaffySharp
{
    /// <summary>
    /// The available space
    /// </summary>
    public class AvailableSpace
    {
        /// <summary>
        /// The available width
        /// </summary>
        public AvailableSpaceLength Width { get; set; }

        /// <summary>
        /// The available height
        /// </summary>
        public AvailableSpaceLength Height { get; set; }

        /// <summary>
        /// Create a new available space from a unit length width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static AvailableSpace Definite(AvailableSpaceLength width, AvailableSpaceLength height) => new(width, height);

        /// <summary>
        /// Create a new available space from a unit length width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static AvailableSpace Definite(float width, float height) => new(AvailableSpaceLength.Definitive(width), AvailableSpaceLength.Definitive(height));

        /// <summary>
        /// Create a new zero-sized available space
        /// </summary>
        public static AvailableSpace Zero => new(AvailableSpaceLength.Zero, AvailableSpaceLength.Zero);

        /// <summary>
        /// Create a new min-content available space
        /// </summary>
        public static AvailableSpace MinContent => new(AvailableSpaceLength.MinContent, AvailableSpaceLength.MinContent);

        /// <summary>
        /// Create a new max-content available space
        /// </summary>
        public static AvailableSpace MaxContent => new(AvailableSpaceLength.MaxContent, AvailableSpaceLength.MaxContent);

        /// <summary>
        /// Create a new available space
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public AvailableSpace(AvailableSpaceLength width, AvailableSpaceLength height) => (Width, Height) = (width, height);

        /// <summary>
        /// Create a new definite available space from a unit length width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public AvailableSpace(float width, float height) => (Width, Height) = (AvailableSpaceLength.Definitive(width), AvailableSpaceLength.Definitive(height));

        /// <summary>
        /// Convert to a C struct
        /// </summary>
        /// <returns></returns>
        internal c_AvailableSpace ToCStruct() => new() { width = Width.ToCStruct(), height = Height.ToCStruct() };
    }
}
