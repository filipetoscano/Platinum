using System;
using System.Diagnostics;
using System.Reflection;

namespace Platinum.Logging
{
    public class LogManager
    {
        public static Logger GetCurrentClassLogger()
        {
            var logger = NLog.LogManager.GetLogger( GetClassFullName() );

            return new Logger( logger );
        }



        /// <summary>
        /// Gets the fully qualified name of the class invoking the LogManager, including the 
        /// namespace but not the assembly.
        /// </summary>
        private static string GetClassFullName()
        {
            string className;
            Type declaringType;
            int framesToSkip = 2;

            do
            {
#if SILVERLIGHT
                StackFrame frame = new StackTrace().GetFrame( framesToSkip );
#else
                StackFrame frame = new StackFrame( framesToSkip, false );
#endif
                MethodBase method = frame.GetMethod();
                declaringType = method.DeclaringType;

                if ( declaringType == null )
                {
                    className = method.Name;
                    break;
                }

                framesToSkip++;
                className = declaringType.FullName;
            } while ( declaringType.Module.Name.Equals( "mscorlib.dll", StringComparison.OrdinalIgnoreCase ) );

            return className;
        }
    }
}
