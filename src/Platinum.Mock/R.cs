using System;

namespace Platinum.Mock
{
    /// <summary>
    /// Random number generator utility.
    /// </summary>
    internal static class R
    {
        private static Random _r = new Random();

        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is greater than or equal to 0 and less than
        /// System.Int32.MaxValue.
        /// </returns>
        public static int Next()
        {
            return _r.Next();
        }


        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random number to be generated. maxValue must
        /// be greater than or equal to 0.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that is greater than or equal to 0, and less than maxValue;
        /// that is, the range of return values ordinarily includes 0 but not maxValue. However,
        /// if maxValue equals 0, maxValue is returned.
        /// </returns>
        public static int Next( int maxValue )
        {
            return _r.Next( maxValue );
        }


        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">
        /// The inclusive lower bound of the random number returned.
        /// </param>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random number returned. maxValue must be greater
        /// than or equal to minValue.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to minValue and less than maxValue;
        /// that is, the range of return values includes minValue but not maxValue. If minValue
        /// equals maxValue, minValue is returned.
        /// </returns>
        public static int Next( int minValue, int maxValue )
        {
            return _r.Next( minValue, maxValue );
        }


        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0,
        /// and less than 1.0.
        /// </summary>
        /// <returns>
        /// A double-precision floating point number that is greater than or equal to 0.0,
        /// and less than 1.0.
        /// </returns>
        public static double NextDouble()
        {
            return _r.NextDouble();
        }


        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0,
        /// and less than maxValue.
        /// </summary>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random number to be generated. maxValue must
        /// be greater than or equal to 0.
        /// </param>
        /// <returns>
        /// A double-precision floating point number that is greater than or equal to 0.0,
        /// and less than maxValue.
        /// </returns>
        public static double NextDouble( double maxValue )
        {
            return _r.NextDouble() * maxValue;
        }


        /// <summary>
        /// Returns a random floating-point number that is within a specified range.
        /// </summary>
        /// <param name="minValue">
        /// The inclusive lower bound of the random number to be generated.
        /// </param>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random number to be generated. maxValue must
        /// be greater than or equal to minValue.
        /// </param>
        /// <returns>
        /// A double-precision floating point number that is greater than or equal to minValue,
        /// and less than maxValue. If minValue equals maxValue, minValue is returned.
        /// </returns>
        public static double NextDouble( double minValue, double maxValue )
        {
            if ( minValue > maxValue )
                throw new ArgumentOutOfRangeException( "minValue must be less than maxValue." );

            if ( minValue == maxValue )
                return minValue;

            return minValue + _r.NextDouble() * (maxValue - minValue);
        }
    }
}
