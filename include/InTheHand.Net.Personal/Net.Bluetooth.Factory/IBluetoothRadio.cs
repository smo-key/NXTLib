// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.Factory.IBluetoothRadio
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
namespace NXTLib.BluetoothWrapper.Bluetooth.Factory
{
    /// <exclude/>
    public interface IBluetoothRadio
    {
#pragma warning disable 1591
        ClassOfDevice ClassOfDevice { get; }
        IntPtr Handle { get; }
        HardwareStatus HardwareStatus { get; }
        BluetoothAddress LocalAddress { get; }
        RadioMode Mode { get; set; }
        string Name { get; set; }
        Manufacturer SoftwareManufacturer { get; }
        string Remote { get; }
        //
        HciVersion HciVersion { get; }
        int HciRevision { get; }
        LmpVersion LmpVersion { get; }
        int LmpSubversion { get; }
        Manufacturer Manufacturer { get; }
    }

}
