using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NXTLib
{
    public abstract class ActiveSensor : Sensor
    {
        public ActiveSensor(Protocol.SensorType type, Protocol.SensorMode mode)
            : base(type, mode)
        {
        }
    }
}
