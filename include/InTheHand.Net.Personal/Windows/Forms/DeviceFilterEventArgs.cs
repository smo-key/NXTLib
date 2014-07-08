// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.Windows.Forms.DeviceFilterEventArgs
// 
// Copyright (c) 2011 In The Hand Ltd, All rights reserved.
// This source code is licensed under the Microsoft Public License (Ms-PL) - see License.txt

using System;

using NXTLib.BluetoothWrapper;
using NXTLib.BluetoothWrapper.Bluetooth;
using NXTLib.BluetoothWrapper.Sockets;

#if false  // Apparently unfinished...
namespace NXTLib.Windows.Forms
{
    public class DeviceFilterEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value that indicates whether the item passes the filter.
        /// </summary>
        public bool Accepted
        {
            get;
            set;
        }

        public BluetoothDeviceInfo Device
        {
            get;
            private set;
        }
    }
}
#endif
