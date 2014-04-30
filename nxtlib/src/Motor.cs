using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NXTLib
{
    
    /// <summary>
    /// <para>Delegate used for the NXT-G like events implemented in the motor.</para>
    /// </summary>
    /// <param name="motor">The motor</param>
    public delegate void MotorEvent(Motor motor);

    /// <summary>
    /// <para>Class representing a motor.</para>
    /// </summary>
    /// <seealso cref="NxtMotorSync"/>
    public class Motor : Pollable
    {
        /// <summary>
        /// <para>The port on the NXT brick that the motor is attached to.</para>
        /// </summary>
        private Protocol.MotorPortSingle motorPort;

        internal Brick Brick;
        /// <summary>
        /// <para>The port on the NXT brick that the motor is attached to.</para>
        /// </summary>
        internal protected Protocol.MotorPortSingle Port
        {
            get { return motorPort; }
            set { motorPort = value; }
        }

    }
}
