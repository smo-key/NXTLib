using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NXTLib.Sensors
{
    public class NoSensor : Sensor
    {
        public NoSensor()
            : base(Protocol.SensorType.No_Sensor, Protocol.SensorMode.Raw)
        { }

        /// <summary>
        /// <para>This empty poll method effectively cancels polling on this port.</para>
        /// </summary>
        public override void Poll()
        { }
    }
}
