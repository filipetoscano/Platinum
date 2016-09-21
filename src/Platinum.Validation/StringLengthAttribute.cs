using System;

namespace Platinum.Validation
{
    /// <summary />
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class StringLengthAttribute : Attribute, IValidationRule
    {
        /// <summary />
        public StringLengthAttribute( int minLength, int maxLength )
        {
            #region Validations

            if ( minLength < 1 )
                throw new ArgumentOutOfRangeException( nameof( minLength ), "Min length must be greater than zero." );

            if ( minLength > maxLength )
                throw new ArgumentOutOfRangeException( nameof( minLength ), "Min length must be smaller or equal to max length." );

            #endregion

            this.MinLength = minLength;
            this.MaxLength = maxLength;
        }


        /// <summary />
        public int MinLength
        {
            get;
            private set;
        }


        /// <summary />
        public int MaxLength
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

            if ( v.Length < this.MinLength )
            {
                ValidationException vex = new ValidationException( ER.StringLength_Min, context.Path, context.Property, this.MinLength );
                result.AddError( vex );
            }

            if ( v.Length > this.MaxLength )
            {
                ValidationException vex = new ValidationException( ER.StringLength_Max, context.Path, context.Property, this.MaxLength );
                result.AddError( vex );
            }
        }
    }
}
