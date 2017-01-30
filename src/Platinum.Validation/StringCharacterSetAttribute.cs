using System;
using System.Linq;

namespace Platinum.Validation
{
    /// <summary />
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class StringCharacterSetAttribute : Attribute, IValidationRule
    {
        /// <summary />
        public StringCharacterSetAttribute( string charset )
        {
            #region Validations

            if ( charset == null )
                throw new ArgumentNullException( nameof( charset ) );

            #endregion

            this.Charset = charset;
        }


        /// <summary />
        public string Charset
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

            if ( value.GetType() != typeof( string ) )
                return;

            string v = (string) value;

            if ( v.Length == 0 )
                return;

            if ( v.All( c => this.Charset.Contains( c ) ) == false )
            {
                ValidationException vex = new ValidationException( ER.StringCharacterSet_Invalid, context.Path, context.Property, this.Charset );
                result.AddError( vex );
            }
        }
    }
}
