using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NXTLib.Sensors
{
    public class TouchSensor : PassiveSensor
    {
        /// <summary>
        /// <para>The NXT Touch sensor.</para>
        /// </summary>
        public TouchSensor()
            : base(Protocol.SensorType.Switch, Protocol.SensorMode.Boolean)
        {
        }

        /// <summary>
        /// <para>Indicating if the sensor button is pressed or not.</para>
        /// </summary>
        public bool? IsPressed
        {
            get
            {
                if (pollData != null)
                    return (pollData.Value.value_scaled == 1);
                else
                    return null;
            }
        }

        /// <summary>
        /// <para>This event is fired when the touch sensor is pressed.</para>
        /// </summary>
        /// <seealso cref="OnReleased"/>
        /// <seealso cref="Poll"/>
        public event SensorEvent OnPressed;

        /// <summary>
        /// <para>This event is fired when the touch sensor is released.</para>
        /// </summary>
        /// <seealso cref="OnPressed"/>
        /// <seealso cref="Poll"/>
        public event SensorEvent OnReleased;

        // No implementation for the OnBumped NXT-G like event.

        private object pollDataLock = new object();

        /// <summary>
        /// <para>Polls the sensor, and fires events if appropriate.</para>
        /// </summary>
        /// <seealso cref="OnPressed"/>
        /// <seealso cref="OnReleased"/>
        public override void Poll()
        {
            if (brick.IsConnected)
            {
                bool? oldIsPressed, newIsPressed;
                lock (pollDataLock)
                {
                    oldIsPressed = this.IsPressed;
                    base.Poll();
                    newIsPressed = this.IsPressed;
                }

                if (oldIsPressed != null && newIsPressed != null)
                {
                    if (oldIsPressed.Value == false && newIsPressed.Value == true)
                    {
                        if (OnPressed != null) OnPressed(this);
                    }

                    if (oldIsPressed.Value == true && newIsPressed.Value == false)
                    {
                        if (OnReleased != null) OnReleased(this);
                    }
                }
            }
        }

    }
}
