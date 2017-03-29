namespace Platinum.Validation
{
    /// <summary>
    /// Condition which is always true.
    /// </summary>
    public class TrueCondition : ICondition
    {
        /// <summary>
        /// Always returns true.
        /// </summary>
        /// <param name="obj">
        /// Instance of object.
        /// </param>
        /// <returns>
        /// Always true.
        /// </returns>
        public bool IsTrue( object obj )
        {
            return true;
        }
    }
}
