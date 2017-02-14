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
    }
}
