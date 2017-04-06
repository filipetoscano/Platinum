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
        /// Gets a name for the given regular expression.
        /// </summary>
        public string Name
        {
            get;
            set;
        }


        /// <summary>
        /// Gets the regular expression which must be matched.
        /// </summary>
        public string Pattern
        {
            get;
            private set;
        }


        /// <summary>
        /// If set to true, the regular expression does not have to start with ^ and end
        /// with $. Otherwise, it MUST fully emcompass the string value.
        /// </summary>
        public bool Flex
        {
            get;
            set;
        } = false;


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

            if ( sv.Length == 0 )
                return;


            /*
             * 
             */
            Regex regex;

            if ( this.Flex == false )
            {
                bool strictError = false;

                if ( this.Pattern.StartsWith( "^" ) == false )
                {
                    ValidationException vex = new ValidationException( ER.RegularExpression_Strict_NotStart, context.Path, context.Property, this.Pattern );
                    result.AddError( vex );
                    strictError = true;
                }

                if ( this.Pattern.EndsWith( "$" ) == false )
                {
                    ValidationException vex = new ValidationException( ER.RegularExpression_Strict_NotEnd, context.Path, context.Property, this.Pattern );
                    result.AddError( vex );
                    strictError = true;
                }

                if ( strictError == true )
                    return;
            }

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
