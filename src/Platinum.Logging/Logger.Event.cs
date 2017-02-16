namespace Platinum.Logging
{
    public partial class Logger
    {
        public void Event( string eventId )
        {
            this._logger.Event( eventId );
        }


        public void Event( string eventId, params object[] args )
        {
            this._logger.Event( eventId, args );
        }
    }
}
