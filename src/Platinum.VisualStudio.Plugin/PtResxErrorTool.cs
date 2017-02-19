using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using VSLangProj80;

namespace Platinum.VisualStudio.Plugin
{
    /// <summary>
    /// Generate code-behind class and ER.resx files.
    /// </summary>
    [ComVisible( true )]
    [Guid( "504e9977-7be1-4518-82d9-9b14a098c773" )]
    [CodeGeneratorRegistration( typeof( PtResxErrorTool ), "Generates error resources.", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( PtResxErrorTool ) )]
    public class PtResxErrorTool : BasePlugin<VisualStudio.PtResxErrorTool>
    {
    }
}
