using System;
using System.Linq;

namespace Platinum.Validation
{
    /// <summary />
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class StringLowerCaseAttribute : Attribute, IValidationRule
    {
        /// <summary />
        public StringLowerCaseAttribute()
        {
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

            if ( v.All( c => char.IsLower( c ) ) == false )
            {
                ValidationException vex = new ValidationException( ER.StringLowerCase_Invalid, context.Path, context.Property );
                result.AddError( vex );
            }
        }
    }
}
