using System.Configuration;

namespace Platinum.Configuration
{
    public class KeyValueConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty( "key", IsKey = true, IsRequired = true )]
        public string Key
        {
            get { return (string) this[ "key" ]; }
            set { this[ "key" ] = value; }
        }


        [ConfigurationProperty( "value", IsRequired = true )]
        public string Value
        {
            get { return (string) this[ "value" ]; }
            set { this[ "value" ] = value; }
        }
    }
}

/* eof */