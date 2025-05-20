using TaffySharp.Lib;

namespace TaffySharp
{
    public class Point<T>
    {
        /// <summary>
        /// The x
        /// </summary>
        public T X { get; set; }

        /// <summary>
        /// The y
        /// </summary>
        public T Y { get; set; }

        /// <summary>
        /// Create a new point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(T x, T y) => (X, Y) = (x, y);

        /// <summary>
        /// Convert to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{X}, {Y}";
    }
}