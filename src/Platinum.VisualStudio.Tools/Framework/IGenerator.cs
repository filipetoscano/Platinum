namespace Platinum.VisualStudio
{
    /// <summary>
    /// Internal interface: used when one tool wants to re-use another tool.
    /// Returns the content to the caller, but exceptions are *NOT* handled.
    /// </summary>
    public interface IGenerator
    {
        /// <summary>
        /// Generates the output.
        /// </summary>
        string Generate( ToolGenerateArgs args );
    }
}
