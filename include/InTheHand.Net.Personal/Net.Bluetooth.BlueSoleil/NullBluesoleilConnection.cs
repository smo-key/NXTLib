// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.BlueSoleil.NullBluesoleilConnection
// 
// Copyright (c) 2010 Alan J McFarlane, All rights reserved.
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;

namespace NXTLib.BluetoothWrapper.Bluetooth.BlueSoleil
{
    internal class NullBluesoleilConnection : IBluesoleilConnection
    {
        static readonly NullBluesoleilConnection _inst = new NullBluesoleilConnection();

        internal static NullBluesoleilConnection Instance
        {
            get { return _inst; }
        }

        private NullBluesoleilConnection()
        {
        }

        void IBluesoleilConnection.CloseNetworkOrInternal()
        {
        }
    }//class
}
