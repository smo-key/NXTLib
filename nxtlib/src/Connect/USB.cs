using NXTLib.USBWrapper;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;

namespace NXTLib
{
    /// <summary>
    /// <para>The communication protocols specific to USB.</para>
    /// </summary>
    public class USB : Protocol
    {
        // This value was found in the fantomv.inf file. Search for [WinUsb_Inst_HW_AddReg].
        private static readonly Guid NXT_GUID = new Guid("{761ED34A-CCFA-416b-94BB-33486DB1F5D5}");
        private static UsbCommunication usb = new UsbCommunication(NXT_GUID);

        /// <summary>
        /// <para>The communication protocols specific to USB.</para>
        /// </summary>
        public USB()
        {
            Initialize();
        }
        public override void Initialize()
        {
            IsInitialized = true;
        }

        /// <summary>
        /// <para>Search for bricks connected via USB.</para>
        /// </summary>
        /// <returns>A list of brick information (returns max 1 brick!).</returns>
        public override List<Brick> Search()
        {
            List<Brick> list = new List<Brick>();
            if (IsConnected)
            {
                BrickInfo _brick = new BrickInfo();
                _brick.address = new byte[6] { 0, 0, 0, 0, 0, 0 };
                _brick.name = "NXT";

                GetDeviceInfoReply reply = GetDeviceInfo();
                _brick.address = reply.Address;
                _brick.name = reply.Name;
                Brick brick = new Brick(this, _brick);
                list.Add(brick);
            }
            else { throw new NXTNoBricksFound(); }
            return list;
        }

        /// <summary>
        /// <para>Useless when connecting via USB.</para>
        /// </summary>
        public override void Connect(Brick brick)
        {
            if (!IsConnected) { throw new NXTNotConnected(); }
        }

        /// <summary>
        /// <para>This method has no function for an USB connection.</para>
        /// </summary>
        public override void Disconnect(Brick brick)
        {
            return;
        }

        /// <summary>
        /// <para>Indicates if protocol is supported.</para>
        /// </summary>
        /// <returns>True if protocol is supported on current platform.</returns>
        public override bool IsSupported
        {
            get { return true; }
        }

        /// <summary>
        /// <para>Indicates if connected to the NXT brick.</para>
        /// </summary>
        /// <returns>Returns true if NXT found via USB.</returns>
        public override bool IsConnected
        {
            get { return usb.FindMyDevice(); }
        }

        /// <summary>
        /// <para>Object to control mutex locking via USB.</para>
        /// </summary>
        private object commLock = new object();

        /// <summary>
        /// <para>Sends a request to the NXT brick.</para>
        /// </summary>
        /// <param name="request">The request, as a byte array.</param>
        public override void Send(byte[] request)
        {
            if (!IsConnected) { throw new NXTNotConnected(); }
            lock (commLock)
            {
                usb.SendDataViaBulkTransfers(request);
                return;
            }
        }

        /// <summary> 
        /// Recieves the reply from the NXT.
        /// </summary>
        /// <returns>Returns the reply from the NXT, as a byte array.</returns>
        public override byte[] RecieveReply()
        {
            if (!IsConnected) { throw new NXTNotConnected(); }
            lock (commLock)
            {
                byte[] reply = usb.ReadDataViaBulkTransfer();
                if (reply == null) { throw new NXTNoReply(); }
                return reply;
            }
        }
        
    }
}
