// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.SdpQueryType
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt


namespace NXTLib.BluetoothWrapper.Bluetooth
{

#if NETCF

    // Used by NXTLib.BluetoothWrapper.Sockets.BluetoothDeviceInfo.GetServiceRecordsUnparsed(System.Guid)
    // Moved from /BTHNS_RESTRICTIONBLOB.cs.
    internal enum SdpQueryType : int
    {
        SearchRequest = 1,
        AttributeRequest = 2,
        SearchAttributeRequest = 3,
    }//enum

#endif

}