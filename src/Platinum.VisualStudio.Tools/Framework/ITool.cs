namespace Platinum.VisualStudio
{
    /// <summary>
    /// Command-line interface. Content is immediately written to the
    /// target file. Exceptions are *NOT* caught.
    /// </summary>
    public interface ITool
    {
        /// <summary>
        /// Executes the tool.
        /// </summary>
        /// <param name="args">
        /// Tool arguments.
        /// </param>
        void Run( ToolRunArgs args );
    }
}
