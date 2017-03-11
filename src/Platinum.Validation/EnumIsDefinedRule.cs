using System;

namespace Platinum.Validation
{
    /// <summary />
    public class EnumIsDefinedRule : IValidationRule
    {
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

            Type enumType = value.GetType();

            if ( Enum.IsDefined( enumType, value ) == false )
            {
                ValidationException vex = new ValidationException( ER.Enum_IsNotDefined, context.Path, context.Property );
                result.AddError( vex );
            }
        }
    }
}
