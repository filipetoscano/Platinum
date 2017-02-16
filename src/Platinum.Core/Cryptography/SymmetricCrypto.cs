using Platinum.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Platinum.Cryptography
{
    /// <summary />
    public class SymmetricCrypto
    {
        private string _name;
        private SymmetricAlgorithm _algo;


        /// <summary>
        /// Initializes a new instance of <see cref="SymmetricCrypto" />, which will
        /// be initialized by reading configuration values directly from application
        /// configuration file.
        /// </summary>
        /// <param name="name">Name of values set.</param>
        public SymmetricCrypto( string name )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion

            _name = name;
        }


        /// <summary>
        /// Initializes a new instance of <see cref="SymmetricCrypto" /> by
        /// explicitly specifying the parameters.
        /// </summary>
        /// <param name="algorithm">Symmetric encryption algorithm.</param>
        /// <param name="key">Secret key, in base 64.</param>
        /// <param name="iv">Initialization vector, in base 64.</param>
        public SymmetricCrypto( SymmetricCryptoAlgorithm algorithm, string key, string iv )
        {
            #region Validations

            if ( key == null )
                throw new ArgumentNullException( nameof( key ) );

            if ( iv == null )
                throw new ArgumentNullException( nameof( iv ) );

            #endregion

            byte[] bkey;
            byte[] biv;

            try
            {
                bkey = Convert.FromBase64String( key );
            }
            catch ( FormatException )
            {
                throw new CryptographyException( ER.SymmetricCrypto_Ctor_InvalidKey );
            }

            try
            {
                biv = Convert.FromBase64String( iv );
            }
            catch ( FormatException )
            {
                throw new CryptographyException( ER.SymmetricCrypto_Ctor_InvalidIv );
            }

            SymmetricAlgorithm algo = For( algorithm );
            algo.Key = bkey;
            algo.IV = biv;

            _algo = algo;
        }


        /// <summary>
        /// Initializes a new instance of <see cref="SymmetricCrypto" /> by
        /// explicitly specifying the parameters.
        /// </summary>
        /// <param name="algorithm">Symmetric encryption algorithm.</param>
        /// <param name="key">Secret key.</param>
        /// <param name="iv">Initialization vector.</param>
        public SymmetricCrypto( SymmetricCryptoAlgorithm algorithm, byte[] key, byte[] iv )
        {
            #region Validations

            if ( key == null )
                throw new ArgumentNullException( nameof( key ) );

            if ( iv == null )
                throw new ArgumentNullException( nameof( iv ) );

            #endregion

            SymmetricAlgorithm algo = For( algorithm );
            algo.Key = key;
            algo.IV = iv;

            _algo = algo;
        }


        /// <summary>
        /// Decrypts the string value.
        /// </summary>
        /// <param name="value">Encrypted value, in base64.</param>
        /// <returns>Decrypted value.</returns>
        [SuppressMessage( "Microsoft.Usage", "CA2202:Do not dispose objects multiple times" )]
        public string Decrypt( string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion


            /*
             * 
             */
            EnsureInitialize();
            ICryptoTransform decryptor;

            try
            {
                decryptor = _algo.CreateDecryptor();
            }
            catch ( CryptographicException ex )
            {
                throw new CryptographyException( ER.SymmetricCrypto_Decrypt_CreateDecryptor, ex, _algo.GetType().FullName );
            }


            /*
             * 
             */
            byte[] crypted = Convert.FromBase64String( value );
            byte[] raw;

            using ( decryptor )
            {
                using ( MemoryStream ms = new MemoryStream() )
                {
                    using ( CryptoStream cs = new CryptoStream( ms, decryptor, CryptoStreamMode.Write ) )
                    {
                        cs.Write( crypted, 0, crypted.Length );
                    }

                    raw = ms.ToArray();
                }
            }

            return Encoding.UTF8.GetString( raw );
        }


        /// <summary>
        /// Encrypts the string value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Encrypted value.</returns>
        [SuppressMessage( "Microsoft.Usage", "CA2202:Do not dispose objects multiple times" )]
        public string Encrypt( string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion


            /*
             * 
             */
            EnsureInitialize();
            ICryptoTransform encryptor;

            try
            {
                encryptor = _algo.CreateEncryptor();
            }
            catch ( CryptographicException ex )
            {
                throw new CryptographyException( ER.SymmetricCrypto_Encrypt_CreateEncryptor, ex, _algo.GetType().FullName );
            }


            /*
             * 
             */
            byte[] raw = Encoding.UTF8.GetBytes( value );
            byte[] crypted;

            using ( encryptor )
            {
                using ( MemoryStream ms = new MemoryStream() )
                {
                    using ( CryptoStream cs = new CryptoStream( ms, encryptor, CryptoStreamMode.Write ) )
                    {
                        cs.Write( raw, 0, raw.Length );
                    }

                    crypted = ms.ToArray();
                }
            }

            return Convert.ToBase64String( crypted );
        }


        private void EnsureInitialize()
        {
            if ( _algo != null )
                return;

            lock ( this )
            {
                if ( _algo != null )
                    return;

                string keyName = _name + ":Key";
                string ivName = _name + ":Iv";

                SymmetricCryptoAlgorithm name = AppConfiguration.Get<SymmetricCryptoAlgorithm>( _name + ":Algorithm" );
                string key = AppConfiguration.Get<string>( keyName );
                string iv = AppConfiguration.Get<string>( ivName );


                /*
                 * 
                 */
                byte[] bkey;
                byte[] biv;

                try
                {
                    bkey = Convert.FromBase64String( key );
                }
                catch ( FormatException )
                {
                    throw new CryptographyException( ER.SymmetricCrypto_Config_InvalidKey, _name );
                }

                try
                {
                    biv = Convert.FromBase64String( iv );
                }
                catch ( FormatException )
                {
                    throw new CryptographyException( ER.SymmetricCrypto_Config_InvalidIv, _name );
                }


                /*
                 * 
                 */
                SymmetricAlgorithm algo = For( name );
                algo.Key = bkey;
                algo.IV = biv;

                _algo = algo;
            }
        }


        /// <summary>
        /// Returns an instance of <see cref="SymmetricAlgorithm"/> for the given
        /// symmetric encryption algorithm.
        /// </summary>
        /// <param name="algorithm">Algorithm.</param>
        /// <returns>Instance of <see cref="SymmetricAlgorithm"/>.</returns>
        private static SymmetricAlgorithm For( SymmetricCryptoAlgorithm algorithm )
        {
            SymmetricAlgorithm obj;

            switch ( algorithm )
            {
                case SymmetricCryptoAlgorithm.Rijndael:
                    obj = new RijndaelManaged();
                    break;

                case SymmetricCryptoAlgorithm.TripleDES:
                    obj = new TripleDESCryptoServiceProvider();
                    break;

                case SymmetricCryptoAlgorithm.Aes:
                    obj = new AesCryptoServiceProvider();
                    break;

                case SymmetricCryptoAlgorithm.AesManaged:
                    obj = new AesManaged();
                    break;

                default:
                    obj = new RijndaelManaged();
                    break;
            }

            return obj;
        }
    }
}

/* eof */