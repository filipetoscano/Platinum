using System;
using System.IO;
using System.Reflection;

namespace Platinum.Resolver
{
    public class RelfileResolver : IUrlResolver
    {
        public Uri ResolveUri( Uri baseUri, string relativeUri )
        {
            if ( baseUri == null )
            {
                string rel = AppDomain.CurrentDomain.BaseDirectory;
                string path = new Uri( relativeUri ).LocalPath;

                if ( AppDomain.CurrentDomain.FriendlyName.StartsWith( "UnitTestAdapter" ) == true )
                    path = "../.." + path;
                else
                    path = "." + path;

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
