using System.Collections.Generic;

namespace Platinum.Mock
{
    public interface IMockFunction
    {
        object Random( Dictionary<string,string> settings );
    }
}
