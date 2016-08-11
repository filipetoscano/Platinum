namespace Platinum
{
    public static class Extensions
    {
        public static T Mirror<T>( this object obj ) where T : class
        {
            return Platinum.Mirror.To<T>( obj );
        }
    }
}
