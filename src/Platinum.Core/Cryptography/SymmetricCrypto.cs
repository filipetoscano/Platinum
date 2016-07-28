using Platinum.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Platinum.Cryptography
{
    public class SymmetricCrypto
    {
        private string _name;
        private SymmetricAlgorithm _algo;


        public SymmetricCrypto( string name )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion

            _name = name;
        }


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
        }


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
            ICryptoTransform c;

            try
            {
                c = _algo.CreateDecryptor();
            }
            catch ( CryptographicException ex )
            {
                throw new CryptographyException( ER.SymmetricCrypto_Decrypt_CreateDecryptor, ex, _algo.GetType().FullName );
            }


            /*
             * 
             */
            string r;

            using ( c )
            {
                MemoryStream ms = new MemoryStream( Convert.FromBase64String( value ) );
                CryptoStream cs = new CryptoStream( ms, c, CryptoStreamMode.Read );
                StreamReader sr = new StreamReader( cs, Encoding.UTF8 );

                try
                {
                    r = sr.ReadToEnd();
                }
                catch ( CryptographicException ex )
                {
                    throw new CryptographyException( ER.SymmetricCrypto_Decrypt_Fail, ex, _algo.GetType().FullName );
                }
            }

            return r;
        }


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
            ICryptoTransform c;

            try
            {
                c = _algo.CreateEncryptor();
            }
            catch ( CryptographicException ex )
            {
                throw new CryptographyException( ER.SymmetricCrypto_Encrypt_CreateEncryptor, ex, _algo.GetType().FullName );
            }


            /*
             * 
             */
            MemoryStream ms;

            using ( c )
            {
                ms = new MemoryStream();

                CryptoStream cs = new CryptoStream( ms, c, CryptoStreamMode.Write );
                StreamWriter sw = new StreamWriter( cs, Encoding.UTF8 );

                sw.Write( value );
            }

            return Convert.ToBase64String( ms.ToArray() );
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
                string ivName = _name = ":Iv";

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
                    throw new CryptographyException( ER.SymmetricCrypto_Config_InvalidKey, keyName );
                }

                try
                {
                    biv = Convert.FromBase64String( iv );
                }
                catch ( FormatException )
                {
                    throw new CryptographyException( ER.SymmetricCrypto_Config_InvalidIv, ivName );
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