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


        private Dictionary<string, Dictionary<string,string>> _functions = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, List<string>> _sets = new Dictionary<string, List<string>>();


        /// <summary>
        /// Initializes a new instance of <see cref="MockData" /> interface.
        /// </summary>
        public MockData()
        {
            LoadData();
        }


        private void LoadData()
        {
            foreach ( var loader in MockConfiguration.Current.Loaders )
            {
                var l = Activator.Create<DataLoader.IDataLoader>( loader.Type );
                var d = l.Load( loader.Settings.AsDictionary() );

                // Merge
                Merge( _functions, d.Functions );
                Merge( _sets, d.Sets );
            }
        }


        private void Merge<T>( Dictionary<string,T> into, Dictionary<string,T> from )
        {
            #region Validations

            if ( into == null )
                throw new ArgumentNullException( nameof( into ) );

            if ( from == null )
                throw new ArgumentNullException( nameof( from ) );

            #endregion

            foreach ( var k in from.Keys )
            {
                if ( into.ContainsKey( k ) == true )
                    into[ k ] = from[ k ];
                else
                    into.Add( k, from[ k ] );
            }
        }


        /// <summary>
        /// Randomizes a value.
        /// </summary>
        /// <typeparam name="T">Expected return-type.</typeparam>
        /// <param name="randomizer">Which randomizer to use.</param>
        /// <param name="name">Expected return-type.</param>
        /// <returns>Random value.</returns>
        public T Random<T>( Type randomizer, string name )
        {
            Type type = typeof( T );

            return (T) Random( randomizer, type, name );
        }


        /// <summary>
        /// Randomizes a value.
        /// </summary>
        /// <param name="randomizer">Which randomizer to use.</param>
        /// <param name="type">Expected return-type.</param>
        /// <param name="name">Name of mock data.</param>
        /// <returnsRandom value.></returns>
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
                object v = null;

                if ( _sets.ContainsKey( name ) == true )
                    v = DataPoint( randomizer, type, name );
                else if ( _functions.ContainsKey( name ) == true )
                    v = FunctionPoint( type, name );

                if ( v != null )
                    return v;
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
        /// <returns>Random data-point.</returns>
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


            /*
             * 
             */
            List<string> values;

            if ( _sets.TryGetValue( name, out values ) == false )
                throw new MockException( ER.MockData_DataSetNotFound, name );

            string v = values[ R.Next( values.Count ) ];


            /*
             * 
             */
            return Randomizers.All[ randomizer ].Parse( type, v );
        }


        /// <summary>
        /// Gets a data-point, as a result of evaluating a mocking
        /// function.
        /// </summary>
        /// <param name="type">Expected return type.</param>
        /// <param name="name">Name of function.</param>
        /// <returns>Random data-point.</returns>
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
            Dictionary<string,string> dict;

            if ( _functions.TryGetValue( name, out dict ) == false )
                throw new MockException( ER.MockData_FunctionNotFound, name );

            if ( string.IsNullOrEmpty( dict[ "type" ] ) == true )
                throw new MockException( ER.MockData_FunctionNoMoniker, name );


            /*
             * 
             */
            IMockFunction fn;

            try
            {
                fn = Activator.Create<IMockFunction>( dict[ "type" ] );
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
                value = fn.Random( dict );
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
