using System;

namespace Platinum.Reflection
{
    /// <summary />
    public static class AppDomainExtensions
    {
        /// <summary />
        public static void PreLoad( this AppDomain domain, bool isWebApplication = true )
        {
            AppDomainUtils.PreLoad( isWebApplication );
        }
    }
}
