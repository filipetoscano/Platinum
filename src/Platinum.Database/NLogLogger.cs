using DbUp.Engine.Output;
using NLog;
using System;

namespace Platinum.Database
{
    /// <summary>
    /// Emit DbUp upgrade messages through the NLog pipeline.
    /// </summary>
    public class NLogLogger : IUpgradeLog
    {
        private Logger logger;


        /// <summary>
        /// Initializes a new instance of NLogLogger.
        /// </summary>
        /// <param name="logger">NLog logger.</param>
        public NLogLogger( Logger logger )
        {
            #region Validations

            if ( logger == null )
                throw new ArgumentNullException( nameof( logger ) );

            #endregion

            this.logger = logger;
        }


        /// <summary>
        /// Writes an error as a fatal message.
        /// </summary>
        /// <param name="format">A string containing format items.</param>
        /// <param name="args">Arguments to format.</param>
        public void WriteError( string format, params object[] args )
        {
            logger.Fatal( format, args );
        }


        /// <summary>
        /// Writes a warning.
        /// </summary>
        /// <param name="format">A string containing format items.</param>
        /// <param name="args">Arguments to format.</param>
        public void WriteWarning( string format, params object[] args )
        {
            logger.Warn( format, args );
        }


        /// <summary>
        /// Writes an information message.
        /// </summary>
        /// <param name="format">A string containing format items.</param>
        /// <param name="args">Arguments to format.</param>
        public void WriteInformation( string format, params object[] args )
        {
            logger.Info( format, args );
        }
    }
}
