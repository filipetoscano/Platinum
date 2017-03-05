using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Platinum.Core.Tests
{
    /// <summary>
    /// As per: https://docs.oracle.com/javase/7/docs/api/java/math/RoundingMode.html
    /// </summary>
    [TestClass]
    public class MathTests
    {
        private static List<decimal> input = new List<decimal>()
        {
            0.55m, 0.25m, 0.16m, 0.11m, 0.10m,
            -0.10m, -0.11m, -0.16m, -0.25m, -0.55m,
        };


        /// <summary>
        /// RoundingMode.Up.
        /// </summary>
        [TestMethod]
        public void Up()
        {
            List<decimal> expected = new List<decimal>()
            {
                0.6m, 0.3m, 0.2m, 0.2m, 0.1m,
                -0.1m, -0.2m, -0.2m, -0.3m, -0.6m
            };

            RunSequence( expected, RoundingMode.Up );
        }


        /// <summary>
        /// RoundingMode.Down.
        /// </summary>
        [TestMethod]
        public void Down()
        {
            List<decimal> expected = new List<decimal>()
            {
                0.5m, 0.2m, 0.1m, 0.1m, 0.1m,
                -0.1m, -0.1m, -0.1m, -0.2m, -0.5m
            };

            RunSequence( expected, RoundingMode.Down );
        }


        /// <summary>
        /// RoundingMode.Ceiling.
        /// </summary>
        [TestMethod]
        public void Ceiling()
        {
            List<decimal> expected = new List<decimal>()
            {
                0.6m, 0.3m, 0.2m, 0.2m, 0.1m,
                -0.1m, -0.1m, -0.1m, -0.2m, -0.5m
            };

            RunSequence( expected, RoundingMode.Ceiling );
        }


        /// <summary>
        /// RoundingMode.Floor.
        /// </summary>
        [TestMethod]
        public void Floor()
        {
            List<decimal> expected = new List<decimal>()
            {
                0.5m, 0.2m, 0.1m, 0.1m, 0.1m,
                -0.1m, -0.2m, -0.2m, -0.3m, -0.6m
            };

            RunSequence( expected, RoundingMode.Floor );
        }


        /// <summary>
        /// RoundingMode.HalfUp.
        /// </summary>
        [TestMethod]
        public void HalfUp()
        {
            List<decimal> expected = new List<decimal>()
            {
                0.6m, 0.3m, 0.2m, 0.1m, 0.1m,
                -0.1m, -0.1m, -0.2m, -0.3m, -0.6m
            };

            RunSequence( expected, RoundingMode.HalfUp );
        }


        /// <summary>
        /// RoundingMode.Down.
        /// </summary>
        [TestMethod]
        public void HalfDown()
        {
            List<decimal> expected = new List<decimal>()
            {
                0.5m, 0.2m, 0.2m, 0.1m, 0.1m,
                -0.1m, -0.1m, -0.2m, -0.2m, -0.5m
            };

            RunSequence( expected, RoundingMode.HalfDown );

            // Extra
            Assert.AreEqual( 0.55m, Math.Round( 0.5550m, 2, RoundingMode.HalfDown ) );
            Assert.AreEqual( 0.555m, Math.Round( 0.555500m, 3, RoundingMode.HalfDown ) );
        }


        /// <summary>
        /// RoundingMode.HalfEven.
        /// </summary>
        [TestMethod]
        public void HalfEven()
        {
            List<decimal> expected = new List<decimal>()
            {
                0.6m, 0.2m, 0.2m, 0.1m, 0.1m,
                -0.1m, -0.1m, -0.2m, -0.2m, -0.6m
            };

            RunSequence( expected, RoundingMode.HalfEven );
        }


        /// <summary />
        private static void RunSequence( List<decimal> expected, RoundingMode mode )
        {
            List<decimal> actual = new List<decimal>();

            for ( int i = 0; i < input.Count; i++ )
                actual.Add( Math.Round( input[ i ], 1, mode ) );

            for ( int i = 0; i < expected.Count; i++ )
                Assert.AreEqual( expected[ i ], actual[ i ], $"{ i }th value is incorrect." );
        }
    }
}
