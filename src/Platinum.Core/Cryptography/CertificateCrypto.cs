using Platinum.Configuration;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;

namespace Platinum.Cryptography
{
    public class CertificateCrypto
    {
        private string _name;
        private X509Certificate2 _certificate;


        /// <summary>
        /// Initializes a new instance of <see cref="SymmetricCrypto" />, which will
        /// be initialized by reading configuration values directly from application
        /// configuration file.
        /// </summary>
        /// <param name="name">Logical name of the certificate.</param>
        /// <remarks>
        /// The logical name of the certificate will be used as key to load the
        /// X509 certificate from the (local) certificate store.
        /// </remarks>
        public CertificateCrypto( string name )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion

            _name = name;
        }


        public string SerialNumber
        {
            get
            {
                EnsureCertificate();

                return _certificate.SerialNumber;
            }
        }


        /// <summary>
        /// Encrypts the byte array.
        /// </summary>
        /// <param name="buffer">Raw byte array.</param>
        /// <returns>Encrypted byte array.</returns>
        public byte[] EncryptBuffer( byte[] buffer )
        {
            #region Validations

            if ( buffer == null )
                throw new ArgumentNullException( nameof( buffer ) );

            #endregion


            /*
             * 
             */
            EnsureCertificate();

            if ( buffer.Length > ( _certificate.PublicKey.Key.KeySize / 8 ) )
                throw new CryptographyException( ER.CertificateCrypto_EncryptBufferTooLarge, _name, _certificate.Subject, _certificate.PublicKey.Key.KeySize / 8 );


            /*
             * 
             */
            byte[] response;

            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider) _certificate.PublicKey.Key;

            try
            {
                response = rsa.Encrypt( buffer, false );
            }
            catch ( CryptographicException ex )
            {
                throw new CryptographyException( ER.CertificateCrypto_EncryptBuffer, ex, _name, _certificate.Subject );
            }

            return response;
        }


        /// <summary>
        /// Encrypts the string value.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <returns>Encrypted value, in base 64.</returns>
        public string Encrypt( string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            byte[] vi = Encoding.UTF8.GetBytes( value );
            byte[] vo = EncryptBuffer( vi );

            return Convert.ToBase64String( vo );
        }


        /// <summary>
        /// Decrypts the byte array.
        /// </summary>
        /// <param name="buffer">Encrypted byte array.</param>
        /// <returns>Decrypted byte array.</returns>
        public byte[] DecryptBuffer( byte[] buffer )
        {
            #region Validations

            if ( buffer == null )
                throw new ArgumentNullException( nameof( buffer ) );

            #endregion

            EnsureCertificate();

            if ( _certificate.HasPrivateKey == false )
                throw new CryptographyException( ER.CertificateCrypto_CertificateWithoutPrivateKey, _certificate.Subject );

            byte[] response;

            RSACryptoServiceProvider rsa;

            try
            {
                rsa = (RSACryptoServiceProvider) _certificate.PrivateKey;
            }
            catch ( CryptographicException ex )
            {
                if ( ex.Message == "The handle is invalid." )
                    throw new CryptographyException( ER.CertificateCrypto_DecryptBuffer_PrivateKey_InvalidHandle, ex, _certificate.Subject );
                else
                    throw new CryptographyException( ER.CertificateCrypto_DecryptBuffer_PrivateKey_Other, ex, _certificate.Subject );
            }

            try
            {
                response = rsa.Decrypt( buffer, false );
            }
            catch ( CryptographicException ex )
            {
                throw new CryptographyException( ER.CertificateCrypto_DecryptBuffer, ex, _certificate.Subject );
            }

            return response;
        }


        /// <summary>
        /// Decrypts the string value.
        /// </summary>
        /// <param name="value">Encrypted value, in base 64.</param>
        /// <returns>Decrypted value.</returns>
        public string Decrypt( string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            byte[] vi;

            try
            {
                vi = Convert.FromBase64String( value );
            }
            catch ( FormatException ex )
            {
                throw new CryptographyException( ER.CertificateCrypto_DecryptValue_FromBase64, ex );
            }

            byte[] vo = DecryptBuffer( vi );

            return Encoding.UTF8.GetString( vo );
        }


        private void EnsureCertificate()
        {
            if ( _certificate == null )
            {
                lock ( this )
                {
                    if ( _certificate == null )
                    {
                        _certificate = Load( _name );
                    }
                }
            }
        }


        private static X509Certificate2 Load( string name )
        {
            /*
             * 
             */
            string subjectNameKey = string.Concat( name, ":Certificate.SubjectName" );
            string storeNameKey = string.Concat( name, ":Certificate.StoreName" );
            string storeLocationKey = string.Concat( name, ":Certificate.StoreLocation" );

            string subjectName = AppConfiguration.Get<string>( subjectNameKey );
            StoreName storeName = AppConfiguration.Get<StoreName>( storeNameKey );
            StoreLocation storeLocation = AppConfiguration.Get<StoreLocation>( storeLocationKey );

            string whoAmI = WindowsIdentity.GetCurrent().Name;


            /*
             * 
             */
            X509Store store = new X509Store( storeName, storeLocation );
            store.Open( OpenFlags.ReadOnly );

            X509Certificate2Collection certs = store.Certificates.Find( X509FindType.FindBySubjectDistinguishedName, subjectName, false );

            if ( certs.Count == 0 )
            {
                store.Close();
                throw new CryptographyException( ER.CertificateCrypto_CertificateNotFound, whoAmI, subjectName, storeLocation, storeName );
            }

            if ( certs.Count > 1 )
            {
                store.Close();
                throw new CryptographyException( ER.CertificateCrypto_MultipleCertifificatesFound, whoAmI, subjectName, certs.Count );
            }


            /*
             * 
             */
            X509Certificate2 cert = certs[ 0 ];
            store.Close();

            return cert;
        }
    }
}

/* eof */