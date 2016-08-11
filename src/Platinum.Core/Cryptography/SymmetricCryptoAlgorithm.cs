namespace Platinum.Cryptography
{
    public enum SymmetricCryptoAlgorithm
    {
        /// <summary>
        /// Rijndael, a subset of AES.
        /// </summary>
        /// <remarks>
        /// Key sizes: 128, 192, 256.
        /// Block sizes: 128.
        /// See: https://en.wikipedia.org/wiki/Advanced_Encryption_Standard
        /// </remarks>
        Rijndael,

        /// <summary>
        /// Triple DES (3DES).
        /// </summary>
        /// <remarks>
        /// Key sizes: 168, 112, 56.
        /// Block sizes: 64.
        /// See: https://en.wikipedia.org/wiki/Triple_DES
        /// </remarks>
        TripleDES,

        /// <summary>
        /// AES
        /// </summary>
        Aes,

        /// <summary>
        /// CLR managed implementation of AES.
        /// </summary>
        AesManaged
    }
}

/* eof */