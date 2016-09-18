using System;

namespace Platinum.Core.Tests
{
    public enum TestEnum
    {
        Value1,
        Value2,
        Value3,
        Value4,
    }


    [Flags]
    public enum TestFlagsEnum
    {
        None = 0,

        Value1 = 1,

        Value2 = 2,

        Value3 = 4,

        Value4 = 8,
    }
}
