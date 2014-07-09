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
        private static readonly Guid NXT_GUID = BluetoothService.SerialPort;
        private static BluetoothClient client = null;
        private bool initialized = false;

        /// <summary
        /// <para>The communication protocols specific to Bluetooth.</para>
        /// </summary>
        public Bluetooth()
        {
            
        }

        private void Initialize()
        {
            if (initialized) { return; }
            try
            {
                client = new BluetoothClient();
                if (radio == null) { throw new NXTLinkNotSupported(); }
                radiomode = RadioMode.Discoverable;
                wirelessTimeout = new TimeSpan(0, 0, 30);
            }
            catch
            {
                throw new NXTLinkNotSupported();
            }
            initialized = true;
        }

        public BluetoothRadio radio { get { return BluetoothRadio.PrimaryRadio; } }
        public RadioMode radiomode { get { return radio.Mode; } set { radio.Mode = value; } }
        public TimeSpan wirelessTimeout { get { return client.InquiryLength; } set { client.InquiryLength = value; } }

        /// <summary>
        /// <para>Object to control mutex locking via Bluetooth.</para>
        /// </summary>
        private object commLock = new object();

        /// <summary>
        /// <para>Search for bricks connected via Bluetooth.</para>
        /// </summary>
        /// <returns>A list of brick information.</returns>
        public override List<Brick> Search()
        {
            Initialize();

            List<Brick> bricks = new List<Brick>();
            lock (commLock)
            {
                BluetoothDeviceInfo[] peers = client.DiscoverDevicesInRange();
                    
                foreach (BluetoothDeviceInfo info in peers)
                {
                    if (info.ClassOfDevice.Value != 2052) { continue; }

                    BluetoothEndPoint ep = new BluetoothEndPoint(info.DeviceAddress, NXT_GUID);
                    //BluetoothSecurity.SetPin(info.DeviceAddress, "1234");
                    BrickInfo _brick = new BrickInfo();
                    _brick.address = info.DeviceAddress.ToByteArray();
                    _brick.name = info.DeviceName;
                    Brick brick = new Brick(this, _brick);
                    bricks.Add(brick);
                }
            }
            if (bricks.Count == 0) { throw new NXTNoBricksFound(); }
            return bricks;
        }

        /// <summary> 
        /// Connect to the device via Bluetooth.
        /// </summary>
        public override void Connect(Brick brick)
        {
            Initialize();
            if (!IsConnected)
            {
                lock (commLock)
                {
                    BluetoothAddress adr = new BluetoothAddress(brick.brickinfo.address);
                    BluetoothDeviceInfo device = new BluetoothDeviceInfo(adr);

                    //BluetoothSecurity.RevokePin(adr);
                    //BluetoothSecurity.RemoveDevice(adr);

                    BluetoothSecurity.PairRequest(adr, "1234");
                    client.Connect(adr, NXT_GUID);
                }
                if (!IsConnected) { throw new NXTNotConnected(); }
            }
            return;
        }
        /// <summary> 
        /// Disconnects from the NXT via Bluetooth.
        /// </summary>
        public override void Disconnect(Brick brick)
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
                BluetoothRadio radio = BluetoothRadio.PrimaryRadio;
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
                Initialize();
                return client.Connected;
            }
        }
        /// <summary> 
        /// Sends a message via Bluetooth.
        /// </summary>
        /// <param name="request">The request, as a byte array.</param>
        public override void Send(byte[] request)
        {
            if (!IsConnected) { throw new NXTNotConnected(); }
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
            if (!IsConnected) { throw new NXTNotConnected(); }
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