using System;

namespace Platinum.Logging
{
    public partial class Logger
    {
        private NLog.Logger _logger;


        internal Logger( NLog.Logger logger )
        {
            if ( logger == null )
                throw new ArgumentNullException( nameof( logger ) );

            this._logger = logger;
        }


        public NLog.Logger Raw
        {
            get { return _logger; }
        }


        public void Info( ActorException exception )
        {
            #region Validations

            if ( exception == null )
                throw new ArgumentNullException( nameof( exception ) );

            #endregion

            this._logger.Info( exception );
        }


        public void Info<T>( string eventId, params object[] args ) where T : ActorException
        {
            #region Validations

            if ( eventId == null )
                throw new ArgumentNullException( nameof( eventId ) );

            #endregion

            object[] xargs = new object[ 1 ];
            xargs[ 0 ] = eventId;

            T t = (T) System.Activator.CreateInstance( typeof( T ), xargs );
            this._logger.Error( t );
        }


        public void Warn( ActorException exception )
        {
            this._logger.Warn( exception );
        }


        public void Error( ActorException exception )
        {
            this._logger.Error( exception );
        }


        public void Fatal( ActorException exception )
        {
            this._logger.Fatal( exception );
        }
    }
}
