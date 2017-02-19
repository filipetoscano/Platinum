using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using VSLangProj80;

namespace Platinum.VisualStudio.Plugin
{
    /// <summary>
    /// Automatically generates exception classes.
    /// </summary>
    [ComVisible( true )]
    [Guid( "fc195c8e-72c7-47d5-a8fd-e4dba1e32d17" )]
    [CodeGeneratorRegistration( typeof( PtResxExceptionTool ), "Platinum Exception", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( PtResxExceptionTool ) )]
    public class PtResxExceptionTool : BasePlugin<ResxExceptionTool>
    {
    }
}
