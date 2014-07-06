using NXTLib.USBWrapper;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace NXTLib
{
    /// <summary>
    /// <para>The communication protocols specific to USB.</para>
    /// </summary>
    public class USB : Protocol
    {
        // This value was found in the fantomv.inf file. Search for [WinUsb_Inst_HW_AddReg].
        private static readonly Guid NXT_WINUSB_GUID = new Guid("{761ED34A-CCFA-416b-94BB-33486DB1F5D5}");
        private static UsbCommunication usb = new UsbCommunication(NXT_WINUSB_GUID);

        /// <summary>
        /// <para>The communication protocols specific to USB.</para>
        /// </summary>
        public USB()
        {
            
        }

        /// <summary>
        /// <para>Useless when connecting via USB.</para>
        /// </summary>
        /// <returns>Always true.</returns>
        public override bool Connect()
        {
            return true;
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
