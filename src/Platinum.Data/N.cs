using System;

namespace Platinum.Data
{
    /// <summary>
    /// Null values, but with type information.
    /// </summary>
    public class N
    {
        /// <summary>
        /// Null value, but with type information about String.
        /// </summary>
        public string String
        {
            get { return null; }
        }


        /// <summary>
        /// Null value, but with type information about Guid.
        /// </summary>
        public Guid? Guid
        {
            get { return null; }
        }


        /// <summary>
        /// Null value, but with type information about bool.
        /// </summary>
        public bool? Boolean
        {
            get { return null; }
        }


        /// <summary>
        /// Null value, but with type information about Int16.
        /// </summary>
        public short? Int16
        {
            get { return null; }
        }


        /// <summary>
        /// Null value, but with type information about Int32.
        /// </summary>
        public int? Int32
        {
            get { return null; }
        }


        /// <summary>
        /// Null value, but with type information about Decimal.
        /// </summary>
        public decimal? Decimal
        {
            get { return null; }
        }


        /// <summary>
        /// Null value, but with type information about DateTime.
        /// </summary>
        public DateTime? DateTime
        {
            get { return null; }
        }


        /// <summary>
        /// Null value, but with type information about byte[].
        /// </summary>
        public byte[] Binary
        {
            get { return null; }
        }
    }
}
