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
        /// <param name="inputNamespace">CLR namespace of file.</param>
        /// <param name="whatIf">Whether to perform changes or not.</param>
        void Execute( string inputFileName, string inputNamespace, bool whatIf );
    }
}
