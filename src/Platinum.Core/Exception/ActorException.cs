using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Platinum
{
    /// <summary />
    [Serializable]
    public abstract class ActorException : Exception
    {
        /// <summary />
        public ActorException( string message )
            : base( message )
        {
        }


        /// <summary />
        public ActorException( string message, Exception innerException )
            : base( message, innerException )
        {
        }


        /// <summary />
        protected ActorException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        {
        }


        /// <summary>
        /// Gets the name of the actor which raised the error/exception.
        /// </summary>
        public abstract string Actor
        {
            get;
        }


        /// <summary>
        /// Gets the error code of the error/exception.
        /// </summary>
        public abstract int Code
        {
            get;
        }


        /// <summary>
        /// Gets the programmer friendly description of the error/exception.
        /// </summary>
        /// <remarks>
        /// No application/business code should ever make use of this value! If
        /// conditions exist over errors/exceptions they should make use of the
        /// .Actor/.Code properties.
        /// </remarks>
        public abstract string Description
        {
            get;
        }


        /// <summary>
        /// Override the default implementation of .ToString(), so that all
        /// relevant information is available in the string representation.
        /// </summary>
        /// <returns>String representation of error.</returns>
        public override string ToString()
        {
            string s = string.Format( CultureInfo.InvariantCulture, "({0}/{1}) {2} [{3}]", this.Actor, this.Code, this.Description, this.Message );

            if ( this.StackTrace != null )
                s = s + "\n" + this.StackTrace;

            if ( this.InnerException != null )
                s = s + "\n\n" + this.InnerException.ToString();

            return s;
        }
    }
}

/* eof */
