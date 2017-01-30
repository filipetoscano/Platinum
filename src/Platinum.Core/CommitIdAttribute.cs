using System;

namespace Platinum
{
    /// <summary>
    /// Used to mark an assembly as having been compiled from a specific version.
    /// </summary>
    [AttributeUsage( AttributeTargets.Assembly )]
    public sealed class CommitIdAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommitIdAttribute" />, marking
        /// an assembly as having been built from the given commit.
        /// </summary>
        /// <param name="commitId">Commit identifier.</param>
        public CommitIdAttribute( string commitId )
        {
            #region Validations

            if ( commitId == null )
                throw new ArgumentNullException( nameof( commitId ) );

            #endregion

            this.CommitId = commitId;
        }


        /// <summary>
        /// Gets the Git commit identifier.
        /// </summary>
        public string CommitId
        {
            get;
            private set;
        }
    }
}
