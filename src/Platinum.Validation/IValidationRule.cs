namespace Platinum.Validation
{
    /// <summary />
    public interface IValidationRule
    {
        /// <summary />
        void Validate( ValidationContext context, ValidationResult result, object value );
    }
}
