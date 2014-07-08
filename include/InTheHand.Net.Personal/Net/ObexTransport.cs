// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.ObexTransport
// 
// Copyright (c) 2003-2006 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;

namespace NXTLib.BluetoothWrapper
{
    /// <summary>
    /// Supported network transports for Object Exchange.
    /// </summary>
    public enum ObexTransport
    {
        /// <summary>
        /// Infrared (IrDA)
        /// </summary>
        IrDA,
        /// <summary>
        /// Bluetooth
        /// </summary>
        Bluetooth,
        /// <summary>
        /// TCP/IP
        /// </summary>
        Tcp,
    }
}
