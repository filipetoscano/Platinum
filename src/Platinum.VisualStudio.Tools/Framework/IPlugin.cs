namespace Platinum.VisualStudio
{
    /// <summary>
    /// Visual Studio interface: returns the content to Visual Studio.
    /// Any exceptions are caught, serialized and returned as content.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Generates the output.
        /// </summary>
        byte[] Generate( ToolGenerateArgs args );
    }
}
