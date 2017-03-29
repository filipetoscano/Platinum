using System.Collections.Generic;

namespace Platinum.Validation
{
    /// <summary>
    /// Rule set: list of validation rules for a given object.
    /// </summary>
    public interface IValidationRuleSet
    {
        /// <summary>
        /// List of field rules.
        /// </summary>
        IEnumerable<FieldRule> Fields { get; }
    }
}
