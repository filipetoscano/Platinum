using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Platinum.Database
{
    /// <summary />
    internal static class Extensions
    {
        /// <summary />
        internal static string LoadEmbeddedResource( this Assembly assembly, Type type, string resourceName )
        {
            #region Validations

            if ( assembly == null )
                throw new ArgumentNullException( nameof( assembly ) );

            if ( type == null )
                throw new ArgumentNullException( nameof( type ) );

            if ( resourceName == null )
                throw new ArgumentNullException( nameof( resourceName ) );

            #endregion

            var stream = assembly.GetManifestResourceStream( type, resourceName );

            using ( StreamReader sr = new StreamReader( stream, Encoding.UTF8 ) )
            {
                return sr.ReadToEnd();
            }
        }


        /// <summary />
        internal static string LoadEmbeddedResource( this Assembly assembly, string resourceName )
        {
            #region Validations

            if ( assembly == null )
                throw new ArgumentNullException( nameof( assembly ) );

            if ( resourceName == null )
                throw new ArgumentNullException( nameof( resourceName ) );

            #endregion

            var stream = assembly.GetManifestResourceStream( resourceName );

            using ( StreamReader sr = new StreamReader( stream, Encoding.UTF8 ) )
            {
                return sr.ReadToEnd();
            }
        }
    }
}
