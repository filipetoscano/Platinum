using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Platinum
{
    public class DisposableList : List<IDisposable>, IDisposable
    {
        public T Add<T>( Func<T> factory ) where T : IDisposable
        {
            var item = factory();

            base.Add( item );
            return item;
        }


        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }


        [SuppressMessage( "Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations" )]
        protected virtual void Dispose( bool disposing )
        {
            if ( disposing == false )
                return;

            if ( this.Count > 0 )
            {
                List<Exception> exceptions = new List<Exception>();

                foreach ( var disposable in this )
                {
                    try
                    {
                        disposable.Dispose();
                    }
                    catch ( Exception ex )
                    {
                        exceptions.Add( ex );
                    }
                }

                base.Clear();

                if ( exceptions.Count > 0 )
                    throw new AggregateException( exceptions );
            }
        }
    }
}

/* eof */
