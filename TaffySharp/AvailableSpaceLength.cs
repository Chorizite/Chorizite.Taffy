using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffySharp.Lib;

namespace TaffySharp
{
    /// <summary>
    /// The amount of space available to a node in a given axis
    /// </summary>
    public class AvailableSpaceLength
    {
        /// <summary>
        /// The type of available space
        /// </summary>
        public enum AvailableSpaceType
        {
            /// <summary>
            /// The amount of space available is the specified number of pixels.
            /// </summary>
            Definite,

            /// <summary>
            /// The amount of space available is indefinite and the node should be laid out under a min-content constraint
            /// </summary>
            MinContent,

            /// <summary>
            /// The amount of space available is indefinite and the node should be laid out under a max-content constraint
            /// </summary>
            MaxContent
        }

        /// <summary>
        /// The type of available space
        /// </summary>
        public AvailableSpaceType Type { get; set; }

        /// <summary>
        /// The amount of space available, only applicable if <see cref="Type"/> is <see cref="AvailableSpaceType.Definite"/>
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// A zero-sized AvailableSpace
        /// </summary>
        public static AvailableSpaceLength Zero => new AvailableSpaceLength(AvailableSpaceType.Definite, 0);

        /// <summary>
        /// An AvailableSpace representing min-content
        /// </summary>
        public static AvailableSpaceLength MinContent => new AvailableSpaceLength(AvailableSpaceType.MinContent, 0);

        /// <summary>
        /// An AvailableSpace representing max-content
        /// </summary>
        public static AvailableSpaceLength MaxContent => new AvailableSpaceLength(AvailableSpaceType.MaxContent, 0);

        /// <summary>
        /// Create a new AvailableSpace from a definite value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static AvailableSpaceLength Definitive(float value) => new AvailableSpaceLength(AvailableSpaceType.Definite, value);


        /// <summary>
        /// Create a new AvailableSpace
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public AvailableSpaceLength(AvailableSpaceType type, float value) => (Type, Value) = (type, value);


        internal c_Length ToCStruct() => new() { dim = (int)Type, value = Value };
    }
}
