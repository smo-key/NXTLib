using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace NXTLib
{
    public class Bluetooth : Protocol
    {

        /// <summary>
        /// <para>The communication protocols specific to Bluetooth.</para>
        /// </summary>
        /// <param name="serialport">The COM port used by the Bluetooth link.  For example, COM3 would be the third COM port.</param>
        public Bluetooth(string serialport)
        {
            this.serialPort = new SerialPort(serialport);
            this.port = serialport;
            createnew = true;
        }
        /// <summary>
        /// <para>The communication protocols specific to Bluetooth.</para>
        /// </summary>
        /// <param name="serialport">The custom port used by the Bluetooth link.  This is required if you want to use custom settings.</param>
        public Bluetooth(SerialPort serialport)
        {
            this.serialPort = serialport;
            createnew = false;
        }

        private bool createnew;

        /// <summary>
        /// <para>The COM port value used by the Bluetooth connection.</para>
        /// </summary>
        private string port;
        /// <summary>
        /// <para>The serial port used by the Bluetooth connection.</para>
        /// </summary>
        private SerialPort serialPort = new SerialPort();
        /// <summary>
        /// <para>Object to control mutex locking on the serial port.</para>
        /// </summary>
        private object serialPortLock = new object();


        /// <summary> 
        /// Connect to the device via Bluetooth.
        /// </summary>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public override bool Connect()
        {
            try
            {
                lock (serialPortLock)
                {
                    if (createnew == true)
                    {
                        serialPort.PortName = port;  //<<<< Change this number for your computer
                        //Bluetooth.BaudRate = 96000;
                        //Bluetooth.Parity = System.IO.Ports.Parity.None;
                        //Bluetooth.DataBits = 8;
                        //Bluetooth.StopBits = System.IO.Ports.StopBits.One;
                        serialPort.ReadTimeout = 300;  //5000ms
                        serialPort.WriteTimeout = 300; //5000ms
                    }
                    serialPort.Open();
                }
                return true;
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
                lock (serialPortLock)
                {
                    serialPort.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
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
                try
                {
                    return
                        serialPort != null &&
                        serialPort.IsOpen &&
                        serialPort.CtsHolding;  // Necessary, or a NXT that is turned of will report as Connected!
                }
                catch (System.ObjectDisposedException)
                {
                    return false;
                }
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
                lock (serialPortLock)
                {
                    int length = request.Length;

                    // Create a Bluetooth request.
                    byte[] btRequest = new byte[request.Length + 2];
                    btRequest[0] = (byte)(length & 0xFF);
                    btRequest[1] = (byte)((length & 0xFF00) >> 8);
                    request.CopyTo(btRequest, 2);

                    // Write the request to the NXT brick.
                    serialPort.Write(btRequest, 0, btRequest.Length);
                }
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
                lock (serialPortLock)
                {
                    int length = serialPort.ReadByte() + 256 * serialPort.ReadByte();
                    for (int i = 0; i < length; i++)
                    {
                        int data = serialPort.ReadByte();
                        byte bit = Convert.ToByte(data);
                        byteIn[i] = bit;
                    }
                }
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