using Platinum.Mock.Randomizer;
using System;
using System.Collections.Generic;

namespace Platinum.Mock
{
    /// <summary />
    public static class Randomizers
    {
        /// <summary />
        public static Dictionary<Type, IRandomizer> All
        {
            get
            {
                Dictionary<Type, IRandomizer> r = new Dictionary<Type, IRandomizer>();
                r.Add( typeof( byte[] ), new BinaryRandomizer() );
                r.Add( typeof( Enum ), new EnumRandomizer() );

                r.Add( typeof( bool ), new BoolRandomizer() );

                r.Add( typeof( byte ), new ByteRandomizer() );
                r.Add( typeof( short ), new ShortRandomizer() );
                r.Add( typeof( int ), new IntegerRandomizer() );
                r.Add( typeof( long ), new LongRandomizer() );

                r.Add( typeof( float ), new SingleRandomizer() );
                r.Add( typeof( double ), new DoubleRandomizer() );
                r.Add( typeof( decimal ), new DecimalRandomizer() );

                r.Add( typeof( DateTime ), new DateTimeRandomizer() );
                r.Add( typeof( Date ), new DateRandomizer() );
                r.Add( typeof( Time ), new TimeRandomizer() );

                r.Add( typeof( char ), new CharRandomizer() );
                r.Add( typeof( string ), new StringRandomizer() );

                return r;
            }
        }
    }
}
