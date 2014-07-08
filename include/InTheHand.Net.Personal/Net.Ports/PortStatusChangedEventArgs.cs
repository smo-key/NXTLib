// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Ports.PortStatusChangedEventArgs
// 
// Copyright (c) 2011 In The Hand Ltd, All rights reserved.
// Copyright (c) 2011 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;

namespace NXTLib.BluetoothWrapper.Ports
{
#pragma warning disable 1591 // "Missing XML comment for publicly visible type or member..."
    public class PortStatusChangedEventArgs : EventArgs
    {
        public PortStatusChangedEventArgs(bool connected, string portName, BluetoothAddress address)
        {
            Connected = connected;
            PortName = portName;
            Address = address;
        }

        public bool Connected { get; private set; }
        public string PortName { get; private set; }
        public BluetoothAddress Address { get; private set; }
    }

}