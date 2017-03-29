using System;

namespace Platinum.Validation
{
    /// <summary>
    /// Asserts that a property does NOT have a value.
    /// </summary>
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class ForbiddenAttribute : Attribute, IValidationRule
    {
        /// <summary />
        public ForbiddenAttribute()
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

            if ( value.HasValue() == false )
                return;

            ValidationException vex = new ValidationException( ER.Forbidden, context.Path, context.Property );
            result.AddError( vex );
        }
    }
}
