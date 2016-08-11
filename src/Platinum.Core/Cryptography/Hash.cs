using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using HA = System.Security.Cryptography.HashAlgorithm;

namespace Platinum.Cryptography
{
    public static class Hash
    {
        private static readonly uint[] _lookup32 = CreateLookup32();


        /// <summary>
        /// Computes the hash of a string value, using the given hashing
        /// algorithm.
        /// </summary>
        /// <param name="algorithm">
        /// Hashing algorithm.
        /// </param>
        /// <param name="value">
        /// String value.
        /// </param>
        /// <param name="format">
        /// Format in which the hash should be returned.
        /// </param>
        /// <returns>
        /// The computed hash, in the desired format.
        /// </returns>
        public static string Compute( HashAlgorithm algorithm, string value, HashFormat format = HashFormat.Hex )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            using ( HA ha = For( algorithm ) )
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes( value );

                byte[] hashBytes = ha.ComputeHash( plainTextBytes );

                if ( format == HashFormat.Base64 )
                    return Convert.ToBase64String( hashBytes );
                else
                    return ByteArrayToHex( hashBytes );
            }
        }


        /// <summary>
        /// Computes the hash value for the specified byte array,
        /// using the given algorithm.
        /// </summary>
        /// <param name="algorithm">
        /// Hashing algorithm.
        /// </param>
        /// <param name="bytes">
        /// The input to compute the hash code for.
        /// </param>
        /// <param name="format">
        /// Format in which the hash should be returned.
        /// </param>
        /// <returns>
        /// The computed hash, in the desired format.
        /// </returns>
        public static string Compute( HashAlgorithm algorithm, byte[] bytes, HashFormat format = HashFormat.Hex )
        {
            #region Validations

            if ( bytes == null )
                throw new ArgumentNullException( nameof( bytes ) );

            if ( bytes.Length == 0 )
                throw new ArgumentOutOfRangeException( nameof( bytes ), "Byte array must be non-empty." );

            #endregion

            using ( HA ha = For( algorithm ) )
            {
                byte[] hashBytes = ha.ComputeHash( bytes );

                if ( format == HashFormat.Base64 )
                    return Convert.ToBase64String( hashBytes );
                else
                    return ByteArrayToHex( hashBytes );
            }
        }


        /// <summary>
        /// Computes the MD5 hash of the string value.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <returns>MD5 hash, in hex format.</returns>
        public static string MD5( string value )
        {
            return Compute( HashAlgorithm.MD5, value, HashFormat.Hex );
        }


        /// <summary>
        /// Computes the SHA1 hash of the string value.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <returns>SHA1 hash, in hex format.</returns>
        public static string SHA1( string value )
        {
            return Compute( HashAlgorithm.SHA1, value, HashFormat.Hex );
        }


        /// <summary>
        /// Computes the SHA512 hash of the string value.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <returns>SHA512 hash, in hex format.</returns>
        public static string SHA512( string value )
        {
            return Compute( HashAlgorithm.SHA512, value, HashFormat.Hex );
        }



        [SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sha" )]
        public static string EbcdicSha1( string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            if ( value.Length == 0 )
                throw new ArgumentOutOfRangeException( nameof( value ), "String must not be empty." );

            #endregion


            /*
             * Ascii to Ebcdic mapping
             */
            byte[] a2e = new byte[ 256 ]
                {
                      0,   1,   2,   3,  55,  45,  46,  47,  22,   5,  37,  11,  12,  13,  14,  15,
                     16,  17,  18,  19,  60,  61,  50,  38,  24,  25,  63,  39,  28,  29,  30,  31,
                     64,  79, 127, 123,  91, 108,  80, 125,  77,  93,  92,  78, 107,  96,  75,  97,
                    240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 122,  94,  76, 126, 110, 111,
                    124, 193, 194, 195, 196, 197, 198, 199, 200, 201, 209, 210, 211, 212, 213, 214,
                    215, 216, 217, 226, 227, 228, 229, 230, 231, 232, 233,  74, 224,  90,  95, 109,
                    121, 129, 130, 131, 132, 133, 134, 135, 136, 137, 145, 146, 147, 148, 149, 150,
                    151, 152, 153, 162, 163, 164, 165, 166, 167, 168, 169, 192, 106, 208, 161,   7,
                     32,  33,  34,  35,  36,  21,   6,  23,  40,  41,  42,  43,  44,   9,  10,  27,
                     48,  49,  26,  51,  52,  53,  54,   8,  56,  57,  58,  59,   4,  20,  62, 225,
                     65,  66,  67,  68,  69,  70,  71,  72,  73,  81,  82,  83,  84,  85,  86,  87,
                     88,  89,  98,  99, 100, 101, 102, 103, 104, 105, 112, 113, 114, 115, 116, 117,
                    118, 119, 120, 128, 138, 139, 140, 141, 142, 143, 144, 154, 155, 156, 157, 158,
                    159, 160, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183,
                    184, 185, 186, 187, 188, 189, 190, 191, 202, 203, 204, 205, 206, 207, 218, 219,
                    220, 221, 222, 223, 234, 235, 236, 237, 238, 239, 250, 251, 252, 253, 254, 255
                };


            /*
             * 
             */
            int hashSize = 30;

            if ( value.Length > 30 )
                hashSize = value.Length;

            byte[] bHASH = new byte[ hashSize ];


            /*
             * The first mask ensures that we are working with an 8bit=Ascii
             * character, essentially stripping all high bits.
             */
            for ( int i = 0; i < value.Length; i++ )
                bHASH[ i ] = Convert.ToByte( (int) value[ i ] & 0x00ff );

            for ( int i = value.Length; i < hashSize; i++ )
                bHASH[ i ] = Convert.ToByte( ' ' );

            for ( int i = 0; i < hashSize; i++ )
                bHASH[ i ] = a2e[ bHASH[ i ] ];


            /*
             * Compute the hash
             */
            string base64String;

            using ( SHA1 sha = new SHA1CryptoServiceProvider() )
            {
                byte[] result = sha.ComputeHash( bHASH );
                base64String = Convert.ToBase64String( result, 0, 20 );
            }

            return base64String;
        }


        private static HA For( HashAlgorithm algorithm )
        {
            switch ( algorithm )
            {
                case HashAlgorithm.MD5:
                    return System.Security.Cryptography.MD5.Create();

                case HashAlgorithm.SHA1:
                    return new SHA1Managed();

                case HashAlgorithm.SHA256:
                    return new SHA256Managed();

                case HashAlgorithm.SHA384:
                    return new SHA384Managed();

                case HashAlgorithm.SHA512:
                    return new SHA512Managed();
            }

            throw new NotSupportedException();
        }


        private static uint[] CreateLookup32()
        {
            var result = new uint[ 256 ];
            for ( int i = 0; i < 256; i++ )
            {
                string s = i.ToString( "x2" );
                result[ i ] = ((uint) s[ 0 ]) + ((uint) s[ 1 ] << 16);
            }

            return result;
        }


        private static string ByteArrayToHex( byte[] bytes )
        {
            #region Validation

            if ( bytes == null )
                throw new ArgumentNullException( nameof( bytes ) );

            #endregion

            var lookup32 = _lookup32;
            var result = new char[ bytes.Length * 2 ];

            for ( int i = 0; i < bytes.Length; i++ )
            {
                var val = lookup32[ bytes[ i ] ];
                result[ 2 * i ] = (char) val;
                result[ 2 * i + 1 ] = (char) (val >> 16);
            }

            return new string( result );
        }
    }
}

/* eof */