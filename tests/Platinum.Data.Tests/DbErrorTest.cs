using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Threading.Tasks;
using Q = Platinum.Data.Tests.Statements.Sql;
using DER = Platinum.Data.ER;
using System;

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
            catch ( ActorException ex )
            {
                Assert.AreEqual( ER.Sql_DbError_ErrorPlain, ex.Message );
                Assert.AreEqual( App.Name, ex.Actor );
                Assert.AreEqual( 1001, ex.Code );
                Assert.AreEqual( 0, ex.Data.Keys.Count );
            }
            catch ( Exception )
            {
                Assert.Fail( "Expected actor exception" );
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
            catch ( ActorException ex )
            {
                Assert.AreEqual( ER.Sql_DbError_ErrorPlain, ex.Message );
                Assert.AreEqual( App.Name, ex.Actor );
                Assert.AreEqual( 1001, ex.Code );
                Assert.AreEqual( 0, ex.Data.Keys.Count );
            }
            catch ( Exception )
            {
                Assert.Fail( "Expected actor exception" );
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
            catch ( ActorException ex )
            {
                Assert.AreEqual( ER.Sql_DbError_ErrorPlain, ex.Message );
                Assert.AreEqual( App.Name, ex.Actor );
                Assert.AreEqual( 1001, ex.Code );
                Assert.AreEqual( 0, ex.Data.Keys.Count );
            }
            catch ( Exception )
            {
                Assert.Fail( "Expected actor exception" );
            }
        }


        /// <summary>
        /// What if the exception has argument placeholders?
        /// </summary>
        [TestMethod]
        public async Task QueryAsyncWithArgs()
        {
            try
            {
                DataConnection conn = Db.Connection( "TestDb" );

                var rows = await conn.QueryAsync<dynamic>( Q.DbErrorArg, new { } );

                Assert.Fail( "Expected exception" );
            }
            catch ( ActorException ex )
            {
                Assert.AreEqual( ER.Sql_DbErrorArg_ErrorWithArgs, ex.Message );
                Assert.AreEqual( App.Name, ex.Actor );
                Assert.AreEqual( 1002, ex.Code );
                Assert.AreEqual( 2, ex.Data.Keys.Count );
                Assert.AreEqual( "32", ex.Data[ "Arg2" ] );
            }
            catch ( Exception )
            {
                Assert.Fail( "Expected actor exception" );
            }
        }


        /// <summary>
        /// What if the exception isn't declared?
        /// </summary>
        [TestMethod]
        public async Task QueryAsyncMissingError()
        {
            try
            {
                DataConnection conn = Db.Connection( "TestDb" );

                var rows = await conn.QueryAsync<dynamic>( Q.DbErrorMissingError, new { } );

                Assert.Fail( "Expected exception" );
            }
            catch ( ActorException ex )
            {
                Assert.AreEqual( "Sql_DbErrorMissingError_MissingError", ex.Message );
                Assert.AreEqual( 1100, ex.Code );
                Assert.AreEqual( "Platinum.Data.Tests.DataDbException#Impl", ex.Actor );
            }
            catch ( Exception )
            {
                Assert.Fail( "Expected actor exception" );
            }
        }
    }
}
