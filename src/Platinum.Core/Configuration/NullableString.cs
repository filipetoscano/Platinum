using System;
using System.ComponentModel;
using System.Globalization;

namespace Platinum.Configuration
{
    /// <summary>
    /// Wrapper type to emulate nullable strings.
    /// </summary>
    /// <remarks>
    /// When declaring a configuration property as a string, if the property
    /// is not required one would expect that 'DefaultValue = null' in the 
    /// attribute would override the system default of string.Empty... But
    /// it doesn't! :-(
    /// 
    /// Hence the need for this class: so that we can really identify when
    /// the attribute was specified (ie, has any string value) and was not
    /// (ie, is null).
    /// </remarks>
    public struct NullableString
    {
        private readonly string _value;

        private NullableString( string value )
        {
            this._value = value;
        }


        /// <summary>
        /// Casts string value into instance of <see cref="NullableString" />.
        /// </summary>
        /// <param name="value">String value.</param>
        public static implicit operator NullableString( string value )
        {
            return new NullableString( value );
        }


        /// <summary>
        /// Casts instance value to string value.
        /// </summary>
        /// <param name="string">Instance of <see cref="NullableString" />.</param>
        public static implicit operator string( NullableString @string )
        {
            return @string._value;
        }
    }


    /// <summary>
    /// Converts from string value into <see cref="NullableString" />.
    /// </summary>
    public sealed class NullableStringConverter : TypeConverter
    {
        /// <summary />
        public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
        {
            return sourceType == typeof( string );
        }


        /// <summary />
        public override object ConvertFrom( ITypeDescriptorContext context, CultureInfo culture, object value )
        {
            return (NullableString) (string) value;
        }
    }
}
