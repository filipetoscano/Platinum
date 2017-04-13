namespace Platinum.Validation.Tests
{
    /// <summary>
    /// Value in list support for strings.
    /// </summary>
    public class InList1
    {
        [InList( "One Two" )]
        public string Value { get; set; }
    }


    public enum InListEnum
    {
        One,
        Two,
        Three,
        Four,
        Five,
    }


    /// <summary>
    /// Value in list support for enums
    /// </summary>
    public class InList2
    {
        [InList( "One,Two", "," )]
        public InListEnum Value { get; set; }
    }
}
