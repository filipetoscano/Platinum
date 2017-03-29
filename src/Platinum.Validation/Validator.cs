using Platinum.Reflection;
using System;
using System.Collections.Generic;
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


        /// <summary />
        public static ValidationResult Validate<T, T1>( T obj )
            where T1 : IValidationRuleSet
        {
            return ValidateRuleSet<T>( new Type[ 1 ] { typeof( T1 ) }, obj );
        }


        /// <summary />
        public static ValidationResult Validate<T, T1, T2>( T obj )
            where T1 : IValidationRuleSet
            where T2 : IValidationRuleSet
        {
            return ValidateRuleSet<T>( new Type[ 2 ] { typeof( T1 ), typeof( T2 ) }, obj );
        }


        /// <summary>
        /// Validate rule sets.
        /// </summary>
        public static ValidationResult ValidateRuleSet<T>( Type[] ruleSets, T obj )
        {
            #region Validations

            if ( ruleSets == null )
                throw new ArgumentNullException( nameof( ruleSets ) );

            if ( obj == null )
                throw new ArgumentNullException( nameof( obj ) );

            #endregion

            ValidationResult result = new ValidationResult();

            ValidationContext context = new ValidationContext();
            context.Path = null;


            /*
             * 
             */
            Dictionary<string, FieldRule> fieldRules = new Dictionary<string, FieldRule>();

            foreach ( var type in ruleSets )
            {
                var rs = Activator.Create<IValidationRuleSet>( type );

                foreach ( var r in rs.Fields )
                {
                    if ( fieldRules.ContainsKey( r.Name ) == true )
                        fieldRules[ r.Name ] = r;
                    else
                        fieldRules.Add( r.Name, r );
                }
            }


            /*
             * 
             */
            Type t = typeof( T );

            foreach ( var field in fieldRules )
            {
                context.Path = null;
                context.Property = field.Key;

                var propertyInfo = t.GetProperty( field.Key );

                if ( propertyInfo == null )
                {
                    ValidationException vex = new ValidationException( ER.RuleSet_FieldNotFound, context.Path, context.Property, field.Key, t.FullName );
                    result.AddError( vex );

                    continue;
                }

                var propertyValue = propertyInfo.GetValue( obj );

                foreach ( var fr in field.Value.RuleSets )
                {
                    if ( fr.Condition.IsTrue( obj ) == true )
                    {
                        foreach ( var r in fr.Rules )
                        {
                            r.Validate( context, result, propertyValue );
                        }

                        break;
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Validate declarative/inline property rules.
        /// </summary>
        /// <param name="context">Validation context.</param>
        /// <param name="result">Validation result.</param>
        /// <param name="obj">Object instance being validated.</param>
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
                           .ToList();


                /*
                 * Special handling for enums: assert that the value
                 * of the enum is always a supported value.
                 */
                if ( propType.IsEnum == true )
                    rules.Add( new EnumIsDefinedRule() );

                if ( propType.IsNullable() == true && propType.GetNullableType().IsEnum == true )
                    rules.Add( new EnumIsDefinedRule() );


                /*
                 * 
                 */
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
                        for ( int i = 0; i < array.Length; i++ )
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
