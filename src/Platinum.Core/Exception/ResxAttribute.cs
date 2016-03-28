using System;

namespace Platinum
{
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = false )]
    public sealed class ResxAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the ResxAttribute attribute.
        /// </summary>
        /// <param name="resourceName">
        /// Fully qualified name of the RESX embedded resource which
        /// contains the actor / code / description properties.
        /// </param>
        public ResxAttribute( string resourceName )
        {
            #region Validations

            if ( resourceName == null )
                throw new ArgumentNullException( "resourceName" );

            #endregion

            this.ResourceName = resourceName;
        }


        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        public string ResourceName
        {
            get;
            private set;
        }
    }
}

/* eof */