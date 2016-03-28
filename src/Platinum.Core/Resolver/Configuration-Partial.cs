using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Resolver
{
    public partial class ResolverDefinition
    {
        internal IUrlResolver ResolverInstance
        {
            get;
            set;
        }
    }
}

/* eof */