// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.BLUETOOTH_DEVICE_INFO
// 
// Copyright (c) 2003-2008 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Runtime.InteropServices;
#if WinXP
using NXTLib.Win32;
using System.Text;
using System.Diagnostics;
#endif

namespace NXTLib.BluetoothWrapper.Bluetooth.Msft
{

#if WinXP
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
#endif
    internal struct BLUETOOTH_DEVICE_INFO
    {
        internal int dwSize;
        internal long Address;
        internal uint ulClassofDevice;
#if WinXP
        [MarshalAs(UnmanagedType.Bool)]
#endif
        internal bool fConnected;
#if WinXP
        [MarshalAs(UnmanagedType.Bool)]
#endif
        internal bool fRemembered;
#if WinXP
        [MarshalAs(UnmanagedType.Bool)]
#endif
        internal bool fAuthenticated;
#if WinXP
        internal SYSTEMTIME stLastSeen;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U1)]
        internal SYSTEMTIME stLastUsed;  
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=248)]
#endif
        internal string szName;

        public BLUETOOTH_DEVICE_INFO(long address)
        {
            dwSize = 560;
            this.Address = address;
            ulClassofDevice = 0;
            fConnected = false;
            fRemembered = false;
            fAuthenticated = false;
#if WinXP
            stLastSeen = new SYSTEMTIME();
            stLastUsed = new SYSTEMTIME();

            // The size is much smaller on CE (no times and string not inline) it
            // appears to ignore the bad dwSize value.  So don't check this on CF.
            System.Diagnostics.Debug.Assert(Marshal.SizeOf(typeof(BLUETOOTH_DEVICE_INFO)) == dwSize, "BLUETOOTH_DEVICE_INFO SizeOf == dwSize");
#endif
            szName = "";
        }

        public BLUETOOTH_DEVICE_INFO(BluetoothAddress address)
        {
            if (address == null) {
                throw new ArgumentNullException("address");
            }
            dwSize = 560;
            this.Address = address.ToInt64();
            ulClassofDevice = 0;
            fConnected = false;
            fRemembered = false;
            fAuthenticated = false;
#if WinXP
            stLastSeen = new SYSTEMTIME();
            stLastUsed = new SYSTEMTIME();

            // The size is much smaller on CE (no times and string not inline) it
            // appears to ignore the bad dwSize value.  So don't check this on CF.
            System.Diagnostics.Debug.Assert(Marshal.SizeOf(typeof(BLUETOOTH_DEVICE_INFO)) == dwSize, "BLUETOOTH_DEVICE_INFO SizeOf == dwSize");
#endif
            szName = "";
        }

#if WinXP
        internal DateTime LastSeen
        {
            get
            {
                return stLastSeen.ToDateTime(DateTimeKind.Utc);
            }
        }
        internal DateTime LastUsed
        {
            get
            {
                return stLastUsed.ToDateTime(DateTimeKind.Utc);
            }
        }
#endif

        //--------
#if !NETCF
        internal static BLUETOOTH_DEVICE_INFO Create(BTH_DEVICE_INFO deviceInfo)
        {
            Debug.Assert(0 != (deviceInfo.flags & BluetoothDeviceInfoProperties.Address),
                "BTH_DEVICE_INFO Address field flagged as empty!: " + deviceInfo.address.ToString("X12"));
            BLUETOOTH_DEVICE_INFO bdi0 = new BLUETOOTH_DEVICE_INFO(deviceInfo.address);
            //
            if (0 != (deviceInfo.flags & BluetoothDeviceInfoProperties.Cod)) {
                bdi0.ulClassofDevice = deviceInfo.classOfDevice;
            }
            byte[] nameUtf8 = deviceInfo.name;
            if (0 != (deviceInfo.flags & BluetoothDeviceInfoProperties.Name)) {
                int end = Array.IndexOf<byte>(nameUtf8, 0);
                if (end != -1) {
                    string name = Encoding.UTF8.GetString(nameUtf8, 0, end);
                    bdi0.szName = name;
                }
            }
            bdi0.fAuthenticated = 0 != (deviceInfo.flags & BluetoothDeviceInfoProperties.Paired);
            bdi0.fConnected = 0 != (deviceInfo.flags & BluetoothDeviceInfoProperties.Connected);
            bdi0.fRemembered = 0 != (deviceInfo.flags & BluetoothDeviceInfoProperties.Personal);
            //
            return bdi0;
        }
#endif

    }
}
