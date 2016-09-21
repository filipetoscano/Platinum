using Platinum.Reflection;
using System;
using System.Linq;

namespace Platinum.Validation
{
    /// <summary />
    public static class Validator
    {
        /// <summary />
        public static ValidationResult Validate<T>( T obj )
        {
            #region Validations

            if ( obj == null )
                throw new ArgumentNullException( nameof( obj ) );

            #endregion

            ValidationResult vr = new ValidationResult();

            ValidationContext ctx = new ValidationContext();
            ctx.Path = null;

            Validate( ctx, vr, obj );

            return vr;
        }


        private static void Validate( ValidationContext context, ValidationResult result, object obj )
        {
            #region Validations

            if ( context == null )
                throw new ArgumentNullException( nameof( context ) );

            if ( result == null )
                throw new ArgumentNullException( nameof( result ) );

            if ( obj == null )
                throw new ArgumentNullException( nameof( obj ) );

            #endregion

            /*
             * 
             */
            string root = context.Path;
            Type type = obj.GetType();

            foreach ( var prop in type.GetProperties() )
            {
                Type propType = prop.PropertyType;

                var rules = prop.GetCustomAttributes( false )
                           .Where( x => typeof( IValidationRule ).IsAssignableFrom( x.GetType() ) )
                           .Select( x => (IValidationRule) x )
                           .ToArray();

                if ( propType.IsArray == true )
                {
                    Type arrayType = prop.PropertyType.GetElementType();
                    Array array = (Array) prop.GetValue( obj );

                    context.Path = root;
                    context.Property = prop.Name;

                    foreach ( var rule in rules )
                    {
                        rule.Validate( context, result, array );
                    }

                    if ( array != null && arrayType.IsCustomClass() == true )
                    {
                        for ( int i=0; i<array.Length; i++ )
                        {
                            object av = array.GetValue( i );

                            context.Path = BuildPath( root, prop.Name, i );
                            Validate( context, result, av );
                        }
                    }
                }
                else
                {
                    object pv = prop.GetValue( obj );

                    context.Path = root;
                    context.Property = prop.Name;

                    foreach ( var rule in rules )
                    {
                        rule.Validate( context, result, pv );
                    }

                    if ( pv != null && propType.IsCustomClass() == true )
                    {
                        context.Path = BuildPath( root, prop.Name );
                        Validate( context, result, pv );
                    }
                }
            }
        }


        private static string BuildPath( string root, string name )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( "name" );

            #endregion

            return $"{ root }.{ name }";
        }


        private static string BuildPath( string root, string name, int i )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( "name" );

            #endregion

            return $"{ root }.{ name }[{ i }]";
        }
    }
}
