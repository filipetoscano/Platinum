using System;
using System.Globalization;

namespace Platinum.Validation
{
    /// <summary />
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class MinValueAttribute : Attribute, IValidationRule
    {
        /// <summary />
        public MinValueAttribute( string minValue )
        {
            #region Validations

            if ( minValue == null )
                throw new ArgumentNullException( nameof( minValue ) );

            #endregion

            this.MinValue = minValue;
        }


        /// <summary>
        /// Gets the min value.
        /// </summary>
        public string MinValue
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets whether the maxValue boundary is exclusive or not.
        /// </summary>
        public bool IsExclusive
        {
            get;
            set;
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

            if ( value is byte )
            {
                byte v = (byte) value;
                byte mx = (byte) Convert.ChangeType( this.MinValue, typeof( byte ), CultureInfo.InvariantCulture );

                Validate<byte>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is short )
            {
                short v = (short) value;
                short mx = (short) Convert.ChangeType( this.MinValue, typeof( short ), CultureInfo.InvariantCulture );

                Validate<short>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is int )
            {
                int v = (int) value;
                int mx = (int) Convert.ChangeType( this.MinValue, typeof( int ), CultureInfo.InvariantCulture );

                Validate<int>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is long )
            {
                long v = (long) value;
                long mx = (long) Convert.ChangeType( this.MinValue, typeof( long ), CultureInfo.InvariantCulture );

                Validate<long>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is float )
            {
                float v = (float) value;
                float mx = (float) Convert.ChangeType( this.MinValue, typeof( float ), CultureInfo.InvariantCulture );

                Validate<float>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is double )
            {
                double v = (double) value;
                double mx = (double) Convert.ChangeType( this.MinValue, typeof( double ), CultureInfo.InvariantCulture );

                Validate<double>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is decimal )
            {
                decimal v = (decimal) value;
                decimal mx = (decimal) Convert.ChangeType( this.MinValue, typeof( decimal ), CultureInfo.InvariantCulture );

                Validate<decimal>( context, result, v, mx, this.IsExclusive );
                return;
            }
        }


        public static void Validate<T>( ValidationContext context, ValidationResult result, T value, T minValue, bool isExclusive )
            where T : IComparable<T>
        {
            int cr = value.CompareTo( minValue );

            if ( cr < 0 )
            {
                ValidationException vex = new ValidationException( ER.MinValue_LessThan, context.Path, context.Property, minValue );
                result.AddError( vex );
            }

            if ( cr == 0 && isExclusive == true )
            {
                ValidationException vex = new ValidationException( ER.MinValue_EqualTo, context.Path, context.Property, minValue );
                result.AddError( vex );
            }
        }
    }
}
