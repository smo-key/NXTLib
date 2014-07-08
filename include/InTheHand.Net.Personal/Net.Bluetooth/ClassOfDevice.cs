// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.ClassOfDevice
// 
// Copyright (c) 2003-2006 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using NXTLib.BluetoothWrapper.Bluetooth;

namespace NXTLib.BluetoothWrapper.Bluetooth
{
    /// <summary>
    /// Describes the device and service capabilities of a device.
    /// </summary>
    /// -
    /// <remarks>
    /// <para>Is returned by the properties
    /// <see cref="P:NXTLib.BluetoothWrapper.Sockets.BluetoothDeviceInfo.ClassOfDevice">BluetoothDeviceInfo.ClassOfDevice</see>
    /// and
    /// <see cref="P:NXTLib.BluetoothWrapper.Bluetooth.BluetoothRadio.ClassOfDevice">BluetoothRadio.ClassOfDevice</see>.
    /// </para>
    /// </remarks>
#if !NETCF
    [Serializable]
#endif
    public sealed class ClassOfDevice : IEquatable<ClassOfDevice>
    {
        private readonly uint cod;

        /// <summary>
        /// Initialize a new instance of class <see cref="T:NXTLib.BluetoothWrapper.Bluetooth.ClassOfDevice"/>.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>An example raw value is 0x00020104, which stands for
        /// device: DesktopComputer, service: Network.
        /// </para>
        /// </remarks>
        /// -
        /// <param name="cod">A <see cref="T:System.UInt32"/> containing the
        /// raw Class of Device value.
        /// </param>
        [CLSCompliant(false)] // Not meant for public use, only for factories.
        public ClassOfDevice(uint cod)
        {
            this.cod = cod;
        }


        /// <summary>
        /// Initialize a new instance of class <see cref="T:NXTLib.BluetoothWrapper.Bluetooth.ClassOfDevice"/>.
        /// </summary>
        /// -
        /// <param name="device">A <see cref="T:NXTLib.BluetoothWrapper.Bluetooth.DeviceClass"/>
        /// value.
        /// </param>
        /// <param name="service">A <see cref="T:NXTLib.BluetoothWrapper.Bluetooth.ServiceClass"/>
        /// value.
        /// </param>
        public ClassOfDevice(DeviceClass device, ServiceClass service)
        {
            var scU = ((uint)service) << 13;
            this.cod = (uint)device | scU;
        }

        //experimental
        /*internal ClassOfDevice(Sockets.IrDAHints hints)
        {
            switch (hints)
            {
                
                case NXTLib.BluetoothWrapper.Sockets.IrDAHints.FileServer:
                    cod | ((uint)ServiceClass.Information << 13);
                
                case NXTLib.BluetoothWrapper.Sockets.IrDAHints.Obex:
                    cod | ((uint)ServiceClass.ObjectTransfer << 13);

                case NXTLib.BluetoothWrapper.Sockets.IrDAHints.PdaAndPalmtop:
                    cod | DeviceClass.PdaComputer;
                    break;
                case NXTLib.BluetoothWrapper.Sockets.IrDAHints.Computer:
                    cod | DeviceClass.Computer;
                    break;
                case NXTLib.BluetoothWrapper.Sockets.IrDAHints.Printer:
                    cod | DeviceClass.ImagingPrinter;
                    break;
                case NXTLib.BluetoothWrapper.Sockets.IrDAHints.Fax:
                case NXTLib.BluetoothWrapper.Sockets.IrDAHints.Modem:
                case NXTLib.BluetoothWrapper.Sockets.IrDAHints.Telephony:
                    cod | DeviceClass.Phone;
                    break;
                case NXTLib.BluetoothWrapper.Sockets.IrDAHints.LanAccess:
                    cod | DeviceClass.AccessPointAvailable;
                    break;
            }
        }*/

        /// <summary>
        /// Returns the device type.
        /// </summary>
        public DeviceClass Device
        {
            get
            {
                return (DeviceClass)(cod & 0x001ffc);
            }
        }

        /// <summary>
        /// Returns the major device type.
        /// </summary>
        public DeviceClass MajorDevice
        {
            get
            {
                return (DeviceClass)(cod & 0x001f00);
            }
        }

        /// <summary>
        /// Returns supported service types.
        /// </summary>
        public ServiceClass Service
        {
            get
            {
                return (ServiceClass)(cod >> 13);
            }
        }

        /// <summary>
        /// Gets the numerical value.
        /// </summary>
        /// <seealso cref="P:NXTLib.BluetoothWrapper.Bluetooth.ClassOfDevice.ValueAsInt32"/>
        [CLSCompliant(false)]//use ValueAsInt32
        public uint Value
        {
            get { return cod; }
        }

        /// <summary>
        /// Gets the numerical value, suitable for CLS Compliance.
        /// </summary>
        /// <seealso cref="P:NXTLib.BluetoothWrapper.Bluetooth.ClassOfDevice.Value"/>
        public int ValueAsInt32
        {
            get { return unchecked((int)cod); }
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        public byte FormatType
        {
            get
            {
                return (byte)(cod & 0x03);
            }
        }*/

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return Convert.ToInt32(cod);
        }

        /// <summary>
        /// Returns the numerical value represented in a hexadecimal.
        /// </summary>
        /// -
        /// <returns>A <see cref="T:System.String"/> containing
        /// the numerical value represented in a hexadecimal
        /// e.g. "720104", "5A020C".
        /// </returns>
        public override string ToString()
        {
            return cod.ToString("X");
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified 
        /// object.
        /// </summary>
        /// <param name="obj">An object
        /// value to compare with the current instance.
        /// </param>
        /// <returns>true if <paramref name="obj"/> is an instance of <see cref="T:NXTLib.BluetoothWrapper.Bluetooth.ClassOfDevice"/>
        /// and equals the value of this instance; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ClassOfDevice);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified 
        /// <see cref="T:NXTLib.BluetoothWrapper.Bluetooth.ClassOfDevice"/> value.
        /// </summary>
        /// <param name="other">An <see cref="T:NXTLib.BluetoothWrapper.Bluetooth.ClassOfDevice"/>
        /// value to compare with the current instance.
        /// </param>
        /// <returns>true if <paramref name="other"/>
        /// has the same value as this instance; otherwise, false.
        /// </returns>
        public bool Equals(ClassOfDevice other)
        {
            if (other == null)
                return false;
            return this.cod == other.cod;
        }

    }
}
