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
            
        }

        /// <summary>
        /// <para>Useless when connecting via USB.</para>
        /// </summary>
        /// <returns>Search for bricks connected via USB.</returns>
        public override List<BrickInfo> Search(Protocol link)
        {
            if (IsConnected)
            {
                List<BrickInfo> list = new List<BrickInfo>();
                BrickInfo brick = new BrickInfo();
                brick.address = new byte[] { };
                brick.name = "NXT";

                GetDeviceInfoReply? reply = link.GetDeviceInfo();
                if (reply.HasValue)
                {
                    brick.address = reply.Value.Address;
                    brick.name = reply.Value.Name;
                }
                list.Add(brick);
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// <para>Useless when connecting via USB.</para>
        /// </summary>
        /// <returns>Always true.</returns>
        public override bool Connect(BrickInfo brick)
        {
            return IsConnected;
        }

        /// <summary>
        /// <para>This method has no function for an USB connection.</para>
        /// </summary>
        /// <returns>Always true.</returns>
        public override bool Disconnect()
        {
            return true;
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
        object usbLock = new object();

        /// <summary>
        /// <para>Sends a request to the NXT brick.</para>
        /// </summary>
        /// <param name="request">The request, as a byte array.</param>
        /// <returns>True if successful.</returns>
        public override bool Send(byte[] request)
        {
            try
            {
                lock (usbLock)
                {
                    usb.SendDataViaBulkTransfers(request);
                    return true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary> 
        /// Recieves the reply from the NXT.
        /// </summary>
        /// <returns>Returns the reply from the NXT, as a byte array.</returns>
        public override byte[] RecieveReply()
        {
            try
            {
                lock (usbLock)
                {
                    return usb.ReadDataViaBulkTransfer();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }
        
    }
}
