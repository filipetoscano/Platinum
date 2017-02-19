namespace Platinum.VisualStudio
{
    /// <summary>
    /// Arguments for the Run command.
    /// </summary>
    public struct ToolRunArgs
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
        /// Whether tool should perform writes.
        /// </summary>
        public bool WhatIf { get; set; }
    }
}
