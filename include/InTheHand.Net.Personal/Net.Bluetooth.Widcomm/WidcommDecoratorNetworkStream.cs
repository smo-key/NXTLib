// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Widcomm.WidcommSocketExceptions
// 
// Copyright (c) 2008-2009 In The Hand Ltd, All rights reserved.
// Copyright (c) 2008-2009 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using NXTLib.BluetoothWrapper.Bluetooth.Factory;

namespace NXTLib.BluetoothWrapper.Bluetooth.Widcomm
{
    sealed class WidcommDecoratorNetworkStream : DecoratorNetworkStream
    {
        readonly CommonRfcommStream m_childWrs; // WRS required for DataAvailable property.

        internal WidcommDecoratorNetworkStream(CommonRfcommStream childWrs)
            : base(childWrs)
        {
            if (!childWrs.Connected) // Although the base constructor will have checked already.
                throw new ArgumentException("Child stream must be connected.");
            m_childWrs = childWrs;
        }

        public override bool DataAvailable
        {
            get { return m_childWrs.DataAvailable; }
        }
    }

}
