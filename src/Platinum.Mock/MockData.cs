using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Platinum.Mock
{
    public class MockData
    {
        private static MockData _mock;
        private static object _lock = new object();

        public static MockData Current
        {
            get
            {
                if ( _mock == null )
                {
                    lock ( _lock )
                    {
                        if ( _mock == null )
                        {
                            _mock = new MockData();
                        }
                    }
                }

                return _mock;
            }
        }


        private Dictionary<string, NameValueCollection> _functions = new Dictionary<string, NameValueCollection>();
        private Dictionary<string, List<string>> _sets = new Dictionary<string, List<string>>();


        public MockData()
        {
        }



        public T Random<T>( Type randomizer, string name )
        {
            Type type = typeof( T );

            return (T) Random( randomizer, type, name );
        }


        public object Random( Type randomizer, Type type, string name )
        {
            #region Validations

            if ( randomizer == null )
                throw new ArgumentNullException( nameof( randomizer ) );

            if ( type == null )
                throw new ArgumentNullException( nameof( type ) );

            #endregion

            if ( name != null )
            {
                if ( _sets.ContainsKey( name ) == true )
                    return DataPoint( randomizer, type, name );

                if ( _functions.ContainsKey( name ) == true )
                    return FunctionPoint( type, name );
            }

            if ( Randomizers.All.ContainsKey( randomizer ) == true )
                return Randomizers.All[ randomizer ].Random( type );

            return null;
        }


        /// <summary>
        /// Gets a single data-point for the given data point.
        /// </summary>
        /// <param name="type">Expected return type.</param>
        /// <param name="name">Name of data-point set.</param>
        /// <returns></returns>
        private object DataPoint( Type randomizer, Type type, string name )
        {
            #region Validations

            if ( randomizer == null )
                throw new ArgumentNullException( nameof( randomizer ) );

            if ( type == null )
                throw new ArgumentNullException( nameof( type ) );

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion

            List<string> values;

            if ( _sets.TryGetValue( name, out values ) == false )
                throw new MockException( ER.MockData_DataSetNotFound, name );

            string v = values[ R.Next( values.Count ) ];

            return Randomizers.All[ randomizer ].Parse( type, v );
        }



        private object FunctionPoint( Type type, string name )
        {
            #region Validations

            if ( type == null )
                throw new ArgumentNullException( nameof( type ) );

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion


            /*
             * 
             */
            NameValueCollection nvc;

            if ( _functions.TryGetValue( name, out nvc ) == false )
                throw new MockException( ER.MockData_FunctionNotFound, name );

            if ( string.IsNullOrEmpty( nvc[ "Moniker" ] ) == true )
                throw new MockException( ER.MockData_FunctionNoMoniker, name );


            /*
             * 
             */
            IMockFunction fn;

            try
            {
                fn = Activator.Create<IMockFunction>( nvc[ "Moniker" ] );
            }
            catch ( ActorException ex )
            {
                throw new MockException( ER.MockData_FunctionCreateFail, ex, name );
            }


            /*
             * 
             */
            object value;

            try
            {
                value = fn.Random( nvc );
            }
            catch ( Exception ex )
            {
                throw new MockException( ER.MockData_FunctionFail, ex, name );
            }



            /*
             * 
             */
            if ( value == null )
                return null;

            if ( type.IsAssignableFrom( value.GetType() ) == false )
                throw new MockException( ER.MockData_FunctionReturn, name, type.GetType().FullName, value.GetType().FullName );

            return value;
        }
    }
}
