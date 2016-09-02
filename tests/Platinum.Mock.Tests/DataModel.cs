using System;
using System.Xml.Serialization;

namespace Platinum.Mock.Tests
{
    /// <summary>
    /// Basic .NET types.
    /// </summary>
    public class BasicDataClass
    {
        [MockData( "type/bool" )]
        public bool BooleanProperty { get; set; }

        [MockData( "type/byte" )]
        public byte ByteProperty { get; set; }

        [MockData( "type/short" )]
        public short ShortProperty { get; set; }

        [MockData( "type/integer" )]
        public int IntegerProperty { get; set; }

        [MockData( "type/long" )]
        public long LongProperty { get; set; }

        [MockData( "type/single" )]
        public float SingleProperty { get; set; }

        [MockData( "type/double" )]
        public double DoubleProperty { get; set; }

        [MockData( "type/decimal" )]
        public decimal DecimalProperty { get; set; }

        [MockData( "type/dateTime" )]
        public DateTime DateTimeProperty { get; set; }

        [MockData( "type/date" )]
        [XmlElement( DataType = "date" )]
        public DateTime DateProperty { get; set; }

        [MockData( "type/time" )]
        [XmlElement( DataType = "time" )]
        public DateTime TimeProperty { get; set; }

        [MockData( "type/char" )]
        public char CharProperty { get; set; }

        [MockData( "type/string" )]
        public string StringProperty { get; set; }

        [MockData( "type/enum" )]
        public EnumEnumerate EnumProperty { get; set; }

        [MockData( "type/enum" )]
        public FlagEnumerate FlagProperty { get; set; }
    }


    /// <summary>
    /// Basic .NET types.
    /// </summary>
    public class MatrixCharacter
    {
        [MockData( "matrix/name" )]
        public string Name { get; set; }

        [MockData( "matrix/thisDecade" )]
        [XmlElement( DataType = "date" )]
        public DateTime DateOfBirth { get; set; }
    }
}
