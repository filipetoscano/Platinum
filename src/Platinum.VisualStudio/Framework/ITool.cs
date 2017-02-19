namespace Platinum.VisualStudio
{
    /// <summary>
    /// Designates a Visual Studio tool.
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
