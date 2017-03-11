using System;

namespace Platinum.Validation
{
    /// <summary />
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class TotalDigitsAttribute : Attribute, IValidationRule
    {
        /// <summary />
        public TotalDigitsAttribute( int maxTotalDigits )
        {
            this.MaxTotalDigits = maxTotalDigits;
        }


        /// <summary>
        /// Gets the maximum number of decimal digits.
        /// </summary>
        public int MaxTotalDigits
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
            int dd = Math.TotalDigits( v );

            if ( dd > this.MaxTotalDigits )
            {
                ValidationException vex = new ValidationException( ER.TotalDigits_Max, context.Path, context.Property, this.MaxTotalDigits );
                result.AddError( vex );
            }
        }
    }
}
