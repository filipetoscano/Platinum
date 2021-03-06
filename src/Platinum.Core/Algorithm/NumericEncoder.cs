﻿using Platinum.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Platinum.Algorithm
{
    /// <summary>
    /// Numerical encoder/decoder, which can be used to convert a number
    /// (representable using characters 0-9, or power of 10) into a string,
    /// where more characters are available (eg: A-Z, or power of 26).
    /// </summary>
    public class NumericEncoder
    {
        /// <summary>
        /// Initializes a new instance of NumericEncoder, given the specified
        /// character set and encoding length.
        /// </summary>
        /// <param name="characterSet">String containing all valid characters.</param>
        /// <param name="length">Minimal encoded length.</param>
        public NumericEncoder( string characterSet, int length )
        {
            #region Validations

            if ( characterSet == null )
                throw new ArgumentNullException( nameof( characterSet ) );

            if ( length < 1 )
                throw new ArgumentOutOfRangeException( nameof( length ), length, "Value must be strictly positive." );

            #endregion

            var r = characterSet.ToCharArray().GroupBy( c => c ).Where( g => g.Count() > 1 );

            if ( r.Count() > 0 )
                throw new AlgorithmException( ER.NumericEncoder_DuplicateCharacterInSet, characterSet, r.ElementAt( 0 ).Key );

            this.CharacterSet = characterSet;
            this.EncodedLength = length;
        }


        /// <summary>
        /// Gets the character set for this Numerical encoder.
        /// </summary>
        public string CharacterSet
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets the minimum length of the encoded string.
        /// </summary>
        public int EncodedLength
        {
            get;
            private set;
        }


        /// <summary>
        /// Converts the string value into it's numerical value.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <returns>Numerical value.</returns>
        public int ToNumber( string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            if ( value.Length == 0 )
                throw new ArgumentOutOfRangeException( nameof( value ), "Value must be a non-empty string" );

            #endregion

            int acc = 0;

            for ( int i = 0; i < value.Length; i++ )
            {
                int power = value.Length - i - 1;
                int v = this.CharacterSet.IndexOf( value[ i ] );

                if ( v == -1 )
                    throw new AlgorithmException( ER.NumericEncoder_CodeCharacterNotInSet, this.CharacterSet, value[ i ] );

                if ( v == 0 )
                    continue;

                int add = (int) System.Math.Pow( this.CharacterSet.Length, power ) * v;
                acc += add;
            }

            return acc;
        }


        /// <summary>
        /// Convers the numerical value into it's string value.
        /// </summary>
        /// <param name="number">Numerical value.</param>
        /// <returns>String value.</returns>
        public string ToCode( int number )
        {
            #region Validations

            if ( number < 0 )
                throw new ArgumentOutOfRangeException( nameof( number ), number, "Value must be non-negative." );

            #endregion


            /*
             * 
             */
            List<string> list = new List<string>();
            int index = 0;

            while ( index < this.EncodedLength )
            {
                int res = ((int) System.Math.Floor( number / System.Math.Pow( this.CharacterSet.Length, index ) ) % this.CharacterSet.Length);
                list.Add( this.CharacterSet[ res ].ToString() );

                index++;
            }

            list.Reverse();


            /*
             * 
             */
            string code = string.Join( "", list.ToArray() );
            return code.PadLeft( this.EncodedLength, this.CharacterSet[ 0 ] );
        }


        /// <summary>
        /// Builds a numeric encoder, reading the values from the
        /// application configuration file.
        /// </summary>
        /// <param name="name">Logical name of numeric encoder.</param>
        /// <returns>Instance of <see cref="NumericEncoder" />.</returns>
        public static NumericEncoder Build( string name )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion

            string characterSet = AppConfiguration.Get<string>( name + ":CharacterSet" );
            int length = AppConfiguration.Get<int>( name + ":Length" );

            return new NumericEncoder( characterSet, length );
        }
    }
}

/* eof */
