using System;
using System.Xml.Serialization;

namespace Platinum.Mock.Tests
{
    public enum EnumEnumerate
    {
        Value1,
        Value2,
        Value3,
        Value4,
    }


    [Flags]
    public enum FlagEnumerate
    {
        Value1 = 1,
        Value2 = 2,
        Value3 = 4,
        Value4 = 8,
    }


    /// <summary>
    /// Basic .NET types.
    /// </summary>
    public class BasicClass
    {
        public bool BooleanProperty { get; set; }
        public byte ByteProperty { get; set; }

        public short ShortProperty { get; set; }
        public int IntegerProperty { get; set; }
        public long LongProperty { get; set; }

        public float SingleProperty { get; set; }
        public double DoubleProperty { get; set; }
        public decimal DecimalProperty { get; set; }

        public DateTime DateTimeProperty { get; set; }

        public char CharProperty { get; set; }
        public string StringProperty { get; set; }

        public EnumEnumerate EnumProperty { get; set; }
        public FlagEnumerate FlagProperty { get; set; }
    }


    /// <summary>
    /// Variations of the DateTime.
    /// </summary>
    public class DateAndtimeClass
    {
        public DateTime DateTimeProperty { get; set; }

        [XmlElement( DataType = "date" )]
        public DateTime DateProperty { get; set; }

        [XmlElement( DataType = "time" )]
        public DateTime TimeProperty { get; set; }
    }


    /// <summary>
    /// Nullable basic .NET types.
    /// </summary>
    public class BasicNullableClass
    {
        public bool? BooleanProperty { get; set; }
        public byte? ByteProperty { get; set; }

        public short? ShortProperty { get; set; }
        public int? IntegerProperty { get; set; }
        public long? LongProperty { get; set; }

        public float? SingleProperty { get; set; }
        public double? DoubleProperty { get; set; }
        public decimal? DecimalProperty { get; set; }

        public DateTime? DateTimeProperty { get; set; }

        public char? CharProperty { get; set; }

        public EnumEnumerate? EnumProperty { get; set; }
        public FlagEnumerate? FlagProperty { get; set; }
    }


    /// <summary>
    /// Arrays of .NET types.
    /// </summary>
    public class BasicArrayClass
    {
        public bool[] BooleanProperty { get; set; }

        public short[] ShortProperty { get; set; }
        public int[] IntegerProperty { get; set; }
        public long[] LongProperty { get; set; }

        public float[] SingleProperty { get; set; }
        public double[] DoubleProperty { get; set; }
        public decimal[] DecimalProperty { get; set; }

        public DateTime[] DateTimeProperty { get; set; }

        public char[] CharProperty { get; set; }
        public string[] StringProperty { get; set; }

        public EnumEnumerate[] EnumProperty { get; set; }
        public FlagEnumerate[] FlagProperty { get; set; }
    }


    public class ChildClass
    {
        public string ChildProperty { get; set; }
    }


    public class NestedClass
    {
        public ChildClass Child { get; set; }
    }


    public class NestedArrayClass
    {
        public ChildClass[] Child { get; set; }
    }


    public class BridgeClass
    {
        public BridgeClass Recursive { get; set; }
    }


    public class RecursiveNestedClass
    {
        public string RecursiveProperty { get; set; }
        public BridgeClass Recursive { get; set; }
    }


    public class BridgeOneClass
    {
        public BridgeTwoClass Two { get; set; }
    }


    public class BridgeTwoClass
    {
        public BridgeOneClass One { get; set; }
    }


    public class Recursive2NestedClass
    {
        public BridgeOneClass One { get; set; }
    }
}
