using System;

namespace Platinum.Validation
{
    /// <summary />
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

            throw new NotImplementedException();
        }
    }
}
