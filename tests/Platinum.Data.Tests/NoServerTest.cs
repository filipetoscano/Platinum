using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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
                var x = await conn.QuerySingleAsync<dynamic>( Db.Sql( "Sql/Single" ), new { } );

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
                var x = await conn.QueryAsync<dynamic>( Db.Sql( "Sql/Query" ), new { } );
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



        public async Task QueryMultipleAsync()
        {
            DbConnection conn = null;
            DbTransaction tx = null;

            try
            {
                conn = Db.Connection( "TestDb" );
                await conn.OpenAsync();

                tx = conn.BeginTransaction( IsolationLevel.RepeatableRead );

                using ( var rows = await conn.QueryMultipleAsync( Db.Sql( "Sql/Multi1" ), new
                {
                    a = 1,
                    b = 2
                }, tx ) )
                {
                    dynamic head;
                    List<dynamic> body;
                    List<dynamic> more = null;

                    head = rows.Read().Single();
                    body = rows.Read().ToList();

                    if ( rows.IsConsumed == false )
                        more = rows.Read().ToList();
                }


                using ( var rows = await conn.QueryMultipleAsync( Db.Sql( "Sql/Multi2" ), new
                {
                    a = 1,
                    b = 2
                }, tx ) )
                {
                    dynamic head;
                    List<dynamic> body;
                    List<dynamic> more = null;

                    head = rows.Read().Single();
                    body = rows.Read().ToList();

                    if ( rows.IsConsumed == false )
                        more = rows.Read().ToList();
                }

                tx.Commit();
            }
            catch
            {
                tx?.Rollback();
                throw;
            }
            finally
            {
                conn?.Close();
            }
        }


        [TestMethod]
        public async Task QueryMultipleAsync5()
        {
            for ( int i = 0; i < 5; i++ )
                await QueryMultipleAsync();
        }
    }
}
