using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffySharp.Lib;

namespace TaffySharp
{
    /// <summary>
    /// A unit of linear measurement
    /// </summary>
    public class Dimension : Length
    {
        private DimensionType _type;
        private float _value;

        /// <summary>
        /// The dimension should be automatically computed according to algorithm-specific rules
        /// regarding the default size of boxes.
        /// </summary>
        public static Dimension Auto => new(DimensionType.Auto, 0);

        /// <summary>
        /// A dimension of zero
        /// </summary>
        public static Dimension Zero => new(DimensionType.Length, 0);

        /// <summary>
        /// An absolute length in some abstract units. Users of Taffy may define what they correspond
        /// to in their application (pixels, logical pixels, mm, etc) as they see fit.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Dimension FromLength(float length) => new(DimensionType.Length, length);

        /// <summary>
        /// A percentage length relative to the size of the containing block.
        ///
        /// <para>**NOTE: percentages are represented as a float value in the range [0.0, 1.0] NOT the range [0.0, 100.0]**</para>
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static Dimension FromPercentage(float percentage) => new(DimensionType.Percent, percentage);

        /// <inheritdoc />
        public override DimensionType Type
        {
            get => _type;
            set
            {
                // check value is a valid enum entry:
                if (!Enum.IsDefined(typeof(DimensionType), value))
                {
                    throw new ArgumentException($"Invalid dimension type: {value}. Should be one of: {string.Join(", ", Enum.GetNames(typeof(DimensionType)))}");
                }

                _type = value;
            }
        }

        /// <inheritdoc />
        public override float Value
        {
            get => _value;
            set => _value = value;
        }

        /// <summary>
        /// Create a new dimension
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public Dimension(DimensionType type, float value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Create a new dimension of length
        /// </summary>
        /// <param name="length"></param>
        public Dimension(float length) : this(DimensionType.Length, length) {
        
        }

        // casting
        public static implicit operator Dimension(float value) => new(DimensionType.Length, value);
        public static implicit operator float(Dimension dimension) => dimension.Value;
    }
}
