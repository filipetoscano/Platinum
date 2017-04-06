namespace Platinum.Validation.Tests
{
    public class RegexClass
    {
        [RegularExpression( @"^\d+$" )]
        public string Value { get; set; }
    }


    public class RegexBrokenClass
    {
        [RegularExpression( @"^\X+$" )]
        public string Value { get; set; }
    }
}
