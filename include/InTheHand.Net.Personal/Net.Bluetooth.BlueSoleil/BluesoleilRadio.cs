﻿// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.Widcomm.WidcommBluetoothFactoryBase
// 
// Copyright (c) 2010 Alan J McFarlane, All rights reserved.
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Collections.Generic;
using System.Text;
using NXTLib.BluetoothWrapper.Bluetooth.Factory;
using System.Diagnostics;

namespace NXTLib.BluetoothWrapper.Bluetooth.BlueSoleil
{
    class BluesoleilRadio : IBluetoothRadio
    {
        readonly BluesoleilFactory _fcty;
        readonly BluetoothAddress _addr;
        readonly string _name;
        readonly ClassOfDevice _cod;
        readonly Structs.BtSdkLocalLMPInfoStru _lmp;

        internal BluesoleilRadio(BluesoleilFactory fcty)
        {
            _fcty = fcty;
            _fcty.SdkInit();
            BtSdkError retAddr, ret;
            byte[] bd_addr = new byte[StackConsts.BTSDK_BDADDR_LEN];
            ret = retAddr = _fcty.Api.Btsdk_GetLocalDeviceAddress(bd_addr);
            BluesoleilUtils.Assert(retAddr, "Btsdk_GetLocalDeviceAddress");
            if (ret == BtSdkError.OK) {
                _addr = BluesoleilUtils.ToBluetoothAddress(bd_addr);
            } else {
                _addr = BluetoothAddress.None;
            }
            //
            byte[] arr = new byte[500];
            UInt16 len = checked((UInt16)arr.Length);
            ret = _fcty.Api.Btsdk_GetLocalName(arr, ref len);
            if (retAddr == BtSdkError.OK) BluesoleilUtils.Assert(ret, "Btsdk_GetLocalName");
            if (ret == BtSdkError.OK) {
                _name = BluesoleilUtils.FromNameString(arr, len);
            } else {
                _name = string.Empty;
            }
            //
            uint cod;
            ret = _fcty.Api.Btsdk_GetLocalDeviceClass(out cod);
            //BluesoleilUtils.CheckAndThrow(ret);
            if (retAddr == BtSdkError.OK) Debug.Assert(ret == BtSdkError.OK, "Btsdk_GetLocalDeviceClass ret: " + ret);
            _cod = new ClassOfDevice(cod);
            //
            _lmp = new Structs.BtSdkLocalLMPInfoStru(HciVersion.Unknown);
            ret = _fcty.Api.Btsdk_GetLocalLMPInfo(ref _lmp);
            if (retAddr == BtSdkError.OK) BluesoleilUtils.Assert(ret, "Btsdk_GetLocalLMPInfo");
        }

        //----

        public BluetoothAddress LocalAddress
        {
            get { return _addr; }
        }

        public string Name
        {
            get { return _name; }
            set { throw new NotImplementedException(); }
        }

        public RadioMode Mode
        {
            get
            {
                var hwStatus = HardwareStatus;
                if (hwStatus != HardwareStatus.Running) {
                    return RadioMode.PowerOff;
                }
                StackConsts.DiscoveryMode mode;
                BtSdkError ret = _fcty.Api.Btsdk_GetDiscoveryMode(out mode);
                if (ret != BtSdkError.OK) {
                    Debug.Fail("Btsdk_GetDiscoveryMode FAIL: " + ret);
                    return RadioMode.PowerOff;
                }
                Debug.WriteLine("BlueSoleil Radio mode: " + mode);
                if ((mode & StackConsts.DiscoveryMode.BTSDK_CONNECTABLE)
                        == StackConsts.DiscoveryMode.BTSDK_CONNECTABLE) {
                    if ((mode & StackConsts.DiscoveryMode.BTSDK_GENERAL_DISCOVERABLE)
                            == StackConsts.DiscoveryMode.BTSDK_GENERAL_DISCOVERABLE) {
                        return RadioMode.Discoverable;
                    } else {
                        return RadioMode.Connectable;
                    }
                }
                return RadioMode.PowerOff;
            }
            set
            {
                BtSdkError ret;
                if (value == RadioMode.PowerOff) {
                    ret = _fcty.Api.Btsdk_StopBluetooth();
                    BluesoleilUtils.Assert(ret, "Radio.set_Mode Stop");
                } else {
                    ret = _fcty.Api.Btsdk_StartBluetooth();
                    BluesoleilUtils.Assert(ret, "Radio.set_Mode Start");
                    StackConsts.DiscoveryMode dMode;
                    ret = _fcty.Api.Btsdk_GetDiscoveryMode(out dMode);
                    BluesoleilUtils.Assert(ret, "Radio.set_Mode Get");
                    if (ret != BtSdkError.OK) {
                        dMode = StackConsts.BTSDK_DISCOVERY_DEFAULT_MODE;
                    }
                    // Not PowerOff, so must be Conno, and check if Disco.
                    dMode |= StackConsts.DiscoveryMode.BTSDK_CONNECTABLE;
                    if ((value & RadioMode.Discoverable) == RadioMode.Discoverable) {
                        dMode |= StackConsts.DiscoveryMode.BTSDK_GENERAL_DISCOVERABLE;
                    } else {
                        dMode &= ~StackConsts.DiscoveryMode.BTSDK_GENERAL_DISCOVERABLE;
                    }
                    ret = _fcty.Api.Btsdk_SetDiscoveryMode(dMode);
                    BluesoleilUtils.Assert(ret, "Radio.set_Mode Set");
                }
            }
        }

        public ClassOfDevice ClassOfDevice
        {
            get { return _cod; }
        }

        public Manufacturer SoftwareManufacturer
        {
#pragma warning disable 618
            get { return Manufacturer.IvtBlueSoleilXxxx; }
#pragma warning restore 618
        }

        public IntPtr Handle
        {
            [DebuggerNonUserCode]
            get { throw new NotSupportedException(); }
        }

        public string Remote
        {
            get { return null; }
        }

        public HardwareStatus HardwareStatus
        {
            get
            {
                if (!_fcty.Api.Btsdk_IsBluetoothHardwareExisted())
                    return HardwareStatus.NotPresent;
                if (!_fcty.Api.Btsdk_IsBluetoothReady())
                    return HardwareStatus.Shutdown;
                return HardwareStatus.Running;
            }
        }

        public LmpVersion LmpVersion
        {
            get { return (LmpVersion)_lmp.lmp_version; }
        }

        public int LmpSubversion
        {
            get { return _lmp.lmp_subversion; }
        }

        public HciVersion HciVersion
        {
            get { return (HciVersion)_lmp.hci_version; }
        }

        public int HciRevision
        {
            get { return _lmp.hci_revision; }
        }

        public Manufacturer Manufacturer
        {
            get { return BluesoleilUtils.FromManufName(_lmp.manuf_name); }
        }

    }
}
