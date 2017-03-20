namespace Platinum.VisualStudio
{
    /// <summary>
    /// Arguments for the Run command.
    /// </summary>
    public struct ToolGenerateArgs
    {
        /// <summary>
        /// CLR namespace.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Filesystem path.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Whether tool should perform writes.
        /// </summary>
        public bool WhatIf { get; set; }

        /// <summary>
        /// Whether tool is being invoked interactively (through Visual Studio
        /// plugin) or through command-line.
        /// </summary>
        public bool IsInteractive { get; set; }
    }
}
