using System;

namespace Platinum.VisualStudio
{
    /// <summary />
    public class ToolException : Exception
    {
        /// <summary />
        public ToolException()
        {
        }


        /// <summary />
        public ToolException( string message )
            : base( message )
        {
        }


        /// <summary />
        public ToolException( string message, Exception innerException )
            : base( message, innerException )
        {
        }
    }
}
