// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.Windows.Forms.NativeMethods
// 
// Copyright (c) 2003-2011 In The Hand Ltd, All rights reserved.
// This source code is licensed under the Microsoft Public License (Ms-PL) - see License.txt

#region Using directives

using System;
using System.Runtime.InteropServices;
using NXTLib.BluetoothWrapper.Bluetooth;

#endregion

#if WinXP

namespace NXTLib.Windows.Forms
{
    internal static class NativeMethods
    {
        [DllImport("Irprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothSelectDevices(ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);

        [DllImport("Irprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothSelectDevicesFree(ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);
    }
}

#endif