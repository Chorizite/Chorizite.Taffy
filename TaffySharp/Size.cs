using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffySharp.Lib;

namespace TaffySharp
{
    /// <summary>
    /// A width and height
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Size<T>
    {
        /// <summary>
        /// True if either width or height is a <see cref="Length"/>
        /// </summary>
#if NETFRAMEWORK || NETSTANDARD
        internal bool IsLengthType => Width is Length;
#else
        internal bool IsLengthType => typeof(T).IsAssignableTo(typeof(Length));
#endif

        /// <summary>
        /// The width
        /// </summary>
        public T Width { get; set; }

        /// <summary>
        /// The height
        /// </summary>
        public T Height { get; set; }

        /// <summary>
        /// Create a new size
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Size(T width, T height) => (Width, Height) = (width, height);

        /// <summary>
        /// Create a new size <see cref="Dimension"/> of Zero
        /// </summary>
        public static Size<Dimension> Dimension_Zero => new(Dimension.Zero, Dimension.Zero);

        /// <summary>
        /// Create a new size <see cref="Dimension"/> of Auto
        /// </summary>
        public static Size<Dimension> Dimension_Auto => new(Dimension.Auto, Dimension.Auto);

        /// <summary>
        /// Create a new size <see cref="Dimension"/> from a unit length width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Size<Dimension> Dimension_FromLength(float width, float height) =>
            new(Dimension.FromLength(width), Dimension.FromLength(height));

        /// <summary>
        /// Create a new size <see cref="Dimension"/> from a percentage width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Size<Dimension> Dimension_FromPercentage(float width, float height) =>
            new(Dimension.FromPercentage(width), Dimension.FromPercentage(height));

        /// <summary>
        /// Create a new size <see cref="LengthPercentage"/> of Zero
        /// </summary>
        public static Size<LengthPercentage> LengthPercentage_Zero => new(LengthPercentage.Zero, LengthPercentage.Zero);

        /// <summary>
        /// Create a new size <see cref="Dimension"/> from a unit length width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Size<LengthPercentage> LengthPercentage_FromLength(float width, float height) =>
            new(LengthPercentage.FromLength(width), LengthPercentage.FromLength(height));

        /// <summary>
        /// Create a new size <see cref="LengthPercentage"/> from a percentage width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Size<LengthPercentage> LengthPercentage_FromPercentage(float width, float height) =>
            new(LengthPercentage.FromPercentage(width), LengthPercentage.FromPercentage(height));

        /// <summary>
        /// Convert to a C struct
        /// </summary>
        /// <returns></returns>
        internal c_Size ToCStruct()
        {
            if (IsLengthType && Width is Length lengthWidth && Height is Length lengthHeight)
            {
                return new c_Size
                {
                    width = lengthWidth.ToCStruct(),
                    height = lengthHeight.ToCStruct()
                };
            }
            throw new InvalidOperationException($"Size is not a length type ({typeof(T)}), can't convert to C struct");
        }
    }
}
