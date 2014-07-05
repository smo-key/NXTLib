using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NXTLib
{
    public partial class Brick
    {

        //The brick is the front-end of the library.  It contains a simplified version of ALL FUNCTIONS in NXTLib.Protocol.
        //TODO: Add more functions to front-end
        //TODO: Add timer

    #region Connection and Timer
        public enum LinkType { Bluetooth, USB }
        public Brick(LinkType type, string port)
        {
            switch (type)
            {
                case LinkType.Bluetooth:
                    ProtocolLink = new Bluetooth(port);
                    break;
                case LinkType.USB:
                    ProtocolLink = new USB();
                    break;
                default:
                    throw new Exception("[NXTLib] Parameter 'type' formatted incorrectly.");
            }
        }
        public Protocol ProtocolLink
        {
            get
            {
                return _link;
            }
            internal set
            {
                _link = value;
            }
        }
        private Protocol _link;
        private string error;
        public string LastError
        {
            get
            {
                return error;
            }
            internal set
            {
                error = value;
            }
        }

        public bool Connect()
        {
            if (!IsConnected)
            {
                if (!ProtocolLink.Connect())
                {
                    error = ProtocolLink.LastError;
                    return false;
                }
                InitSensors();
                EnableAutoPoll();
                return true;
            }
            else
            {
                return true;
            }
        }
        public bool Disconnect()
        {
            if (IsConnected)
            {
                if (!ProtocolLink.Disconnect())
                {
                    error = ProtocolLink.LastError;
                    return false;
                }
                DisableAutoPoll();
                return true;
            }
            else
            {
                return true;
            }
        }
        public bool IsConnected
        {
            get { return ProtocolLink.IsConnected; }
        }

        //private Timer timer = null;
        private void timer_Callback(object state)
        {
            if (IsConnected)
                ProtocolLink.KeepAlive();
        }
    #endregion

    #region Sensors
        
        private Dictionary<Protocol.SensorPort, Sensor> sensorArr = 
            new Dictionary<Protocol.SensorPort, Sensor>();

        /// <summary>
        /// <para>The sensor attached to port 1.</para>
        /// </summary>
        public Sensor Sensor1
        {
            get { return sensorArr[Protocol.SensorPort.Port_1]; }
            set { AttachSensor(value, Protocol.SensorPort.Port_1); }
        }

        /// <summary>
        /// <para>The sensor attached to port 2.</para>
        /// </summary>
        public Sensor Sensor2
        {
            get { return sensorArr[Protocol.SensorPort.Port_2]; }
            set { AttachSensor(value, Protocol.SensorPort.Port_2); }
        }

        /// <summary>
        /// <para>The sensor attached to port 3.</para>
        /// </summary>
        public Sensor Sensor3
        {
            get { return sensorArr[Protocol.SensorPort.Port_3]; }
            set { AttachSensor(value, Protocol.SensorPort.Port_3); }
        }

        /// <summary>
        /// <para>The sensor attached to port 4.</para>
        /// </summary>
        public Sensor Sensor4
        {
            get { return sensorArr[Protocol.SensorPort.Port_4]; }
            set { AttachSensor(value, Protocol.SensorPort.Port_4); }
        }

        /// <summary>
        /// <para>Attaches a sensor to the NXT brick.</para>
        /// </summary>
        /// <param name="sensor">The sensor</param>
        /// <param name="port">The port to attach the sensor to</param>
        private void AttachSensor(Sensor sensor, Protocol.SensorPort port)
        {
            if (sensor != null)
            {
                sensor.brick = this;
                sensor.Port = port;
                sensorArr[port] = sensor;
            }
        }

        /// <summary>
        /// <para>Initializes all the sensors of the brick.</para>
        /// <remarks>Normally this is done as part of the Connect() method, and is not something you need to worry about. However, whenever a program is started on the brick e.g. the MotorControl-program, you will need to call this method to re-initialize the sensors.</remarks>
        /// </summary>
        public void InitSensors()
        {
            foreach (Sensor sensor in sensorArr.Values)
            {
                sensor.InitSensor();
            }
        }

        private void EnableAutoPoll()
        {
            foreach (Motor motor in motorArr.Values)
            {
                motor.EnableAutoPoll();
            }

            foreach (Sensor sensor in sensorArr.Values)
            {
                sensor.EnableAutoPoll();
            }
        }

        private void DisableAutoPoll()
        {
            foreach (Sensor sensor in sensorArr.Values)
            {
                sensor.DisableAutoPoll();
            }

            foreach (Motor motor in motorArr.Values)
            {
                motor.DisableAutoPoll();
            }
        }

    #endregion

    #region Motors
        private Dictionary<Protocol.MotorPortSingle, Motor> motorArr = new Dictionary<Protocol.MotorPortSingle, Motor>();

        
        /// <summary>
        /// <para>The motor attached to port A.</para>
        /// </summary>
        public Motor MotorA
        {
            get { return motorArr[Protocol.MotorPortSingle.A]; }
            set { AttachMotor(value, Protocol.MotorPortSingle.A); }
        }

        /// <summary>
        /// <para>The motor attached to port B.</para>
        /// </summary>
        public Motor MotorB
        {
            get { return motorArr[Protocol.MotorPortSingle.B]; }
            set { AttachMotor(value, Protocol.MotorPortSingle.B); }
        }

        /// <summary>
        /// <para>The motor attached to port C.</para>
        /// </summary>
        public Motor MotorC
        {
            get { return motorArr[Protocol.MotorPortSingle.C]; }
            set { AttachMotor(value, Protocol.MotorPortSingle.C); }
        }

        /// <summary>
        /// <para>Attaches a motor to the NXT brick.</para>
        /// </summary>
        /// <param name="motor">The motor</param>
        /// <param name="port">The port to attach the motor to</param>
        private void AttachMotor(Motor motor, Protocol.MotorPortSingle port)
        {
            if (motor != null)
            {
                motorArr[port] = motor;
                motor.Brick = this;
                motor.Port = port;
            }
        }
    #endregion

    #region Files

        private string[] FindFiles(string fileMask)
        {
            List<string> fileArr = new List<string>();

            Protocol.FindFileReply? reply = ProtocolLink.FindFirst(fileMask);
            while (reply.HasValue && reply.Value.fileFound)
            {
                fileArr.Add(reply.Value.fileName);
                reply = ProtocolLink.FindNext(reply.Value.handle);
            }

            return fileArr.ToArray();
        }

        /// <summary>
        /// Uploads a file to the NXT, overwriting contents.
        /// </summary>
        /// <param name="filenamelocal">The name of the file to read from local disk.</param>
        /// <param name="filenameonbrick">The target file to upload to, with expension.</param>
        /// <returns>True if no error.</returns>
        public bool UploadFile(string filenamelocal, string filenameonbrick)
        {
            try
            {
                UInt32 filesize = 0;
                byte? filehandle;

                //Delete File, if Exists
                if (_link.DoesExist(filenameonbrick))
                {
                    if (!_link.Delete(filenameonbrick))
                    {
                        throw new Exception(_link.LastError);
                    }
                }

                //Read Local File
                byte[] localcontents;
                using (var stream = System.IO.File.OpenRead(filenamelocal))
                {
                    localcontents = new byte[(int)stream.Length];
                    int offset = 0;
                    while (offset < localcontents.Length)
                    {
                        int chunk = stream.Read(localcontents, offset, localcontents.Length - offset);
                        if (chunk == 0) { throw new Exception("Not all bytes written!"); }
                        offset += chunk;
                    }
                    stream.Close();
                }

                //Find Length of Local File
                filesize = (UInt32)localcontents.Length;

                //Open New File for Reading
                filehandle = _link.OpenWrite(filenameonbrick, filesize);
                if (!filehandle.HasValue) { throw new Exception(_link.LastError); }

                //Copy Local to Remote
                int? reply = _link.Write(filehandle.Value, localcontents);
                if (!reply.HasValue) { throw new Exception(_link.LastError); }

                //Close Remote Files
                if (!_link.Close(filehandle.Value)) { throw new Exception(_link.LastError); }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

    #endregion

    #region Programs

        public string Program
        {
            get
            {
                try
                {
                    return ProtocolLink.GetCurrentProgramName();
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    return null;
                }
            }
            set
            {
                string fileName = value ?? "";

                fileName = fileName.Trim();

                if (fileName != "")
                    ProtocolLink.StartProgram(fileName);
                else
                    ProtocolLink.StopProgram();
            }
        }

        public string[] Programs
        {
            get { return FindFiles("*.rxe"); }
        }

    #endregion

    #region Sounds

        /// <summary>
        /// <para>The sounds currently stored in the NXT brick.</para>
        /// </summary>
        public string[] Sounds
        {
            get { return FindFiles("*.rso"); }
        }

        /// <summary>
        /// <para>Plays a sound file stored in the NXT brick.</para>
        /// </summary>
        /// <param name="soundFile">The sound file</param>
        public void PlaySoundfile(string soundFile)
        {
            ProtocolLink.PlaySoundFile(false, soundFile);
        }

        /// <summary>
        /// <para>Stops all playing sound; sound files and tones.</para>
        /// </summary>
        public void StopSound()
        {
            ProtocolLink.StopSoundPlayback();
        }

        /// <summary>
        /// <para>Plays a tone.</para>
        /// </summary>
        /// <param name="frequency">Frequency for the tone, Hz</param>
        /// <param name="duration">Duration of the tone, ms</param>
        public void PlayTone(UInt16 frequency, UInt16 duration)
        {
            ProtocolLink.PlayTone(frequency, duration);
        }

    #endregion

    #region Miscellaneous

        /// <summary>
        /// <para>The name of the NXT brick.</para>
        /// </summary>
        /// <remarks>
        /// <para>For some reason only the first 8 characters is remembered when the NXT is turned off. This is with version 1.4 of the firmware, and it may be fixed with newer versions.</para>
        /// </remarks>
        public string Name
        {
            get
            {
                Protocol.GetDeviceInfoReply? reply = ProtocolLink.GetDeviceInfo();
                if (reply.HasValue)
                    return reply.Value.Name;
                else
                    return null;
            }
            set { ProtocolLink.SetBrickName(value); }
        }

        /// <summary>
        /// <para>The battery level of the NXT brick in millivolts.</para>
        /// </summary>
        public int BatteryLevel
        {
            get
            {
                int? reply = ProtocolLink.GetBatteryLevel();
                if (reply.HasValue)
                    return reply.Value;
                else
                    return 0;
            }
        }

    #endregion

    }
}
