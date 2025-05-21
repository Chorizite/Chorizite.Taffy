using System;
using TaffySharp.Lib;

namespace TaffySharp
{
    /// <summary>
    /// Represents a grid placement
    /// </summary>
    public class GridPlacement
    {
        /// <summary>
        /// The line/span number
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// The type of placement
        /// </summary>
        public GridPlacementType Type { get; set; }

        private GridPlacement(int value, GridPlacementType type)
        {
            if (type == GridPlacementType.Span && value < 0)
                throw new ArgumentException("Span value must be non-negative.");
            if (type == GridPlacementType.Auto && value != 0)
                throw new ArgumentException("Auto placement must have a value of 0.");
            (Value, Type) = (value, type);
        }

        /// <summary>
        /// Create a new auto placement
        /// </summary>
        /// <returns></returns>
        public static GridPlacement Auto() => new GridPlacement(0, GridPlacementType.Auto);

        /// <summary>
        /// Create a new line placement
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static GridPlacement Line(int line) => new GridPlacement(line, GridPlacementType.Line);

        /// <summary>
        /// Create a new span placement
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public static GridPlacement Span(int span) => new GridPlacement(span, GridPlacementType.Span);

        /// <summary>
        /// Converts this GridPlacement to a c_GridPlacement struct for the Taffy binding
        /// Assumes this GridPlacement specifies the start, with end set to Auto
        /// </summary>
        internal c_GridPlacement ToCStruct()
        {
            var gridIndex = new c_GridIndex
            {
                kind = Type switch
                {
                    GridPlacementType.Auto => 0,
                    GridPlacementType.Line => 1,
                    GridPlacementType.Span => 2,
                    _ => throw new NotImplementedException($"Unsupported GridPlacementType: {Type}")
                },
                value = Type == GridPlacementType.Span && Value > short.MaxValue
                    ? throw new ArgumentException($"Span value {Value} exceeds maximum allowed value {short.MaxValue}")
                    : (short)Value
            };

            return new c_GridPlacement
            {
                start = gridIndex,
                end = new c_GridIndex { kind = 0, value = 0 } // Auto
            };
        }
    }
}