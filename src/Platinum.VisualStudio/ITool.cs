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
        /// <param name="inputFileName">Name of file to process.</param>
        /// <param name="fileNamespace">CLR namespace of file.</param>
        void Execute( string inputFileName, string fileNamespace );
    }
}
