using System.Collections.Generic;

namespace Platinum.Mock.DataLoader
{
    /// <summary />
    public interface IDataLoader
    {
        /// <summary />
        Data Load( Dictionary<string, string> settings );
    }
}
