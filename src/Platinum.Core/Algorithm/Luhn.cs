using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Platinum.Algorithm
{
    /// <summary>
    /// Luhn encoder/decoder, also known as the Modulus 10 algorithm, which
    /// generates/validates a checksum for string sequences with a limited
    /// character set.
    /// </summary>
    /// <remarks>
    /// See also: https://en.wikipedia.org/wiki/Luhn_algorithm
    /// </remarks>
    [SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Luhn" )]
    public class Luhn
    {
        private static Dictionary<string, Luhn> _dict = new Dictionary<string, Luhn>();
        private static object _lock = new object();

        private int _b;
        private Dictionary<char, LP> _f;
        private Dictionary<int, LP> _r;


        /// <summary>
        /// Initializes a Luhn encoder/decoder when the given character set.
        /// </summary>
        /// <param name="characterSet">String containing all valid characters.</param>
        public Luhn( string characterSet )
        {
            #region Validations

            if ( characterSet == null )
                throw new ArgumentNullException( "characterSet" );

            #endregion


            /*
             *
             */
            int b = characterSet.Length;
            Dictionary<char, LP> f = new Dictionary<char, LP>();
            Dictionary<int, LP> r = new Dictionary<int, LP>();

            for ( int i = 0; i < characterSet.Length; i++ )
            {
                char c = characterSet[ i ];

                if ( f.ContainsKey( c ) == true )
                    throw new AlgorithmException( ER.Luhn_DuplicateCharacterInSet, characterSet, c );

                int f1 = i;
                int f2 = ( ( 2 * i ) % b ) + ( (int) ( ( 2 * i ) / b ) );

                LP point = new LP( c, i, f1, f2 );
                f.Add( point.Digit, point );
                r.Add( point.Value, point );
            }


            /*
             * 
             */
            CharacterSet = characterSet;
            _b = b;
            _f = f;
            _r = r;
        }


        /// <summary>
        /// Gets the character set for this Luhn encoder.
        /// </summary>
        public string CharacterSet
        {
            get;
            private set;
        }


        /// <summary>
        /// Computes whether the provided value has a valid checksum.
        /// </summary>
        /// <param name="code">Luhn encoded value, including checksum.</param>
        /// <returns>True if the value is valid, false otherwise.</returns>
        public bool IsValid( string code )
        {
            #region Validations

            if ( code == null )
                throw new ArgumentNullException( "code" );

            #endregion

            int mul = code.Length % 2;
            int acc = 0;

            for ( int i = 0; i < code.Length; i++, mul++ )
            {
                char c = code[ i ];

                if ( _f.ContainsKey( c ) == false )
                    throw new AlgorithmException( ER.Luhn_CodeCharacterNotInSet, c );

                LP j = _f[ c ];

                if ( ( mul % 2 ) == 0 )
                {
                    acc += j.F2;
                }
                else
                {
                    acc += j.F1;
                }
            }

            int mod = ( _b - ( acc % _b ) ) % _b;
            return ( mod == 0 );
        }


        /// <summary>
        /// Calculates the checksum for the given value.
        /// </summary>
        /// <param name="value">String sequence.</param>
        /// <returns>Checksum character.</returns>
        public char Checksum( string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( "value" );

            #endregion

            int mul = ( value.Length + 1 ) % 2;
            int acc = 0;

            for ( int i = 0; i < value.Length; i++, mul++ )
            {
                char c = value[ i ];

                if ( _f.ContainsKey( c ) == false )
                    throw new AlgorithmException( ER.Luhn_CodeCharacterNotInSet, value, c );

                LP j = _f[ c ];

                if ( ( mul % 2 ) == 0 )
                {
                    acc += j.F2;
                }
                else
                {
                    acc += j.F1;
                }
            }

            int mod = ( _b - ( acc % _b ) ) % _b;
            return _r[ mod ].Digit;
        }


        /// <summary>
        /// Calculates the checksum for the given value, and appends it
        /// to the end of the value.
        /// </summary>
        /// <param name="value">String sequence.</param>
        /// <returns>String sequence with original value and appended checksum.</returns>
        public string AppendChecksum( string value )
        {
            return string.Concat( value, Checksum( value ) );
        }


        /// <summary>
        /// Builds a Luhn encoder/decoder.
        /// </summary>
        /// <param name="characterSet">List of valid characters.</param>
        /// <returns>Instance of Luhn encoder/decoder, or will raise exception if the
        /// characterSet is invalid.</returns>
        public static Luhn Build( string characterSet )
        {
            #region Validations

            if ( characterSet == null )
                throw new ArgumentNullException( "characterSet" );

            #endregion

            if ( _dict.ContainsKey( characterSet ) == false )
            {
                lock ( _lock )
                {
                    if ( _dict.ContainsKey( characterSet ) == false )
                    {
                        Luhn l = new Luhn( characterSet );
                        _dict.Add( l.CharacterSet, l );
                    }
                }
            }

            return _dict[ characterSet ];
        }
    }
}

/* eof */
