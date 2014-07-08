// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.Msft.WinCEBluetoothRadio
// 
// Copyright (c) 2003-2011 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Collections;
using System.ComponentModel;
using Microsoft.Win32;
using NXTLib.BluetoothWrapper.Bluetooth;
using NXTLib.BluetoothWrapper.Bluetooth.Factory;
using System.Diagnostics;

#if NETCF
namespace NXTLib.BluetoothWrapper.Bluetooth.Msft
{
    sealed class WindowsBluetoothRadio : IBluetoothRadio //: IDisposable
    {
        //
        readonly LmpVersion _lmpV;
        readonly int _lmpSubv;
        readonly HciVersion _hciV;
        readonly int _hciRev;
        readonly Manufacturer manufacturer;

        public string Remote { get { return null; } }

#region IsPlatformSupported
        internal static bool IsPlatformSupported
        {
            get
            {
                PlatformVerification.ThrowException();

                //without the base dll no further interrogation is necessary
                if (!System.IO.File.Exists("\\windows\\btdrt.dll"))
                    return false;

                try
                {
                        HardwareStatus status = 0;
                        int result = NativeMethods.BthGetHardwareStatus(ref status);
                        if (result == 0)
                        {
                            if (status != HardwareStatus.NotPresent)
                            {
                                return true;
                            }
                        }
                }
                catch
                {
                }

                return false;
            }
        }
#endregion

        private static bool hasBthUtil = System.IO.File.Exists("\\Windows\\BthUtil.dll");
        //private IntPtr msgQueueHandle;
        //private IntPtr notificationHandle;


#region Constructor
        internal WindowsBluetoothRadio(IntPtr handle)
        {
            //get version/manufacturer
            byte CHECK0 = 0;
            byte hv;
            byte CHECK1 = 0;
            ushort hr;
            byte CHECK2 = 0;
            byte lv;
            byte CHECK3 = 0;
            ushort ls;
            byte CHECK4 = 0;
            ushort man;
            byte CHECK5 = 0;
            const int HciFeaturesLength = 8;
            var fea = new byte[HciFeaturesLength];
            byte CHECK6 = 0;

            int hresult = NativeMethods.BthReadLocalVersion(out hv, out hr, out lv, out ls, out man, fea);
            Debug.Assert((CHECK0 == 0) && (CHECK1 == 0) && (CHECK2 == 0) && (CHECK3 == 0) && (CHECK4 == 0) && (CHECK5 == 0) && (CHECK6 == 0));
            if (hresult == 0) {
                manufacturer = (Manufacturer)man;
                _hciV = (HciVersion)hv;
                _hciRev = hr;
                _lmpV = (LmpVersion)lv;
                _lmpSubv = ls;
            } else {
                manufacturer = Manufacturer.Unknown;
                _hciV = HciVersion.Unknown;
                _lmpV = LmpVersion.Unknown;
            }

            //setup message queue

            /*NativeMethods.MSGQUEUEOPTIONS mqo = new NativeMethods.MSGQUEUEOPTIONS();
            mqo.dwFlags = 0;
            mqo.cbMaxMessage = 72;
            mqo.bReadAccess = true;
            mqo.dwSize = System.Runtime.InteropServices.Marshal.SizeOf(mqo);
            msgQueueHandle = NativeMethods.CreateMsgQueue("NXTLib.BluetoothWrapper.Bluetooth.BluetoothRadio", ref mqo);

            notificationHandle = NativeMethods.RequestBluetoothNotifications(NativeMethods.BTE_CLASS.CONNECTIONS | NativeMethods.BTE_CLASS.DEVICE | NativeMethods.BTE_CLASS.PAIRING | NativeMethods.BTE_CLASS.STACK, msgQueueHandle);

            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(EventThread));
            t.IsBackground = true;
            t.Start();*/
        }
#endregion

#region Primary Radio

        private static WindowsBluetoothRadio primaryRadio;
        internal static IBluetoothRadio GetPrimaryRadio()
        {
            if (primaryRadio == null)
            {
                if (IsPlatformSupported) {
                    primaryRadio = new WindowsBluetoothRadio(IntPtr.Zero);
                } else {
                    throw new PlatformNotSupportedException("No Radio.");
                }
            }
            return primaryRadio;
        }
#endregion

#region All Radios
        internal static IBluetoothRadio[] AllRadios
        {
            get
            {
                if (IsPlatformSupported)
                {
                    return new IBluetoothRadio[] { new WindowsBluetoothRadio(IntPtr.Zero) };
                }
                return new IBluetoothRadio[0] { };
            }
        }
#endregion



#region Handle
        public IntPtr Handle
        {
            get
            {
                return IntPtr.Zero;
            }
        }
#endregion

#region Hardware Status
        public HardwareStatus HardwareStatus
        {
            get
            {
                HardwareStatus status = 0;
                int result = NativeMethods.BthGetHardwareStatus(ref status);

                if (result != 0)
                {
                    throw new System.ComponentModel.Win32Exception(result, "Error retrieving Bluetooth hardware status");
                }
                return status;
            }
        }

#endregion

#region Mode
        public RadioMode Mode
        {
            get
            {
                if (hasBthUtil)
                {
                    RadioMode val;              
				    int result = NativeMethods.BthGetMode(out val);
				    if(result!=0)
				    {
					    throw new System.ComponentModel.Win32Exception(result, "Error getting BluetoothRadio mode");
				    }
				    return val;
                }
                else
                {
				byte mask;
                int result = NativeMethods.BthReadScanEnableMask(out mask);

                if (result != 0)
                {
                    throw new System.ComponentModel.Win32Exception(result, "Error getting BluetoothRadio mode");
                }

                switch (mask)
                {
                    case 2:
                        return RadioMode.Connectable;
                    case 3:
                        return RadioMode.Discoverable;
                    default:
                        return RadioMode.PowerOff;
                }
				}
            }
            set
            {
                int result = 0;
                if (hasBthUtil)
                {
                    result = NativeMethods.BthSetMode(value);
                }
                else
                {
                byte mask = 0;
                switch (value)
                {
                    case RadioMode.Connectable:
                        mask = 2;
                        break;
                    case RadioMode.Discoverable:
                        mask = 3;
                        break;
                }
                    result = NativeMethods.BthWriteScanEnableMask(mask);
                }

				if(result!=0)
				{
					throw new System.ComponentModel.Win32Exception(result, "Error setting BluetoothRadio mode");
				}
            }
        }
#endregion

#region Local Address

