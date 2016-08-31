using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                string prefix;

                /*
                 * Rather ugly, we know. :/
                 * 
                 * When unit tests are invoked by MSTest, then the DLL (and
                 * only the DLL) are copied over under TestResults -- and the
                 * tests are run there.
                 * 
                 * When unit tests are run through Visual Studio, than they
                 * stay where they are.
                 */
                if ( Assembly.GetExecutingAssembly().Location.Contains( @"\TestResults\" ) == true )
                    prefix = @"..\..\..\tests\Platinum.Core.Tests\";
                else
                    prefix = @"..\..\";

                string dir = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
                string file = Path.Combine( dir, prefix + "Resolver\\hello.xslt" );

                return new Uri( file, UriKind.Absolute );
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
