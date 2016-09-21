namespace Platinum.Cryptography
{
    /// <summary />
    public enum HashAlgorithm
    {
        /// <summary>
        /// MD5 algorithm, resulting in 128 bit hashes.
        /// </summary>
        MD5,

        /// <summary>
        /// SHA algorithm, resulting in 160 bit hashes.
        /// </summary>
        SHA1,

        /// <summary>
        /// SHA algorithm, resulting in 256 bit hashes.
        /// </summary>
        SHA256,

        /// <summary>
        /// SHA algorithm, resulting in 384 bit hashes.
        /// </summary>
        SHA384,

        /// <summary>
        /// SHA algorithm, resulting in 512 bit hashes.
        /// </summary>
        SHA512,
    }
}

/* eof */