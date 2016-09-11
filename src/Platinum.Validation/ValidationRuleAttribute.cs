using System;

namespace Platinum.Validation
{
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class ValidationRuleAttribute : Attribute, IValidationRule
    {
        public ValidationRuleAttribute( string name )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion

            this.Name = name;
        }


        /// <summary>
        /// Gets the name of the validation rule.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }



        public void Validate( ValidationContext context, ValidationResult result, object value )
        {
        }
    }
}
