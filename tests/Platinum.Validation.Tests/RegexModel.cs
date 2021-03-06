﻿namespace Platinum.Validation.Tests
{
    public enum RegexEnum
    {
        One,
        Two,
        Three,
        Four,
        Five,
    }


    public class RegexClass
    {
        [RegularExpression( @"^\d+$" )]
        public string Value { get; set; }

        [RegularExpression( @"^(One|Two)$" )]
        public RegexEnum? Enum { get; set; }
    }


    public class RegexBrokenClass
    {
        [RegularExpression( @"^\X+$" )]
        public string Value { get; set; }
    }
}
