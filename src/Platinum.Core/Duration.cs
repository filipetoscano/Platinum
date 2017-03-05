using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Platinum
{
    /// <summary>
    /// Indicates the sign of a Duration time interval.
    /// </summary>
    public enum DurationDirection
    {
        /// <summary>
        /// The duration represents a positive direction in time.
        /// </summary>
        Positive,

        /// <summary>
        /// The duration represents a negative direction in time.
        /// </summary>
        Negative
    }


    /// <summary>
    /// Represents a time interval.
    /// </summary>
    [Serializable]
    [XmlSchemaProviderAttribute( "SchemaGet" )]
    public struct Duration : IComparable<Duration>, IXmlSerializable
    {
        // 
        private const string RegularExpression = @"^(?<direction>-)?P(?:(?:(?<years>\d+)Y)?(?:(?<months>\d+)M)?(?:(?<days>\d+)D)?)?(?:T(?:(?<hours>\d+)H)?(?:(?<minutes>\d+)M)?(?:(?<seconds>\d+(?:.\d+)?)S)?)?$";
        private static Regex _reg = new Regex( RegularExpression, RegexOptions.Compiled );

        /// <summary>
        /// Gets or sets wether the duration is positive or negative.
        /// </summary>
        public DurationDirection Direction
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the number of years in the duration.
        /// </summary>
        public int Years
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the number of months in the duration.
        /// </summary>
        public int Months
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the number of days in the duration.
        /// </summary>
        public int Days
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the number of hours in the duration.
        /// </summary>
        public int Hours
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the number of minutes in the duration.
        /// </summary>
        public int Minutes
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the number of seconds in the duration.
        /// </summary>
        public int Seconds
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the number of milli seconds in the duration.
        /// </summary>
        public int Milliseconds
        {
            get;
            set;
        }


        /// <summary>
        /// Initializes a new instance of the Duration class.
        /// </summary>
        /// <param name="direction">Direction of duration.</param>
        /// <param name="years">Number of years. Must be a positive value.</param>
        /// <param name="months">Number of months. Must be a positive value.</param>
        /// <param name="days">Number of days. Must be a positive value.</param>
        /// <param name="hours">Number of hours. Must be a positive value.</param>
        /// <param name="minutes">Number of minutes. Must be a positive value.</param>
        /// <param name="seconds">Number of seconds. Must be a positive value.</param>
        /// <param name="milliseconds">Number of milli-seconds. Must be a positive value.</param>
        public Duration( DurationDirection direction, int years, int months, int days, int hours, int minutes, int seconds, int milliseconds )
            : this()
        {
            #region Validations

            if ( years < 0 )
                throw new ArgumentOutOfRangeException( nameof( years ), years, "Value must be non-negative" );

            if ( months < 0 )
                throw new ArgumentOutOfRangeException( nameof( months ), months, "Value must be non-negative" );

            if ( days < 0 )
                throw new ArgumentOutOfRangeException( nameof( days ), days, "Value must be non-negative" );

            if ( hours < 0 )
                throw new ArgumentOutOfRangeException( nameof( hours ), hours, "Value must be non-negative" );

            if ( minutes < 0 )
                throw new ArgumentOutOfRangeException( nameof( minutes ), minutes, "Value must be non-negative" );

            if ( seconds < 0 )
                throw new ArgumentOutOfRangeException( nameof( seconds ), seconds, "Value must be non-negative" );

            if ( milliseconds < 0 )
                throw new ArgumentOutOfRangeException( nameof( milliseconds ), milliseconds, "Value must be non-negative" );

            #endregion

            this.Direction = direction;
            this.Years = years;
            this.Months = months;
            this.Days = days;
            this.Hours = hours;
            this.Minutes = minutes;
            this.Seconds = seconds;
            this.Milliseconds = milliseconds;
        }


        /// <summary>
        /// Initializes a new instance of the Duration class.
        /// </summary>
        /// <param name="years">Number of years. Must be a positive value.</param>
        /// <param name="months">Number of months. Must be a positive value.</param>
        /// <param name="days">Number of days. Must be a positive value.</param>
        /// <param name="hours">Number of hours. Must be a positive value.</param>
        /// <param name="minutes">Number of minutes. Must be a positive value.</param>
        /// <param name="seconds">Number of seconds. Must be a positive value.</param>
        /// <param name="milliseconds">Number of milli-seconds. Must be a positive value.</param>
        public Duration( int years, int months, int days, int hours, int minutes, int seconds, int milliseconds )
            : this( DurationDirection.Positive, years, months, days, hours, minutes, seconds, milliseconds )
        {
        }


        /// <summary>
        /// Initializes a new instance of the Duration class.
        /// </summary>
        /// <param name="direction">Direction of duration.</param>
        /// <param name="years">Number of years. Must be a positive value.</param>
        /// <param name="months">Number of months. Must be a positive value.</param>
        /// <param name="days">Number of days. Must be a positive value.</param>
        public Duration( DurationDirection direction, int years, int months, int days )
            : this( direction, years, months, days, 0, 0, 0, 0 )
        {
        }


        /// <summary>
        /// Initializes a new instance of the Duration class.
        /// </summary>
        /// <param name="years">Number of years. Must be a positive value.</param>
        /// <param name="months">Number of months. Must be a positive value.</param>
        /// <param name="days">Number of days. Must be a positive value.</param>
        public Duration( int years, int months, int days )
            : this( DurationDirection.Positive, years, months, days, 0, 0, 0, 0 )
        {
        }


        /// <summary>
        /// Returns the string representation of the current instance.
        /// </summary>
        /// <returns>ISO representation of the current duration.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if ( Direction == DurationDirection.Negative )
                sb.Append( "-" );

            sb.Append( "P" );

            // YMD
            if ( Years > 0 )
                sb.AppendFormat( "{0}Y", Years );

            if ( Months > 0 )
                sb.AppendFormat( "{0}M", Months );

            if ( Days > 0 )
                sb.AppendFormat( "{0}D", Days );

            if ( Hours > 0 || Minutes > 0 || Seconds > 0 || Milliseconds > 0 )
            {
                // As per spec, the T seperator is *always* necessary if H,M or S parts
                // are non-zero.
                sb.Append( "T" );

                if ( Hours > 0 )
                    sb.AppendFormat( "{0}H", Hours );

                if ( Minutes > 0 )
                    sb.AppendFormat( "{0}M", Minutes );

                if ( Seconds > 0 || Milliseconds > 0 )
                {
                    if ( Seconds > 0 )
                        sb.AppendFormat( "{0}", Seconds );
                    else
                        sb.Append( "0" );

                    if ( Milliseconds > 0 )
                    {
                        sb.Append( "." );
                        sb.Append( Milliseconds.ToString( CultureInfo.InvariantCulture ).PadLeft( 3, '0' ).TrimEnd( '0' ) );
                    }

                    sb.Append( "S" );
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// Returns an instance of <see cref="TimeSpan"/> which represents the
        /// current duration value. If the current value has a Years or Month
        /// component, then this conversion will fail since TimeSpan has no
        /// equivalent properties.
        /// </summary>
        /// <returns>Instance of <see cref="TimeSpan"/>.</returns>
        public TimeSpan ToTimeSpan()
        {
            if ( this.Years > 0 || this.Months > 0 )
                throw new ArgumentException( "TimeSpan does not support Year and Month offsets.", "value" );

            return new TimeSpan( this.Days, this.Hours, this.Minutes, this.Seconds, this.Milliseconds );
        }


        /// <summary>
        /// Returns an instance of <see cref="TimeSpan"/> which represents the
        /// current duration value, with the Years and Months components being
        /// converted to the number of days as indicated.
        /// </summary>
        /// <param name="yearLength">Number of days to be counted per year, default 365.</param>
        /// <param name="monthLength">Number of days to be counted per month, default 30.</param>
        /// <returns>Instance of <see cref="TimeSpan"/>.</returns>
        public TimeSpan ToTimeSpanEquiv( int yearLength = 365, int monthLength = 30 )
        {
            #region Validations

            if ( yearLength < 0 )
                throw new ArgumentOutOfRangeException( nameof( yearLength ), yearLength, "Value must be non-negative" );

            if ( monthLength < 0 )
                throw new ArgumentOutOfRangeException( nameof( monthLength ), monthLength, "Value must be non-negative" );

            #endregion

            int yy = this.Years * yearLength;
            int MM = this.Months * monthLength;
            int dd = this.Days + MM + yy;

            return new TimeSpan( dd, this.Hours, this.Minutes, this.Seconds, this.Milliseconds );
        }


        /// <summary>
        /// Constructs a Duration from a duration indicated by the specified
        /// string.
        /// </summary>
        /// <param name="value">A string.</param>
        /// <returns>A Duration that corresponds to s.</returns>
        [SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "s" )]
        public static Duration Parse( string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            Match m = _reg.Match( value );

            if ( m.Success == false )
                throw new ArgumentException( "Value is not a valid xsd:duration.", "value" );

            int YY = 0, MM = 0, DD = 0, hh = 0, mm = 0;
            float secs = 0;
            DurationDirection direction = DurationDirection.Positive;

            if ( m.Groups[ "direction" ].Value.Length > 0 )
                direction = DurationDirection.Negative;

            if ( m.Groups[ "years" ].Value.Length > 0 )
                YY = int.Parse( m.Groups[ "years" ].Value, CultureInfo.InvariantCulture );

            if ( m.Groups[ "months" ].Value.Length > 0 )
                MM = int.Parse( m.Groups[ "months" ].Value, CultureInfo.InvariantCulture );

            if ( m.Groups[ "days" ].Value.Length > 0 )
                DD = int.Parse( m.Groups[ "days" ].Value, CultureInfo.InvariantCulture );

            if ( m.Groups[ "hours" ].Value.Length > 0 )
                hh = int.Parse( m.Groups[ "hours" ].Value, CultureInfo.InvariantCulture );

            if ( m.Groups[ "minutes" ].Value.Length > 0 )
                mm = int.Parse( m.Groups[ "minutes" ].Value, CultureInfo.InvariantCulture );

            if ( m.Groups[ "seconds" ].Value.Length > 0 )
                secs = float.Parse( m.Groups[ "seconds" ].Value, CultureInfo.InvariantCulture );

            int ss = (int) secs;
            int ms = (int) System.Math.Round( (secs - ss) * 1000.0 );

            Duration duration = new Duration( direction, YY, MM, DD, hh, mm, ss, ms );
            return duration;
        }


        /// <summary>
        /// Constructs a TimeSpan from a duration indicated by the specified
        /// string.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <returns>A TimeSpan that corresponds to value.</returns>
        public static TimeSpan ParseTimeSpan( string value )
        {
            Duration duration = Duration.Parse( value );

            if ( duration.Years > 0 || duration.Months > 0 )
                throw new ArgumentException( "TimeSpan does not support Year and Month offsets.", "value" );

            TimeSpan ts = new TimeSpan( duration.Days, duration.Hours, duration.Minutes, duration.Seconds, duration.Milliseconds );

            return ts;
        }


        /// <summary>
        /// Adds a duration to a start date.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="duration">A duration.</param>
        /// <returns>The resulting end date.</returns>
        public static DateTime AddDuration( DateTime startDate, Duration duration )
        {
            DateTime endDate = startDate;

            if ( duration.Direction == DurationDirection.Positive )
            {
                endDate = endDate.AddYears( duration.Years );
                endDate = endDate.AddMonths( duration.Months );
                endDate = endDate.AddDays( duration.Days );
                endDate = endDate.AddHours( duration.Hours );
                endDate = endDate.AddMinutes( duration.Minutes );
                endDate = endDate.AddSeconds( duration.Seconds );
                endDate = endDate.AddMilliseconds( duration.Milliseconds );
            }
            else
            {
                endDate = endDate.AddYears( -duration.Years );
                endDate = endDate.AddMonths( -duration.Months );
                endDate = endDate.AddDays( -duration.Days );
                endDate = endDate.AddHours( -duration.Hours );
                endDate = endDate.AddMinutes( -duration.Minutes );
                endDate = endDate.AddSeconds( -duration.Seconds );
                endDate = endDate.AddMilliseconds( -duration.Milliseconds );
            }

            return endDate;
        }


        /// <summary>
        /// Adds a duration to a start date.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="duration">A duration in string format.</param>
        /// <returns>The resulting end date.</returns>
        public static DateTime AddDuration( DateTime startDate, string duration )
        {
            Duration d = Duration.Parse( duration );
            return Duration.AddDuration( startDate, d );
        }


        /// <summary>
        /// Adds two duration values.
        /// </summary>
        /// <param name="d1">First duration.</param>
        /// <param name="d2">Second duration.</param>
        /// <returns>Sum of both durations.</returns>
        public static Duration operator +( Duration d1, Duration d2 )
        {
            if ( d1.Direction != d2.Direction )
                throw new ArgumentException( "When adding durations, both arguments must have same direction.", "d2" );

            /*
             * Our present implementation of addition is going to be very simple,
             * because we restrict it to requiring that both durations have the
             * same strict direction.
             */
            int yy = d1.Years + d2.Years;
            int MM = d1.Months + d2.Months;
            int dd = d1.Days + d2.Days;
            int hh = d1.Hours + d2.Hours;
            int mm = d1.Minutes + d2.Minutes;
            int ss = d1.Seconds + d2.Seconds;
            int ms = d1.Milliseconds + d2.Seconds;

            return new Duration( d1.Direction, yy, MM, dd, hh, mm, ss, ms );
        }


        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared.
        /// The return value has the following meanings:
        /// (a) less than zero, this object is strictly smaller than the other;
        /// (b) greater than zero, this object is strictly greater than the other;
        /// (c) zero, both objects have the same value.
        /// </returns>
        public int CompareTo( Duration other )
        {
            /*
             * If the durations have different directions, then we don't
             * need to compare the values are all. If this means anything
             * semantically, that's an entirely different discussion :)
             */
            if ( this.Direction == DurationDirection.Negative && other.Direction == DurationDirection.Positive )
                return -1;

            if ( this.Direction == DurationDirection.Positive && other.Direction == DurationDirection.Negative )
                return 1;


            /*
             * For positive durations, the greater one has the highest
             * values per component: therefore, c > 0.
             * 
             * But for negative durations, the greater one has the smallest
             * values per component: therefore, c < 0.
             */
            int c = 1;

            if ( this.Direction == DurationDirection.Negative )
                c = -1;

            // Years
            if ( this.Years < other.Years )
                return -c;

            if ( this.Years > other.Years )
                return c;

            // Months
            if ( this.Months < other.Months )
                return -c;

            if ( this.Months > other.Months )
                return c;

            // Days
            if ( this.Days < other.Days )
                return -c;

            if ( this.Days > other.Days )
                return c;

            // Hours
            if ( this.Hours < other.Hours )
                return -c;

            if ( this.Hours > other.Hours )
                return c;

            // Minutes
            if ( this.Minutes < other.Minutes )
                return -c;

            if ( this.Minutes > other.Minutes )
                return c;

            // Seconds
            if ( this.Seconds < other.Seconds )
                return -c;

            if ( this.Seconds > other.Seconds )
                return c;

            // Milliseconds
            if ( this.Milliseconds < other.Milliseconds )
                return -c;

            if ( this.Milliseconds > other.Milliseconds )
                return c;

            return 0;
        }



        /// <summary>
        /// Reserved, and not used. Instead, since we need to describe the schema
        /// of this value, we use the [XmlSchemaProviderAttribute] on the struct
        /// which instructs the serialization framework to make use of SchemaGet
        /// method.
        /// </summary>
        /// <returns>Always null.</returns>
        public XmlSchema GetSchema()
        {
            return null;
        }


        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">
        /// The <see cref="XmlReader" /> stream from which the object is deserialized.
        /// </param>
        public void ReadXml( XmlReader reader )
        {
            string duration = reader.ReadElementContentAsString();

            Duration d = Duration.Parse( duration );

            this.Direction = d.Direction;
            this.Years = d.Years;
            this.Months = d.Months;
            this.Days = d.Days;
            this.Hours = d.Hours;
            this.Minutes = d.Minutes;
            this.Seconds = d.Seconds;
            this.Milliseconds = d.Milliseconds;
        }


        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="XmlWriter" /> stream to which the object is serialized.
        /// </param>
        public void WriteXml( XmlWriter writer )
        {
            writer.WriteString( this.ToString() );
        }


        /// <summary>
        /// Returns the XML Schema definition for the present data-type.
        /// </summary>
        /// <param name="xs">Schema set.</param>
        /// <returns>
        /// XML schema representation for the present element.
        /// </returns>
        public static XmlQualifiedName SchemaGet( XmlSchemaSet xs )
        {
            return new XmlQualifiedName( "duration", "http://www.w3.org/2001/XMLSchema" );
        }
    }
}

/* eof */