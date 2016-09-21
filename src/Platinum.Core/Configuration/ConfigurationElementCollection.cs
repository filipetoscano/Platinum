using System.Collections.Generic;
using System.Configuration;

namespace Platinum.Configuration
{
    /// <summary>
    /// Generic collection for configuration elements.
    /// </summary>
    /// <typeparam name="T">Type of the repeated element.</typeparam>
    public class ConfigurationElementCollection<T>
        : ConfigurationElementCollection, IEnumerable<T> where T : ConfigurationElement, new()
    {
        private List<T> _elements = new List<T>();


        protected override ConfigurationElement CreateNewElement()
        {
            T newElement = new T();
            _elements.Add( newElement );
            return newElement;
        }


        protected override object GetElementKey( ConfigurationElement element )
        {
            return _elements.Find( e => e.Equals( element ) );
        }


        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to get or set.
        /// </param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public T this[ int index ]
        {
            get { return _elements[ index ]; }
            set { _elements[ index ] = value; }
        }


        /// <summary>
        /// Returns an enumerator that iterates through the properties
        /// list.
        /// </summary>
        /// <returns>
        /// A IEnumerator over the list of elements.
        /// </returns>
        public new IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }
}

/* eof */