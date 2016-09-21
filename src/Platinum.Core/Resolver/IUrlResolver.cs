using System;

namespace Platinum.Resolver
{
    /// <summary />
    public interface IUrlResolver
    {
        /// <summary>
        /// Resolves the absolute URI from the base and relative URIs.
        /// </summary>
        /// <param name="baseUri">The base URI used to resolve the relative URI.</param>
        /// <param name="relativeUri">The URI to resolve. The URI can be absolute or relative.
        /// If absolute, this value effectively replaces the baseUri value. If relative, it
        /// combines with the baseUri to make an absolute URI.</param>
        /// <returns>A <see cref="Uri"/> representing the absolute URI, or null if the relative
        /// URI cannot be resolved.</returns>
        Uri ResolveUri( Uri baseUri, string relativeUri );
    }
}