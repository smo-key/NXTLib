// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Widcomm.WidcommSocketExceptions
// 
// Copyright (c) 2008-2010 In The Hand Ltd, All rights reserved.
// Copyright (c) 2008-2010 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace NXTLib.BluetoothWrapper.Bluetooth.Widcomm
{
    internal sealed class WidcommRfcommPort : IRfcommPort
    {
        internal static class NativeBits
        {
#if NETCF
            internal const string WidcommDll = "32feetWidcomm"; //"wm2";
#else
            internal const string WidcommDll = "32feetWidcomm"; //"32feetWidcommWin32";
#endif

            internal delegate void RfcommPort_DataReceivedCallbackDelegate(IntPtr data, UInt16 len);
            internal delegate void RfcommPort_EventReceivedCallbackDelegate(UInt32 data);

        }

        private static class NativeMethods
        {

            [DllImport(NativeBits.WidcommDll)]
            internal static extern IntPtr RfcommPort_Create(out IntPtr ppRfcommPort,
                NativeBits.RfcommPort_DataReceivedCallbackDelegate handleDataReceived,
                NativeBits.RfcommPort_EventReceivedCallbackDelegate handleEvent);

            [DllImport(NativeBits.WidcommDll)]
            internal static extern void RfcommPort_Destroy(IntPtr pRfcommPort);

            [DllImport(NativeBits.WidcommDll)]
            internal static extern PORT_RETURN_CODE RfcommPort_OpenClient(IntPtr pRfcommPort, int scn, byte[] address);

            [DllImport(NativeBits.WidcommDll)]
            internal static extern PORT_RETURN_CODE RfcommPort_OpenServer(IntPtr pRfcommPort, int scn);

            [DllImport(NativeBits.WidcommDll)]
            internal static extern PORT_RETURN_CODE RfcommPort_Write(IntPtr pRfcommPort, byte[] p_data, UInt16 len_to_write, out UInt16 p_len_written);

            [DllImport(NativeBits.WidcommDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool RfcommPort_IsConnected(IntPtr pObj, [Out] byte[] p_remote_bdaddr, int bufLen);

            [DllImport(NativeBits.WidcommDll)]
            internal static extern PORT_RETURN_CODE RfcommPort_Close(IntPtr pRfcommPort);
        }

        IntPtr m_pRfcommPort;
        WidcommRfcommStreamBase m_parent;
        //
        // Stop the delegates being GC'd, as the native code is calling their thunks.
        NativeBits.RfcommPort_DataReceivedCallbackDelegate m_handleDataReceived;
        NativeBits.RfcommPort_EventReceivedCallbackDelegate m_handleEvent;


        public void SetParentStream(WidcommRfcommStreamBase parent)
        {
            if (m_parent != null)
                throw new InvalidOperationException("Can only have one parent.");
            m_parent = parent;
        }

        public void Create()
        {
            m_handleDataReceived = HandleDataReceived;
            m_handleEvent = HandleEventReceived;
            NativeMethods.RfcommPort_Create(out m_pRfcommPort, m_handleDataReceived, m_handleEvent);
            Utils.MiscUtils.Trace_WriteLine("WidcommRfcommPort.Create'd: " + DebugId);
            if (m_pRfcommPort == IntPtr.Zero)
                throw new InvalidOperationException("Native object creation failed.");
        }

        public string DebugId
        {
            get
            {
                if (m_pRfcommPort == IntPtr.Zero)
                    Utils.MiscUtils.Trace_WriteLine("Can't call get_DebugId before initialised.");
                return m_pRfcommPort.ToInt64().ToString("X");
            }
        }

        void HandleDataReceived(IntPtr buffer, UInt16 len)
        {
            //Debug.WriteLine("HandleReceive: len: " + len.ToString());
            byte[] arr = WidcommUtils.GetByteArray(buffer, len);
            m_parent.HandlePortReceive(arr, this);
        }

        void HandleEventReceived(UInt32 eventId)
        {
            Utils.MiscUtils.Trace_WriteLine("{2} HandleEvent: {0}=0x{0:X}={1}", eventId, (PORT_EV)eventId,
                DateTime.Now.TimeOfDay.ToString());
            m_parent.HandlePortEvent((PORT_EV)eventId, this);
        }

        public PORT_RETURN_CODE OpenClient(int scn, byte[] address)
        {
            var scnB = checked((byte)scn);
            if (scnB < BluetoothEndPoint.MinScn || scnB == 0xFF || scnB > BluetoothEndPoint.MaxScn)
                throw new ArgumentOutOfRangeException("scn", "Should be >0 and <31, is: " + scnB + ".");
            if (address == null || address.Length != 6)
                throw new ArgumentException("Parameter 'address' must be non-null and six-bytes long.");
            PORT_RETURN_CODE ret = NativeMethods.RfcommPort_OpenClient(m_pRfcommPort, scnB, address);
            Utils.MiscUtils.Trace_WriteLine("NativeMethods.RfcommPort_OpenClient ret: {0}=0x{0:X}", ret);
            return ret;
        }

        public PORT_RETURN_CODE OpenServer(int scn)
        {
            var scnB = checked((byte)scn);
            if (scnB < BluetoothEndPoint.MinScn || scnB == 0xFF || scnB > BluetoothEndPoint.MaxScn)
                throw new ArgumentOutOfRangeException("scn", "Should be >0 and <31, is: " + scnB + ".");
            PORT_RETURN_CODE ret = NativeMethods.RfcommPort_OpenServer(m_pRfcommPort, scnB);
            Utils.MiscUtils.Trace_WriteLine("NativeMethods.RfcommPort_OpenServer ret: {0}=0x{0:X}", ret);
            return ret;
        }

        public PORT_RETURN_CODE Write(byte[] data, ushort lenToWrite, out ushort lenWritten)
        {
            return NativeMethods.RfcommPort_Write(m_pRfcommPort, data, lenToWrite, out lenWritten);
        }

        public bool IsConnected(out BluetoothAddress p_remote_bdaddr)
        {
            byte[] bdaddr = new byte[WidcommStructs.BD_ADDR_LEN];
            bool ret = NativeMethods.RfcommPort_IsConnected(m_pRfcommPort, bdaddr, bdaddr.Length);
            p_remote_bdaddr = WidcommUtils.ToBluetoothAddress(bdaddr);
            return ret;
        }

        public PORT_RETURN_CODE Close()
        {
            return NativeMethods.RfcommPort_Close(m_pRfcommPort);
        }

        public void Destroy()
        {
            Debug.Assert(m_pRfcommPort != IntPtr.Zero, "WidcommRfcommPort Already Destroyed");
            if (m_pRfcommPort != IntPtr.Zero) {
                NativeMethods.RfcommPort_Destroy(m_pRfcommPort);
                m_pRfcommPort = IntPtr.Zero;
            }
        }

    }
}
