using System;

namespace Platinum.Validation
{
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class MaxLengthAttribute : Attribute, IValidationRule
    {
        public MaxLengthAttribute( int maxLength )
        {
            this.MaxLength = maxLength;
        }


        public int MaxLength
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

            if ( v.Length < this.MaxLength )
            {
                ValidationException vex = new ValidationException( ER.StringLength_Max, context.Path, context.Property, this.MaxLength );
                result.AddError( vex );
            }
        }
    }
}
