using System;

namespace Platinum.Mock
{
    /// <summary>
    /// Markup, indicating that a property is subject to random
    /// data being generated
    /// </summary>
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
    public class MockDataAttribute : Attribute
    {
        /// <summary>
        /// Marks a property as having mock data from the given data set or
        /// function.
        /// </summary>
        /// <param name="name">
        /// Name of mock data set or function name.
        /// </param>
        public MockDataAttribute( string name )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion

            this.Name = name;
        }


        /// <summary>
        /// Gets the name of the mock data set or function name.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets the likelihood, in percentage, that the attributed property will have
        /// a 'null' value.
        /// </summary>
        /// <remarks>
        /// The valid range is 0..100. Any other value will be ignored.
        /// </remarks>
        public int LikelihoodNull
        {
            get;
            set;
        } = 0;
    }
}
