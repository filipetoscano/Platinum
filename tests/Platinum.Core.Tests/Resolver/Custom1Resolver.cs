using Platinum.Resolver;
using System;
using System.IO;
using System.Reflection;

namespace Platinum.Core.Tests.Resolver
{
    public class Custom1Resolver : IUrlResolver
    {
        public Uri ResolveUri( Uri baseUri, string relativeUri )
        {
            if ( baseUri == null )
            {
                string dir = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
                string file = Path.Combine( dir, "..\\..\\Resolver\\hello.xslt" );

                return new Uri( file, UriKind.Absolute );
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
