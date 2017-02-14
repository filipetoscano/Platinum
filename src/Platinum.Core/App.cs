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
        /// Gets the name of the environment in which application is
        /// deployed, or raises an exception if the value is not defined.
        /// </summary>
        public static string Environment
        {
            get
            {
                return AppConfiguration.Get<string>( "Environment" );
            }
        }


        /// <summary>
        /// Gets the name of the application, or returns a hardcoded value
        /// if the key is not defined.
        /// </summary>
        /// <remarks>
        /// For internal use only.
        /// </remarks>
        internal static string SafeName
        {
            get
            {
                return AppConfiguration.Get<string>( "Application", "##NotDefined##" );
            }
        }


        /// <summary>
        /// Gets the name of the environment in which application is
        /// deployed, or returns a hardcoded value if key is not defined.
        /// </summary>
        /// <remarks>
        /// For internal use only.
        /// </remarks>
        internal static string SafeEnvironment
        {
            get
            {
                return AppConfiguration.Get<string>( "Environment", "##NotDefined##" );
            }
        }
    }
}

/* eof */