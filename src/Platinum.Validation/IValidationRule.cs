namespace Platinum.Validation
{
    /// <summary>
    /// Property value validation rule.
    /// </summary>
    public interface IValidationRule
    {
        /// <summary>
        /// Validates a property.
        /// </summary>
        void Validate( ValidationContext context, ValidationResult result, object value );
    }
}
