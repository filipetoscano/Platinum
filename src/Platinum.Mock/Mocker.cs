using Platinum.Mock.Randomizer;
using Platinum.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Platinum.Mock
{
    /// <summary>
    /// Mocks an object.
    /// </summary>
    public class Mocker
    {
        private const int ArrayLength = 3;
        private const int TypeRecurse = 2;


        /// <summary>
        /// Returns an instance of the designated type, filled with mock
        /// values based on the metadata markup placed on each of the
        /// properties.
        /// </summary>
        /// <typeparam name="T">
        /// Type of object to return.
        /// </typeparam>
        /// <returns>
        /// Instance of T, but with mocked values.
        /// </returns>
        public static T Mock<T>() where T : class
        {
            T instance = (T) Mock( new Stack<Type>(), typeof( T ) );
            return instance;
        }


        private static object Mock( Stack<Type> stack, Type type )
        {
            #region Validations

            if ( stack == null )
                throw new ArgumentNullException( nameof( stack ) );

            if ( type == null )
                throw new ArgumentNullException( nameof( type ) );

            #endregion


            /*
             * How many of this type in the stack?
             */
            if ( stack.ToArray().Where( x => x == type ).Count() >= TypeRecurse )
                return null;


            /*
             * 
             */
            object obj = System.Activator.CreateInstance( type );
            stack.Push( type );


            /*
             * 
             */
            foreach ( var prop in type.GetProperties() )
            {
                Type propType = prop.PropertyType;


                /*
                 * Yeah, several 'types' are mapped to the same .NET type, and
                 * we need additional information/metadata to know which 'data'
                 * it is!
                 */
                string xsdType = null;
                XmlElementAttribute xea = prop.GetCustomAttribute<XmlElementAttribute>();
                XmlAttributeAttribute xaa = prop.GetCustomAttribute<XmlAttributeAttribute>();

                if ( xea != null )
                    xsdType = xea.DataType;
                else if ( xaa != null )
                    xsdType = xaa.DataType;


                /*
                 * In order to have richer values, we can also markup the
                 * properties with our own metadata. So there :)
                 */
                MockDataAttribute mda = prop.GetCustomAttribute<MockDataAttribute>();


                /*
                 * Breakdown is as follows:
                 * 
                 * - Binary [1]
                 * - Array of
                 *   - Custom class
                 *   - Base type
                 * - Not Array
                 *   - Custom class
                 *   - Base type and Nullables
                 * 
                 * [1] Binary content requires special handling because we want to
                 * handle 'byte[]' -- which is an array -- as a base type.
                 */
                if ( propType == typeof( byte[] ) )
                {
                    object pv = MockValue( prop.Name, propType, null, mda );

                    if ( pv != null )
                        prop.SetValue( obj, pv );
                }
                else if ( propType.IsArray == true )
                {
                    Type arrType = prop.PropertyType.GetElementType();
                    Array arr = Array.CreateInstance( arrType, ArrayLength );

                    for ( int i = 0; i < ArrayLength; i++ )
                    {
                        if ( arrType.IsCustomClass() == true )
                        {
                            object av = Mock( stack, arrType );
                            arr.SetValue( av, i );
                        }
                        else
                        {
                            object av = MockValue( prop.Name, arrType, xsdType, mda );

                            if ( av != null )
                                arr.SetValue( av, i );
                        }
                    }

                    prop.SetValue( obj, arr );
                }
                else
                {
                    if ( propType.IsCustomClass() == true )
                    {
                        object pv = Mock( stack, propType );
                        prop.SetValue( obj, pv );
                    }
                    else
                    {
                        if ( propType.IsNullable() == true )
                            propType = propType.GetGenericArguments()[ 0 ];

                        object pv = MockValue( prop.Name, propType, xsdType, mda );

                        if ( pv != null )
                            prop.SetValue( obj, pv );
                    }
                }
            }

            stack.Pop();
            return obj;
        }


        private static object MockValue( string propertyName, Type type, string xsdType, MockDataAttribute mockData )
        {
            #region Validations

            if ( propertyName == null )
                throw new ArgumentNullException( nameof( propertyName ) );

            if ( type == null )
                throw new ArgumentNullException( nameof( type ) );

            #endregion


            /*
             * Fake types of randomizers, so that they can have different code
             * even though they return the same .NET type return.
             */
            Type randomizerType = type;

            if ( xsdType == "date" )
                randomizerType = typeof( Date );

            if ( xsdType == "time" )
                randomizerType = typeof( Time );

            if ( type.IsEnum == true )
                randomizerType = typeof( Enum );


            /*
             * 
             */
            if ( mockData != null )
            {
                if ( mockData.LikelihoodNull < 0 || mockData.LikelihoodNull > 99 )
                    return null;

                if ( R.Next( 100 ) < mockData.LikelihoodNull )
                    return null;
            }


            /*
             * 
             */
            return MockData.Current.Random( randomizerType, type, mockData?.Name );
        }
    }
}