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
    [Guid( "26a9b3bd-62e9-4145-9cfb-10a700295fa2" )]
    [CodeGeneratorRegistration( typeof( PtDataStatementsTool ), "SQL statements tool", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( PtDataStatementsTool ) )]
    public class PtDataStatementsTool : BasePlugin<VisualStudio.PtDataStatementsTool>
    {
    }
}
