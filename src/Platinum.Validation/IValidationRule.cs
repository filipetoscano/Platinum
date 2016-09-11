namespace Platinum.Validation
{
    public interface IValidationRule
    {
        void Validate( ValidationContext context, ValidationResult result, object value );
    }
}
