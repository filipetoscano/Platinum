﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Platinum
{
    [Serializable]
    public class ActorAggregateException : ActorException
    {
        private List<ActorException> _exceptions = new List<ActorException>();


        public ActorAggregateException( IEnumerable<ActorException> exceptions )
            : base( "##collection" )
        {
            if ( exceptions == null )
                return;

            _exceptions.AddRange( exceptions );
        }



        public override string Actor
        {
            get
            {
                if ( _exceptions.Count == 0 )
                    return "##empty";

                return _exceptions[ 0 ].Actor;
            }
        }


        public override int Code
        {
            get
            {
                if ( _exceptions.Count == 0 )
                    return 1;

                return _exceptions[ 0 ].Code;
            }
        }


        public override string Description
        {
            get
            {
                if ( _exceptions.Count == 0 )
                    return "";

                return _exceptions[ 0 ].Description;
            }
        }


        public void Add( ActorException item )
        {
            #region Validations

            if ( item == null )
                throw new ArgumentNullException( nameof( item ) );

            #endregion

            _exceptions.Add( item );
        }


        public void Clear()
        {
            _exceptions.Clear();
        }


        public IEnumerator<ActorException> GetEnumerator()
        {
            return _exceptions.GetEnumerator();
        }


        protected ActorAggregateException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        {
            #region Validations

            if ( info == null )
                throw new ArgumentNullException( nameof( info ) );

            #endregion

            _exceptions = (List<ActorException>) info.GetValue( "Exceptions", typeof( List<ActorException> ) );
        }


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
    }
}

/* eof */