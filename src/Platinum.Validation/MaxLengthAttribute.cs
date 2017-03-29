using System;

namespace Platinum.Validation
{
    /// <summary>
    /// Asserts that a string value has up to 'MaxLength' characters. If the string
    /// is null, no validation will be run.
    /// </summary>
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class MaxLengthAttribute : Attribute, IValidationRule
    {
        /// <summary />
        public MaxLengthAttribute( int maxLength )
        {
            #region Validations

            if ( maxLength < 1 )
                throw new ArgumentOutOfRangeException( nameof( maxLength ), "Max length must be greater than zero." );

            #endregion

            this.MaxLength = maxLength;
        }


        /// <summary>
        /// Maximum length of a string.
        /// </summary>
        public int MaxLength
        {
            get;
            private set;
        }


        /// <summary>
        /// Performs validation.
        /// </summary>
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

            if ( v.Length == 0 )
                return;

            if ( v.Length > this.MaxLength )
            {
                ValidationException vex = new ValidationException( ER.StringLength_Max, context.Path, context.Property, this.MaxLength );
                result.AddError( vex );
            }
        }
    }
}
