using System;

namespace Platinum.Mock
{
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
    public class MockDataAttribute : Attribute
    {
        public MockDataAttribute( string set )
        {
            #region Validations

            if ( set == null )
                throw new ArgumentNullException( nameof( set ) );

            #endregion

            this.Set = set;
        }


        public string Set
        {
            get;
            private set;
        }


        public int LikelihoodNull
        {
            get;
            set;
        } = 0;
    }
}
