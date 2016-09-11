using System;

namespace Platinum.Validation
{
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class RequiredAttribute : Attribute, IValidationRule
    {
        public RequiredAttribute()
        {
        }


        public void Validate( ValidationContext context, ValidationResult result, object value )
        {
            #region Validations

            if ( context == null )
                throw new ArgumentNullException( nameof( context ) );

            if ( result == null )
                throw new ArgumentNullException( nameof( result ) );

            #endregion

            if ( value.HasValue() == true )
                return;

            ValidationException vex = new ValidationException( ER.Required, context.Path, context.Property );
            result.AddError( vex );
        }
    }
}
