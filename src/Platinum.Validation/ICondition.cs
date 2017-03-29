namespace Platinum.Validation
{
    /// <summary>
    /// Condition under which a (field) rule set is applicable.
    /// </summary>
    public interface ICondition
    {
        /// <summary>
        /// Checks whether the rule set is conditionally applicable.
        /// </summary>
        /// <param name="obj">
        /// Object which will be used, to determine condition.
        /// </param>
        /// <returns>
        /// True if the rule set is to be run, False otherwise.
        /// </returns>
        bool IsTrue( object obj );
    }
}
