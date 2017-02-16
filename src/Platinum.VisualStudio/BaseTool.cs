using Microsoft.Samples.VisualStudio.GeneratorSample;
using System;
using System.Text;

namespace Platinum.VisualStudio
{
    /// <summary />
    public abstract class BaseTool : BaseCodeGeneratorWithSite
    {
        /// <summary />
        protected override byte[] GenerateCode( string inputFileContent )
        {
            string outputContent;

            try
            {
                outputContent = Execute( this.FileNameSpace, this.InputFilePath, inputFileContent );
            }
            catch ( Exception ex )
            {
                outputContent = "EX" + ex.ToString();
            }

            return Encoding.UTF8.GetBytes( outputContent );
        }


        /// <summary />
        protected abstract string Execute( string fileNamespace, string inputFileName, string inputContent );
    }
}