        private BluetoothAddress localAddress;
        public BluetoothAddress LocalAddress
        {
            get
            {
                if (localAddress == null)
                {
                    // Get correct size of address array
                    byte[] bytes = new BluetoothAddress().ToByteArray();
                    int hresult = NativeMethods.BthReadLocalAddr(bytes);
                    BluetoothAddress ba = new BluetoothAddress(bytes);

                    if (hresult == 0)
                    {
                        localAddress = ba;
                    }
                }
                return localAddress;
            }
        }

#endregion

#region Name
        public string Name
        {
            get
            {
                string radioName = string.Empty;
                //get name from registry
                RegistryKey keyName = Registry.CurrentUser.OpenSubKey("\\Software\\Microsoft\\Bluetooth\\Settings");
                if (keyName != null)
                {
                    radioName = (string)keyName.GetValue("LocalName", string.Empty);
                    keyName.Close();
                }
                return radioName;
            }
            set
            {
                RegistryKey keyName = Registry.CurrentUser.CreateSubKey("\\Software\\Microsoft\\Bluetooth\\Settings");
                if (keyName != null)
                {
                    keyName.SetValue("LocalName", value);
                    keyName.Close();
                }
            }
        }
#endregion

#region Class Of Device
        public ClassOfDevice ClassOfDevice
        {
            get
            {
                uint cod;
                int hresult = NativeMethods.BthReadCOD(out cod);
                if (hresult == 0)
                {
                    return new ClassOfDevice(cod);
                }
                else
                {
                    return new ClassOfDevice((uint)DeviceClass.Uncategorized);
                }          
            }
        }

#endregion

#region Manufacturer

        public Manufacturer Manufacturer
        {
            get { return manufacturer; }
        }
#endregion

#region Lmp/Hci versions
        LmpVersion IBluetoothRadio.LmpVersion
        {
            get { return _lmpV; }
        }

        int IBluetoothRadio.LmpSubversion
        {
            get { return _lmpSubv; }
        }

        HciVersion IBluetoothRadio.HciVersion
        {
            get { return _hciV; }
        }

        int IBluetoothRadio.HciRevision
        {
            get { return _hciRev; }
        }
#endregion

#region Stack
        public Manufacturer SoftwareManufacturer
        {
            get
            {
                return Manufacturer.Microsoft;
            }
        }
#endregion


        //events
        /*private void EventThread()
        {
            int len = 72;
            IntPtr buffer = System.Runtime.InteropServices.Marshal.AllocHGlobal(len);
            int received = 0;
            int flags = 0;

            try
            {
                while (msgQueueHandle != IntPtr.Zero)
                {
                    bool success = NativeMethods.ReadMsgQueue(msgQueueHandle, buffer, len, out received, -1, out flags);
                    if (success)
                    {
                        NativeMethods.BTEVENT bte = (NativeMethods.BTEVENT)System.Runtime.InteropServices.Marshal.PtrToStructure(buffer, typeof(NativeMethods.BTEVENT));
                        switch (bte.dwEventId)
                        {
                            case NativeMethods.BTE.CONNECTION:
                                if (this.Connection != null)
                                {
                                    this.Connection(this, EventArgs.Empty);
                                }
                                break;
                            case NativeMethods.BTE.DISCONNECTION:
                                if (this.Disconnection != null)
                                {
                                    this.Disconnection(this, EventArgs.Empty);
                                }
                                break;
                            case NativeMethods.BTE.PAGE_TIMEOUT:
                                if (this.PageTimeout != null)
                                {
                                    this.PageTimeout(this, EventArgs.Empty);
                                }
                                break;
                        }
                    }

                }
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.FreeHGlobal(buffer);
            }
        }*/

        //public event EventHandler Connection;
        //public event EventHandler Disconnection;
        //public event EventHandler PageTimeout;


#region IDisposable Members
        /*
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (notificationHandle != IntPtr.Zero)
            {
                NativeMethods.StopBluetoothNotifications(notificationHandle);
                notificationHandle = IntPtr.Zero;
            }
            if (msgQueueHandle != IntPtr.Zero)
            {
                NativeMethods.CloseMsgQueue(msgQueueHandle);
                msgQueueHandle = IntPtr.Zero;
            }
        }

        ~BluetoothRadio()
        {
            Dispose(false);
        }*/
#endregion
    }
}
#endif