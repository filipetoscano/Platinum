namespace Platinum.Logging
{
    public partial class Logger
    {
        public void Event( string eventId )
        {
            this._logger.Event( eventId );
        }


        public void Event( ActorException @event )
        {
            this._logger.Event( @event );
        }
    }
}
