using System;
using System.Runtime.Remoting;

namespace Platinum
{
    public static class Activator
    {
        /// <summary>
        /// Creates an instance of the type whose name is specified, using the
        /// default constructor.
        /// </summary>
        /// <typeparam name="T">Casts instance to this type.</typeparam>
        /// <param name="moniker">The type of the class to be instance.</param>
        /// <returns>Instance of the designated type, casted to type T.</returns>
        public static T Create<T>( Type type )
        {
            #region Validations

            if ( type == null )
                throw new ArgumentNullException( nameof( type ) );

            #endregion

            object o = System.Activator.CreateInstance( type );

            T t;

            try
            {
                t = (T) o;
            }
            catch ( InvalidCastException ex )
            {
                throw new CoreException( ER.Activator_Cast, ex, type.FullName, typeof( T ).FullName );
            }

            return t;
        }


        /// <summary>
        /// Creates an instance of the type whose name is specified, using the
        /// default constructor.
        /// </summary>
        /// <typeparam name="T">Casts instance to this type.</typeparam>
        /// <param name="moniker">The full name of the type.</param>
        /// <returns>Instance of the designated type, casted to type.</returns>
        public static T Create<T>( string moniker )
        {
            #region Validations

            if ( moniker == null )
                throw new ArgumentNullException( nameof( moniker ) );

            #endregion

            string[] parts = moniker.Split( ',' );

            if ( parts.Length < 2 )
                throw new CoreException( ER.Activator_MonikerInvalid, moniker );

            string assemblyName = parts[ 1 ].Trim();
            string typeName = parts[ 0 ].Trim();

            return Create<T>( assemblyName, typeName );
        }


        /// <summary>
        /// Creates an instance of the type whose name is specified, using the named
        /// assembly and default constructor.
        /// </summary>
        /// <typeparam name="T">Casts instance to this type.</typeparam>
        /// <param name="assemblyName">
        /// The name of the assembly where the type named typeName is sought. If assemblyName
        /// is null, the executing assembly is searched.
        /// </param>
        /// <param name="typeName">The name of the preferred type.</param>
        /// <returns>Instance of the designated type, casted to type.</returns>
        public static T Create<T>( string assemblyName, string typeName )
        {
            #region Validations

            if ( assemblyName == null )
                throw new ArgumentNullException( nameof( assemblyName ) );

            if ( typeName == null )
                throw new ArgumentNullException( nameof( typeName ) );

            #endregion


            /*
             * 
             */
            ObjectHandle oh;

            try
            {
                oh = System.Activator.CreateInstance( assemblyName, typeName );
            }
            catch ( MissingMethodException ex )
            {
                throw new CoreException( ER.Activator_Create, ex, assemblyName, typeName );
            }
            catch ( TypeLoadException ex )
            {
                throw new CoreException( ER.Activator_Create, ex, assemblyName, typeName );
            }
            catch ( System.IO.FileNotFoundException ex )
            {
                throw new CoreException( ER.Activator_Create, ex, assemblyName, typeName );
            }
            catch ( MethodAccessException ex )
            {
                throw new CoreException( ER.Activator_Create, ex, assemblyName, typeName );
            }
            catch ( MemberAccessException ex )
            {
                throw new CoreException( ER.Activator_Create, ex, assemblyName, typeName );
            }
            catch ( System.Reflection.TargetInvocationException ex )
            {
                throw new CoreException( ER.Activator_Create, ex, assemblyName, typeName );
            }
            catch ( System.Runtime.InteropServices.InvalidComObjectException ex )
            {
                throw new CoreException( ER.Activator_Create, ex, assemblyName, typeName );
            }
            catch ( System.NotSupportedException ex )
            {
                throw new CoreException( ER.Activator_Create, ex, assemblyName, typeName );
            }
            catch ( System.BadImageFormatException ex )
            {
                throw new CoreException( ER.Activator_Create, ex, assemblyName, typeName );
            }
            catch ( System.IO.FileLoadException ex )
            {
                throw new CoreException( ER.Activator_Create, ex, assemblyName, typeName );
            }


            /*
             * 
             */
            T t;
            object o = oh.Unwrap();

            try
            {
                t = (T) o;
            }
            catch ( InvalidCastException ex )
            {
                throw new CoreException( ER.Activator_Cast, ex, o.GetType().FullName, typeof( T ).FullName );
            }

            return t;
        }
    }
}

/* eof */