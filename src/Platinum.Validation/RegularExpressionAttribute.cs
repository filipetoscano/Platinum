using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Platinum.Validation
{
    /// <summary />
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class RegularExpressionAttribute : Attribute, IValidationRule
    {
        /// <summary />
        public RegularExpressionAttribute( string pattern )
        {
            #region Validations

            if ( pattern == null )
                throw new ArgumentNullException( nameof( pattern ) );

            #endregion

            this.Pattern = pattern;
        }


        /// <summary>
        /// Gets the regular expression which must be matched.
        /// </summary>
        public string Pattern
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


            /*
             * 
             */
            string sv;

            if ( value is string )
                sv = (string) value;
            else
                sv = (string) Convert.ChangeType( value, typeof( string ), CultureInfo.InvariantCulture );


            /*
             * 
             */
            Regex regex;

            try
            {
                regex = new Regex( this.Pattern );
            }
            catch ( ArgumentException )
            {
                ValidationException vex = new ValidationException( ER.RegularExpression_Invalid, context.Path, context.Property, this.Pattern );
                result.AddError( vex );
                return;
            }

            if ( regex.IsMatch( sv ) == false )
            {
                ValidationException vex = new ValidationException( ER.RegularExpression, context.Path, context.Property, this.Pattern );
                result.AddError( vex );
            }
        }
    }
}
