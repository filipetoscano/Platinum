using Platinum.Resolver;
using System;

namespace Platinum.Core.Tests.Resolver
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
