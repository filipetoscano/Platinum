using System.Collections.Generic;

namespace Platinum.Validation
{
    /// <summary>
    /// Describes rules that apply to a given field.
    /// </summary>
    public class FieldRule
    {
        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public string Name { get; set; }

        
        /// <summary>
        /// Get the ordered sequence of 'rule sets' which (conditionally)
        /// apply to this field. The first one which returns True gets
        /// run: others are ignored.
        /// </summary>
        public IEnumerable<FieldRuleSet> RuleSets { get; set; }
    }
}
