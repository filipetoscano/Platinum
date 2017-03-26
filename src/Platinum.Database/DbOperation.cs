using System;

namespace Platinum.Database
{
    /// <summary>
    /// Describes the list of operations which will be performed on the database.
    /// </summary>
    [Flags]
    public enum DbOperation
    {
        /// <summary>
        /// Don't run any operation on database.
        /// </summary>
        None = 0,

        /// <summary>
        /// Whether to reset the database, dropping all user objects.
        /// </summary>
        Reset = 1024,

        /// <summary>
        /// Whether to sequentially run the schema objects.
        /// </summary>
        Schema = 1,

        /// <summary>
        /// Whether to run the data files.
        /// </summary>
        Data = 2,
    }
}
