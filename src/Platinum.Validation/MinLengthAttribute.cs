using System;

namespace Platinum.Validation
{
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class MinLengthAttribute : Attribute, IValidationRule
    {
        public MinLengthAttribute( int minLength )
        {
            this.MinLength = minLength;
        }


        public int MinLength
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

            if ( v.Length < this.MinLength )
            {
                ValidationException vex = new ValidationException( ER.StringLength_Min, context.Path, context.Property, this.MinLength );
                result.AddError( vex );
            }
        }
    }
}
