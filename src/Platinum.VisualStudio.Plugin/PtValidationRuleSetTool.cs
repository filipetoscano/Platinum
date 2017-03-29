using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using VSLangProj80;

namespace Platinum.VisualStudio.Plugin
{
    /// <summary>
    /// Automatically generates validation rule sets.
    /// </summary>
    [ComVisible( true )]
    [Guid( "70577cb1-f491-4815-b8ee-360718054682" )]
    [CodeGeneratorRegistration( typeof( PtValidationRuleSetTool ), "Platinum Validation RuleSets", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( PtValidationRuleSetTool ) )]
    public class PtValidationRuleSetTool : BasePlugin<VisualStudio.PtValidationRuleSetTool>
    {
    }
}
