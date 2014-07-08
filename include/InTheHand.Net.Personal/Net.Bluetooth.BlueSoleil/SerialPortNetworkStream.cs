// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.Widcomm.WidcommBluetoothFactoryBase
// 
// Copyright (c) 2010 Alan J McFarlane, All rights reserved.
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Diagnostics;
using System.IO.Ports;
using NXTLib.BluetoothWrapper.Bluetooth.Factory;
using System.Diagnostics.CodeAnalysis;


namespace NXTLib.BluetoothWrapper.Bluetooth.BlueSoleil
{
    abstract class SerialPortNetworkStream : NXTLib.BluetoothWrapper.Bluetooth.Factory.DecoratorNetworkStream
    {
        readonly protected ISerialPortWrapper _port;

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SerialPortNetworkStream(SerialPort port,
            IBluetoothClient cli)
            : this(new SerialPortWrapper(port), cli)
        {
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cli")]
        internal SerialPortNetworkStream(ISerialPortWrapper port,
            IBluetoothClient cli)
            : base(port.BaseStream)
        {
            _port = port;
        }

        public override bool DataAvailable
        {
            get { return _port.BytesToRead > 0; }
        }

        internal int Available
        {
            get { return _port.BytesToRead; }
        }

        /// <summary>
        /// For FooBarClient.Connected
        /// </summary>
        internal abstract bool Connected { get; }

        //----
        public override void Flush()
        {
            try {
                base.Flush();
            } catch (ObjectDisposedException) {
            }
        }

        //----
        ~SerialPortNetworkStream()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            try {
                if (disposing) {
                    _port.Close();
                }
            } finally {
                base.Dispose(disposing);
            }
        }

    }//class
}