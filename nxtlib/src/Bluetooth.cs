using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NXTLib
{
    public class Bluetooth : Protocol
    {

        #region Base

        /// <summary> 
        /// A class for connecting to the NXT via Bluetooth.
        /// </summary>
        /// <param name="port">The COM port used in bluetooth communication.</param>
        public Bluetooth(string port)
        {
            link = new ConnectLib.Bluetooth(port);
        }

        private new ConnectLib.Bluetooth link = null;
        /// <summary> 
        /// Connects to the NXT via Bluetooth.
        /// </summary>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public override bool Connect()
        {
            return link.Connect();
        }
        /// <summary> 
        /// Checks if the NXT is connected via Bluetooth.
        /// </summary>
        /// <returns>Returns true if NXT is connected, false otherwise.</returns>
        public override bool IsConnected
        {
            get
            {
                return link.IsConnected;
            }
        }
        /// <summary> 
        /// Sends a message via Bluetooth.
        /// </summary>
        /// <param name="port">The request, as a byte array.</param>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        internal override bool Send(byte[] request)
        {
            return link.Send(request);
        }
        /// <summary> 
        /// Disconnects from the NXT via Bluetooth.
        /// </summary>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public override bool Disconnect()
        {
            return link.Disconnect();
        }
        /// <summary> 
        /// Recieves the reply from the NXT.
        /// </summary>
        /// <returns>Returns the reply from the NXT, as a byte array.</returns>
        internal override byte[] RecieveReply()
        {
 	        return link.RecieveReply();
        }
        /// <summary> 
        /// The last error that occured.
        /// </summary>
        public override string LastError { get { return link.LastError; } }

        #endregion               
        
    }
}