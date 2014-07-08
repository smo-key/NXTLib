// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Widcomm.WidcommSocketExceptions
// 
// Copyright (c) 2008-2010 In The Hand Ltd, All rights reserved.
// Copyright (c) 2008-2010 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

#if NETCF
#define DEBUG // Mapping Trace.WriteLine to Debug.WriteLine for NETCF pre-3.5!
using System;
using System.Diagnostics;

namespace Utils
{
    partial class MiscUtils
    {
        private static void Trace_WriteLine_NETCF(string message)
        {
            // DEBUG always defined above.
            Debug.WriteLine(message);
        }
    }
}
#endif
