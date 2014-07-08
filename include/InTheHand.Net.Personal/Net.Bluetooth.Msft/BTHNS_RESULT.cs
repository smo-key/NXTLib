// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.Msft.BTHNS_RESULT
// 
// Copyright (c) 2003-2006,2011 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;

namespace NXTLib.BluetoothWrapper.Bluetooth.Msft
{
#if WinXP
	/// <summary>
	/// Bluetooth specific flags returned from WSALookupServiceNext 
	/// in WSAQUERYSET.dwOutputFlags in response to device inquiry.
	/// </summary>
	[Flags()]
	internal enum BTHNS_RESULT : int
	{
		None = 0,
		Connected     = 0x00010000,
		Remembered    = 0x00020000,
		Authenticated = 0x00040000,
	}
#endif
}
