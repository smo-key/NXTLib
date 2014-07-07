﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.IO;

namespace NXTLib
{
    public class Bluetooth : Protocol
    {
        // This value was found in the fantomv.inf file. Search for [WinUsb_Inst_HW_AddReg].
        private static readonly Guid NXT_GUID = BluetoothService.SerialPort;
        private static BluetoothClient client = new BluetoothClient();

        /// <summary
        /// <para>The communication protocols specific to Bluetooth.</para>
        /// </summary>
        /// <param name="serialport">The COM port used by the Bluetooth link.  For example, COM3 would be the third COM port.</param>
        public Bluetooth()
        {
            radio = BluetoothRadio.PrimaryRadio;
        }

        public BluetoothRadio radio { get; set; }

        public RadioMode radiomode { get { return radio.Mode; } set { radio.Mode = value; } }

        /// <summary> 
        /// Connect to the device via Bluetooth.
        /// </summary>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public override bool Connect()
        {
            try
            {
                radiomode = RadioMode.Discoverable;
                
                client.InquiryLength = new TimeSpan(0, 0, 30);
                BluetoothDeviceInfo[] peers = client.DiscoverDevices();
                BluetoothDeviceInfo brick;
                foreach (BluetoothDeviceInfo info in peers)
                {
                    if (info.ClassOfDevice.Value != 2052) { continue; }
                    BluetoothEndPoint ep = new BluetoothEndPoint(info.DeviceAddress, NXT_GUID);
                    //BluetoothSecurity.SetPin(info.DeviceAddress, "1234");
                    BluetoothSecurity.PairRequest(info.DeviceAddress, "1234");
                    client.Connect(ep);
                    if (IsConnected) {
                        brick = info;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
        /// <summary> 
        /// Disconnects from the NXT via Bluetooth.
        /// </summary>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public override bool Disconnect()
        {
            try
            {
                
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// <para>Indicates if protocol is supported.</para>
        /// </summary>
        /// <returns>True if protocol is supported on current platform.</returns>
        public override bool IsSupported
        {
            get
            {
                radio = BluetoothRadio.PrimaryRadio;
                if (radio == null)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary> 
        /// Checks if the NXT is connected via Bluetooth.
        /// </summary>
        /// <returns>Returns true if NXT is connected, false otherwise.</returns>
        public override bool IsConnected
        {
            get
            {
                return client.Connected;
            }
        }
        /// <summary> 
        /// Sends a message via Bluetooth.
        /// </summary>
        /// <param name="request">The request, as a byte array.</param>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public override bool Send(byte[] request)
        {
            try
            {
                /*lock (serialPortLock)
                {
                    int length = request.Length;

                    // Create a Bluetooth request.
                    byte[] btRequest = new byte[request.Length + 2];
                    btRequest[0] = (byte)(length & 0xFF);
                    btRequest[1] = (byte)((length & 0xFF00) >> 8);
                    request.CopyTo(btRequest, 2);

                    // Write the request to the NXT brick.
                    serialPort.Write(btRequest, 0, btRequest.Length);
                }*/
                return true;
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
                byte[] byteIn = new byte[256];
                /*lock (serialPortLock)
                {
                    int length = serialPort.ReadByte() + 256 * serialPort.ReadByte();
                    for (int i = 0; i < length; i++)
                    {
                        int data = serialPort.ReadByte();
                        byte bit = Convert.ToByte(data);
                        byteIn[i] = bit;
                    }
                }*/
                return byteIn;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }
        
    }
}