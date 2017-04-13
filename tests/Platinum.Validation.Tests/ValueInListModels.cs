namespace Platinum.Validation.Tests
{
    /// <summary>
    /// Value in list support for strings.
    /// </summary>
    public class ValueInList1
    {
        [ValueInList( "One Two" )]
        public string Value { get; set; }
    }


    public enum ValueInListEnum
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
    public class ValueInList2
    {
        [ValueInList( "One,Two", "," )]
        public ValueInListEnum Value { get; set; }
    }
}
