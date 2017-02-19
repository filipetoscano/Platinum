namespace Platinum.VisualStudio
{
    /// <summary>
    /// Designates a Visual Studio tool.
    /// </summary>
    public interface IToolChain
    {
        /// <summary>
        /// Generates the output.
        /// </summary>
        string Generate( ToolGenerateArgs args );
    }
}
