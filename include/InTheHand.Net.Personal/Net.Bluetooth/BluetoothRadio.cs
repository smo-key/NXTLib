#define WIDCOMM
// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.BluetoothRadio
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Collections;
using NXTLib.BluetoothWrapper.Bluetooth.Factory;
using System.Diagnostics;

namespace NXTLib.BluetoothWrapper.Bluetooth
{
    /// <summary>
    /// Represents a Bluetooth Radio device.
    /// </summary>
    /// <remarks>Allows you to query properties of the radio hardware and set the mode.</remarks>
    [System.Diagnostics.DebuggerDisplay("impl={m_impl}")]//DebugImplType}")]
    public sealed class BluetoothRadio //: IDisposable
    {

        /// <summary>
        ///  Gets an array of all Bluetooth radios on the system.  
        /// </summary>
        /// <remarks>Under Windows CE this will only ever return a single <see cref="BluetoothRadio"/> device.
        /// <para>If the device has a third-party stack this property will return an empty collection</para></remarks>
        public static BluetoothRadio[] AllRadios
        {
            get
            {
                //#if !V1   System.Collections.Generic.List<BluetoothRadio> result = new System.Collections.Generic.List<BluetoothRadio>();
                ArrayList result = new ArrayList();
                System.Collections.IEnumerable fList;
                try {
                    fList = BluetoothFactory.Factories;
                } catch (PlatformNotSupportedException) {
                    return new BluetoothRadio[0];
                }
                foreach (BluetoothFactory curF in fList) {
                    IBluetoothRadio[] radios = curF.DoGetAllRadios();
                    foreach (IBluetoothRadio curR in radios) {
                        result.Add(new BluetoothRadio(curF, curR));
                    }
                }
                return (BluetoothRadio[])result.ToArray(typeof(BluetoothRadio));
            }
        }

        /// <summary>
        /// Gets the primary <see cref="BluetoothRadio"/>.
        /// </summary>
        /// <remarks>For Windows CE based devices this is the only <see cref="BluetoothRadio"/>, for Windows XP this is the first available <see cref="BluetoothRadio"/> device.
        /// <para>If the device has a third-party stack this property will return null</para></remarks>
        public static BluetoothRadio PrimaryRadio
        {
            get
            {
                try {
                    return new BluetoothRadio(BluetoothFactory.Factory, BluetoothFactory.Factory.DoGetPrimaryRadio());
                } catch (PlatformNotSupportedException) {
                    return null;
                }
            }
        }

        #region IsSupported
        /// <summary>
        /// Gets a value that indicates whether the 32feet.NET library can be used with the current device.
        /// </summary>
        public static bool IsSupported
        {
            get { return (AllRadios.Length > 0); }
        }
        #endregion

        //----------------------------------------------------------------------
        readonly IBluetoothRadio m_impl;
        readonly BluetoothPublicFactory m_publicFactory;

        //----------------------------------------------------------------------
        private BluetoothRadio(BluetoothFactory factory, IBluetoothRadio impl)
        {
            Debug.Assert(impl != null);
            m_impl = impl;
            m_publicFactory = new BluetoothPublicFactory(factory);
        }

        //----------------------------------------------------------------------

        /// <summary>
        /// Gets a class factory for creating client and listener instances on a particular stack.
        /// </summary>
        public BluetoothPublicFactory StackFactory
        {
            [DebuggerStepThrough]
            get { return m_publicFactory; }
        }

        //----------------------------------------------------------------------

        /// <summary>
        /// Gets whether the radio is on a Bluetooth stack on a remote machine.
        /// </summary>
        /// -
        /// <value>Is <see langword="null"/> if the radio is on to the local
        /// machine, otherwise it&#x2019;s the name of the remote machine to which the
        /// radio is attached.
        /// </value>
        public string Remote { [DebuggerStepThrough]get { return m_impl.Remote; } }

        /// <summary>
        /// Gets the handle for this radio.
        /// </summary>
        /// <remarks>Relevant only on Windows XP.</remarks>
        public IntPtr Handle
        {
            [DebuggerStepThrough]
            get { return m_impl.Handle; }
        }

        /// <summary>
        /// Returns the current status of the Bluetooth radio hardware.
        /// </summary>
        /// <value>A member of the <see cref="HardwareStatus"/> enumeration.</value>
        public HardwareStatus HardwareStatus
        {
            [DebuggerStepThrough]
            get { return m_impl.HardwareStatus; }
        }

