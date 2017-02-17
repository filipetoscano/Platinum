namespace Platinum.VisualStudio.Command
{
    /// <summary>
    /// Command-line flags.
    /// </summary>
    public class CommandLine
    {
        /// <summary>
        /// File name of .sln file to process.
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// File name of .csproj file to process.
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// Whether to execute the tools, or just to probe.
        /// </summary>
        public bool WhatIf { get; set; }
    }
}
