using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using VSLangProj80;

namespace Platinum.VisualStudio.Plugin
{
    /// <summary>
    /// Generate configuration section handler.
    /// </summary>
    [ComVisible( true )]
    [Guid( "1de76d29-65be-4aba-baba-ae8719e5dd0e" )]
    [CodeGeneratorRegistration( typeof( PtConfigGenTool ), "Configuration section generator.", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( PtConfigGenTool ) )]
    public class PtConfigGenTool : BasePlugin<VisualStudio.PtConfigGenTool>
    {
    }
}
