// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.Msft.BTHNS_INQUIRYBLOB
// 
// Copyright (c) 2003-2011 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Runtime.InteropServices;
using NXTLib.BluetoothWrapper.Sockets;

namespace NXTLib.BluetoothWrapper.Bluetooth.Msft
{
    [StructLayout(LayoutKind.Sequential, Size=6)]
    internal struct BTHNS_INQUIRYBLOB
    {
        internal int LAP;
        internal byte length;
        internal byte num_responses;
    }
}
