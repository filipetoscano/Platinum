// autogenerated: do NOT edit manually
using Platinum.Configuration;
using System;
using System.ComponentModel;
using System.Configuration;
using KvConfig = Platinum.Configuration.KeyValueConfigurationElement;
using NullableString = Platinum.Configuration.NullableString;

namespace Platinum.Resolver
{
    /// <summary />
    public partial class ResolverConfiguration : ConfigurationSection
    {
        /// <summary>
        /// Gets the configuration instance for section 'platinum/resolver'
        /// </summary>
        public static ResolverConfiguration Current
        {
            get { return AppConfiguration.SectionGet<ResolverConfiguration>( "platinum/resolver" ); }
        }


        /// <summary />
        [ConfigurationProperty( "customResolvers", IsDefaultCollection = false )]
        [ConfigurationCollection( typeof( ResolverDefinition ), AddItemName = "add" )]
        public ConfigurationElementCollection<ResolverDefinition> CustomResolvers
        {
            get { return (ConfigurationElementCollection<ResolverDefinition>) base[ "customResolvers" ]; }
        }
    }


    /// <summary />
    public partial class ResolverDefinition : ConfigurationElement
    {

        /// <summary />
        [ConfigurationProperty( "scheme", IsKey = true, IsRequired = true )]
        public string Scheme
        {
            get { return (string) this[ "scheme" ]; }
            set { this[ "scheme" ] = value; }
        }


        /// <summary />
        [ConfigurationProperty( "moniker", IsRequired = true )]
        public string Moniker
        {
            get { return (string) this[ "moniker" ]; }
            set { this[ "moniker" ] = value; }
        }
    }

}

/* eof */
