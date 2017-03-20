using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using VSLangProj80;

namespace Platinum.VisualStudio.Plugin
{
    /// <summary>
    /// Generate SQL statement loader, to support Platinum.Data functionality.
    /// </summary>
    [ComVisible( true )]
    [Guid( "70cb7841-beb1-4c9a-90d2-f55c94ab3f08" )]
    [CodeGeneratorRegistration( typeof( PtDataStatementsTool ), "SQL statements tool", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( PtDataStatementsTool ) )]
    public class PtDataStatementsTool : BasePlugin<VisualStudio.PtDataStatementsTool>
    {
    }
}
