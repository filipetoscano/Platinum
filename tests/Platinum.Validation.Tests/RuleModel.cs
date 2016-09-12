namespace Platinum.Validation.Tests
{
    public class ValidateClass1
    {
        [Required]
        [ValidationRule( "person/name" )]
        public string Name { get; set; }
    }
}
