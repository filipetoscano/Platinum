using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using VSLangProj80;

namespace Platinum.VisualStudio.Plugin
{
    /// <summary>
    /// Applies an XSLT, generating C# as 'code-behind'.
    /// </summary>
    [ComVisible( true )]
    [Guid( "fc3f5148-b5c0-4877-bc80-7023e32de35d" )]
    [CodeGeneratorRegistration( typeof( PtXsltTool ), "XSLT Tool", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( PtXsltTool ) )]
    public class PtXsltTool : BasePlugin<VisualStudio.PtXsltTool>
    {
    }
}
