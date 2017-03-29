using System;

namespace Platinum.Validation
{
    /// <summary>
    /// Asserts that a string value has at least 'MinLength' characters. If the string
    /// is null, no validation will be run.
    /// </summary>
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class MinLengthAttribute : Attribute, IValidationRule
    {
        /// <summary />
        public MinLengthAttribute( int minLength )
        {
            #region Validations

            if ( minLength < 1 )
                throw new ArgumentOutOfRangeException( nameof( minLength ), "Min length must be greater than zero." );

            #endregion

            this.MinLength = minLength;
        }


        /// <summary>
        /// Minimum length of a string.
        /// </summary>
        public int MinLength
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

            if ( v.Length < this.MinLength )
            {
                ValidationException vex = new ValidationException( ER.StringLength_Min, context.Path, context.Property, this.MinLength );
                result.AddError( vex );
            }
        }
    }
}
