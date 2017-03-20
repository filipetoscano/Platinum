using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Threading.Tasks;
using Q = Platinum.Data.Tests.Statements.Sql;

namespace Platinum.Data.Tests
{
    [TestClass]
    public class NoServerTest
    {
        [TestMethod]
        public async Task QuerySingleAsync()
        {
            DataConnection conn = Db.Connection( "NoServerDb" );

            try
            {
                var x = await conn.QuerySingleAsync<dynamic>( Q.Single, new { } );

                Assert.Fail( "Expected exception" );
            }
            catch ( DbException )
            {
                Assert.Fail( "Expected DataException" );
            }
            catch ( DataException ex )
            {
                Assert.AreEqual( ex.Message, ER.Open_ConnectFailed );
            }
        }


        [TestMethod]
        public async Task QueryAsync()
        {
            DbConnection conn = Db.Connection( "NoServerDb" );

            try
            {
                var x = await conn.QueryAsync<dynamic>( Q.Query, new { } );
            }
            catch ( DbException )
            {
                Assert.Fail( "Expected DataException" );
            }
            catch ( DataException ex )
            {
                Assert.AreEqual( ex.Message, ER.Open_ConnectFailed );
            }
        }
    }
}
