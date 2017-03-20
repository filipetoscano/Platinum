using Elasticsearch.Net;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using Platinum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Festo.Logging
{
    /// <summary>
    /// Target which publishes log events to ElasticSearch in a structured
    /// format.
    /// </summary>
    [Target( "ElasticSearch" )]
    public class ElasticSearchTarget : TargetWithLayout
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
        /// Creates a new instance of the <see cref="ElasticSearchTarget" /> class.
        /// </summary>
        public ElasticSearchTarget()
        {
            Name = "ElasticSearch";
            Uri = "http://localhost:9200";
            DocumentType = "logevent";
            Index = "logstash-${date:format=yyyy.MM.dd}";
        }


        /// <summary>
        /// Initializes the current instance.
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
        /// <param name="logEvents">Array of NLog Event.</param>
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
                var logEvents = events.Select( e => e.LogEvent );
                var payload = ToPostData( logEvents );
                var result = _client.Bulk<byte[]>( payload );

                if ( result.Success == true )
                    return;


                /*
                 * 
                 */
                InternalLogger.Error( "Failed to send log messages to elasticsearch: status={0}, message=\"{1}\"",
                    result.HttpStatusCode,
                    result.OriginalException?.Message ?? "No error message. Enable Trace logging for more information." );

                InternalLogger.Trace( "Failed to send log messages to elasticsearch: result={0}", result );

                if ( result.OriginalException != null )
                    throw result.OriginalException;
            }
            catch ( Exception ex )
            {
                InternalLogger.Error( "Error while sending log messages to elasticsearch: message=\"{0}\"", ex.Message );

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
                var document = new Dictionary<string, object>
                {
                    { "@timestamp", logEvent.TimeStamp },
                    { "level", logEvent.Level.Name },
                    { "message", Layout.Render( logEvent ) },
                    { "application", App.SafeName },
                    { "environment", App.SafeEnvironment },
                    { "host", Environment.MachineName },
                };

                if ( logEvent.Exception != null )
                {
                    if ( logEvent.Exception is ActorException )
                    {
                        ActorException ae = (ActorException) logEvent.Exception;

                        document[ "message" ] = ae.Description;
                        document.Add( "exid", ae.Message );
                        document.Add( "actor", ae.Actor );
                        document.Add( "code", ae.Code );
                        document.Add( "exception", ae.ToString() );
                    }
                    else
                    {
                        document.Add( "exception", logEvent.Exception.ToString() );
                    }
                }

                var index = Index.Render( logEvent ).ToLowerInvariant();
                var type = DocumentType.Render( logEvent );

                payload.Add( new { index = new { _index = index, _type = type } } );
                payload.Add( document );
            }

            return payload;
        }
    }
}
