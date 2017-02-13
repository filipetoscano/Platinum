using Dapper;
using Platinum.Data.TypeHandler;

namespace Platinum.Data
{
    /// <summary />
    public static class DataConfig
    {
        /// <summary />
        public static void Register()
        {
            SqlMapper.AddTypeHandler( new DateTimeHandler() );
            SqlMapper.AddTypeHandler( new DurationHandler() );
        }
    }
}
