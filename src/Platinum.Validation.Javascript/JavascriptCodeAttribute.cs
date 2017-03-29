using System;

namespace Platinum.Validation
{
    /// <summary />
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = true )]
    public class JavascriptCodeAttribute : Attribute, IValidationRule
    {
        /// <summary />
        public JavascriptCodeAttribute( string functionName )
        {
            #region Validations

            if ( functionName == null )
                throw new ArgumentNullException( nameof( functionName ) );

            #endregion

            this.Function = functionName;
        }


        /// <summary />
        public string Function
        {
            get;
            private set;
        }


        /// <summary />
        public string FunctionCode
        {
            get;
            private set;
        }


        /// <summary />
        public void Validate( ValidationContext context, ValidationResult result, object value )
        {
            throw new NotImplementedException();
        }
    }
}
