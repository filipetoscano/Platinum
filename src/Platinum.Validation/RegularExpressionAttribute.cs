using System;

namespace Platinum.Validation
{
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class RegularExpressionAttribute : Attribute, IValidationRule
    {
        public RegularExpressionAttribute( string pattern )
        {
            #region Validations

            if ( pattern == null )
                throw new ArgumentNullException( nameof( pattern ) );

            #endregion

            this.Pattern = pattern;
        }


        public string Pattern
        {
            get;
            private set;
        }


        public void Validate( ValidationContext context, ValidationResult result, object value )
        {
            #region Validations

            if ( context == null )
                throw new ArgumentNullException( nameof( context ) );

            if ( result == null )
                throw new ArgumentNullException( nameof( result ) );

            #endregion

            if ( value == null )
                return;

            if ( value.GetType() != typeof( string ) )
                return;

            string v = (string) value;
        }
    }
}
