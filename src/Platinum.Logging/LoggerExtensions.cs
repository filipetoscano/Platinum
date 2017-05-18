using Platinum;
using System;

namespace NLog
{
    public static class LoggerExtensions
    {
        /// <summary>
        /// Writes the exception. If the exception does not have a specified
        /// log level, will write at Warn.
        /// </summary>
        /// <param name="logger">
        /// Instance of NLog logger.
        /// </param>
        /// <param name="exception">
        /// An exception to be logged.
        /// </param>
        public static void Log( this Logger logger, ActorException exception )
        {
            #region Validations

            if ( exception == null )
                throw new ArgumentNullException( nameof( exception ) );

            #endregion

            logger.Warn( exception, null );
        }


        /// <summary>
        /// Writes the exception at the Fatal level.
        /// </summary>
        /// <param name="logger">
        /// Instance of NLog logger.
        /// </param>
        /// <param name="exception">
        /// An exception to be logged.
        /// </param>
        public static void Fatal( this Logger logger, ActorException exception )
        {
            #region Validations

            if ( exception == null )
                throw new ArgumentNullException( nameof( exception ) );

            #endregion

            logger.Fatal( exception, null );
        }


        /// <summary>
        /// Writes the exception at the Error level.
        /// </summary>
        /// <param name="logger">
        /// Instance of NLog logger.
        /// </param>
        /// <param name="exception">
        /// An exception to be logged.
        /// </param>
        public static void Error( this Logger logger, ActorException exception )
        {
            #region Validations

            if ( exception == null )
                throw new ArgumentNullException( nameof( exception ) );

            #endregion

            logger.Error( exception, null );
        }


        /// <summary>
        /// Writes the exception at the Warn level.
        /// </summary>
        /// <param name="logger">
        /// Instance of NLog logger.
        /// </param>
        /// <param name="exception">
        /// An exception to be logged.
        /// </param>
        public static void Warn( this Logger logger, ActorException exception )
        {
            #region Validations

            if ( exception == null )
                throw new ArgumentNullException( nameof( exception ) );

            #endregion

            logger.Warn( exception, null );
        }


        /// <summary>
        /// Writes the exception at the Info level.
        /// </summary>
        /// <param name="logger">
        /// Instance of NLog logger.
        /// </param>
        /// <param name="exception">
        /// An exception to be logged.
        /// </param>
        public static void Info( this Logger logger, ActorException exception )
        {
            #region Validations

            if ( exception == null )
                throw new ArgumentNullException( nameof( exception ) );

            #endregion

            logger.Info( exception, null );
        }
    }
}
