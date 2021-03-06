﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace Platinum
{
    /// <summary />
    [Serializable]
    public class ActorAggregateException : ActorException
    {
        private List<ActorException> _exceptions = new List<ActorException>();


        /// <summary />
        public ActorAggregateException( IEnumerable<ActorException> exceptions )
            : base( "##collection" )
        {
            if ( exceptions == null )
                return;

            _exceptions.AddRange( exceptions );
        }


        /// <summary>
        /// Gets the name of the actor of the first error/exception.
        /// </summary>
        public override string Actor
        {
            get
            {
                if ( _exceptions.Count == 0 )
                    return "##empty";

                return _exceptions[ 0 ].Actor;
            }
        }


        /// <summary>
        /// Gets the error code of the first error/exception.
        /// </summary>
        public override int Code
        {
            get
            {
                if ( _exceptions.Count == 0 )
                    return 1;

                return _exceptions[ 0 ].Code;
            }
        }


        /// <summary>
        /// Gets the programmer friendly description of the first error/exception.
        /// </summary>
        public override string Description
        {
            get
            {
                if ( _exceptions.Count == 0 )
                    return "";

                return _exceptions[ 0 ].Description;
            }
        }


        /// <summary />
        public void Add( ActorException item )
        {
            #region Validations

            if ( item == null )
                throw new ArgumentNullException( nameof( item ) );

            #endregion

            _exceptions.Add( item );
        }


        /// <summary />
        public void Clear()
        {
            _exceptions.Clear();
        }


        /// <summary />
        public IEnumerator<ActorException> GetEnumerator()
        {
            return _exceptions.GetEnumerator();
        }


        /// <summary />
        protected ActorAggregateException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        {
            #region Validations

            if ( info == null )
                throw new ArgumentNullException( nameof( info ) );

            #endregion

            _exceptions = (List<ActorException>) info.GetValue( "Exceptions", typeof( List<ActorException> ) );
        }


        /// <summary />
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter = true )]
        public override void GetObjectData( SerializationInfo info, StreamingContext context )
        {
            #region Validations

            if ( info == null )
                throw new ArgumentNullException( nameof( info ) );

            #endregion

            info.AddValue( "Exceptions", _exceptions );

            base.GetObjectData( info, context );
        }


        /// <summary>
        /// Override the default implementation of .ToString(), so that all
        /// relevant information is available in the string representation.
        /// </summary>
        /// <returns>String representation of error.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int counter = 0;

            foreach ( ActorException iex in this )
            {
                if ( counter > 0 )
                    sb.Append( "\n\n" );

                sb.AppendFormat( CultureInfo.InvariantCulture, "--- Inner exception #{0}\n", counter++ );
                sb.Append( iex.ToString() );
            }

            return sb.ToString();
        }
    }
}

/* eof */