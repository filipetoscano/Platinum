using System;
using System.Collections.Generic;

namespace Platinum.Validation
{
    /// <summary />
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class ValidationRuleAttribute : Attribute, IValidationRule
    {
        /// <summary />
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


        /// <summary />
        public void Validate( ValidationContext context, ValidationResult result, object value )
        {
            IValidationRule[] rules = RulesFor( this.Name );

            if ( rules == null || rules.Length == 0 )
                return;

            foreach ( var r in rules )
            {
                r.Validate( context, result, value );
            }
        }


        private IValidationRule[] RulesFor( string name )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion

            // TODO
            List<IValidationRule> rules = new List<IValidationRule>();

            rules.Add( new RequiredAttribute() );
            rules.Add( new StringLengthAttribute( 5, 10 ) );

            return rules.ToArray();
        }
    }
}
