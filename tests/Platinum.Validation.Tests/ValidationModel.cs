using System;

namespace Platinum.Validation.Tests
{
    public enum TheEnum
    {
        Value1,
        Value2,
        Value3,
    }


    public class RequiredClass
    {
        [Required]
        public string String { get; set; }

        [Required]
        public TheEnum? Enum { get; set; }

        [Required]
        public int? Int { get; set; }

        [Required]
        public DateTime? DateTime { get; set; }
    }


    public class StringLengthClass
    {
        [StringLength( 5, 20 )]
        public string OnlyMin { get; set; }
    }
}
