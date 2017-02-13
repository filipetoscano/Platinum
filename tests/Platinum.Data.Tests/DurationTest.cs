using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Platinum.Data.Tests
{
    [TestClass]
    public class DurationTest
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
            Duration @in = new Duration();
            @in.Days = 10;

            DataConnection conn = Db.Connection( "TestDb" );

            Duration d = await conn.ExecuteScalarAsync<Duration>( " select @Value as Scalar ", new
            {
                Value = @in
            } );

            Assert.AreEqual( d.Days, @in.Days );
        }


        /// <summary>
        /// Are DateTime in tables corrected?
        /// </summary>
        [TestMethod]
        public async Task QueryFirst()
        {
            Duration @in = new Duration();
            @in.Days = 11;

            DataConnection conn = Db.Connection( "TestDb" );

            var row = await conn.QueryFirstAsync<DurationTestRow>( " select 'Hello' as World, @Value as Duration ", new
            {
                Value = @in
            } );

            Assert.IsTrue( row != null );
            Assert.AreEqual( row.Duration.Days, @in.Days );
        }


        /// <summary>
        /// Are nullable DateTime in tables corrected?
        /// </summary>
        [TestMethod]
        public async Task NullableQueryFirst1()
        {
            Duration @in = new Duration();
            @in.Days = 12;

            DataConnection conn = Db.Connection( "TestDb" );

            var row = await conn.QueryFirstAsync<NullableDurationTimeTestRow>( " select 'Hello' as World, @Value as Duration ", new
            {
                Value = @in
            } );

            Assert.IsTrue( row != null );
            Assert.AreEqual( row.Duration.HasValue, true );
            Assert.AreEqual( row.Duration.Value.Days, @in.Days );
        }


        /// <summary>
        /// Are nullable DateTime in tables corrected?
        /// </summary>
        [TestMethod]
        public async Task NullableQueryFirst2()
        {
            Duration? @in = null;

            DataConnection conn = Db.Connection( "TestDb" );

            var row = await conn.QueryFirstAsync<NullableDurationTimeTestRow>( " select 'Hello' as World, @Value as Duration ", new
            {
                Value = @in
            } );

            Assert.IsTrue( row != null );
            Assert.AreEqual( row.Duration.HasValue, false );
        }


        [TestMethod]
        public async Task Query()
        {
            Duration @in = new Duration();
            @in.Days = 13;

            DataConnection conn = Db.Connection( "TestDb" );

            var rows = await conn.QueryAsync<DurationTestRow>( " select 'Hello' as World, @Value as Duration ", new
            {
                Value = @in
            } );
            var rowList = rows.ToList();

            Assert.IsTrue( rowList != null );
            Assert.AreEqual( rowList.Count(), 1 );
            Assert.AreEqual( rowList[ 0 ].Duration.Days, @in.Days );
        }


        public class DurationTestRow
        {
            public Duration Duration { get; set; }
        }


        public class NullableDurationTimeTestRow
        {
            public Duration? Duration { get; set; }
        }
    }
}
