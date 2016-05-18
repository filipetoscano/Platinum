using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;
using System.Security.Permissions;
using CI = System.Globalization.CultureInfo;

namespace Platinum
{
    [Serializable]
    public abstract class ResxActorException : ActorException
    {
        private string _actor;
        private int _code;
        private string _description;


        public ResxActorException( string code )
            : base( code )
        {
            ResxLoad( code, null );
        }


        public ResxActorException( string code, params object[] args )
            : base( code )
        {
            ResxLoad( code, args );
        }


        public ResxActorException( string code, Exception innerException )
            : base( code, innerException )
        {
            ResxLoad( code, null );
        }


        public ResxActorException( string code, Exception innerException, params object[] args )
            : base( code, innerException )
        {
            ResxLoad( code, args );
        }


        private void ResxLoad( string code, object[] args )
        {
            /*
             * 
             */
            Type type = this.GetType();
            ResxAttribute[] attrs = (ResxAttribute[]) type.GetCustomAttributes( typeof( ResxAttribute ), false );

            if ( attrs == null || attrs.Length == 0 )
            {
                _actor = type.FullName + "#Impl";
                _code = 1000;
                _description = string.Format( CI.InvariantCulture, "Exception class '{0}' is not decorated with [ResxAttribute] attribute.", type.FullName );

                return;
            }

            string resourceName = attrs[ 0 ].ResourceName;


            /*
             * There's no point checking if 'rm' is null or not, since the constructor
             * never fails...
             */
            Assembly assembly = type.Assembly;
            ResourceManager rm = new ResourceManager( resourceName, assembly );


            /*
             * The only check occurs when we finally try to call a resource lookup
             * using the ResourceManager. Only then do we finally know if the RM
             * is pointing to an actual embedded resource.
             */
            string actor;

            try
            {
                actor = rm.GetString( code + "_Actor" );
            }
            catch ( MissingManifestResourceException )
            {
                _actor = type.FullName + "#Impl";
                _code = 1001;
                _description = string.Format( CI.InvariantCulture, "Could not load resources '{1}' from assembly '{0}'.", assembly.FullName, resourceName );

                return;
            }
            catch ( MissingSatelliteAssemblyException )
            {
                _actor = type.FullName + "#Impl";
                _code = 1002;
                _description = string.Format( CI.InvariantCulture, "Could not load resources '{1}' from assembly '{0}'.", assembly.FullName, resourceName );

                return;
            }


            /*
             * 
             */
            string scode = rm.GetString( code + "_Code" );
            string description = rm.GetString( code + "_Description" );

            if ( string.IsNullOrEmpty( actor ) == true && string.IsNullOrEmpty( scode ) == true )
            {
                _actor = type.FullName + "#Impl";
                _code = 1100;
                _description = string.Format( CI.InvariantCulture, "Exception '{0}' has missing values in RM.", code );

                return;
            }


            /*
             * .Code
             */
            int ncode;

            if ( int.TryParse( scode, out ncode ) == false )
            {
                _actor = type.FullName + "#Impl";
                _code = 1200;
                _description = string.Format( CI.InvariantCulture, "Exception '{0}' has a non-numeric code '{1}'.", code, scode );

                return;
            }

            _code = ncode;


            /*
             * .Actor
             * We cannot use App.Name, since that will raise an ResxActorException
             * that the key does not exist -- which will pass through this exact
             * same code section!
             */
            _actor = actor.Replace( "{Application}", App.SafeName );


            /*
             * .Description
             */
            if ( string.IsNullOrEmpty( description ) == true )
                description = "(empty)";

            if ( args != null && args.Length > 0 )
            {
                try
                {
                    _description = string.Format( CultureInfo.InvariantCulture, description, args );
                }
                catch ( FormatException ex )
                {
                    _description = description + " | ex:" + ex.Message; 
                }
            }
            else
            {
                _description = description;
            }
        }


        public override string Actor
        {
            get { return _actor; }
        }


        public override int Code
        {
            get { return _code; }
        }


        public override string Description
        {
            get { return _description; }
        }


        protected ResxActorException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        {
            #region Validations

            if ( info == null )
                throw new ArgumentNullException( "info" );

            #endregion

            _actor = info.GetString( "Actor" );
            _code = info.GetInt32( "Code" );
            _description = info.GetString( "Description" );
        }


        [SecurityPermission( SecurityAction.Demand, SerializationFormatter = true )]
        public override void GetObjectData( SerializationInfo info, StreamingContext context )
        {
            #region Validations

            if ( info == null )
                throw new ArgumentNullException( "info" );

            #endregion

            info.AddValue( "Actor", this.Actor );
            info.AddValue( "Code", this.Code );
            info.AddValue( "Description", this.Description );

            base.GetObjectData( info, context );
        }
    }
}

/* eof */