using Platinum.Configuration;

namespace Platinum
{
    /// <summary />
    public static class App
    {
        /// <summary>
        /// Gets the name of the application, or raises an exception if
        /// the value is not defined.
        /// </summary>
        public static string Name
        {
            get
            {
                return AppConfiguration.Get<string>( "Application" ); 
            }
        }


        /// <summary>
        /// Gets the name of the application, or returns a hardcoded value
        /// if the value is not defined.
        /// </summary>
        internal static string SafeName
        {
            get
            {
                return AppConfiguration.Get<string>( "Application", "##NotDefined##" );
            }
        }
    }
}

/* eof */