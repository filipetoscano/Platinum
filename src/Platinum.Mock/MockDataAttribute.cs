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
        public MockDataAttribute( string name )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion

            this.Name = name;
        }


        /// <summary>
        /// Gets the name of the mock data set or function.
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
