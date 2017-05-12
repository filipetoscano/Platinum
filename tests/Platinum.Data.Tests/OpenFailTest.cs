using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Threading.Tasks;
using Q = Platinum.Data.Tests.Statements.Sql;

namespace Platinum.Data.Tests
{
    [TestClass]
    public class OpenFailTest
    {
        [TestMethod]
        public async Task InvalidUserId()
        {
            DataConnection conn = Db.Connection( "InvalidUserIdDb" );

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
                Assert.AreEqual( ER.Open_LoginFailed, ex.Message );
                Assert.IsTrue( ex.Data[ "DbState" ] != null );

                byte dbState = (byte) ex.Data[ "DbState" ];
                Assert.IsTrue( dbState > 0 );
            }
        }


        [TestMethod]
        public async Task InvalidPassword()
        {
            DataConnection conn = Db.Connection( "InvalidPasswordDb" );

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
                Assert.AreEqual( ER.Open_LoginFailed, ex.Message );
                Assert.IsTrue( ex.Data[ "DbState" ] != null );

                byte dbState = (byte) ex.Data[ "DbState" ];
                Assert.IsTrue( dbState > 0 );
            }
        }
    }
}
