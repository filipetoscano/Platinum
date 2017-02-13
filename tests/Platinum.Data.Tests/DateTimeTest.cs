using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Platinum.Data.Tests
{
    [TestClass]
    public class DateTimeTest
    {
        [TestInitialize]
        public void Initialize()
        {
            DataConfig.Register();
        }


        /// <summary>
        /// Are scalar DateTime corrected?
        /// </summary>
        [TestMethod]
        public async Task ExecuteScalar()
        {
            DataConnection conn = Db.Connection( "TestDb" );

            DateTime dt = await conn.ExecuteScalarAsync<DateTime>( " select getutcdate() " );

            Assert.IsTrue( dt.Kind == DateTimeKind.Utc );
        }


        /// <summary>
        /// Are DateTime in tables corrected?
        /// </summary>
        [TestMethod]
        public async Task QueryFirst()
        {
            DataConnection conn = Db.Connection( "TestDb" );

            DateTimeTestRow row = await conn.QueryFirstAsync<DateTimeTestRow>( " select 'Hello' as World, getutcdate() as DateTime " );

            Assert.IsTrue( row != null );
            Assert.IsTrue( row.World == "Hello" );
            Assert.IsTrue( row.DateTime.Kind == DateTimeKind.Utc );
        }


        /// <summary>
        /// Are nullable DateTime in tables corrected?
        /// </summary>
        [TestMethod]
        public async Task NullableQueryFirst1()
        {
            DataConnection conn = Db.Connection( "TestDb" );

            var row = await conn.QueryFirstAsync<NullableDateTimeTestRow>( " select 'Hello' as World, getutcdate() as DateTime " );

            Assert.IsTrue( row != null );
            Assert.IsTrue( row.World == "Hello" );
            Assert.IsTrue( row.DateTime.HasValue == true );
            Assert.IsTrue( row.DateTime.Value.Kind == DateTimeKind.Utc );
        }


        /// <summary>
        /// Are nullable DateTime in tables corrected?
        /// </summary>
        [TestMethod]
        public async Task NullableQueryFirst2()
        {
            DataConnection conn = Db.Connection( "TestDb" );

            var row = await conn.QueryFirstAsync<NullableDateTimeTestRow>( " select 'Hello' as World, null as DateTime " );

            Assert.IsTrue( row != null );
            Assert.IsTrue( row.World == "Hello" );
            Assert.IsTrue( row.DateTime.HasValue == false );
        }


        [TestMethod]
        public async Task Query()
        {
            DataConnection conn = Db.Connection( "TestDb" );

            var rows = await conn.QueryAsync<DateTimeTestRow>( " select 'Hello' as World, getutcdate() as DateTime " );
            var rowList = rows.ToList();

            Assert.IsTrue( rowList != null );
            Assert.IsTrue( rowList.Count() == 1 );
            Assert.IsTrue( rowList[ 0 ].World == "Hello" );
            Assert.IsTrue( rowList[ 0 ].DateTime.Kind == DateTimeKind.Utc );
        }


        public class DateTimeTestRow
        {
            public string World { get; set; }
            public DateTime DateTime { get; set; }
        }


        public class NullableDateTimeTestRow
        {
            public string World { get; set; }
            public DateTime? DateTime { get; set; }
        }
    }
}
