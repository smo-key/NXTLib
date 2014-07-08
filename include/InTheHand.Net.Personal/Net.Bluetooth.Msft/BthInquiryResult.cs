// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.Msft.BthInquiryResult
// 
// Copyright (c) 2003-2008 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Runtime.InteropServices;

#if NETCF
namespace NXTLib.BluetoothWrapper.Bluetooth.Msft
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct BthInquiryResult
    {
        internal long ba;
        internal uint cod;
        internal ushort clock_offset;
        internal byte page_scan_mode;
        internal byte page_scan_period_mode;
        internal byte page_scan_repetition_mode;
    }
}
#endif