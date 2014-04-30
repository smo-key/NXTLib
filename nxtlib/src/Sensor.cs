using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NXTLib
{
    public delegate void SensorEvent(Sensor sensor);
    public abstract class Sensor : Pollable
    {
        protected Protocol.SensorPort port;
        private Protocol.SensorMode mode;
        protected Protocol.SensorType type;

        /// <summary>
        /// <para>Constructor for NXT Sensors.</para>
        /// </summary>
        /// <param name="type">The type of the sensor.</param>
        /// <param name="mode">The sensor's mode.</param>
        public Sensor(Protocol.SensorType type, Protocol.SensorMode mode)
        {
            this.type = type;
            this.mode = mode;
        }
        internal virtual void InitSensor()
        {
            if (brick.IsConnected)
            {
                brick.ProtocolLink.SetSensorMode(port, type, mode);
            }
        }
        internal Protocol.SensorPort Port
        {
            set { port = value; }
        }
        public override void Poll()
        {
            base.Poll();
        }

    }
}
