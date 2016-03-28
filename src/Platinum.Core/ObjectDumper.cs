using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Platinum
{
    public class ObjectDumper
    {
        private int _level;
        private readonly int _indentSize;
        private readonly StringBuilder _stringBuilder;
        private readonly List<int> _hashListOfFoundElements;


        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDumper" /> class.
        /// </summary>
        /// <param name="indentSize">When walking through the data graph, indent
        /// the data by so many characters.</param>
        private ObjectDumper( int indentSize )
        {
            _indentSize = indentSize;
            _stringBuilder = new StringBuilder();
            _hashListOfFoundElements = new List<int>();
        }


        private string DumpObject( object element )
        {
            if ( element == null || element is ValueType || element is string || element is Uri )
            {
                Write( FormatValue( element, null ) );
            }
            else
            {
                var objectType = element.GetType();
                if ( typeof( IEnumerable ).IsAssignableFrom( objectType ) == false )
                {
                    Write( "{{{0}}}", objectType.FullName );
                    _hashListOfFoundElements.Add( element.GetHashCode() );
                    _level++;
                }

                var enumerableElement = element as IEnumerable;
                if ( enumerableElement != null )
                {
                    foreach ( object item in enumerableElement )
                    {
                        if ( item is IEnumerable && !(item is string) )
                        {
                            _level++;
                            DumpObject( item );
                            _level--;
                        }
                        else
                        {
                            if ( !AlreadyTouched( item ) )
                                DumpObject( item );
                            else
                                Write( "{{{0}}} <-- bidirectional reference found", item.GetType().FullName );
                        }
                    }
                }
                else
                {
                    MemberInfo[] members = element.GetType().GetMembers( BindingFlags.Public | BindingFlags.Instance );
                    var sorted = members.OrderBy( i => i.Name );

                    foreach ( var memberInfo in sorted )
                    {
                        var fieldInfo = memberInfo as FieldInfo;
                        var propertyInfo = memberInfo as PropertyInfo;

                        if ( fieldInfo == null && propertyInfo == null )
                            continue;

                        var type = fieldInfo != null ? fieldInfo.FieldType : propertyInfo.PropertyType;

                        if ( type.FullName.StartsWith( "System.Reflection.", StringComparison.Ordinal ) == true )
                            continue;

                        XmlElementAttribute[] xmlAttrs = (XmlElementAttribute[]) memberInfo.GetCustomAttributes( typeof( XmlElementAttribute ), false );
                        XmlElementAttribute xmlAttr = (xmlAttrs != null && xmlAttrs.Length > 0) ? xmlAttrs[ 0 ] : null;

                        object value = fieldInfo != null
                                           ? fieldInfo.GetValue( element )
                                           : propertyInfo.GetValue( element, null );

                        if ( type.IsValueType || type == typeof( string ) || type == typeof( Uri ) )
                        {
                            Write( "{0}: {1}", memberInfo.Name, FormatValue( value, xmlAttr ) );
                        }
                        else
                        {
                            var isEnumerable = typeof( IEnumerable ).IsAssignableFrom( type );
                            Write( "{0}: {1}", memberInfo.Name, isEnumerable ? "..." : "{ }" );

                            var alreadyTouched = !isEnumerable && AlreadyTouched( value );
                            _level++;

                            if ( !alreadyTouched )
                                DumpObject( value );
                            else
                                Write( "{{{0}}} <-- bidirectional reference found", value.GetType().FullName );

                            _level--;
                        }
                    }
                }

                if ( !typeof( IEnumerable ).IsAssignableFrom( objectType ) )
                {
                    _level--;
                }
            }

            return _stringBuilder.ToString();
        }


        private bool AlreadyTouched( object value )
        {
            if ( value == null )
                return false;

            var hash = value.GetHashCode();

            for ( var i = 0; i < _hashListOfFoundElements.Count; i++ )
            {
                if ( _hashListOfFoundElements[ i ] == hash )
                    return true;
            }

            return false;
        }


        private void Write( string value, params object[] args )
        {
            var space = new string( ' ', _level * _indentSize );

            if ( args != null )
                value = string.Format( value, args );

            _stringBuilder.AppendLine( space + value );
        }


        private static string FormatValue( object o, XmlElementAttribute xml )
        {
            if ( o == null )
                return "null";

            if ( o is DateTime )
            {
                DateTime dt = (DateTime) o;

                if ( xml != null )
                {
                    if ( xml.DataType == "xsd:date" )
                        return dt.ToString( "yyyy-MM-dd", CultureInfo.InvariantCulture );

                    if ( xml.DataType == "xsd:dateTime" )
                        return dt.ToString( "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture );

                    if ( xml.DataType == "xsd:time" )
                        return dt.ToString( "hh:mm:ss", CultureInfo.InvariantCulture );
                }

                return dt.ToString( "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture );
            }

            if ( o is string )
                return string.Format( "\"{0}\"", o );

            if ( o is char && (char) o == '\0' )
                return string.Empty;

            if ( o is ValueType )
                return o.ToString();

            if ( o is IEnumerable )
                return "...";

            if ( o is Uri )
                return "URI(" + o.ToString() + ")";

            return "{ }";
        }


        /// <summary>
        /// Returns a string representation of the object.
        /// </summary>
        /// <param name="value">Object to dump.</param>
        /// <returns>String representation.</returns>
        public static string Dump( object value )
        {
            return Dump( value, 2 );
        }


        /// <summary>
        /// Returns a string representation of the object.
        /// </summary>
        /// <param name="value">Object to dump.</param>
        /// <param name="indentSize">Indentation level of the output.</param>
        /// <returns>String representation.</returns>
        public static string Dump( object value, int indentSize )
        {
            var instance = new ObjectDumper( indentSize );
            return instance.DumpObject( value );
        }
    }
}

/* eof */