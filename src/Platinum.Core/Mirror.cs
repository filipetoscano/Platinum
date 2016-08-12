using System;
using System.Reflection;
using Platinum.Reflection;

namespace Platinum
{
    public static class Mirror
    {
        /// <summary>
        /// Performs a mirror of the given instance, projecting its values
        /// to a type of a similar structure.
        /// </summary>
        /// <typeparam name="T">Type of object to be mirrored to.</typeparam>
        /// <param name="obj">Instance to be mirrored from.</param>
        /// <returns>Instance of type T.</returns>
        public static T To<T>( object obj ) where T : class
        {
            if ( obj == null )
                return null;

            return (T) DoMirror( obj, typeof( T ) );
        }


        private static object DoMirror( object obj, Type toType )
        {
            #region Validations

            if ( toType == null )
                throw new ArgumentNullException( nameof( toType ) );

            #endregion

            if ( obj == null )
                return null;


            /*
             * 
             */
            Type srcType = obj.GetType();
            Type dstType = toType;


            /*
             * 
             */
            object mirror = System.Activator.CreateInstance( toType );


            /*
             * 
             */
            foreach ( var dstProp in dstType.GetProperties() )
            {
                PropertyInfo srcProp = srcType.GetProperty( dstProp.Name );

                if ( srcProp == null )
                    continue;

                if ( dstProp.PropertyType.IsArray == true )
                {
                    if ( srcProp.PropertyType.IsArray == false )
                        continue;

                    if ( srcProp.PropertyType.GetElementType().IsCustomClass() == true &&
                         dstProp.PropertyType.GetElementType().IsCustomClass() == false )
                        continue;

                    if ( srcProp.PropertyType.GetElementType().IsCustomClass() == false &&
                         dstProp.PropertyType.GetElementType().IsCustomClass() == true )
                        continue;

                    if ( dstProp.PropertyType.GetElementType().IsCustomClass() )
                    {
                        Array v = (Array) srcProp.GetValue( obj );
                        Array m = Array.CreateInstance( dstProp.PropertyType.GetElementType(), v.Length );

                        for ( int i = 0; i < v.Length; i++ )
                        {
                            var vx = v.GetValue( i );
                            var mx = DoMirror( vx, dstProp.PropertyType.GetElementType() );
                            m.SetValue( mx, i );
                        }

                        dstProp.SetValue( mirror, m );
                    }
                    else
                    {
                        Array v = (Array) srcProp.GetValue( obj );
                        dstProp.SetValue( mirror, v );
                    }
                }
                else if ( dstProp.PropertyType.IsCustomClass() == true )
                {
                    if ( srcProp.PropertyType.IsCustomClass() == false )
                        continue;

                    object v = srcProp.GetValue( obj );
                    object m = DoMirror( v, dstProp.PropertyType );
                    dstProp.SetValue( mirror, m );
                }
                else if ( dstProp.PropertyType.IsEnum == true )
                {
                    if ( srcProp.PropertyType.IsEnum == false )
                        continue;

                    object v = srcProp.GetValue( obj );
                    dstProp.SetValue( mirror, v );
                }
                else
                {
                    if ( dstProp.PropertyType != srcProp.PropertyType )
                        continue;

                    object v = srcProp.GetValue( obj );
                    dstProp.SetValue( mirror, v );
                }
            }

            return mirror;
        }
    }
}
