using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffySharp
{
    /// <summary>
    ///  A unit of linear measurement. Can be length, or percentage.
    /// </summary>
    public class LengthPercentage : Length
    {
        private DimensionType _type;
        private float _value;

        /// <summary>
        /// A LengthPercentage of zero
        /// </summary>
        public static LengthPercentage Zero => new(DimensionType.Length, 0);

        /// <summary>
        /// An absolute length in some abstract units. Users of Taffy may define what they correspond
        /// to in their application (pixels, logical pixels, mm, etc) as they see fit.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static LengthPercentage FromLength(float length) => new(DimensionType.Length, length);

        /// <summary>
        /// A percentage length relative to the size of the containing block.
        ///
        /// <para>**NOTE: percentages are represented as a float value in the range [0.0, 1.0] NOT the range [0.0, 100.0]**</para>
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static LengthPercentage FromPercentage(float percentage) => new(DimensionType.Percent, percentage);

        /// <inheritdoc />
        public override DimensionType Type
        {
            get => _type;
            set
            {
                // check value is a valid enum entry:
                switch (value)
                {
                    case DimensionType.Length:
                    case DimensionType.Percent:
                        break;
                    default:
                        throw new ArgumentException($"Invalid dimension type: {value}. Should be one of: Length, Percent.");
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
        /// Create a new LengthPercentage
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public LengthPercentage(DimensionType type, float value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Create a new LengthPercentage of length
        /// </summary>
        /// <param name="length"></param>
        public LengthPercentage(float length) : this(DimensionType.Length, length) { }

        //casting
        public static implicit operator LengthPercentage(float value) => new(DimensionType.Length, value);
        public static implicit operator float(LengthPercentage value) => value.Value;
    }
}
