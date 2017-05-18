using NLog.Common;
using NLog.Targets;
using System;
using System.Linq;

namespace Platinum.Metrics
{
    /// <summary>
    /// Writes metric events to Console.
    /// </summary>
    [Target( "ConsoleMetrics" )]
    public class ConsoleMetricTarget : Target
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ConsoleMetricTarget" />
        /// </summary>
        public ConsoleMetricTarget()
        {
        }


        /// <summary>
        /// Writes a single event to the console.
        /// </summary>
        /// <param name="logEvent">NLog Event.</param>
        protected override void Write( AsyncLogEventInfo logEvent )
        {
            Write( new[] { logEvent } );
        }


        /// <summary>
        /// Writes a batch of events to the console.
        /// </summary>
        /// <param name="logEvents">NLog events.</param>
        protected override void Write( AsyncLogEventInfo[] logEvents )
        {
            if ( logEvents.Length == 0 )
                return;

            foreach ( var logEvent in logEvents.Select( x => x.LogEvent ) )
            {
                if ( logEvent.Parameters == null || logEvent.Parameters.Count() != 1 )
                    continue;


                /*
                 * 
                 */
                string measureName = logEvent.Message;
                object measure = logEvent.Parameters[ 0 ];
                Type measureType = measure.GetType();

                Console.WriteLine( "Measure {0}", measureName );

                foreach ( var p in measureType.GetProperties() )
                {
                    Console.WriteLine( "    {0} = {1}", p.Name, p.GetValue( measure ) );
                }
            }
        }
    }
}
