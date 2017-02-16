using System;

namespace Platinum.Logging
{
    public static class LoggerExtensions
    {
        /// <summary>
        /// Writes the exception at the Fatal level.
        /// </summary>
        /// <param name="logger">
        /// Instance of NLog logger.
        /// </param>
        /// <param name="exception">
        /// An exception to be logged.
        /// </param>
        public static void Fatal( this NLog.Logger logger, ActorException exception )
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
        public static void Error( this NLog.Logger logger, ActorException exception )
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
        public static void Warn( this NLog.Logger logger, ActorException exception )
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
        public static void Info( this NLog.Logger logger, ActorException exception )
        {
            #region Validations

            if ( exception == null )
                throw new ArgumentNullException( nameof( exception ) );

            #endregion

            logger.Info( exception, null );
        }


        public static void Event( this NLog.Logger logger, string eventId )
        {
            Event( logger, eventId, null );
        }


        public static void Event( this NLog.Logger logger, string eventId, params object[] args )
        {
            if ( eventId == null )
                throw new ArgumentNullException( nameof( eventId ) );

        }
    }
}
