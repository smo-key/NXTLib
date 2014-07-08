using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Utils
{
    static partial class MiscUtils
    {
        internal static void Trace_WriteLine(string message)
        {
#if !NETCF
            Trace.WriteLine(message);
#else
            Trace_WriteLine_NETCF(message); // :-( T.WL only in 3.5
#endif
        }

        internal static void Trace_WriteLine(string format, params object[] args)
        {
            Trace_WriteLine(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                format, args));
        }

        [Conditional("DEBUG")]
        internal static void ConsoleDebug_WriteLine(string value)
        {
#if !NETCF
            Console.WriteLine(value);
#endif
        }

    }
}
