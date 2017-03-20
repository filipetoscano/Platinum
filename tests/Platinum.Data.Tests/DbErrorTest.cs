using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Threading.Tasks;
using Q = Platinum.Data.Tests.Statements.Sql;

namespace Platinum.Data.Tests
{
    [TestClass]
    public class DbErrorTest
    {
        /// <summary>
        /// Does ExecuteAsync (aka: ExecuteNonQuery) do custom error handling?
        /// </summary>
        [TestMethod]
        public async Task ExecuteAsync()
        {
            try
            {
                DataConnection conn = Db.Connection( "TestDb" );

                await conn.ExecuteAsync( Q.DbError, new { } );

                Assert.Fail( "Expected exception" );
            }
            catch ( DbException )
            {
                Assert.Fail( "Expected ActorException" );
            }
            catch ( ActorException ex )
            {
                if ( ex.Message == ER.ExecuteNonQuery )
                {
                    Assert.Fail( "Expected custom error from DB" );
                }
                else
                {
                    Assert.IsTrue( ex.Actor != null );
                    Assert.IsTrue( ex.Code > 0 );
                    Assert.IsTrue( ex.Description != null );
                    Assert.IsTrue( ex.Message != null );
                }
            }
        }


        /// <summary>
        /// Does ExecuteScalarAsync do custom error handling?
        /// </summary>
        [TestMethod]
        public async Task ExecuteScalarAsync()
        {
            try
            {
                DataConnection conn = Db.Connection( "TestDb" );

                await conn.ExecuteScalarAsync<int>( Q.DbError, new { } );

                Assert.Fail( "Expected exception" );
            }
            catch ( DbException )
            {
                Assert.Fail( "Expected ActorException" );
            }
            catch ( ActorException ex )
            {
                if ( ex.Message == ER.ExecuteScalar )
                {
                    Assert.Fail( "Expected custom error from DB" );
                }
                else
                {
                    Assert.IsTrue( ex.Actor != null );
                    Assert.IsTrue( ex.Code > 0 );
                    Assert.IsTrue( ex.Description != null );
                    Assert.IsTrue( ex.Message != null );
                }
            }
        }


        /// <summary>
        /// Does QueryAsync do custom error handling?
        /// </summary>
        [TestMethod]
        public async Task QueryAsync()
        {
            try
            {
                DataConnection conn = Db.Connection( "TestDb" );

                var rows = await conn.QueryAsync<dynamic>( Q.DbError, new { } );

                Assert.Fail( "Expected exception" );
            }
            catch ( DbException )
            {
                Assert.Fail( "Expected ActorException" );
            }
            catch ( ActorException ex )
            {
                if ( ex.Message == ER.ExecuteDbDataReader )
                {
                    Assert.Fail( "Expected custom error from DB" );
                }
                else
                {
                    Assert.IsTrue( ex.Actor != null );
                    Assert.IsTrue( ex.Code > 0 );
                    Assert.IsTrue( ex.Description != null );
                    Assert.IsTrue( ex.Message != null );
                }
            }
        }
    }
}
