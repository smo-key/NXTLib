using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using NXTLib.BluetoothWrapper;
using NXTLib.BluetoothWrapper.Sockets;
using NXTLib.BluetoothWrapper.Bluetooth;
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
        /// <para>Object to control mutex locking via Bluetooth.</para>
        /// </summary>
        private object commLock = new object();

        /// <summary>
        /// <para>Search for bricks connected via Bluetooth.</para>
        /// </summary>
        /// <returns>A list of brick information.</returns>
        public override List<BrickInfo> Search()
        {
            radiomode = RadioMode.Connectable;
            List<BrickInfo> bricks = new List<BrickInfo>();
            lock (commLock)
            {
                radiomode = RadioMode.Discoverable;

                client.InquiryLength = new TimeSpan(0, 0, 30);
                BluetoothDeviceInfo[] peers = client.DiscoverDevicesInRange();
                    
                foreach (BluetoothDeviceInfo info in peers)
                {
                    if (info.ClassOfDevice.Value != 2052) { continue; }

                    BluetoothEndPoint ep = new BluetoothEndPoint(info.DeviceAddress, NXT_GUID);
                    //BluetoothSecurity.SetPin(info.DeviceAddress, "1234");
                    BrickInfo brick = new BrickInfo();
                    brick.address = info.DeviceAddress.ToByteArray();
                    brick.name = info.DeviceName;
                    bricks.Add(brick);
                }
            }
            if (bricks.Count == 0) { throw new NXTNoBricksFound(); }
            return bricks;
        }

        /// <summary> 
        /// Connect to the device via Bluetooth.
        /// </summary>
        public override void Connect(BrickInfo brick)
        {
            lock (commLock)
            {
                radiomode = RadioMode.Connectable;

                BluetoothAddress adr = new BluetoothAddress(brick.address);
                BluetoothDeviceInfo device = new BluetoothDeviceInfo(adr);

                //BluetoothSecurity.RevokePin(adr);
                //BluetoothSecurity.RemoveDevice(adr);

                BluetoothSecurity.PairRequest(adr, "1234");
                client.Connect(adr, NXT_GUID);
            }
            if (!IsConnected) { throw new NXTNotConnected(); }
            return;
        }
        /// <summary> 
        /// Disconnects from the NXT via Bluetooth.
        /// </summary>
        public override void Disconnect()
        {
            if (!IsConnected) { throw new NXTNotConnected(); }
            lock (commLock)
            {
                client.Close();
            }
            return;
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
        public override void Send(byte[] request)
        {
            lock (commLock)
            {
                Stream stream = client.GetStream();
                int length = request.Length;

                // Create a Bluetooth request.
                byte[] btRequest = new byte[request.Length + 2];
                btRequest[0] = (byte)(length & 0xFF);
                btRequest[1] = (byte)((length & 0xFF00) >> 8);
                request.CopyTo(btRequest, 2);

                // Write the request to the NXT brick.
                stream.Write(btRequest, 0, btRequest.Length);
            }
            return;
        }
        /// <summary> 
        /// Recieves the reply from the NXT.
        /// </summary>
        /// <returns>Returns the reply from the NXT, as a byte array.</returns>
        public override byte[] RecieveReply()
        {
            byte[] byteIn = new byte[256];
            lock (commLock)
            {
                Stream stream = client.GetStream();
                int length = stream.ReadByte() + 256 * stream.ReadByte();
                for (int i = 0; i < length; i++)
                {
                    int data = stream.ReadByte();
                    byte bit = Convert.ToByte(data);
                    byteIn[i] = bit;
                }
            }
            if (byteIn == null) { throw new NXTNoReply(); }
            return byteIn;
        }
        
    }
}