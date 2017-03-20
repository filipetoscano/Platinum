using System;

namespace Platinum.Logging
{
    public abstract class Auditor<T> where T : ActorException
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public static void Event( ActorException exception )
        {
            logger.Error( exception );
        }


        public static void Event( string eventId )
        {
            object[] xargs = new object[ 1 ];
            xargs[ 0 ] = eventId;

            T t = (T) System.Activator.CreateInstance( typeof( T ), xargs );
            logger.Error( t );
        }



        public static void Event( string eventId, Exception innerException )
        {
            object[] xargs = new object[ 1 ];
            xargs[ 0 ] = eventId;
            xargs[ 1 ] = innerException;

            T t = (T) System.Activator.CreateInstance( typeof( T ), xargs );
            logger.Error( t );
        }


        public static void Event( string eventId, params object[] args )
        {
            object[] xargs = new object[ 1 + args.Length ];
            xargs[ 0 ] = eventId;
            Array.Copy( args, 0, xargs, 1, args.Length );

            T t = (T) System.Activator.CreateInstance( typeof( T ), xargs );
            logger.Error( t );
        }


        public static void Event( string eventId, Exception innerException, params object[] args )
        {
            object[] xargs = new object[ 2 + args.Length ];
            xargs[ 0 ] = eventId;
            xargs[ 1 ] = innerException;
            Array.Copy( args, 0, xargs, 2, args.Length );

            T t = (T) System.Activator.CreateInstance( typeof( T ), xargs );
            logger.Error( t );
        }
    }
}
