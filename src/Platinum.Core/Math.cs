using System;
using System.Collections.Generic;

namespace Platinum
{
    /// <summary />
    public static class Math
    {
        /// <summary>
        /// Pre-built list of powers of 10, up-to 8th power.
        /// </summary>
        private static List<int> _pow = new List<int>()
        {
            1,
            10,
            100,
            1000,
            10000,
            100000,
            1000000,
            10000000,
            100000000,
        };


        /// <summary>
        /// Returns the absolute value of a System.Decimal number.
        /// </summary>
        /// <param name="value">
        /// A number that is greater than or equal to <see cref="Decimal.MinValue"/>,
        /// but less than or equal to <see cref="Decimal.MaxValue"/>.
        /// </param>
        /// <returns>
        /// A decimal number, x, such that 0 ≤ x ≤ <see cref="Decimal.MaxValue"/>.
        /// </returns>
        public static decimal Abs( decimal value )
        {
            return System.Math.Abs( value );
        }


        /// <summary>
        /// Returns the smallest integer value that is greater than or equal to the specified
        /// decimal number.
        /// </summary>
        /// <param name="value">
        /// A decimal number.
        /// </param>
        /// <returns>
        /// The smallest integral value that is greater than or equal to value.
        /// </returns>
        public static decimal Ceiling( decimal value )
        {
            return Decimal.Ceiling( value );
        }


        /// <summary>
        /// Returns the largest integer value that is less than or equal to the specified
        /// decimal number.
        /// </summary>
        /// <param name="value">
        /// A decimal number.
        /// </param>
        /// <returns>
        /// The largest integral value that is less than or equal to value.
        /// </returns>
        public static decimal Floor( decimal value )
        {
            return System.Math.Floor( value );
        }


        /// <summary>
        /// Rounds a decimal value to a precision of 0 decimal digits, using
        /// the <see cref="RoundingMode.HalfEven" /> rounding mode.
        /// </summary>
        /// <param name="value">
        /// A decimal number to round.
        /// </param>
        /// <returns>
        /// Rounded number.
        /// </returns>
        public static decimal Round( decimal value )
        {
            return Round( value, 0, RoundingMode.HalfEven );
        }


        /// <summary>
        /// Rounds a decimal value to a specified precision, using
        /// the <see cref="RoundingMode.HalfEven" /> rounding mode.
        /// </summary>
        /// <param name="value">
        /// A decimal number to round.
        /// </param>
        /// <param name="decimals">
        /// The number of significant decimal places (precision) in the return value.
        /// </param>
        /// <returns>
        /// Rounded number.
        /// </returns>
        public static decimal Round( decimal value, int decimals )
        {
            return Round( value, decimals, RoundingMode.HalfEven );
        }


        /// <summary>
        /// Rounds a decimal value to a specified precision, using the rounding
        /// behaviour.
        /// </summary>
        /// <param name="value">
        /// A decimal number to round.
        /// </param>
        /// <param name="decimals">
        /// The number of significant decimal places (precision) in the return value.
        /// </param>
        /// <param name="mode">
        /// A value that specifies how to round value.
        /// </param>
        /// <returns>
        /// Rounded number.
        /// </returns>
        public static decimal Round( decimal value, int decimals, RoundingMode mode )
        {
            #region Validations

            if ( decimals < 0 )
                throw new ArgumentOutOfRangeException( nameof( decimals ), "Value must be non-negative" );

            if ( decimals >= _pow.Count )
                throw new ArgumentOutOfRangeException( nameof( decimals ), $"Can only round up-to { _pow.Count - 1 } digits." );

            #endregion

            int factor = _pow[ decimals ];


            /*
             * See:
             * https://docs.oracle.com/javase/7/docs/api/java/math/RoundingMode.html
             */
            decimal v;

            switch ( mode )
            {
                case RoundingMode.Ceiling:
                    v = Decimal.Ceiling( value * factor ) / factor;
                    break;

                case RoundingMode.Floor:
                    v = Decimal.Floor( value * factor ) / factor;
                    break;


                case RoundingMode.Up:
                    v = System.Math.Ceiling( System.Math.Abs( value * factor ) ) * System.Math.Sign( value ) / factor;
                    break;

                case RoundingMode.Down:
                    v = System.Math.Floor( System.Math.Abs( value * factor ) ) * System.Math.Sign( value ) / factor;
                    break;


                case RoundingMode.HalfUp:
                    v = Decimal.Round( value, decimals, MidpointRounding.AwayFromZero );
                    break;

                case RoundingMode.HalfDown:
                    var pre = value * factor;
                    var rounded = Decimal.Round( pre, MidpointRounding.AwayFromZero );

                    if ( rounded > 0 )
                    {
                        if ( rounded - pre == 0.5m )
                            rounded--;
                    }
                    else
                    {
                        if ( pre - rounded == 0.5m )
                            rounded++;
                    }

                    v = rounded / factor;
                    break;

                case RoundingMode.HalfEven:
                    v = Decimal.Round( value, decimals, MidpointRounding.ToEven );
                    break;


                default:
                    throw new ArgumentOutOfRangeException( nameof( mode ), "Not a valid value." );
            }

            return v;
        }
    }
}
