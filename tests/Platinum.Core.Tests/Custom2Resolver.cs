using Platinum.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Core.Tests
{
    public class Custom2Resolver : IUrlResolver
    {
        public Uri ResolveUri( Uri baseUri, string relativeUri )
        {
            if ( baseUri == null )
            {
                Uri ruri = new Uri( relativeUri );
                string uriString = "assembly:///Platinum.Core.Tests/~/Resolver" + ruri.AbsolutePath;

                return new Uri( uriString );
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
