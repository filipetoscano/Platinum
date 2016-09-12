namespace Platinum.Validation.Tests
{
    public class MinValueClass1
    {
        [MinValue( "5" )]
        public int Value { get; set; }
    }


    public class MinValueClass2
    {
        [MinValue( "5", IsExclusive = true )]
        public int Value { get; set; }
    }


    public class MinValueClass3
    {
        [MinValue( "5" )]
        public int? Value { get; set; }
    }
}
