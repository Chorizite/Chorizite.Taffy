using TaffySharp.Lib;

namespace TaffySharp
{
    /// <summary>
    /// A unit of linear measurement
    /// </summary>
    public abstract class Length
    {
        /// <summary>
        /// The dimension type
        /// </summary>
        public abstract DimensionType Type { get; set; }

        /// <summary>
        /// The dimension value
        /// </summary>
        public abstract float Value { get; set; }

        /// <summary>
        /// Convert to a C struct
        /// </summary>
        /// <returns></returns>
        internal c_Length ToCStruct() => new() { dim = (int)Type, value = Value };
    }
}