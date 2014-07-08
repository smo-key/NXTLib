// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.BlueZ.BluezRadio
// 
// Copyright (c) 2008-2011 In The Hand Ltd, All rights reserved.
// Copyright (c) 2010-2011 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt
#if BlueZ
using System;
using System.Collections.Generic;
using System.Diagnostics;
using NXTLib.BluetoothWrapper.Bluetooth.Factory;
using System.Diagnostics.CodeAnalysis;
using NDesk.DBus;

namespace NXTLib.BluetoothWrapper.Bluetooth.BlueZ
{
    sealed class BluezRadio : IBluetoothRadio
    {
        readonly BluezFactory _fcty;
        readonly int _dd;
        readonly BluetoothAddress _addr;
        readonly byte[] _nameTmp;
        Structs.hci_version _versions;
        readonly ObjectPath _objectPath;

        //----
        internal BluezRadio(BluezFactory fcty, int dd)
        {
            _dd = dd;
            Debug.Assert(fcty != null, "ArgNull");
            _fcty = fcty;
            BluezError ret;
            var bdaddr = BluezUtils.FromBluetoothAddress(BluetoothAddress.None);
            ret = NativeMethods.hci_read_bd_addr(_dd, bdaddr, _fcty.StackTimeout);
            //TODO BluezUtils.CheckAndThrow(ret, "hci_read_bd_addr");
            BluezUtils.Assert(ret, "hci_read_bd_addr");
            if (BluezUtils.IsSuccess(ret)) {
                _addr = BluezUtils.ToBluetoothAddress(bdaddr);
                Console.WriteLine("Radio SUCCESS, addr: " + _addr);
            } else {
                // NEVER used EXCEPT in the debugger if we skip the CheckandThrow above.
                _addr = BluetoothAddress.None;
                Console.WriteLine("Radio FAIL, addr: " + _addr);
            }
            _nameTmp = new byte[250];
            //
            _fcty.BluezDbus.GetAdapterProperties(_addr, out _objectPath);
            GetProperties();
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Bahh  D-Bus library uses Exception.")]
        private IDictionary<string, object> GetProperties()
        {
            ObjectPath objectPath;
            try {
                var prop = _fcty.BluezDbus.GetAdapterProperties(_addr, out objectPath);
                return prop;
            } catch (Exception ex) {
                Debug.Assert(ex.Message.StartsWith("org.bluez.Error.NoSuchAdapter"),
                    "Unexpected exception type: " + ex);
                if (ex.Message.StartsWith("org.bluez.Error.NoSuchAdapter", StringComparison.OrdinalIgnoreCase)) {
                    // NOP
                } else {
                    throw;
                }
            }
            return null;
        }

        private void SetMode(RadioMode value)
        {
            _fcty.BluezDbus.SetAdapterMode(_objectPath, value);
        }

        //----
        BluetoothAddress IBluetoothRadio.LocalAddress
        {
            get { return _addr; }
        }

        string IBluetoothRadio.Name
        {
            get
            {
                var ret = NativeMethods.hci_read_local_name(_dd, _nameTmp.Length, _nameTmp, _fcty.StackTimeout);
                string name;
                BluezUtils.Assert(ret, "hci_read_local_name");
                if (BluezUtils.IsSuccess(ret)) {
                    name = BluezUtils.FromNameString(_nameTmp);
                } else {
                    name = _addr.ToString("C");
                }
                return name;
            }
            set
            {
                byte[] raw = BluezUtils.ToNameString(value);
                var ret = NativeMethods.hci_write_local_name(_fcty.DevDescr, raw, _fcty.StackTimeout);
                BluezUtils.CheckAndThrow(ret, "hci_write_local_name");
                Console.WriteLine("hci_write_local_name returned: " + ret);
            }
        }

        //--
        ClassOfDevice IBluetoothRadio.ClassOfDevice
        {
            get
            {
                ClassOfDevice cod;
                var codArr = new byte[3 + 3];
                var ret = NativeMethods.hci_read_class_of_dev(_dd, codArr, _fcty.StackTimeout);
                BluezUtils.Assert(ret, "hci_read_class_of_dev");
                if (BluezUtils.IsSuccess(ret)) {
                    cod = BluezUtils.ToClassOfDevice(codArr);
                } else {
                    cod = new ClassOfDevice(0);
                }
                return cod;
            }
        }

        //--
        void ReadVersions()
        {
            // TO-DO jst do this once, they won't change!
            var vers = new Structs.hci_version(HciVersion.Unknown);
            var ret = NativeMethods.hci_read_local_version(_dd, ref vers, _fcty.StackTimeout);
            BluezUtils.Assert(ret, "hci_read_local_version");
            if (BluezUtils.IsSuccess(ret)) {
                _versions = vers;
            }
        }

        Manufacturer IBluetoothRadio.Manufacturer
        {
            get
            {
                ReadVersions();
                return (Manufacturer)_versions.manufacturer;
            }
        }

        LmpVersion IBluetoothRadio.LmpVersion
        {
            get
            {
                ReadVersions();
                return (LmpVersion)_versions.lmp_ver;
            }
        }

        int IBluetoothRadio.LmpSubversion
        {
            get
            {
                ReadVersions();
                return _versions.lmp_subver;
            }
        }

        HciVersion IBluetoothRadio.HciVersion
        {
            get
            {
                ReadVersions();
                return (HciVersion)_versions.hci_ver;
            }
        }

        int IBluetoothRadio.HciRevision
        {
            get
            {
                ReadVersions();
                return _versions.hci_rev;
            }
        }

        Manufacturer IBluetoothRadio.SoftwareManufacturer
        {
#pragma warning disable 618 // '...' is obsolete: '...'
            get { return Manufacturer.BlueZXxxx; }
#pragma warning restore 618
        }

        //----
        HardwareStatus IBluetoothRadio.HardwareStatus
        {
            get
            {
                var prop = GetProperties();
                if (prop == null)
                    return HardwareStatus.Shutdown;
                var power = (bool)prop[PropertyName.Powered];
                if (power)
                    return HardwareStatus.Running;
                return HardwareStatus.Shutdown;
            }
        }

        internal static class PropertyName
        {
            internal const string Name = "Name";
            internal const string Class = "Class";
            internal const string Powered = "Powered";
            internal const string Discoverable = "Discoverable";
            internal const string Pairable = "Pairable";
            internal const string PaireableTimeout = "PaireableTimeout";
            internal const string DiscoverableTimeout = "DiscoverableTimeout";
            internal const string Discovering = "Discovering";
            internal const string Devices = "Devices";
            internal const string UUIDs = "UUIDs";
        }

        RadioMode IBluetoothRadio.Mode
        {
            get
            {
                var prop = GetProperties();
                if (prop == null)
                    return RadioMode.PowerOff;
                var power = (bool)prop[PropertyName.Powered];
                if (!power)
                    return RadioMode.PowerOff;
                var disco = (bool)prop[PropertyName.Discoverable];
                if (disco)
                    return RadioMode.Discoverable;
                return RadioMode.Connectable;
            }
            set { SetMode(value); }
        }

        IntPtr IBluetoothRadio.Handle
        {
            get { throw new NotImplementedException(); }
        }

        //--
        string IBluetoothRadio.Remote
        {
            get { return null; }
        }

    }
}
#endif