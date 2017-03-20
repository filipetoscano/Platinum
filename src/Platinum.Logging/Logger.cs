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


        public void Info( ActorException exception )
        {
            this._logger.Info( exception );
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
