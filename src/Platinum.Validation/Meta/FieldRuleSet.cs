using System.Collections.Generic;

namespace Platinum.Validation
{
    /// <summary>
    /// Enumeration of rules which apply to a field.
    /// </summary>
    public class FieldRuleSet
    {
        /// <summary>
        /// Condition which needs to be met, in order for rule set to apply.
        /// </summary>
        public ICondition Condition
        {
            get;
            set;
        }


        /// <summary>
        /// List of validation rules.
        /// </summary>
        public IEnumerable<IValidationRule> Rules
        {
            get;
            set;
        }
    }
}
