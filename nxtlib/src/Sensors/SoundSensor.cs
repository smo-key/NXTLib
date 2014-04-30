using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NXTLib.Sensors
{
    public class SoundSensor : PassiveSensor
    {
        public SoundSensor()
            : base(Protocol.SensorType.Sound_dB, Protocol.SensorMode.PCTFullScale)
        {
            ThresholdIntensity = 50;
        }

        public bool dbA
        {
            get { return (type == Protocol.SensorType.Sound_dBA); }
            set
            {
                type = (value) ? Protocol.SensorType.Sound_dBA : Protocol.SensorType.Sound_dB;
                InitSensor();
            }
        }

        /// <summary>
        /// <para>The measured sound intensity.</para>
        /// </summary>
        public byte? Intensity
        {
            get
            {
                if (pollData != null)
                    return (byte)pollData.Value.value_scaled;
                else
                    return null;
            }
        }

        private byte thresholdIntensity;

        /// <summary>
        /// <para>Threshold sound intensity.</para>
        /// </summary>
        public byte ThresholdIntensity
        {
            get { return thresholdIntensity; }
            set
            {
                thresholdIntensity = value;
                if (thresholdIntensity > 100) thresholdIntensity = 100;
            }
        }

        /// <summary>
        /// <para>This event is fired when the measured sound intensity passes above the threshold.</para>
        /// </summary>
        public event SensorEvent OnAboveThresholdIntensity;

        /// <summary>
        /// <para>This event is fired when the measured sound intensity passes below the threshold.</para>
        /// </summary>
        public event SensorEvent OnBelowThresholdIntensity;

        private object pollDataLock = new object();

        /// <summary>
        /// <para>Polls the sensor, and fires events if appropriate.</para>
        /// </summary>
        public override void Poll()
        {
            if (brick.IsConnected)
            {
                byte? oldIntensity, newIntensity;
                lock (pollDataLock)
                {
                    oldIntensity = Intensity;
                    base.Poll();
                    newIntensity = Intensity;
                }

                if (oldIntensity != null && newIntensity != null)
                {
                    // Have the intensity passed over the threshold-intensity?
                    if (oldIntensity <= ThresholdIntensity && ThresholdIntensity < newIntensity)
                    {
                        if (OnAboveThresholdIntensity != null) OnAboveThresholdIntensity(this);
                    }

                    // Have the intensity passed below the threshold-intensity?
                    if (newIntensity < ThresholdIntensity && ThresholdIntensity <= oldIntensity)
                    {
                        if (OnBelowThresholdIntensity != null) OnBelowThresholdIntensity(this);
                    }
                }
            }
        }

    }
}
