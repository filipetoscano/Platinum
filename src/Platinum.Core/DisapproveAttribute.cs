using System;

namespace Platinum
{
    /// <summary>
    /// Markup used to designate any code which is suspicious and which should
    /// be subject for review.
    /// </summary>
    [AttributeUsage( AttributeTargets.All, Inherited = false )]
    public sealed class ಠ_ಠAttribute : Attribute
    {
        /// <summary />
        public ಠ_ಠAttribute()
        {
        }


        /// <summary />
        public ಠ_ಠAttribute( string reason )
        {
            this.Reason = reason;
        }


        /// <summary>
        /// Gets or sets why this code is suspicious.
        /// </summary>
        public string Reason
        {
            get;
            set;
        }
    }
}

/* eof */