using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Platinum.Globalization
{
    public static class CultureFactory
    {
        private static Dictionary<string, CultureInfo> _cultures = new Dictionary<string, CultureInfo>();


        public static CultureInfo GetCulture( string cultureName )
        {
            #region Validations

            if ( cultureName == null )
                throw new ArgumentNullException( nameof( cultureName ) );

            #endregion

            string name = cultureName.ToLowerInvariant();
            CultureInfo ci;

            if ( _cultures.TryGetValue( name, out ci ) == true )
                return (CultureInfo) ci.Clone();


            /*
             * 
             */
            ci = BuildCulture( name );


            /*
             * 
             */
            if ( _cultures.ContainsKey( name ) == false )
            {
                lock ( _cultures )
                {
                    if ( _cultures.ContainsKey( name ) == false )
                    {
                        _cultures.Add( name, ci );
                    }
                }
            }

            return (CultureInfo) ci.Clone();
        }


        private static CultureInfo BuildCulture( string cultureName )
        {
            #region Validations

            if ( cultureName == null )
                throw new ArgumentNullException( nameof( cultureName ) );

            #endregion


            /*
             * 
             */
            CultureInfo ci;

            try
            {
                ci = new CultureInfo( cultureName );
            }
            catch ( CultureNotFoundException ex )
            {
                throw new GlobalizationException( ER.Factory_Culture_NotFound, ex, cultureName );
            }


            /*
             * 
             */
            CultureOverrideDefinition def = CultureFactoryConfiguration.Current
                .Cultures.First( v => v.Culture.ToLowerInvariant() == cultureName );

            if ( def == null )
                return ci;

            if ( def.DateTimeFormat != null )
            {
                if ( def.DateTimeFormat.ShortDatePattern != null )
                    ci.DateTimeFormat.ShortDatePattern = def.DateTimeFormat.ShortDatePattern;

                if ( def.DateTimeFormat.DateSeparator != null )
                    ci.DateTimeFormat.DateSeparator = def.DateTimeFormat.DateSeparator;

                if ( def.DateTimeFormat.TimeSeparator != null )
                    ci.DateTimeFormat.TimeSeparator = def.DateTimeFormat.TimeSeparator;

                if ( def.DateTimeFormat.FirstDayOfWeek.HasValue == true )
                    ci.DateTimeFormat.FirstDayOfWeek = def.DateTimeFormat.FirstDayOfWeek.Value;
            }

            if ( def.NumberFormat != null )
            {
                if ( def.NumberFormat.NumberDecimalSeparator != null )
                    ci.NumberFormat.NumberDecimalSeparator = def.NumberFormat.NumberDecimalSeparator;

                if ( def.NumberFormat.CurrencyGroupSeparator != null )
                    ci.NumberFormat.CurrencyGroupSeparator = def.NumberFormat.CurrencyGroupSeparator;
            }

            return ci;
        }
    }
}

/* eof */