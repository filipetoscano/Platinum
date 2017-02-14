using System;

namespace Platinum.Logging
{
    public static class LoggerExtensions
    {
        public static void Event( this NLog.Logger logger, string eventId )
        {
            throw new NotImplementedException();
        }


        public static void Event( this NLog.Logger logger, ActorException @event )
        {
            throw new NotImplementedException();
        }
    }
}
