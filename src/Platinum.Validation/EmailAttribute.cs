﻿using System;
using System.Text.RegularExpressions;

namespace Platinum.Validation
{
    /// <summary />
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    public class EmailAttribute : Attribute, IValidationRule
    {
        /// <summary>
        /// Pattern which is used to define an email address.
        /// </summary>
        public const string Pattern = @"^[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:(?!.{256})(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?)$";

        private static Regex _email = new Regex( Pattern, RegexOptions.Compiled );

        /// <summary />
        public EmailAttribute()
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

            if ( value == null )
                return;

            if ( value.GetType() != typeof( string ) )
                return;

            string v = (string) value;

            if ( _email.IsMatch( v ) == false )
            {
                ValidationException vex = new ValidationException( ER.Email_Invalid, context.Path, context.Property );
                result.AddError( vex );
            }
        }
    }
}
