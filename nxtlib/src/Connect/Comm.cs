using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NXTLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class Comm
    {
        /// <summary>
        /// <para>Protocol constructor used for communication.</para>
        /// </summary>
        public Comm()
            : base()
        { }

        /// <summary> 
        /// Connect to the device.
        /// </summary>
        public abstract bool Connect();

        /// <summary> 
        /// Disconnect the device.
        /// </summary>
        public abstract bool Disconnect();

        /// <summary>
        /// <para>Indicates if connected.</para>
        /// </summary>
        public abstract bool IsConnected
        {
            get;
        }

        /// <summary>
        /// <para>Sends a request.</para>
        /// </summary>
        /// <param name="request">The request to be sent.</param>
        public abstract bool Send(byte[] request);

        /// <summary>
        /// <para>Recieve the reply.</para>
        /// </summary>
        public abstract byte[] RecieveReply();
    }
}
