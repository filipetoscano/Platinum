using System;
using System.Globalization;

namespace Platinum.Validation
{
    /// <summary>
    /// Condition which checks if another given property has a specific value.
    /// </summary>
    public class PropertyIsEqualCondition : ICondition
    {
        private string _propertyName;
        private string _value;


        /// <summary>
        /// Initializes a new instance of the condition.
        /// </summary>
        /// <param name="propertyName">
        /// Name of property.
        /// </param>
        /// <param name="value">
        /// Expected value.
        /// </param>
        public PropertyIsEqualCondition( string propertyName, string value )
        {
            #region Validations

            if ( propertyName == null )
                throw new ArgumentNullException( nameof( propertyName ) );

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            _propertyName = propertyName;
            _value = value;
        }


        /// <summary>
        /// Returns true if the given property on the obj instance has the
        /// expected value.
        /// </summary>
        /// <param name="obj">
        /// Instance of object.
        /// </param>
        /// <returns>
        /// True if the property exists and has the expected value, false
        /// otherwise.
        /// </returns>
        public bool IsTrue( object obj )
        {
            #region Validations

            if ( obj == null )
                throw new ArgumentNullException( nameof( obj ) );

            #endregion

            var property = obj.GetType().GetProperty( _propertyName );

            if ( property == null )
                return false;

            var v = property.GetValue( obj );

            var vString = (string) Convert.ChangeType( v, typeof( string ), CultureInfo.InvariantCulture );

            return vString == _value;
        }
    }
}
