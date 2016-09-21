using System;
using System.IO;

namespace Platinum.Resolver
{
    /// <summary />
    public class RelfileResolver : IUrlResolver
    {
        /// <summary />
        public Uri ResolveUri( Uri baseUri, string relativeUri )
        {
            if ( baseUri == null )
            {
                /*
                 * Not a magic number :P
                 * 
                 * relfile:///
                 * 0123456789A
                 */
                string path = relativeUri.Substring( 11 );


                /*
                 * 
                 */
                string rel = AppDomain.CurrentDomain.BaseDirectory;
                string file = Path.Combine( rel, path );

                return new Uri( file, UriKind.Absolute );
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
