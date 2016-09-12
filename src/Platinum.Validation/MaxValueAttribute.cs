using System;
using System.Globalization;

namespace Platinum.Validation
{
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class MaxValueAttribute : Attribute, IValidationRule
    {
        public MaxValueAttribute( string maxValue )
        {
            #region Validations

            if ( maxValue == null )
                throw new ArgumentNullException( nameof( maxValue ) );

            #endregion

            this.MaxValue = maxValue;
        }


        /// <summary>
        /// Gets the max value.
        /// </summary>
        public string MaxValue
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
                byte mx = (byte) Convert.ChangeType( this.MaxValue, typeof( byte ), CultureInfo.InvariantCulture );

                Validate<byte>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is short )
            {
                short v = (short) value;
                short mx = (short) Convert.ChangeType( this.MaxValue, typeof( short ), CultureInfo.InvariantCulture );

                Validate<short>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is int )
            {
                int v = (int) value;
                int mx = (int) Convert.ChangeType( this.MaxValue, typeof( int ), CultureInfo.InvariantCulture );

                Validate<int>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is long )
            {
                long v = (long) value;
                long mx = (long) Convert.ChangeType( this.MaxValue, typeof( long ), CultureInfo.InvariantCulture );

                Validate<long>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is float )
            {
                float v = (float) value;
                float mx = (float) Convert.ChangeType( this.MaxValue, typeof( float ), CultureInfo.InvariantCulture );

                Validate<float>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is double )
            {
                double v = (double) value;
                double mx = (double) Convert.ChangeType( this.MaxValue, typeof( double ), CultureInfo.InvariantCulture );

                Validate<double>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is decimal )
            {
                decimal v = (decimal) value;
                decimal mx = (decimal) Convert.ChangeType( this.MaxValue, typeof( decimal ), CultureInfo.InvariantCulture );

                Validate<decimal>( context, result, v, mx, this.IsExclusive );
                return;
            }
        }


        public static void Validate<T>( ValidationContext context, ValidationResult result, T value, T maxValue, bool isExclusive )
            where T : IComparable<T>
        {
            int cr = value.CompareTo( maxValue );

            if ( cr > 0 )
            {
                ValidationException vex = new ValidationException( ER.MaxValue_GreaterThan, context.Path, context.Property, maxValue );
                result.AddError( vex );
            }

            if ( cr == 0 && isExclusive == true )
            {
                ValidationException vex = new ValidationException( ER.MaxValue_EqualTo, context.Path, context.Property, maxValue );
                result.AddError( vex );
            }
        }
    }
}