        /// <summary>
        /// Gets or Sets the current mode of operation of the Bluetooth radio.
        /// </summary>
        /// <remarks>
        /// <para><strong>Microsoft CE/WM</strong></para>
        /// This setting will be persisted when the device is reset.
        /// An Icon will be displayed in the tray on the Home screen and a ?Windows Mobile device will emit a flashing blue LED when Bluetooth is enabled.
        /// 
        /// <para><strong>Widcomm Win32</strong></para>
        /// <para>Is supported.
        /// </para>
        /// 
        /// <para><strong>Widcomm CE/WM</strong></para>
        /// <para>Get and Set both supported.
        /// </para>
        /// <list type="table">
        /// <listheader><term>Mode</term><term>Get</term><term>Set</term>
        /// </listheader>
        /// <item><term>PowerOff</term><term>Disabled or non-connectable</term>
        /// <term>CONNECT_ALLOW_NONE</term>
        /// </item>
        /// <item><term>Connectable</term><term>Connectable</term>
        /// <term>CONNECT_ALLOW_ALL, note not CONNECT_ALLOW_PAIRED.</term>
        /// </item>
        /// <item><term>Discoverable</term><term>Discoverable</term>
        /// <term>Plus also discoverable.</term>
        /// </item>
        /// </list>
        /// <para>Note also that when the Widcomm stack is disabled/off
        /// we report <c>PowerOff</c> (not in 2.4 and earlier), but
        /// we can't turn put it in that mode from the library.
        /// Neither can we turn it back on, <strong>except</strong> that
        /// it happens when the application first uses Bluetooth!
        /// </para>
        /// 
        /// <para><strong>Widcomm Win32</strong></para>
        /// <para>Set is not supported.  There's no Widcomm API support.
        /// </para>
        /// 
        /// </remarks>
        public RadioMode Mode
        {
            [DebuggerStepThrough]
            get { return m_impl.Mode; }
            [DebuggerStepThrough]
            set { m_impl.Mode = value; }
        }

        /// <summary>
        /// Get the address of the local Bluetooth radio device.
        /// </summary>
        /// -
        /// <remarks><para>The property can return a <see langword="null"/> value in
        /// some cases.  For instance on CE when the radio is powered-off the value 
        /// will be <see>null</see>.</para>
        /// </remarks>
        /// -
        /// <value>The address of the local Bluetooth radio device.
        /// </value>
        public BluetoothAddress LocalAddress
        {
            [DebuggerStepThrough]
            get { return m_impl.LocalAddress; }
        }

        /// <summary>
        /// Returns the friendly name of the local Bluetooth radio.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>Devices normally cache the remote device name, only reading it the first
        /// time the remote device is discovered.  It is generally not useful then to change
        /// the name to provide a status update.  For instance on desktop Windows
        /// with the Microsoft stack we haven't found a good way for the name to be
        /// flushed so that it is re-read, even deleting the device didn't flush the
        /// name if I remember correctly.
        /// </para>
        /// <para>Currently read-only on Widcomm stack.  Probably could be supported,
        /// let us know if you need this function.
        /// </para>
        /// </remarks>
        public string Name
        {
            [DebuggerStepThrough]
            get { return m_impl.Name; }
            [DebuggerStepThrough]
            set { m_impl.Name = value; }
        }

        /// <summary>
        /// Returns the Class of Device.
        /// </summary>
        public ClassOfDevice ClassOfDevice
        {
            [DebuggerStepThrough]
            get { return m_impl.ClassOfDevice; }
        }

        /// <summary>
        /// Returns the manufacturer of the <see cref="BluetoothRadio"/> device.
        /// </summary>
        /// <remarks>
        /// See <see cref="HciVersion"/> for more information.
        /// </remarks>
        public Manufacturer Manufacturer
        {
            [DebuggerStepThrough]
            get { return m_impl.Manufacturer; }
        }

        /// <summary>
        /// Bluetooth Version supported by the Host Controller Interface implementation.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>There are five fields returned by the Read Local Version Information
        /// HCI command: HCI Version, HCI Revision, LMP Version,
        /// Manufacturer_Name, and LMP Subversion.
        /// We expose all five, but not all platforms provide access to them all.
        /// The Microsoft stack on desktop Windows exposes all five,
        /// except for Windows XP which only exposes the Manufacturer
        /// and LmpSubversion values.  Bluetopia apparently exposes none of them.
        /// The Microsoft stack on Windows Mobile, Widcomm on both platforms,
        /// BlueSoleil, and BlueZ expose all five.
        /// </para>
        /// </remarks>
        public HciVersion HciVersion
        {
            // MSFT+Win32: 2: lmp_sub, manuf.
            // MSFT+WinCE: 5   +features
            // Widcomm:    5.
            // BSol:       5   +features
            // BlueZ:      5   (+features)
            // SSo/BTPS:   
            [DebuggerStepThrough]
            get { return m_impl.HciVersion; }
        }

        /// <summary>
        /// Manufacture's Revision number of the HCI implementation.
        /// </summary>
        /// <remarks>
        /// See <see cref="HciVersion"/> for more information.
        /// </remarks>
        public int HciRevision
        {
            [DebuggerStepThrough]
            get { return m_impl.HciRevision; }
        }

        /// <summary>
        /// Bluetooth Version supported by the Link Manager Protocol implementation.
        /// </summary>
        /// <remarks>
        /// See <see cref="HciVersion"/> for more information.
        /// </remarks>
        public LmpVersion LmpVersion
        {
            [DebuggerStepThrough]
            get { return m_impl.LmpVersion; }
        }

        /// <summary>
        /// Manufacture's Revision number of the LMP implementation.
        /// </summary>
        /// <remarks>
        /// See <see cref="HciVersion"/> for more information.
        /// </remarks>
        public int LmpSubversion
        {
            [DebuggerStepThrough]
            get { return m_impl.LmpSubversion; }
        }

        /// <summary>
        /// Returns the manufacturer of the Bluetooth software stack running locally.
        /// </summary>
        public Manufacturer SoftwareManufacturer
        {
            [DebuggerStepThrough]
            get { return m_impl.SoftwareManufacturer; }
        }

        //----
        string DebugImplType
        {
            get
            {
                if (m_impl == null) return "(null)";// never occurs...
                return m_impl.GetType().Name;
            }
        }

    }
}
