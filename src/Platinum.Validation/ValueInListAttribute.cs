using System;
using System.Globalization;
using System.Linq;

namespace Platinum.Validation
{
    /// <summary />
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class ValueInListAttribute : Attribute, IValidationRule
    {
        /// <summary />
        public ValueInListAttribute( string values, string separator = " " )
        {
            #region Validations

            if ( values == null )
                throw new ArgumentNullException( nameof( values ) );

            if ( separator == null )
                throw new ArgumentNullException( nameof( separator ) );

            #endregion

            this.Values = values.Split( new string[] { separator }, StringSplitOptions.None );
        }


        /// <summary />
        public string[] Values
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

            if ( sv.Length == 0 )
                return;


            /*
             * 
             */
            if ( this.Values.Contains( sv ) == false )
            {
                ValidationException vex = new ValidationException( ER.ValueInList_Invalid, context.Path, context.Property, string.Join( ",", this.Values ) );
                result.AddError( vex );
            }
        }
    }
}
