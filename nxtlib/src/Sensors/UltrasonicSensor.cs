using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NXTLib.Sensors
{

    // TODO: Add support for single-shot mode.
    // TODO: Add support for event-capture mode. Detect when other Ultrasonic sensors is nearby.

    public class UltrasonicSensor : DigitalSensor
    {
        public UltrasonicSensor()
            : base()
        {
            ThresholdDistanceCm = 50;
        }

        #region Sensor readings.

        /// <summary>
        /// <para>Returns the measured distance in cm.</para>
        /// </summary>
        public byte? DistanceCm
        {
            get { return pollData; }
        }

        #endregion

        #region I2C protocol.

        #region Constants.

        /// <summary>
        /// <para>Returns the "factory zero" of the sensor.</para>
        /// </summary>
        public byte ReadFactoryZero()  // Cal 1
        {
            byte[] request = new byte[] { deviceAddress, 0x11 };
            byte[] reply = SendN(request, 1);
            return reply[0];
        }

        /// <summary>
        /// <para>Returns the "factory scale factor" of the sensor.</para>
        /// </summary>
        public byte ReadFactoryScaleFactor()  // Cal 2
        {
            byte[] request = new byte[] { deviceAddress, 0x12 };
            byte[] reply = SendN(request, 1);
            return reply[0];
        }

        /// <summary>
        /// <para>Returns the factory scale divisor of the sensor.</para>
        /// </summary>
        public byte ReadFactoryScaleDivisor()
        {
            byte[] request = new byte[] { deviceAddress, 0x13 };
            byte[] reply = SendN(request, 1);
            return reply[0];
        }

        /// <summary>
        /// <para>Returns the measurement units if the sensor, e.g. "10E-2m".</para>
        /// </summary>
        public string ReadMeasurementUnits()
        {
            byte[] request = new byte[] { deviceAddress, 0x14 };
            byte[] reply = SendN(request, 7);
            return Encoding.ASCII.GetString(reply, 0, reply.Length).TrimEnd('\0');
        }

        #endregion

        #region Variables.

        public byte? ReadContinuousMeasurementsInterval()
        {
            return ReadByteFromAddress(0x40);
        }

        public byte? ReadCommandState()
        {
            return ReadByteFromAddress(0x41);
        }

        public byte? ReadMeasurementByteX(byte x)
        {
            if (x < 0 || 7 < x)
                throw new ArgumentException("The measurement byte number must be in the interval 0-7.");

            x += 0x42;  // 0 -> 0x42, 7 -> 0x49
            return ReadByteFromAddress(x);
        }

        public byte? ReadActualZero()  // Cal1
        {
            return ReadByteFromAddress(0x50);
        }

        public byte? ReadActualScaleFactor()  // Cal 2
        {
            return ReadByteFromAddress(0x51);
        }

        public byte? ReadActualScaleDivisor()
        {
            return ReadByteFromAddress(0x52);
        }

        #endregion

        #region Commands.

        public void OffCommand()
        {
            CommandToAddress(0x41, 0x00);
        }

        public void SingleShotCommand()
        {
            CommandToAddress(0x41, 0x01);
        }

        public void ContinuousMeasurementCommand()
        {
            CommandToAddress(0x41, 0x02);
        }

        public void EventCaptureCommand()
        {
            CommandToAddress(0x41, 0x03);
        }

        public void RequestWarmReset()
        {
            CommandToAddress(0x41, 0x04);
        }

        public void SetContinuousMeasurementInterval(byte interval)
        {
            CommandToAddress(0x40, interval);
        }

        public void SetActualZero(byte value)  // Cal 1
        {
            CommandToAddress(0x50, value);
        }

        public void SetActualScaleFactor(byte value)  // Cal 2
        {
            CommandToAddress(0x51, value);
        }

        public void SetActualScaleDivisor(byte value)
        {
            CommandToAddress(0x52, value);
        }

        #endregion

        #endregion

        #region NXT-G like events & NxtPollable overrides.

        private byte thresholdDistanceCm;

        public byte ThresholdDistanceCm
        {
            get { return thresholdDistanceCm; }
            set { thresholdDistanceCm = value; }
        }

        /// <summary>
        /// <para>This event is fired if the sensor detects an object within the threshold distance.</para>
        /// </summary>
        public event SensorEvent OnWithinThresholdDistanceCm;

        /// <summary>
        /// <para>This event is fired if the sensor does not detect an object within the threshold distance.</para>
        /// </summary>
        public event SensorEvent OnOutsideThresholdDistanceCm;

        /// <summary>
        /// <para>The data from the previous poll of the sensor.</para>
        /// </summary>
        protected byte? pollData;

        private object pollDataLock = new object();

        /// <summary>
        /// <para>Polls the sensor, and fires the NXT-G like events if appropriate.</para>
        /// </summary>
        public override void Poll()
        {
            if (brick.IsConnected)
            {
                byte? oldDistance, newDistance;
                lock (pollDataLock)
                {
                    oldDistance = DistanceCm;
                    pollData = ReadMeasurementByteX(0);
                    base.Poll();
                    newDistance = DistanceCm;
                }

                if (oldDistance.HasValue && newDistance.HasValue)
                {
                    // Have the intensity passed over the threshold-distance?
                    if (oldDistance <= ThresholdDistanceCm && ThresholdDistanceCm < newDistance)
                    {
                        if (OnOutsideThresholdDistanceCm != null) OnOutsideThresholdDistanceCm(this);
                    }

                    // Have the intensity passed below the threshold-distance?
                    if (newDistance < ThresholdDistanceCm && ThresholdDistanceCm <= oldDistance)
                    {
                        if (OnWithinThresholdDistanceCm != null) OnWithinThresholdDistanceCm(this);
                    }
                }
            }
        }

        #endregion



    }
}
