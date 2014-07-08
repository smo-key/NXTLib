// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Sockets.AddressFamily32
// 
// Copyright (c) 2003-2006 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt
//
// Peter Foot 18/04/2006 - replaces previous separate IrDA and Bluetooth enums.
//

#region Using directives

using System;
using System.Net.Sockets;

#endregion

namespace NXTLib.BluetoothWrapper.Sockets
{
    /// <summary>
    /// Specifies additional addressing schemes that an instance of the <see cref="Socket"/> class can use.
    /// </summary>
    public static class AddressFamily32
    {
        /// <summary>
        /// Bluetooth address.
        /// </summary>
        /// <value>32</value>
        public const AddressFamily Bluetooth = (AddressFamily)32;

        /// <summary>
        /// IrDA address used on some Windows CE platforms (Has a different value to <see cref="System.Net.Sockets.AddressFamily">AddressFamily.IrDA</see>).
        /// </summary>
        /// <value>22</value>
        public const AddressFamily Irda = (AddressFamily)22;

        //----
        internal const AddressFamily BluetoothOnLinuxBlueZ = (AddressFamily)31;
    }
}
