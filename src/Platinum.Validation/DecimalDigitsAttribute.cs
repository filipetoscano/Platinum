using System;

namespace Platinum.Validation
{
    /// <summary>
    /// Asserts that a property value has a given number of decimal digits.
    /// </summary>
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class DecimalDigitsAttribute : Attribute, IValidationRule
    {
        /// <summary />
        public DecimalDigitsAttribute( int maxDecimalDigits )
        {
            this.MaxDecimalDigits = maxDecimalDigits;
        }


        /// <summary>
        /// Gets the maximum number of decimal digits that the value can
        /// have.
        /// </summary>
        public int MaxDecimalDigits
        {
            get;
            private set;
        }


        /// <summary />
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

            // TODO: What for float/double?
            decimal v = (decimal) value;
            int dd = Math.DecimalDigits( v );

            if ( dd > this.MaxDecimalDigits )
            {
                ValidationException vex = new ValidationException( ER.DecimalDigits_Max, context.Path, context.Property, this.MaxDecimalDigits );
                result.AddError( vex );
            }
        }
    }
}
