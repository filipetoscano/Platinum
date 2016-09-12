using System;

namespace Platinum.Validation
{
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class MinValueAttribute : Attribute, IValidationRule
    {
        public MinValueAttribute( double minValue )
        {
            this.MinValue = minValue;
        }


        /// <summary>
        /// Gets the min value.
        /// </summary>
        public double MinValue
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
            private set;
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
                byte mx = (byte) this.MinValue;

                Validate<byte>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is short )
            {
                short v = (short) value;
                short mx = (short) this.MinValue;

                Validate<short>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is int )
            {
                int v = (int) value;
                int mx = (int) this.MinValue;

                Validate<int>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is long )
            {
                long v = (long) value;
                long mx = (long) this.MinValue;

                Validate<long>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is float )
            {
                float v = (float) value;
                float mx = (float) this.MinValue;

                Validate<float>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is double )
            {
                double v = (double) value;
                double mx = (double) this.MinValue;

                Validate<double>( context, result, v, mx, this.IsExclusive );
                return;
            }

            if ( value is decimal )
            {
                decimal v = (decimal) value;
                decimal mx = (decimal) this.MinValue;

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
