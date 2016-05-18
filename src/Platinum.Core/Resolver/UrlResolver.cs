using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Platinum.Resolver
{
    public class UrlResolver : XmlUrlResolver
    {
        private bool _collect;
        private List<Uri> _uri;


        /// <summary>
        /// Initializes a new instance of the UrlResolver class, defaulting
        /// to not collecting URI which are resolved by the instance.
        /// </summary>
        public UrlResolver()
        {
            _collect = false;
        }


        /// <summary>
        /// Initializes a new instance of the UrlResolver class.
        /// </summary>
        /// <param name="collect">Whether to collect URI which are resolved by
        /// this instance.</param>
        public UrlResolver( bool collect )
        {
            _collect = collect;

            if ( collect == true )
                _uri = new List<Uri>();
        }


        /// <summary>
        /// Returns a list of all collected URI which use the file scheme.
        /// </summary>
        /// <returns>List of file URI which were collected by resolver. If the
        /// resolver was configured with collection off, then returns null.</returns>
        public List<string> GetFileUri()
        {
            if ( _collect == false )
                return null;

            return _uri
                .FindAll( i => i.Scheme == "file" )
                .Select( i => i.PathAndQuery )
                .ToList();
        }


        /// <summary>
        /// Maps a URI to an object containing the actual resource.
        /// </summary>
        /// <param name="absoluteUri">The URI returned from <see cref="ResolveUri"/>.</param>
        /// <param name="role">The current implementation does not use this parameter when resolving URIs.
        /// This is provided for future extensibility purposes. For example, this can
        /// be mapped to the xlink: role and used as an implementation specific argument
        /// in other scenarios.</param>
        /// <param name="ofObjectToReturn">The type of object to return. The current implementation
        /// only returns <see cref="Stream"/> objects.</param>
        /// <returns>A <see cref="Stream"/> object or null if a type other than stream is specified.</returns>
        public override object GetEntity( Uri absoluteUri, string role, Type ofObjectToReturn )
        {
            if ( absoluteUri == null )
                return base.GetEntity( absoluteUri, role, ofObjectToReturn );

            if ( absoluteUri.Scheme == "assembly" )
            {
                if ( absoluteUri.Segments.Length != 2 )
                    throw new ResolverException( ER.UrlResolver_Assembly_SegmentCount, absoluteUri.OriginalString );

                string assemblyName = absoluteUri.Segments[ 0 ].TrimEnd( '/' );
                string resourceName = absoluteUri.Segments[ 1 ].TrimEnd( '/' );

                Assembly assembly = Assembly.Load( assemblyName );
                Stream stream = assembly.GetManifestResourceStream( resourceName );

                if ( stream == null )
                    throw new ResolverException( ER.UrlResolver_Assembly_ResourceNotFound, assemblyName, resourceName );

                return stream;
            }
            else
            {
                return base.GetEntity( absoluteUri, role, ofObjectToReturn );
            }
        }


        /// <summary>
        /// Maps a URI to an object containing the actual resource.
        /// </summary>
        /// <param name="absoluteUri">The URI returned from <see cref="ResolveUri"/>.</param>
        /// <returns>A <see cref="Stream"/> object.</returns>
        public Stream GetEntity( Uri absoluteUri )
        {
            return (Stream) GetEntity( absoluteUri, null, typeof( Stream ) );
        }


        /// <summary>
        /// Resolves the absolute URI from the base and relative URIs.
        /// </summary>
        /// <param name="baseUri">The base URI used to resolve the relative URI.</param>
        /// <param name="relativeUri">The URI to resolve. The URI can be absolute or relative.
        /// If absolute, this value effectively replaces the baseUri value. If relative, it
        /// combines with the baseUri to make an absolute URI.</param>
        /// <returns>A <see cref="Uri"/> representing the absolute URI, or null if the relative
        /// URI cannot be resolved.</returns>
        public override Uri ResolveUri( Uri baseUri, string relativeUri )
        {
            IUrlResolver resolver = GetResolver( baseUri, relativeUri );
            Uri uri;

            if ( resolver == null )
                uri = base.ResolveUri( baseUri, relativeUri );
            else
                uri = resolver.ResolveUri( baseUri, relativeUri );

            if ( _collect == true )
            {
                lock ( this )
                {
                    _uri.Add( uri );
                }
            }

            return uri;
        }


        private static IUrlResolver GetResolver( Uri baseUri, string relativeUri )
        {
            if ( baseUri == null )
                return null;


            /*
             * 
             */
            ResolverDefinition rc = ResolverConfiguration.Current.CustomResolvers
                .SingleOrDefault( i => i.Scheme == baseUri.Scheme );

            if ( rc == null )
                return null;

            if ( rc.ResolverInstance != null )
                return rc.ResolverInstance;


            /*
             * 
             */
            IUrlResolver resolver;

            try
            {
                resolver = Activator.Create<IUrlResolver>( rc.Moniker );
            }
            catch ( ActorException ex )
            {
                throw new ResolverException( ER.UrlResolver_InvalidCustomResolver, ex, rc.Scheme, rc.Moniker );
            }


            /*
             * 
             */
            if ( rc.ResolverInstance != null )
            {
                lock ( rc.ResolverInstance )
                {
                    if ( rc.ResolverInstance != null )
                    {
                        rc.ResolverInstance = resolver;
                    }
                }
            }

            return resolver;
        }
    }
}

/* eof */