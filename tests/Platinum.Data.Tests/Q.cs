namespace Platinum.Data.Tests
{
    internal static class Q
    {
        internal static string DbError
        {
            get { return Db.Command( "Sql/DbError" ); }
        }

        internal static string DbErrorArg
        {
            get { return Db.Command( "Sql/DbErrorArg" ); }
        }

        internal static string Multi1
        {
            get { return Db.Command( "Sql/Multi1" ); }
        }

        internal static string Multi2
        {
            get { return Db.Command( "Sql/Multi2" ); }
        }

        internal static string Query
        {
            get { return Db.Command( "Sql/Query" ); }
        }

        internal static string Single
        {
            get { return Db.Command( "Sql/Single" ); }
        }
    }
}
