using System;

namespace Platinum.Logging
{
    /// <summary>
    /// Produces audit information.
    /// </summary>
    /// <typeparam name="T">
    /// Type of exception class which is used to represent the event. This
    /// allows for complete re-use of loading strings from the associated
    /// resource manager.
    /// </typeparam>
    public abstract class Auditor<T> where T : ActorException
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Publishes an exception as an audit event.
        /// </summary>
        /// <param name="exception">
        /// Exception to be audited.
        /// </param>
        /// <remarks>
        /// By default, will create an entry with the log level of 'Warn'. However,
        /// if the exception contains its own log-level, the log level will be
        /// overriden.
        /// </remarks>
        public static void Event( ActorException exception )
        {
            logger.Warn( exception );
        }


        /// <summary>
        /// Publishes an instance of exception <typeparamref name="T"/> with the given event
        /// identifier as an audit event.
        /// </summary>
        /// <param name="eventId">
        /// Event identifier, as defined in the Errors.xml file.
        /// </param>
        /// <remarks>
        /// By default, will create an entry with the log level of 'Warn'. However,
        /// if the exception contains its own log-level, the log level will be
        /// overriden.
        /// </remarks>
        public static void Event( string eventId )
        {
            #region Validations

            if ( eventId == null )
                throw new ArgumentNullException( nameof( eventId ) );

            #endregion

            object[] xargs = new object[ 1 ];
            xargs[ 0 ] = eventId;

            T t = (T) System.Activator.CreateInstance( typeof( T ), xargs );
            logger.Warn( t );
        }



        /// <summary>
        /// Publishes an instance of exception <typeparamref name="T"/> with the given event
        /// identifier as an audit event.
        /// </summary>
        /// <param name="eventId">
        /// Event identifier, as defined in the Errors.xml file.
        /// </param>
        /// <param name="innerException">
        /// Inner exception.
        /// </param>
        /// <remarks>
        /// By default, will create an entry with the log level of 'Warn'. However,
        /// if the exception contains its own log-level, the log level will be
        /// overriden.
        /// </remarks>
        public static void Event( string eventId, Exception innerException )
        {
            #region Validations

            if ( eventId == null )
                throw new ArgumentNullException( nameof( eventId ) );

            #endregion

            object[] xargs = new object[ 2 ];
            xargs[ 0 ] = eventId;
            xargs[ 1 ] = innerException;

            T t = (T) System.Activator.CreateInstance( typeof( T ), xargs );
            logger.Warn( t );
        }


        /// <summary>
        /// Publishes an instance of exception <typeparamref name="T"/> with the given event
        /// identifier as an audit event.
        /// </summary>
        /// <param name="eventId">
        /// Event identifier, as defined in the Errors.xml file.
        /// </param>
        /// <param name="args">
        /// Event arguments.
        /// </param>
        /// <remarks>
        /// By default, will create an entry with the log level of 'Warn'. However,
        /// if the exception contains its own log-level, the log level will be
        /// overriden.
        /// </remarks>
        public static void Event( string eventId, params object[] args )
        {
            #region Validations

            if ( eventId == null )
                throw new ArgumentNullException( nameof( eventId ) );

            #endregion

            object[] xargs = new object[ 1 + args.Length ];
            xargs[ 0 ] = eventId;
            Array.Copy( args, 0, xargs, 1, args.Length );

            T t = (T) System.Activator.CreateInstance( typeof( T ), xargs );
            logger.Warn( t );
        }


        /// <summary>
        /// Publishes an instance of exception <typeparamref name="T"/> with the given event
        /// identifier as an audit event.
        /// </summary>
        /// <param name="eventId">
        /// Event identifier, as defined in the Errors.xml file.
        /// </param>
        /// <param name="innerException">
        /// Inner exception.
        /// </param>
        /// <param name="args">
        /// Event arguments.
        /// </param>
        /// <remarks>
        /// By default, will create an entry with the log level of 'Warn'. However,
        /// if the exception contains its own log-level, the log level will be
        /// overriden.
        /// </remarks>
        public static void Event( string eventId, Exception innerException, params object[] args )
        {
            #region Validations

            if ( eventId == null )
                throw new ArgumentNullException( nameof( eventId ) );

            #endregion

            object[] xargs = new object[ 2 + args.Length ];
            xargs[ 0 ] = eventId;
            xargs[ 1 ] = innerException;
            Array.Copy( args, 0, xargs, 2, args.Length );

            T t = (T) System.Activator.CreateInstance( typeof( T ), xargs );
            logger.Warn( t );
        }
    }
}
