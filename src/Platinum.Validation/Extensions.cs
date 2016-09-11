namespace Platinum.Validation
{
    internal static class Extensions
    {
        internal static bool HasValue( this object value )
        {
            if ( value == null )
                return false;

            if ( value is string )
            {
                string v = (string) value;
                return v.Length > 0;
            }

            return true;
        }
    }
}
