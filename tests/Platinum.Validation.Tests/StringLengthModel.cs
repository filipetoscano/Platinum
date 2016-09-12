using System;

namespace Platinum.Validation.Tests
{
    public class StringMinLengthClass
    {
        [MinLength( 5 )]
        public string Value { get; set; }
    }


    public class StringMaxLengthClass
    {
        [MaxLength( 10 )]
        public string Value { get; set; }
    }


    public class StringLengthClass
    {
        [StringLength( 5, 10 )]
        public string Value { get; set; }
    }
}
