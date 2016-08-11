using System.Collections.Specialized;

namespace Platinum.Mock
{
    public interface IMockFunction
    {
        object Random( NameValueCollection settings );
    }
}
