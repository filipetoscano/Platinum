using Jurassic;
using Platinum.Validation.Javascript;
using System;
using System.IO;
using JER = Platinum.Validation.Javascript.ER;

namespace Platinum.Validation
{
    /// <summary />
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = true )]
    public class JavascriptFunctionAttribute : Attribute, IValidationRule
    {
        /// <summary />
        public JavascriptFunctionAttribute( Type baseType, string resourceName )
        {
            #region Validations

            if ( baseType == null )
                throw new ArgumentNullException( nameof( baseType ) );

            if ( resourceName == null )
                throw new ArgumentNullException( nameof( resourceName ) );

            #endregion

            this.BaseType = baseType;
            this.ResourceName = resourceName;
        }


        /// <summary />
        public Type BaseType
        {
            get;
            private set;
        }


        /// <summary />
        public string Name
        {
            get;
            set;
        }


        /// <summary />
        public string ResourceName
        {
            get;
            private set;
        }


        /// <summary />
        public string GetFunctionCode()
        {
            Stream mrs = this.BaseType.Assembly.GetManifestResourceStream( this.BaseType, this.ResourceName );

            if ( mrs == null )
                return null;

            string code;

            using ( StreamReader sr = new StreamReader( mrs ) )
            {
                code = sr.ReadToEnd();
            }

            return code;
        }


        /// <summary />
        public void Validate( ValidationContext context, ValidationResult result, object value )
        {
            #region Validations

            if ( context == null )
                throw new ArgumentNullException( nameof( context ) );

            if ( result == null )
                throw new ArgumentNullException( nameof( result ) );

            #endregion


            /*
             * 
             */
            string function = GetFunctionCode();

            if ( function == null )
            {
                var vex = new JavascriptValidationException( JER.Function_NotFound, context.Path, context.Property, this.ResourceName );
                result.AddError( vex );

                return;
            }


            /*
             * 
             */
            var scriptEngine = new ScriptEngine();
            var js = "var __Run = " + function;

            try
            {
                scriptEngine.Evaluate( js );
            }
            catch ( JavaScriptException ex )
            {
                var vex = new JavascriptValidationException( JER.Function_Evaluate, ex, context.Path, context.Property, this.ResourceName );
                result.AddError( vex );

                return;
            }


            /*
             * 
             */
            var checkOk = scriptEngine.CallGlobalFunction<bool>( "__Run", value );

            if ( checkOk == false )
            {
                var vex = new JavascriptValidationException( JER.Function_Invalid, context.Path, context.Property, this.ResourceName );
                result.AddError( vex );
            }
        }
    }
}
