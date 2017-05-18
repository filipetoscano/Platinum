using NLog;
using System;

namespace Platinum.Metrics
{
    /// <summary>
    /// Metric helper.
    /// </summary>
    public class Metric
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Writes a time-series data-point.
        /// </summary>
        /// <typeparam name="T">Type of measure.</typeparam>
        /// <param name="measure">Value of measure.</param>
        /// <remarks>
        /// Name of measure will be automatically derived from the (non
        /// namespace-qualified) name of the CLR type.
        /// </remarks>
        public static void Write<T>( T measure )
        {
            #region Validations

            if ( measure == null )
                throw new ArgumentNullException( nameof( measure ) );

            #endregion

            logger.Info( typeof( T ).Name, measure );
        }
    }
}
