using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Platinum.Data.Tests
{
    [TestClass]
    public class BasicTest
    {
        [TestMethod]
        public async Task QuerySingleAsync()
        {
            DataConnection conn = Db.Connection( "TestDb" );

            var row = await conn.QuerySingleAsync<dynamic>( Q.Single, new { } );

            Assert.IsNotNull( row );
            Assert.AreEqual( 1, row.Column1 );
            Assert.AreEqual( "hello", row.Column2 );
        }


        [TestMethod]
        public async Task QueryAsync()
        {
            DbConnection conn = Db.Connection( "TestDb" );

            var rows = await conn.QueryAsync<dynamic>( Q.Query, new { } );

            Assert.IsNotNull( rows );
            Assert.AreEqual( 2, rows.Count() );

            var r1 = rows.First();
            Assert.AreEqual( 1, r1.Column1 );
            Assert.AreEqual( "hello", r1.Column2 );

            var r2 = rows.Skip( 1 ).First();
            Assert.AreEqual( 2, r2.Column1 );
            Assert.AreEqual( "world", r2.Column2 );
        }


        [TestMethod]
        public async Task QueryMultipleAsync()
        {
            DbConnection conn = null;
            DbTransaction tx = null;

            try
            {
                conn = Db.Connection( "TestDb" );
                await conn.OpenAsync();

                tx = conn.BeginTransaction( IsolationLevel.RepeatableRead );

                using ( var rows = await conn.QueryMultipleAsync( Q.Multi1, new
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


                using ( var rows = await conn.QueryMultipleAsync( Q.Multi2, new
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
    }
}
