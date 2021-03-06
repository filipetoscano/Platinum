﻿using Elasticsearch.Net;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Platinum.Metrics
{
    /// <summary>
    /// Writes metric events to Elastic Search.
    /// </summary>
    [Target( "Metrics" )]
    public class ElasticSearchMetricTarget : TargetWithLayout
    {
        private IElasticLowLevelClient _client;

        /// <summary>
        /// Gets or sets the Uri used to connect to ElasticSearch.
        /// </summary>
        [RequiredParameter]
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the name of the elasticsearch index to write to.
        /// </summary>
        [RequiredParameter]
        public Layout Index { get; set; }

        /// <summary>
        /// Gets or sets the document type for the elasticsearch index.
        /// </summary>
        [RequiredParameter]
        public Layout DocumentType { get; set; }

        /// <summary>
        /// Gets or sets if exceptions will be rethrown.
        /// </summary>
        public bool ThrowExceptions { get; set; }


        /// <summary>
        /// Initializes a new instance of <see cref="ElasticSearchMetricTarget" />.
        /// </summary>
        public ElasticSearchMetricTarget()
        {
            Name = "EsMetric";
            Uri = "http://localhost:9200";
            Index = "metrics-${date:format=yyyy.MM.dd}";
            DocumentType = "measure";
        }


        /// <summary>
        /// Initializes the current instance, based on the declarative configuration
        /// in the NLog.config file.
        /// </summary>
        protected override void InitializeTarget()
        {
            base.InitializeTarget();

            var uri = Uri;
            var nodes = uri.Split( new[] { ',' }, StringSplitOptions.RemoveEmptyEntries ).Select( url => new Uri( url ) );
            var connectionPool = new StaticConnectionPool( nodes );

            var config = new ConnectionConfiguration( connectionPool );
            config.DisableAutomaticProxyDetection();

            _client = new ElasticLowLevelClient( config );
        }


        /// <summary>
        /// Writes a single event to ElasticSearch.
        /// </summary>
        /// <param name="logEvent">NLog Event.</param>
        protected override void Write( AsyncLogEventInfo logEvent )
        {
            Write( new[] { logEvent } );
        }


        /// <summary>
        /// Writes a batch of events to ElasticSearch.
        /// </summary>
        /// <param name="logEvents">NLog events.</param>
        protected override void Write( AsyncLogEventInfo[] logEvents )
        {
            SendBatch( logEvents );
        }


        private void SendBatch( IEnumerable<AsyncLogEventInfo> events )
        {
            #region Validations

            if ( events == null )
                throw new ArgumentNullException( nameof( events ) );

            #endregion

            if ( events.Count() == 0 )
                return;

            try
            {
                /*
                 * 
                 */
                var logEvents = events.Select( e => e.LogEvent );
                var payload = ToPostData( logEvents );

                if ( payload == null )
                    return;

                var result = _client.Bulk<byte[]>( payload );

                if ( result.Success == true )
                    return;


                /*
                 * 
                 */
                InternalLogger.Error( "Failed to send metrics: status={0}, message=\"{1}\"",
                    result.HttpStatusCode,
                    result.OriginalException?.Message ?? "No error message. Enable Trace logging for more information." );

                InternalLogger.Trace( "Failed to send metrics: result={0}", result );

                if ( result.OriginalException != null )
                    throw result.OriginalException;
            }
            catch ( Exception ex )
            {
                InternalLogger.Error( "Error while sending metrics: message=\"{0}\"", ex.Message );

                if ( ThrowExceptions == true )
                    throw;
            }
        }


        private object ToPostData( IEnumerable<LogEventInfo> logEvents )
        {
            #region Validations

            if ( logEvents == null )
                throw new ArgumentNullException( nameof( logEvents ) );

            #endregion

            var payload = new List<object>();

            foreach ( var logEvent in logEvents )
            {
                if ( logEvent.Parameters == null || logEvent.Parameters.Count() != 1 )
                    continue;


                /*
                 * 
                 */
                var document = new Dictionary<string, object>
                {
                    { "@timestamp", logEvent.TimeStamp },
                    { "level", logEvent.Level.Name },
                    { "application", App.SafeName },
                    { "environment", App.SafeEnvironment },
                    { "host", Environment.MachineName },
                };


                /*
                 * 
                 */
                string measureName = logEvent.Message;
                object measure = logEvent.Parameters[ 0 ];

                logEvent.Properties[ "measureName" ] = measureName;


                /*
                 * 
                 */
                document.Add( "measure", measureName );


                /*
                 * 
                 */
                Type measureType = measure.GetType();

                foreach ( var p in measureType.GetProperties() )
                {
                    document.Add( p.Name, p.GetValue( measure ) );
                }


                /*
                 * 
                 */
                var index = Index.Render( logEvent ).ToLowerInvariant();
                var type = DocumentType.Render( logEvent );

                payload.Add( new { index = new { _index = index, _type = type } } );
                payload.Add( document );
            }

            if ( payload.Count == 0 )
                return null;

            return payload;
        }
    }
}
