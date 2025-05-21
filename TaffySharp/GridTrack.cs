using System.Runtime.InteropServices;
using TaffySharp.Lib;

namespace TaffySharp
{
    /// <summary>
    /// Represents a grid track
    /// </summary>
    public class GridTrack
    {
        /// <summary>
        /// The type of this track
        /// </summary>
        public TrackSize Type { get; set; }

        /// <summary>
        /// The value of this track
        /// </summary>
        public float? Value { get; set; } // For Length or Fr

        /// <summary>
        /// The repeat count of this track
        /// </summary>
        public int? RepeatCount { get; set; } // For Repeat: -1 (AutoFit), 0 (AutoFill), >0 (Count)

        /// <summary>
        /// The repeat tracks of this track
        /// </summary>
        public List<GridTrack>? RepeatTracks { get; set; } // For Repeat, must be non-repeated tracks

        private GridTrack(TrackSize type, float? value = null, int? repeatCount = null, List<GridTrack>? repeatTracks = null)
        {
            if (type == TrackSize.Repeat)
            {
                if (!repeatCount.HasValue || repeatCount.Value < -1 || repeatTracks == null || !repeatTracks.Any())
                    throw new ArgumentException("For Repeat, RepeatCount must be -1 (AutoFit), 0 (AutoFill), or positive (Count), and RepeatTracks must be non-empty.");
                if (repeatTracks.Any(t => t.Type == TrackSize.Repeat))
                    throw new ArgumentException("RepeatTracks must contain only non-repeated tracks (Length, Fr, Auto).");
            }
            else
            {
                if (repeatCount.HasValue || repeatTracks != null)
                    throw new ArgumentException("RepeatCount and RepeatTracks are only valid for Repeat type.");
                if ((type == TrackSize.Length || type == TrackSize.Fr) && !value.HasValue)
                    throw new ArgumentException("Value is required for Length and Fr types.");
                if (type == TrackSize.Auto && value.HasValue)
                    throw new ArgumentException("Value should not be set for Auto type.");
            }

            Type = type;
            Value = value;
            RepeatCount = repeatCount;
            RepeatTracks = repeatTracks;
        }

        // Helper methods for track types
        public static GridTrack Length(float value) => new GridTrack(TrackSize.Length, value);
        public static GridTrack Fr(float value) => new GridTrack(TrackSize.Fr, value);
        public static GridTrack Auto => new GridTrack(TrackSize.Auto);
        public static GridTrack Repeat(int count, List<GridTrack> tracks)
        {
            if (count <= 0) throw new ArgumentException("Count must be positive for Repeat. Use AutoFit or AutoFill for automatic repetition.");
            return new GridTrack(TrackSize.Repeat, null, count, tracks);
        }
        public static GridTrack AutoFit(List<GridTrack> tracks) => new GridTrack(TrackSize.Repeat, null, -1, tracks);
        public static GridTrack AutoFill(List<GridTrack> tracks) => new GridTrack(TrackSize.Repeat, null, 0, tracks);

        /// <summary>
        /// Converts a non-repeated GridTrack to a c_GridTrackSize struct
        /// </summary>
        internal unsafe c_GridTrackSize ToCGridTrackSize()
        {
            if (Type == TrackSize.Repeat)
                throw new InvalidOperationException("Cannot convert a Repeat track to c_GridTrackSize");

            c_Length min_size;
            c_Length max_size;

            switch (Type)
            {
                case TrackSize.Length:
                    min_size = new c_Length { dim = 1, value = Value.Value }; // Length in pixels
                    max_size = new c_Length { dim = 1, value = Value.Value }; // Length in pixels
                    break;
                case TrackSize.Fr:
                    min_size = new c_Length { dim = 0, value = 0 }; // Auto
                    max_size = new c_Length { dim = 7, value = Value.Value }; // Fr units
                    break;
                case TrackSize.Auto:
                    min_size = new c_Length { dim = 0, value = 0 }; // Auto
                    max_size = new c_Length { dim = 0, value = 0 }; // Auto
                    break;
                default:
                    throw new NotImplementedException($"Unsupported TrackSize: {Type}");
            }

            return new c_GridTrackSize { min_size = min_size, max_size = max_size };
        }

        /// <summary>
        /// Converts this GridTrack to a c_GridTrackSizing struct for the Taffy binding
        /// Note: The caller (e.g., Style class) must free the allocated memory for single and repeat pointers using Marshal.FreeHGlobal
        /// </summary>
        internal unsafe c_GridTrackSizing ToCStruct(List<IntPtr> allocatedPointers)
        {
            if (Type == TrackSize.Repeat)
            {
                var repeatTracksArray = RepeatTracks.Select(t => t.ToCGridTrackSize()).ToArray();
                IntPtr repeatPtr = IntPtr.Zero;
                if (repeatTracksArray.Length > 0)
                {
                    repeatPtr = Marshal.AllocHGlobal(repeatTracksArray.Length * Marshal.SizeOf<c_GridTrackSize>());
                    allocatedPointers.Add(repeatPtr);
                    for (int i = 0; i < repeatTracksArray.Length; i++)
                    {
                        Marshal.StructureToPtr(repeatTracksArray[i], repeatPtr + (i * Marshal.SizeOf<c_GridTrackSize>()), false);
                    }
                }
                return new c_GridTrackSizing
                {
                    repetition = RepeatCount.Value,
                    single = null,
                    repeat = (c_GridTrackSize*)repeatPtr,
                    repeat_count = (nuint)repeatTracksArray.Length
                };
            }
            else
            {
                var trackSize = ToCGridTrackSize();
                IntPtr singlePtr = Marshal.AllocHGlobal(Marshal.SizeOf<c_GridTrackSize>());
                allocatedPointers.Add(singlePtr);
                Marshal.StructureToPtr(trackSize, singlePtr, false);
                return new c_GridTrackSizing
                {
                    repetition = -2,
                    single = (c_GridTrackSize*)singlePtr,
                    repeat = null,
                    repeat_count = 0
                };
            }
        }
    }
}