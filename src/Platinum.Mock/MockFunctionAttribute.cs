using System;

namespace Platinum.Mock
{
    [AttributeUsage( AttributeTargets.Class )]
    public class MockFunctionAttribute : Attribute
    {
        public MockFunctionAttribute( string name )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion

            this.Name = name;
        }


        public string Name
        {
            get;
            private set;
        }
    }
}
