using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffySharp.Lib;

namespace TaffySharp
{
    /// <summary>
    /// A rectangle
    /// </summary>
    /// <typeparam name="T">The length type</typeparam>
    public class Rect<T>
    {
        /// <summary>
        /// Wether the type is <see cref="Length"/>
        /// </summary>
#if NETFRAMEWORK || NETSTANDARD
        internal bool IsLengthType => Left is Length;
#else
        internal bool IsLengthType => typeof(T).IsAssignableTo(typeof(Length));
#endif

        /// <summary>
        /// An auto-sized rectangle for Dimension type
        /// </summary>
        public static Rect<Dimension> Dimension_Auto => new(Dimension.Auto);

        /// <summary>
        /// An zero-sized rectangle for Dimension type
        /// </summary>
        public static Rect<Dimension> Dimension_Zero => new(Dimension.Zero);

        /// <summary>
        /// An auto-sized rectangle for LengthPercentageAuto type
        /// </summary>
        public static Rect<LengthPercentageAuto> LengthPercentageAuto_Auto => new(LengthPercentageAuto.Auto);

        /// <summary>
        /// An zero-sized rectangle for LengthPercentageAuto type
        /// </summary>
        public static Rect<LengthPercentageAuto> LengthPercentageAuto_Zero => new(LengthPercentageAuto.Zero);

        /// <summary>
        /// The left side
        /// </summary>
        public T Left { get; set; }

        /// <summary>
        /// The right side
        /// </summary>
        public T Right { get; set; }

        /// <summary>
        /// The top side
        /// </summary>
        public T Top { get; set; }

        /// <summary>
        /// The bottom side
        /// </summary>
        public T Bottom { get; set; }

        /// <summary>
        /// Create a new rectangle
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        public Rect(T left, T right, T top, T bottom) =>
            (Left, Right, Top, Bottom) = (left, right, top, bottom);

        /// <summary>
        /// Create a new rectangle
        /// </summary>
        /// <param name="all">The value to set all sides to</param>
        public Rect(T all) => (Left, Right, Top, Bottom) = (all, all, all, all);

        /// <summary>
        /// Convert to a C struct
        /// </summary>
        /// <returns></returns>
        internal c_Rect ToCStruct()
        {
            if (IsLengthType && Left is Length lengthLeft && Right is Length lengthRight && Top is Length lengthTop && Bottom is Length lengthBottom)
            {
                return new c_Rect
                {
                    left = lengthLeft.ToCStruct(),
                    right = lengthRight.ToCStruct(),
                    top = lengthTop.ToCStruct(),
                    bottom = lengthBottom.ToCStruct()
                };
            }
            throw new InvalidOperationException($"Rect is not a length type ({typeof(T)}), can't convert to C struct");
        }
    }
}
