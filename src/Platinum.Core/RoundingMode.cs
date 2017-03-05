using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platinum
{
    /// <summary>
    /// Specifies a rounding behavior for numerical operations capable of discarding precision.
    /// </summary>
    public enum RoundingMode
    {
        /// <summary>
        /// Rounding mode to round away from zero.
        /// </summary>
        Up,

        /// <summary>
        /// Rounding mode to round towards zero.
        /// </summary>
        Down,

        /// <summary>
        /// Rounding mode to round towards positive infinity.
        /// </summary>
        Ceiling,

        /// <summary>
        /// Rounding mode to round towards negative infinity.
        /// </summary>
        Floor,

        /// <summary>
        /// Rounding mode to round towards "nearest neighbor" unless both
        /// neighbors are equidistant, in which case round up
        /// </summary>
        /// <remarks>
        /// Equivalent to <see cref="MidpointRounding.AwayFromZero" />.
        /// </remarks>
        HalfUp,

        /// <summary>
        /// Rounding mode to round towards "nearest neighbor" unless both
        /// neighbors are equidistant, in which case round down
        /// </summary>
        HalfDown,

        /// <summary>
        /// Rounding mode to round towards the "nearest neighbor" unless
        /// both neighbors are equidistant, in which case, round towards
        /// the even neighbor
        /// </summary>
        /// <remarks>
        /// Equivalent to <see cref="MidpointRounding.ToEven" />.
        /// </remarks>
        HalfEven
    }
}
