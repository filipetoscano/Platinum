using System.Collections.Generic;

namespace Platinum.Mock.DataLoader
{
    public interface IDataLoader
    {
        Data Load( Dictionary<string, string> settings );
    }
}
