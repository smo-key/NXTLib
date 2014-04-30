using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NXTLib
{
    public abstract class PassiveSensor : Sensor
    {
        /// <summary>
        /// <para>Constructor for all NXT Passive sensors.</para>
        /// </summary>
        /// <param name="sensorType">Sensor type.</param>
        /// <param name="sensorMode">Sensor mode.</param>
        public PassiveSensor(Protocol.SensorType type, Protocol.SensorMode mode)
            : base(type, mode)
        {
        }

        /// <summary>
        /// <para>The data from the last poll of the sensor.</para>
        /// </summary>
        protected Protocol.SensorInput? pollData;

        /// <summary>
        /// <para>Poll the sensor.</para>
        /// </summary>
        public override void Poll()
        {
            pollData = brick.ProtocolLink.GetSensorValues(port);
            base.Poll();
        }

    }
}
