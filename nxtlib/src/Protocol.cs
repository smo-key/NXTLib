using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using NXTLib.BluetoothWrapper;
using NXTLib.BluetoothWrapper.Sockets;
using NXTLib.BluetoothWrapper.Bluetooth;
using System.IO;

namespace NXTLib
{
    public enum LinkType { USB, Bluetooth, Null }

    public abstract class Protocol
    {
        //TODO: Complete the missing portions of the library after phase 3!
        //TODO: Ensure comments are written for each method!

#region Base
        public Protocol()
            : base()
        { }

        public struct BrickInfo
        {
            private byte[] _adr;
            public byte[] address
            {
                get { return _adr; }
                set
                {
                    _adr = new byte[6];
                    for (int i = 0; i < 6; i++)
                    {
                        _adr[i] = value[i];
                    }
                }
            }
            public string name { get; set; }
        }

        public abstract List<Brick> Search();
        public abstract void Connect(Brick brick);
        public abstract void Disconnect(Brick brick);
        public abstract void Send(byte[] request);
        public abstract byte[] RecieveReply();
        internal SerialPort link { get; set; }
        public abstract void Initialize();
        public bool IsInitialized { get; internal set; }
        public abstract bool IsConnected { get; }
        public abstract bool IsSupported { get; }

#endregion

#region Misc
        /// <summary> 
        /// [Native] Starts a program (.rxe) on the NXT brick.
        /// </summary>
        /// <param name="filename">The name of the program you wish to run.</param>
        public void StartProgram(string filename)
        {
            filename = ValidateFilename(filename, FileType.Program, true);
            byte[] fileNameByteArr = Encoding.ASCII.GetBytes(filename);

            byte[] request = new byte[22];
            request[0] = (byte)(0x00);
            request[1] = (byte)(DirectCommand.StartProgram);
            fileNameByteArr.CopyTo(request, 2);
            CompleteRequest(request);

            return;
        }

        /// <summary> 
        /// [Native] Stops the currently running program on the NXT brick.
        /// </summary>
        /// <returns>Returns true if program running then stopped successfully.  Returns false if no program was running.</returns>
        public bool StopProgram()
        {
            try
            {
                byte[] request = new byte[22];
                request[0] = (byte)(0x00);
                request[1] = (byte)(DirectCommand.StopProgram);
                CompleteRequest(request);

                return true;
            }
            catch (NXTNoActiveProgram)
            {
                return false;
            }
        }

        /// <summary> 
        /// [Native] Plays a beep tone on the NXT brick.
        /// </summary>
        /// <param name="frequency">The frequency, in Hz, where frequency is between 200 and 14000 Hz.</param>
        /// <param name="duration">The duration of the tone, in milliseconds.</param>
        public void PlayTone(UInt16 frequency, UInt16 duration)
        {
            if (frequency < 200) frequency = 200;
            if (frequency > 14000) frequency = 14000;

            byte[] request = new byte[6];
            request[0] = (byte)(0x00);
            request[1] = (byte)(DirectCommand.PlayTone);
            SetUInt16(frequency, request, 2);
            SetUInt16(duration, request, 4);
            CompleteRequest(request);

            return;
        }

        /// <summary> 
        /// [Native] Plays a sound file from the NXT.
        /// </summary>
        /// <param name="loop">Set to true to loop the sound until stopped.</param>
        /// <param name="filename">The name of the sound file (.rso) on the NXT.</param>
        public void PlaySoundFile(bool loop, string filename)
        {
            filename = ValidateFilename(filename, FileType.Sound, true);

            byte[] request = new byte[23];
            request[0] = (byte)(0x00);
            request[1] = (byte)(DirectCommand.PlaySoundFile);
            request[2] = (byte)(loop ? 0xFF : 0x00);
            Encoding.ASCII.GetBytes(filename).CopyTo(request, 3);
            CompleteRequest(request);

            return;
        }

        /// <summary> 
        /// [Native] Returns the voltage of the NXT battery, in millivolts.
        /// </summary>
        /// <returns>The battery level, in millivolts.</returns>
        public int GetBatteryLevel()
        {
            byte[] request = new byte[] {
            0x00,
            (byte) DirectCommand.GetBatteryLevel
            };

            byte[] reply = CompleteRequest(request);

            UInt16 voltage = GetUInt16(reply, 3);
            return Convert.ToInt32(voltage);
        }

        /// <summary> 
        /// [Native] Stops sound playback on the NXT.
        /// </summary>
        public void StopSoundPlayback()
        {
            byte[] request = new byte[] {
            0x00,
            (byte) DirectCommand.StopSoundPlayback
            };

            CompleteRequest(request);
            return;
        }

        /// <summary> 
        /// [Native] Keeps the NXT on and
        ///  returns a value saying how long until the NXT falls asleep again, in milliseconds.
        /// </summary>
        /// <returns>The time until the NXT falls asleep again, in milliseconds.</returns>
        public int KeepAlive()
        {
            byte[] request = new byte[] {
            0x00,
            (byte) DirectCommand.KeepAlive
            };

            byte[] reply = CompleteRequest(request);

            UInt32 sleeplimit = GetUInt32(reply, 3);
            return Convert.ToInt32(sleeplimit);
        }

        /// <summary> 
        /// [Native] Gets the name of the currently running program on the NXT brick.
        /// </summary>
        /// <returns>The name of the currently running program, as a string.  Null if no program running.</returns>
        public string GetCurrentProgramName()
        {
            try
            {
                byte[] request = new byte[] {
                (byte) (0x00),
                (byte) (DirectCommand.GetCurrentProgramName),
                };

                byte[] reply = CompleteRequest(request);
                string filename = Encoding.ASCII.GetString(reply, 3, 20).TrimEnd('\0');
                return filename;
            }
            catch (NXTNoActiveProgram)
            {
                return null;
            }
        }

        /// <summary> 
        /// The structure used in GetFirmwareVersion().
        /// </summary>
        public struct GetFirmwareVersionReply
        {
            /// <summary>
            /// <para>The protocol version.</para>
            /// </summary>
            public Version protocolVersion;

            /// <summary>
            /// <para>The firmware version.</para>
            /// </summary>
            public Version firmwareVersion;
        }

        /// <summary> 
        /// The structure used in GetDeviceInfo().
        /// </summary>
        public struct GetDeviceInfoReply
        {
            /// <summary>
            /// <para>The name of the NXT brick.</para>
            /// </summary>
            public string Name;

            /// <summary>
            /// <para>The Bluetooth IP address.</para>
            /// </summary>
            public byte[] Address;

            /// <summary>
            /// <para>The Bluetooth signal strength.</para>
            /// </summary>
            public UInt32 SignalStrength;

            /// <summary>
            /// <para>The size of the free user flash.</para>
            /// </summary>
            public UInt32 freeUserFlash;
        }

        /// <summary> 
        /// [Native] Get the firware and protocol versions of the NXT.
        /// </summary>
        /// <returns>Returns the reply as a nullable GetFirmwareVersionReply.</returns>
        public GetFirmwareVersionReply GetFirmwareVersion()
        {
            byte[] request = new byte[] {
            0x01,
            (byte) MessageCommand.GetFirmwareVersion
            };

            byte[] reply = CompleteRequest(request);
            GetFirmwareVersionReply result;
            result.protocolVersion = new Version(reply[4], reply[3]);
            result.firmwareVersion = new Version(reply[6], reply[5]);
            return result;
        }

        /// <summary> 
        /// [Native] Boot the NXT.  THIS COMMAND MAY ONLY BE ACCEPTED BY USB!
        /// </summary>
        public void Boot()
        {
            byte[] request = new byte[21];
            request[0] = 0x01;
            request[1] = (byte)MessageCommand.Boot;
            Encoding.ASCII.GetBytes("Let's dance: SAMBA").CopyTo(request, 2);

            byte[] reply = CompleteRequest(request);
            string result = Encoding.ASCII.GetString(reply, 3, 4).TrimEnd('\0');
            if (result != "Yes")
            {
                throw new NXTReplyIncorrect();
            }
            return;
        }

        /// <summary> 
        /// [Native] Sets the name of the NXT.
        /// </summary>
        /// <param name="newname">The NXT's new name.</param>
        public void SetBrickName(string newname)
        {
            if (newname.Length > 15)
                newname = newname.Substring(0, 15);

            byte[] request = new byte[18];
            request[0] = 0x01;
            request[1] = (byte)MessageCommand.SetBrickName;
            Encoding.ASCII.GetBytes(newname).CopyTo(request, 2);

            CompleteRequest(request);

            return;
        }

        /// <summary> 
        /// [Native] Returns the NXT's information.
        /// </summary>
        /// <returns>The reply as a nullable structure, GetDeviceInfoReply.</returns>
        public GetDeviceInfoReply GetDeviceInfo()
        {
            byte[] request = new byte[] {
            0x01,
            (byte) MessageCommand.GetDeviceInfo
            };

            byte[] reply = CompleteRequest(request);

            GetDeviceInfoReply result;
            result.Name = Encoding.ASCII.GetString(reply, 3, 15).TrimEnd('\0');
            result.Address = new byte[7];
            Array.Copy(reply, 18, result.Address, 0, 7);
            result.SignalStrength = GetUInt32(reply, 25);
            result.freeUserFlash = GetUInt32(reply, 29);

            return result;
        }

        /// <summary> 
        /// [Native] Completely wipes the NXT's user-created data.  USE WITH CARE!
        /// </summary>
        /// <param name="waitfor">Set to true to wait until the NXT has completed its memory wipe, false otherwise.</param>
        public void DeleteUserFlash(bool waitfor)
        {
            byte[] request = new byte[] {
            0x01,
            (byte) MessageCommand.DeleteUserFlash
            };

            Send(request);
            if (waitfor)
                System.Threading.Thread.Sleep(5000);
            return;
        }

#endregion

#region Sensors
        public enum SensorPort { Port_1 = 0, Port_2 = 1, Port_3 = 2, Port_4 = 3 };
        public enum SensorType
        {
            No_Sensor = 0x00, Switch = 0x01,
            Temperature = 0x02, Reflection = 0x03, Angle = 0x04,
            Light_Active = 0x05, Light_Inactive = 0x06, Sound_dB = 0x07,
            Sound_dBA = 0x08, Custom = 0x09, Lowspeed = 0x0A, Lowspeed_9V = 0x0B,
            Highspeed = 0x0C, ColorFull = 0x0D, ColorRed = 0x0E, ColorGreen = 0x0F,
            ColorBlue = 0x10, ColorNone = 0x11
        };
        public enum SensorMode
        {
            Raw = 0x00, Boolean = 0x20,
            TransitionCNT = 0x40, PeriodCounter = 0x60, PCTFullScale = 0x80,
            Celsius = 0xA0, Fahrenheit = 0xC0, AngleSteps = 0xE0,
            SlopeMask = 0x1F, ModeMask = 0xE0
        };
        public struct SensorInput
        {
            public bool valid;
            public bool calibrated;
            public SensorType type;
            public SensorMode mode;
            public int value_raw;
            public int value_normalized;
            public int value_scaled;
            public int value_calibrated;
        }

        /// <summary>
        /// [Native] Set a sensor's state.  This is REQUIRED
        ///  to read realistic values through GetSensorValues().
        /// </summary>
        /// <param name="port">The port of the attached sensor.</param>
        /// <param name="type">The type of the attached sensor.</param>
        /// <param name="mode">The mode of the attached sensor.</param>
        public void SetSensorMode(SensorPort port,
            SensorType type, SensorMode mode)
        {
            byte[] request = new byte[] {
            (byte) (0x00),
            (byte) (DirectCommand.SetInputMode),
            (byte) port,
            (byte) type,
            (byte) mode
            };

            CompleteRequest(request);
            return;
        }

        /// <summary>
        /// [Native] Get a sensor's current state.
        /// </summary>
        /// <param name="port">The port of the attached sensor.</param>
        /// <returns>Returns a SensorInput package.</returns>
        public SensorInput GetSensorValues(SensorPort port)
        {
            byte[] request = new byte[] {
            0x00,
            (byte) DirectCommand.GetInputValues,
            (byte) port
            };

            byte[] reply = CompleteRequest(request);
            if (reply[3] != (byte)port)
            {
                throw new NXTReplyIncorrect();
            }
            SensorInput result = new SensorInput();
            result.valid = (reply[4] != 0x00);
            result.calibrated = (reply[5] != 0x00);
            result.type = (SensorType)reply[6];
            result.mode = (SensorMode)reply[7];
            result.value_raw = GetUInt16(reply, 8);
            result.value_normalized = GetUInt16(reply, 10);
            result.value_scaled = GetInt16(reply, 12);
            result.value_calibrated = GetInt16(reply, 14);
            return result;
        }

        /// <summary>
        /// [Native] Reset's the selected sensor's scaled value.
        /// </summary>
        /// <param name="port">The port of the attached sensor.</param>
        public void ResetSensorScale(SensorPort port)
        {
            byte[] request = new byte[] {
            (byte) (0x00),
            (byte) (DirectCommand.ResetInputScaledValue),
            (byte) port
            };

            CompleteRequest(request);
            return;
        }

        /// <summary>
        /// [Native] Reads the number of bytes readable in a lowpeed sensor port.
        /// </summary>
        /// <param name="port">The port of the attached sensor.</param>
        /// <returns>Count of available bytes to read.</returns>
        public byte LowspeedGetStatus(SensorPort port)
        {
            byte[] request = new byte[] {
            (byte) (0x00),
            (byte) (DirectCommand.LSGetStatus),
            (byte) port
            };

            byte[] reply = CompleteRequest(request);
            return reply[3];
        }

        /// <summary>
        /// [Native] Writes an array of bytes to a lowspeed sensor port.
        /// </summary>
        /// <param name="port">The port of the attached sensor.</param>
        /// <param name="txData">The tx Data to be written.</param>
        /// <param name="rxDataLength">The rx Data Length. </param>
        public void LowspeedWrite(SensorPort port, byte[] txData, byte rxDataLength)
        {
            byte txDataLength = (byte)txData.Length;
            if (txDataLength == 0)
                throw new NXTException("No data to send.");

            if (txDataLength > 16)
                throw new NXTException("Tx data may not exceed 16 bytes.");

            if (rxDataLength < 0 || 16 < rxDataLength)
                throw new NXTException("Rx data length should be in the interval 0-16.");

            byte[] request = new byte[5 + txDataLength];
            request[0] = (byte)(0x00);
            request[1] = (byte)DirectCommand.LSWrite;
            request[2] = (byte)port;
            request[3] = txDataLength;
            request[4] = rxDataLength;
            txData.CopyTo(request, 5);
            CompleteRequest(request);

            return;
        }

        /// <summary>
        /// [Native] Reads the number of bytes readable in a lowpeed sensor port.
        /// </summary>
        /// <param name="port">The port of the attached sensor.</param>
        /// <returns>The data from the lowspeed sensor, as a byte array.</returns>
        public byte[] LowspeedRead(SensorPort port)
        {
            byte[] request = new byte[] {
            0x00,
            (byte) DirectCommand.LSRead,
            (byte) port
            };

            byte[] reply = CompleteRequest(request);

            byte bytesRead = reply[3];
            byte[] rxData = new byte[bytesRead];
            Array.Copy(reply, 4, rxData, 0, bytesRead);

            return rxData;
        }

#endregion

#region Motors
        public enum MotorPort { A = 0x00, B = 0x01, C = 0x02, AB = 0xC0, AC = 0xC1, BC = 0xC2, ABC = 0xFF };
        public enum MotorPortSingle { A = 0, B = 1, C = 2 };
        public enum MotorPortSyncable { AB, BA, AC, CA, BC, CB };
        public enum MotorStop { Coast, Brake };
        public enum MotorSmooth { Smooth_Start = 0x10, Smooth_Stop = 0x40 };
        /*public class MotorTimer
        {
            public MotorTimer(bool waitfor)
            {
                degrees = 0;
                time = 0;
                mode = TimerMode.Disabled;
                WaitFor = waitfor;
            }
            public MotorTimer(int timer_degrees, bool waitfor)
            {
                degrees = timer_degrees;
                time = 0;
                mode = TimerMode.Degrees;
                WaitFor = waitfor;
            }
            public MotorTimer(int timer_seconds)
            {
                degrees = 0;
                time = timer_seconds;
                mode = TimerMode.Time;
                WaitFor = false;
            }
                
                
            public enum TimerMode { Disabled, Time, Degrees };
            public int time { get; set; }
            public int degrees { get; set; }
            public TimerMode mode { get; set; }
            public bool WaitFor { get; set; }
            internal bool status = false;
            public void WaitFor()
            {
                while (status == false)
                {
                    System.Threading.Thread.Sleep(5);
                }
            }        
               
        }*/

        public struct MotorStateOut
        {
            /// <summary> 
            /// MotorPort power.
            /// </summary>
            public int power { get; internal set; }
            /// <summary> 
            /// Current tachometer position relative to last movement.
            /// </summary>
            public int blocktachocount { get; internal set; }
            /// <summary> 
            /// Tachometer counts relative to last reset.
            /// </summary>
            public int tachocount { get; internal set; }
            /// <summary> 
            /// Current motor position relative to last reset of rotation sensor.
            /// </summary>
            public int rotationcount { get; internal set; }
        }

        /// <summary>
        /// [Native] Reset the motor rotation sensor.
        /// </summary>
        /// <param name="port">The port of the selected motor.</param>
        /// <param name="relative">If set to true, the reset's position will be relative to the last
        ///  movement.  Otherwise, it is set at an absolute position.</param>
        public void ResetMotorPosition(MotorPortSingle port, bool relative)
        {
            byte[] request = new byte[] {
            (byte) (0x00),
            (byte) (DirectCommand.ResetMotorPosition),
            (byte) port,
            (byte) (relative ? 0xFF : 0x00)
            };
            CompleteRequest(request);

            return;
        }

        /// <summary> 
        /// [Native] Get the state of an NXT motor port.
        /// </summary>
        /// <param name="port">The port of the selected motor.</param>
        /// <returns>Returns a MotorStateOut package.</returns>
        public MotorStateOut GetMotorState(MotorPortSingle port)
        {
            byte[] request = new byte[] {
            0x00,
            (byte) DirectCommand.GetOutputState,
            (byte) port
            };

            byte[] reply = CompleteRequest(request);
            byte motorportout = reply[3];
            if (reply[3] != (byte)port)
            {
                throw new NXTReplyIncorrect();
            }
            MotorStateOut result = new MotorStateOut();
            result.power = reply[4];
            result.tachocount = GetInt32(reply, 13);
            result.blocktachocount = GetInt32(reply, 17);
            result.rotationcount = GetInt32(reply, 21);
            return result;

            /*
            NxtGetOutputStateReply result;
            result.power = (sbyte)reply[4];
            result.mode = (NxtMotorMode)reply[5];
            result.regulationMode = (NxtMotorRegulationMode)reply[6];
            result.turnRatio = (sbyte)reply[7];
            result.runState = (NxtMotorRunState)reply[8];
            result.tachoLimit = Util.GetUInt32(reply, 9);
            result.tachoCount = Util.GetInt32(reply, 13);
            result.blockTachoCount = Util.GetInt32(reply, 17);
            result.rotationCount = Util.GetInt32(reply, 21);

            return result;*/
        }

        /// <summary> 
        /// [Native] Move NXT motors.
        /// </summary>
        /// <param name="port">The ports of the selected motors.</param>
        /// <param name="power">The power at which to move the motors, between -100 and 100.</param>
        public void MoveMotors(MotorPort port, int power)
        {
            MotorMode mode = MotorMode.On_Regulated;
            if ((power < 75) && (power > -75)) { mode = MotorMode.MotorOn; }
            if (port != MotorPort.AB && port != MotorPort.AC && port != MotorPort.BC)
            {
                SetOutputState((Motor)((byte)port), (sbyte)power, mode,
                    MotorReg.Speed, 0, MotorState.Running, 0);
            }
            else
            {
                Motor[] motors = new Motor[] { };
                if (port == MotorPort.AB) { motors = new Motor[] { Motor.A, Motor.B }; }
                if (port == MotorPort.AC) { motors = new Motor[] { Motor.A, Motor.C }; }
                if (port == MotorPort.BC) { motors = new Motor[] { Motor.B, Motor.C }; }
                SetOutputState(motors[0], (sbyte)power, mode,
                    MotorReg.Sync, 0, MotorState.Running, (uint)0);
                SetOutputState(motors[1], (sbyte)power, mode,
                    MotorReg.Sync, 0, MotorState.Running, (uint)0);
            }
            return;
        }

        /// <summary> 
        /// [Native] Move NXT motors, specifying whether to brake or coast.
        /// </summary>
        /// <param name="port">The ports of the selected motors.</param>
        /// <param name="power">The power at which to move the motors, between -100 and 100.</param>
        /// <param name="stop">Whether to brake or coast during operation.  Braking uses far more power.</param>
        public void MoveMotors(MotorPort port, int power, MotorStop stop)
        {
            MotorMode mode = MotorMode.On_Regulated;
            if (stop == MotorStop.Coast)
            {
                mode = MotorMode.On_Regulated;
                if ((power < 75) && (power > -75)) { mode = MotorMode.MotorOn; }
            }
            if (stop == MotorStop.Brake)
            {
                mode = MotorMode.On_Brake_Regulated;
                if ((power < 75) && (power > -75)) { mode = MotorMode.On_Brake; }
            }
            if (port != MotorPort.AB && port != MotorPort.AC && port != MotorPort.BC)
            {
                SetOutputState((Motor)((byte)port), (sbyte)power, mode,
                    MotorReg.Speed, 0, MotorState.Running, 0);
            }
            else
            {
                Motor[] motors = new Motor[] { };
                if (port == MotorPort.AB) { motors = new Motor[] { Motor.A, Motor.B }; }
                if (port == MotorPort.AC) { motors = new Motor[] { Motor.A, Motor.C }; }
                if (port == MotorPort.BC) { motors = new Motor[] { Motor.B, Motor.C }; }
                SetOutputState(motors[0], (sbyte)power, mode,
                    MotorReg.Sync, 0, MotorState.Running, (uint)0);
                SetOutputState(motors[1], (sbyte)power, mode,
                    MotorReg.Sync, 0, MotorState.Running, (uint)0);
            }
            return;
        }

        /// <summary> 
        /// [Native] Smoothly start or stop motors, specifying whether to brake or coast.
        /// </summary>
        /// <param name="port">The ports of the selected motors.</param>
        /// <param name="power">The power at which to move the motors, between -100 and 100.</param>
        /// <param name="stop">Whether to brake or coast during operation.  Braking uses far more power.</param>
        /// <param name="smooth">Whether to smoothly spin up or down the motors.</param>
        /// <param name="degrees">The number of degrees the motor revolves until is it fully spun up or down.  Smaller values 
        ///  make the motors move more smoothly.  Set degrees to zero to run infinitely.</param>
        public void MoveMotors(MotorPort port, int power, MotorStop stop, MotorSmooth smooth, int degrees)
        {
            MotorMode mode = MotorMode.On_Regulated;
            if (stop == MotorStop.Coast)
            {
                mode = MotorMode.On_Regulated;
                if ((power < 75) && (power > -75)) { mode = MotorMode.MotorOn; }
            }
            if (stop == MotorStop.Brake)
            {
                mode = MotorMode.On_Brake_Regulated;
                if ((power < 75) && (power > -75)) { mode = MotorMode.On_Brake; }
            }
            if (port != MotorPort.AB && port != MotorPort.AC && port != MotorPort.BC)
            {
                SetOutputState((Motor)((byte)port), (sbyte)power, mode,
                    MotorReg.Speed, 0, MotorState.Running, 0);
            }
            else
            {
                MotorState state = MotorState.Rampup;
                if (smooth == MotorSmooth.Smooth_Stop)
                {
                    state = MotorState.RampDown;
                }
                Motor[] motors = new Motor[] { };
                if (port == MotorPort.AB) { motors = new Motor[] { Motor.A, Motor.B }; }
                if (port == MotorPort.AC) { motors = new Motor[] { Motor.A, Motor.C }; }
                if (port == MotorPort.BC) { motors = new Motor[] { Motor.B, Motor.C }; }
                SetOutputState(motors[0], (sbyte)power, mode,
                    MotorReg.Sync, 0, state, (uint)degrees);
                SetOutputState(motors[1], (sbyte)power, mode,
                    MotorReg.Sync, 0, state, (uint)degrees);
            }
            return;
        }

        /// <summary> 
        /// [Native] Sync two motors as driving motors, specifying whether to brake or coast and the turn ratio.
        /// </summary>
        /// <param name="port">The ports of the selected motors.  Exactly two motors must be selected.</param>
        /// <param name="power">The power at which to move the motors, between -100 and 100.</param>
        /// <param name="stop">Whether to brake or coast during operation.  Braking uses far more power.</param>
        /// <param name="turnratio">The turn ratio of the two motors, between -100 and 100.  A ratio of zero will make both motors move straight.
        ///   A negative ratio will move the left motor more and a positive ratio would move the right motor more.</param>
        public void MoveMotors(MotorPortSyncable port, int power, MotorStop stop, int turnratio)
        {
            int[] pow = new int[] { power, power };

            //Arc and Point rotation


            //Both arc and point rotation may be used for joystick control
            double ratio = (double)turnratio;
            double powr = (double)power;
            //pow = new int[] { Convert.ToInt32(Math.Floor(((50 - diff) / 100) * powr)),
            //Convert.ToInt32(Math.Floor(((50 + diff) / 100) * powr)) };
            //pow = new int[] { Convert.ToInt32(Math.Floor(((50 - diff) / 100) * powr)),
            //Convert.ToInt32(Math.Floor(((50 + diff) / 100) * powr)) };

            //slave motor power = master power * turn ratio

            //find which motor is the master
            if (turnratio != 0)
            {
                bool masterleft = true;
                if (power < 0)
                {
                    if (turnratio < 0)
                    {
                        masterleft = true;
                    }
                    else
                    {
                        masterleft = false;
                    }
                }
                else
                {
                    if (turnratio <= 0)
                    {
                        masterleft = false;
                    }
                    else
                    {
                        masterleft = true;
                    }
                }
                double equ = ((-2 * Math.Abs(ratio)) + 100) / 100 * powr;
                if (masterleft == true)
                {
                    pow = new int[] { power, Convert.ToInt32(Math.Floor(equ)) };
                }
                else
                {
                    pow = new int[] { Convert.ToInt32(Math.Floor(equ)), power };
                }
            }

            MotorMode mode = MotorMode.MotorOn;
            if (stop == MotorStop.Coast)
            {
                mode = MotorMode.On_Regulated;
                if ((power < 75) && (power > -75))
                {
                    mode = MotorMode.MotorOn;
                }
            }
            if (stop == MotorStop.Brake)
            {
                mode = MotorMode.On_Brake_Regulated;
                if ((power < 75) && (power > -75))
                {
                    mode = MotorMode.On_Brake;
                }
            }

            Motor[] motors = new Motor[] { };
            if (port == MotorPortSyncable.AB) { motors = new Motor[] { Motor.A, Motor.B }; }
            if (port == MotorPortSyncable.AC) { motors = new Motor[] { Motor.A, Motor.C }; }
            if (port == MotorPortSyncable.BC) { motors = new Motor[] { Motor.B, Motor.C }; }
            if (port == MotorPortSyncable.BA) { motors = new Motor[] { Motor.B, Motor.A }; }
            if (port == MotorPortSyncable.CA) { motors = new Motor[] { Motor.C, Motor.A }; }
            if (port == MotorPortSyncable.CB) { motors = new Motor[] { Motor.C, Motor.B }; }
            SetOutputState(motors[0], (sbyte)pow[0], mode,
                MotorReg.Speed, (sbyte)turnratio, MotorState.Running, (uint)0);
            SetOutputState(motors[1], (sbyte)pow[1], mode,
                MotorReg.Speed, (sbyte)turnratio, MotorState.Running, (uint)0);

            return;
        }

#region Internal
            internal enum Motor { A = 0x00, B = 0x01, C = 0x02, ABC = 0xFF };
            internal enum MotorMode { MotorOn = 0x01, Brake = 0x02, On_Brake = 0x03, On_Regulated = 0x05, On_Brake_Regulated = 0x07 };
            internal enum MotorReg { Idle = 0x00, Speed = 0x01, Sync = 0x02 };
            internal enum MotorState { Idle = 0x00, Rampup = 0x10, Running = 0x20, RampDown = 0x40 };

            /// <summary>
            /// <para>[Internal] Run motors on the NXT brick.</para>
            /// </summary>
            /// <param name="motorPort">MotorPort Port</param>
            /// <param name="power">Power Set Point, between -100 and 100.</param>
            /// <param name="mode">Mode</param>
            /// <param name="regulationMode">Regulation Mode</param>
            /// <param name="turnRatio">Turn Ratio, between -100 and 100.</param>
            /// <param name="runState">Run State</param>
            /// <param name="tachoLimit">Tacho Limit, 0=run forever</param>
            /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
            internal void SetOutputState(Motor motorPort
                , sbyte power, MotorMode mode
                , MotorReg regulationMode, sbyte turnRatio
                , MotorState runState, UInt32 tachoLimit)
            {
                if (power < -100) power = -100;
                if (power > 100) power = 100;

                if (turnRatio < -100) turnRatio = -100;
                if (turnRatio > 100) turnRatio = 100;

                byte[] request = new byte[12];
                request[0] = (byte)(0x00);
                request[1] = (byte)(DirectCommand.SetOutputState);
                request[2] = (byte)motorPort;
                request[3] = (byte)power;
                request[4] = (byte)mode;
                request[5] = (byte)regulationMode;
                request[6] = (byte)turnRatio;
                request[7] = (byte)runState;
                SetUInt32(tachoLimit, request, 8);

                CompleteRequest(request);
            }

        /*private class Private
        {
            internal static SerialPort w_port;
            internal static MotorPort w_motor;
            internal static MotorPortSyncable w_motorsync;
            internal static MotorStop w_stop;
            internal static int w_time;
            internal static int w_ratio;

            internal static void MotorsWait()
            {
                try
                {
                    MotorPort motors = w_motor;
                    SerialPort port = w_port;
                    int wait = w_time;
                    MotorStop brake = w_stop;
                    System.Threading.Thread.Sleep(wait);
                    Motors.MoveMotors(port, motors, 0, brake);
                }
                catch
                {
                    return;
                }
                return;
            }
            internal static void MotorsWaitSyncable()
            {
                try
                {
                    MotorPortSyncable motors = w_motorsync;
                    SerialPort port = w_port;
                    int wait = w_time;
                    MotorStop brake = w_stop;
                    int ratio = w_ratio;
                    System.Threading.Thread.Sleep(wait);
                    Motors.MoveMotors(port, motors, 0, brake, ratio);
                }
                catch
                {
                    return;
                }
                return;
            }
        }*/

#endregion

#endregion

#region Files

        public struct NXTOpenReadReply
        {
            /// <summary>
            /// <para>File handle.</para>
            /// </summary>
            public byte handle { get; internal set; }

            /// <summary>
            /// <para>File size.</para>
            /// </summary>
            public UInt32 fileSize { get; internal set; }
        }

        public struct NxtOpenAppendDataReply
        {
            /// <summary>
            /// <para>File handle.</para>
            /// </summary>
            public byte handle { get; internal set; }

            /// <summary>
            /// <para>Available file size.</para>
            /// </summary>
            public UInt32 availableFilesize { get; internal set; }
        }

        /// <summary>
        /// [NXTLib] Checks if a file exists.
        /// </summary>
        /// <param name="filename">The name of the file to read, as a string.
        ///   The extension must be included.</param>
        /// <returns>True if exists.</returns>
        public bool DoesExist(string filename)
        {
            try
            {
                FindFileReply reply = FindFirst(filename);
                if (!reply.fileFound) { return false; }
            }
            catch (NXTFileNotFound)
            {
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// [Native] Open a file for reading.  When finished reading, the handle MUST be closed with 
        /// the Close() command.  If an error occurs, this handle will be closed automatically.
        /// </summary>
        /// <param name="filename">The name of the file to read, as a string.
        ///   The extension must be included.  The possible file extensions are: 
        /// Program (.rxe), Graphic (.ric), Sound (.rso), Datalog (.rdt), and Text (.txt).</param>
        /// <returns>The reply as struct NXTOpenReadReply.</returns>
        public NXTOpenReadReply OpenRead(string filename)
        {
            ValidateFilename(filename, new string[] { ".rxe", ".ric", ".rso", ".rdt", ".txt" });

            byte[] request = new byte[22];
            request[0] = 0x01;
            request[1] = (byte)MessageCommand.OpenRead;
            Encoding.ASCII.GetBytes(filename).CopyTo(request, 2);

            byte[] reply = CompleteRequest(request);
            NXTOpenReadReply result = new NXTOpenReadReply();
            result.handle = reply[3];
            result.fileSize = GetUInt32(reply, 4);
            return result;
        }

        /// <summary>
        /// [Native] Open a file for writing.  When finished writing, the handle MUST be closed with 
        /// the Close() command.  If an error occurs, this handle will be closed automatically.
        /// </summary>
        /// <param name="filename">The name of the file to write, as a string.
        ///   The extension must be included.  The possible file extensions are: 
        /// Firmware (.rfw), Program (.rxe), OnBrick Program (.rpg), TryMe Program (.rtm),
        ///  Sound (.rso), Graphic (.ric), and Text (.txt).</param>
        /// <param name="filesize">The size of the file to be written.</param>
        /// <returns>The handle used in the write session.  Use this handle with the Close() command.</returns>
        public byte OpenWrite(string filename, UInt32 filesize)
        {
            ValidateFilename(filename, new string[] { ".rfw", ".rxe", ".ric",
                ".rso", ".rtm", ".rpg", ".txt" });

            byte[] request = new byte[26];
            request[0] = 0x01;
            request[1] = (byte)MessageCommand.OpenWrite;
            Encoding.ASCII.GetBytes(filename).CopyTo(request, 2);
            SetUInt32(filesize, request, 22);

            byte[] reply = CompleteRequest(request);
            return reply[3];
        }

        /// <summary>
        /// [Native] Open a file for writing continuously.
        /// When finished writing, the handle MUST be closed with 
        /// the Close() command.  If an error occurs, this handle will be closed automatically.
        /// </summary>
        /// <param name="filename">The name of the file to write, as a string.
        ///   The extension must be included.  The possible file extensions are: 
        /// Firmware (.rfw), Program (.rxe), OnBrick Program (.rpg), TryMe Program (.rtm),
        ///  Sound (.rso), Graphic (.ric), and Text (.txt).</param>
        /// <param name="filesize">The size of the file to be written.</param>
        /// <returns>The handle used in the write session.  Use this handle with the Close() command.</returns>
        public byte OpenWriteLinear(string filename, UInt32 filesize)
        {
            ValidateFilename(filename);

            byte[] request = new byte[26];
            request[0] = 0x01;
            request[1] = (byte)MessageCommand.OpenWriteLinear;
            Encoding.ASCII.GetBytes(filename).CopyTo(request, 2);
            SetUInt32(filesize, request, 22);  //correct here, since command does not allow space for the null terminator

            byte[] reply = CompleteRequest(request);
            return reply[3];
        }

        /// <summary>
        /// [Internal] Open a file for reading continuously.  (This command is for internal use!)
        /// When finished reading, the handle MUST be closed with 
        /// the Close() command.  If an error occurs, this handle will be closed automatically.
        /// </summary>
        /// <param name="filename">The name of the file to write, as a string.
        ///   The extension must be included.  The possible file extensions are: 
        /// Firmware (.rfw), Program (.rxe), OnBrick Program (.rpg), TryMe Program (.rtm),
        ///  Sound (.rso), Graphic (.ric), and Text (.txt).</param>
        /// <returns>Pointer to the linear memory segment.</returns>
        public UInt32 OpenReadLinear(string filename)
        {
            ValidateFilename(filename);

            byte[] request = new byte[22];
            request[0] = 0x01;
            request[1] = (byte)MessageCommand.OpenReadLinear_Internal;
            Encoding.ASCII.GetBytes(filename).CopyTo(request, 2);

            byte[] reply = CompleteRequest(request);
            UInt32 pointer = GetUInt32(reply, 3);
            return pointer;
        }

        /// <summary>
        /// [Native] Open a file for writing a data stream.
        /// When finished writing, the handle MUST be closed with 
        /// the Close() command.  If an error occurs, this handle will be closed automatically.
        /// </summary>
        /// <param name="filename">The name of the file to write, as a string.
        ///   The extension must be included.  The possible file extensions are: 
        /// Firmware (.rfw), Program (.rxe), OnBrick Program (.rpg), TryMe Program (.rtm),
        ///  Sound (.rso), Graphic (.ric), and Text (.txt).</param>
        /// <param name="filesize">The size of the file to be written.</param>
        /// <returns>The handle used in the write session.  Use this handle with the Close() command.</returns>
        public byte OpenWriteData(string filename, UInt32 filesize)
        {
            ValidateFilename(filename);

            byte[] request = new byte[26];
            request[0] = 0x01;
            request[1] = (byte)MessageCommand.OpenWriteData;
            Encoding.ASCII.GetBytes(filename).CopyTo(request, 2);
            SetUInt32(filesize, request, 22);  //correct here, since message does not allow null terminator

            byte[] reply = CompleteRequest(request);
            byte handle = reply[3];
            return handle;
        }

        /// <summary>
        /// [Native] Open a file for appending to a data stream.
        /// When finished writing, the handle MUST be closed with 
        /// the Close() command.  If an error occurs, this handle will be closed automatically.
        /// </summary>
        /// <param name="filename">The name of the file to write, as a string.
        ///   The extension must be included.  The possible file extensions are: 
        /// Firmware (.rfw), Program (.rxe), OnBrick Program (.rpg), TryMe Program (.rtm),
        ///  Sound (.rso), Graphic (.ric), and Text (.txt).</param>
        /// <returns>The OpenApppendDataReply structure.</returns>
        public NxtOpenAppendDataReply OpenAppendData(string filename)
        {
            ValidateFilename(filename);

            byte[] request = new byte[22];
            request[0] = 0x01;
            request[1] = (byte)MessageCommand.OpenAppendData;
            Encoding.ASCII.GetBytes(filename).CopyTo(request, 2);

            byte[] reply = CompleteRequest(request);
            NxtOpenAppendDataReply result = new NxtOpenAppendDataReply();
            result.handle = reply[3];
            result.availableFilesize = GetUInt32(reply, 4);
            return result;
        }

        /// <summary>
        /// [Native] Reads data from the NXT.
        /// </summary>
        /// <param name="handle">The handle, located in the OpenRead command.</param>
        /// <param name="bytesToRead">The number of bytes to be read.</param>
        /// <returns>The requested data from NXT flash memory.</returns>
        public byte[] Read(byte handle, UInt16 bytesToRead)
        {
            byte[] request = new byte[5];
            request[0] = 0x01;
            request[1] = (byte)MessageCommand.Read;
            request[2] = handle;
            SetUInt16(bytesToRead, request, 3);

            byte[] reply = CompleteRequest(request);
            if (reply[3] != handle)
            {
                throw new NXTReplyIncorrect();
            }

            UInt16 bytesRead = GetUInt16(reply, 4);
            byte[] response = new byte[bytesRead];
            Array.Copy(reply, 6, response, 0, bytesRead);

            return response;
        }

        /// <summary>
        /// [Native] Writes data to an NXT until all data is written.
        /// </summary>
        /// <param name="handle">The handle, located in the OpenWrite command.</param>
        /// <param name="data">The data to be written to flash memory.</param>
        /// <returns>The number of bytes written.</returns>
        public int Write(byte handle, byte[] data)
        {
            UInt16 bytesWritten = 0;
            int n = 0;
            while (bytesWritten < data.Length)
            {
                System.Threading.Thread.Sleep(7);
                int j = 0;
                int max = data.Length;
                if ((61 * (n + 1)) < max) { max = (61 * (n + 1)); }
                byte[] datatowrite = new byte[max - (61*n)];
                for (int i = 61*n; i < max; i++)
                {
                    datatowrite[j] = data[i];
                    j++;
                }
                    
                byte[] request = new byte[3 + datatowrite.Length];
                request[0] = 0x01;
                request[1] = (byte)MessageCommand.Write;
                request[2] = handle;
                datatowrite.CopyTo(request, 3);

                byte[] reply = CompleteRequest(request);
                byte handleOut = reply[3];
                if (handleOut != handle)
                {
                    throw new NXTReplyIncorrect();
                }

                bytesWritten += GetUInt16(reply, 4);
                n++;
            }

            return Convert.ToInt32(bytesWritten);
        }

        /// <summary>
        /// [Native] Close and dispose of a handle.
        /// </summary>
        /// <param name="handle">The read or write handle number.</param>
        public void Close(byte handle)
        {

            byte[] request = new byte[] {
            0x01,
            (byte) MessageCommand.Close,
            handle
            };
            CompleteRequest(request);

            return;
        }


        /// <summary>
        /// [Native] Delete a file from the NXT.
        /// </summary>
        /// <param name="filename">The name of the file to read, as a string.
        ///   The extension must be included.  The possible file extensions are: 
        /// Program (.rxe), OnBrick Program (.rpg), TryMe Program (.rtm),
        ///  Sound (.rso), Datalog (.rdt), Graphic (.ric), and Text (.txt).</param>
        public void Delete(string filename)
        {
            ValidateFilename(filename, new string[] { ".rso", ".ric", ".rxe", ".rpg",
            ".rtm", ".rdt", ".txt"});

            byte[] request = new byte[22];
            request[0] = 0x01;
            request[1] = (byte)MessageCommand.Delete;
            Encoding.ASCII.GetBytes(filename).CopyTo(request, 2);

            CompleteRequest(request);

            /*string fileNameOut = Encoding.ASCII.GetString(reply, 3, 20);
            if (fileNameOut != filename)
            {
                throw new Exception(string.Format(
                    "[NXTLib] The file reported as deleted, '{0}', was different from the file requested, '{1}'."
                    , fileNameOut, filename));
            }*/

            return;
        }

        /// <summary>
        /// The struct used for FindFirst and FindNext.
        /// </summary>
        public struct FindFileReply
        {
            /// <summary>
            /// <para>Boolean indicating if a file was found or not.</para>
            /// </summary>
            public bool fileFound;

            /// <summary>
            /// <para>The file handle.</para>
            /// </summary>
            public byte handle;

            /// <summary>
            /// <para>The file name.</para>
            /// </summary>
            public string filename;

            /// <summary>
            /// <para>The filesize.</para>
            /// </summary>
            public UInt32 fileSize;
        }

        /// <summary>
        /// [Native] Find the first file that matches a pattern.
        /// </summary>
        /// <param name="pattern">The pattern to search against.  Allows the following wildcards:
        ///  filename.Extension, filename.*, *.Extension, and *.*</param>
        /// <returns>The structure FileTypeReply.</returns>
        public FindFileReply FindFirst(string pattern)
        {
            ValidateFilename(pattern);

            byte[] request = new byte[22];
            request[0] = 0x01;
            request[1] = (byte)MessageCommand.FindFirst;
            Encoding.ASCII.GetBytes(pattern).CopyTo(request, 2);

            return Parse_FindFile(request);
        }

        /// <summary>
        /// [Native] Find the next file that matches the pattern stored in a handle.
        /// </summary>
        /// <param name="handle">The handle found in FindFirst.</param>
        /// <returns>The nullable structure FileTypeReply.  Returns null if an error occurs.</returns>
        public FindFileReply FindNext(byte handle)
        {
            byte[] request = new byte[] {
                0x01,
                (byte) MessageCommand.FindNext,
                handle
            };

            return Parse_FindFile(request);
        }

        private FindFileReply Parse_FindFile(byte[] request)
        {
            FindFileReply result;
            byte[] reply = CompleteRequest(request);
            result.fileFound = true;
            result.handle = reply[3];
            result.filename = Encoding.ASCII.GetString(reply, 4, 20).TrimEnd('\0');
            result.fileSize = GetUInt32(reply, 24);

            if (result.filename == "")
            {
                throw new NXTFileNotFound();
            }

            return result;
        }

        /// <summary>
        /// [Native] Poll the length of a command in a specific buffer.
        /// </summary>
        /// <param name="buffer">The buffer number.  Either 0x00 (Poll) or 0x01 (High Speed).</param>
        /// <returns>The number of bytes for the command ready in the buffer (0 = no command ready).</returns>
        public byte PollCommandLength(byte buffer)
        {
            byte[] request = new byte[] {
            0x01,
            (byte) MessageCommand.PollCommandLength,
            buffer
            };

            byte[] reply = CompleteRequest(request);
            byte bufferNoOut = reply[3];
            if (bufferNoOut != buffer)
                throw new NXTReplyIncorrect();

            byte bytesReady = reply[4];
            return bytesReady;
        }

#endregion

#region Messaging

        public enum Mailbox : byte
        {
            Mailbox_1 = 0x00,
            Mailbox_2 = 0x01,
            Mailbox_3 = 0x02,
            Mailbox_4 = 0x03,
            Mailbox_5 = 0x04,
            Mailbox_6 = 0x05,
            Mailbox_7 = 0x06,
            Mailbox_8 = 0x07,
            Mailbox_9 = 0x08,
            Mailbox_10 = 0x09
        }
        public enum MailboxExtended : byte
        {
            Mailbox_1 = 0,
            Mailbox_2 = 1,
            Mailbox_3 = 2,
            Mailbox_4 = 3,
            Mailbox_5 = 4,
            Mailbox_6 = 5,
            Mailbox_7 = 6,
            Mailbox_8 = 7,
            Mailbox_9 = 8,
            Mailbox_10 = 9,
            Mailbox_11 = 10,
            Mailbox_12 = 11,
            Mailbox_13 = 12,
            Mailbox_14 = 13,
            Mailbox_15 = 14,
            Mailbox_16 = 15,
            Mailbox_17 = 16,
            Mailbox_18 = 17,
            Mailbox_19 = 18,
            Mailbox_20 = 19,
        }

        /// <summary> 
        /// [Native] Write a message to an incoming box on the NXT.
        /// </summary>
        /// <param name="box">The mailbox to send the message to.</param>
        /// <param name="message">The message to send, as a string.
        ///   The string must not be longer than 57 characters.</param>
        public void MessageWrite(Mailbox box, string message)
        {
            if (!message.EndsWith("\0"))
                message += '\0';

            int messageSize = message.Length;
            if (messageSize > 59)
            {
                throw new NXTException("Message may not exceed 57 characters.");
            }

            byte[] request = new byte[4 + messageSize];
            request[0] = (byte)(0x00);
            request[1] = (byte)DirectCommand.MessageWrite;
            request[2] = (byte)box;
            request[3] = (byte)messageSize;
            Encoding.ASCII.GetBytes(message).CopyTo(request, 4);
            CompleteRequest(request);

            return;             
        }

        /// <summary> 
        /// [Native] Write a message to an incoming box on the NXT.
        /// </summary>
        /// <param name="box">The mailbox to send the message to.</param>
        /// <param name="message">The message to send, as a byte array.
        ///   The array must not be longer than 59 characters and must conclude with '/0'.</param>
        public void MessageWrite(Mailbox box, byte[] message)
        {
            int messageSize = message.Length + 1;  // Add 1 for the 0-byte at the end.
            if (messageSize > 59)
            {
                throw new NXTException("Message may not exceed 59 characters.");
            }

            byte[] request = new byte[4 + messageSize];
            request[0] = (byte)(0x00);
            request[1] = (byte)DirectCommand.MessageWrite;
            request[2] = (byte)box;
            request[3] = (byte)messageSize;
            message.CopyTo(request, 4);
            request[request.Length - 1] = 0;

            CompleteRequest(request);
            return;
        }

        /// <summary> 
        /// [Native] Read a message from the remote inbox on an NXT.
        /// </summary>
        /// <param name="remoteInboxNo">The mailbox on the remote NXT.</param>
        /// <param name="localInboxNo">The mailbox on the local system.</param>
        /// <param name="remove">Set to true to clear the message from the remote inbox.  Set to false otherwise.</param>
        /// <returns>Returns the messagebox's contents, as a string.</returns>
        public string MessageRead(MailboxExtended remoteInboxNo, Mailbox localInboxNo, bool remove)
        {
            byte[] request = new byte[] {
            0x00,
            (byte) DirectCommand.MessageRead,
            (byte) remoteInboxNo,
            (byte) localInboxNo,
            (byte) (remove ? 0xFF : 0x00)
            };

            byte[] reply = CompleteRequest(request);
            if (reply[3] != (byte)localInboxNo)
            {
                throw new NXTReplyIncorrect();
            }
            byte localInboxNoOut = reply[3];
            byte messageSize = reply[4];
            string message = Encoding.ASCII.GetString(reply, 5, messageSize).TrimEnd('\0');
            return message;
        }

#endregion

#region Internal

        internal enum DirectCommand
        {
            StartProgram = 0x00, StopProgram = 0x01, PlaySoundFile = 0x02,
            PlayTone = 0x03, SetOutputState = 0x04, SetInputMode = 0x05,
            GetOutputState = 0x06, GetInputValues = 0x07, 
            ResetInputScaledValue = 0x08, MessageWrite = 0x09, 
            ResetMotorPosition = 0x0A, GetBatteryLevel = 0x0B,
            StopSoundPlayback = 0x0C, KeepAlive = 0x0D, LSGetStatus = 0x0E,
            LSWrite = 0x0F, LSRead = 0x10, GetCurrentProgramName = 0x11,
            MessageRead = 0x13
        }

        internal enum MessageCommand
        {
            OpenRead = 0x80, OpenWrite = 0x81,
            Read = 0x82, Write = 0x83, Close = 0x84, Delete = 0x85,
            FindFirst = 0x86, FindNext = 0x87, GetFirmwareVersion = 0x88,
            OpenWriteLinear = 0x89, OpenReadLinear_Internal = 0x8A,
            OpenWriteData = 0x8B, OpenAppendData = 0x8C, Boot = 0x97,
            SetBrickName = 0x98, GetDeviceInfo = 0x9B, DeleteUserFlash = 0xA0,
            PollCommandLength = 0xA1, Poll = 0xA2, BluetoothFactoryReset = 0xA4
            //BluetoothFactoryReset and Boot may only be accepted by USB
        }

        internal byte[] CompleteRequest(byte[] request)
        {
            Send(request);
            byte[] reply = RecieveReply();
            if (reply == null)
            {
                throw new NXTException("No reply recieved.");
            }
            if (reply[2] != 0x00)
            {
                ThrowError(reply[2]);
            }
            return reply;
        }

        internal void ValidateFilename(string filename)
        {
            if (filename.Length > 19)
            {
                throw new NXTException("Filename cannot be larger than 19 characters.");
            }
            return;
        }

        internal void ValidateFilename(string filename, string[] possible_extensions)
        {
            if (filename.Length > 19)
            {
                throw new NXTException("Filename cannot be larger than 19 characters.");
            }
            if (filename.Contains(".") == false)
            {
                string error = "Filename MUST include a valid extension.  The valid extensions are: ";
                for (int i = 0; i < possible_extensions.Length; i++)
                {
                    error += possible_extensions[i] + " ";
                }
                throw new NXTException(error);
            }
            bool valid = false;
            for (int i = 0; i < possible_extensions.Length; i++)
			{
			    if (filename.EndsWith(possible_extensions[i]))
                {
                    valid = true;
                }
			}
            if (valid == false)
            {
                string error = "Filename MUST include a valid extension.  The valid extensions are: ";
                for (int i = 0; i < possible_extensions.Length; i++)
                {
                    error += possible_extensions[i] + " ";
                }
                throw new NXTException(error);
            }
            return;
        }

        internal enum FileType { Program = 0, Internal_Program = 1, TryMe = 2, Image = 3, Sound = 4, System = 5, Sensor = 6, TXT = 7, LOG = 8, Firmware = 9 };

        internal string ValidateFilename(string filename, FileType filetype, bool appendifmissing)
        {
            string name = filename;
            String[] FileTypes = new String[] { ".rxe", ".rpg", ".rtm", ".ric", ".rso", ".sys", ".cal", ".txt", ".log", ".rfw" };
            string type = FileTypes[(int)(filetype)];
            if (filename.Length > 19)
            {
                throw new NXTException("Filename cannot be larger than 19 characters.");
            }
            if (appendifmissing == false)
            {
                if (filename.Contains(type) == false)
                {
                    throw new NXTException("Filename must end with '" + type + "' extension.");
                }
            }
            else
            {
                if ((filename.Contains(type) == false) && (filename.Contains(".")))
                {
                    throw new NXTException("Filename must end with '" + type + "' extension.");
                }
                else
                {
                    if ((filename.Contains(type) == true))
                    {

                    }
                    else
                    {
                        name = name + type;
                    }
                }
            }

            return name;
        }

        internal static Int16 GetInt16(byte[] byteArr, byte index)
        {
            Int16 result = 0;
            for (sbyte offset = 1; offset >= 0; offset--)
            {
                result <<= 8;
                if (offset == 1)
                    result += (sbyte)byteArr[index + offset];
                else
                    result += (byte)byteArr[index + offset];
            }

            return result;
        }

        internal static UInt16 GetUInt16(byte[] byteArr, byte index)
        {
            UInt16 result = 0;
            for (sbyte offset = 1; offset >= 0; offset--)
            {
                result <<= 8;
                result += (byte)byteArr[index + offset];
            }

            return result;
        }

        internal static void SetUInt16(UInt16 number, byte[] byteArr, byte index)
        {
            for (byte offset = 0; offset <= 1; offset++)
            {
                byteArr[index + offset] = (byte)(number % 0x100);
                number >>= 8;
            };
        }

        internal static Int32 GetInt32(byte[] byteArr, byte index)
        {
            Int32 result = 0;
            for (sbyte offset = 3; offset >= 0; offset--)
            {
                result <<= 8;
                if (offset == 3)
                    result += (sbyte)byteArr[index + offset];
                else
                    result += (byte)byteArr[index + offset];
            }

            return result;
        }

        internal static UInt32 GetUInt32(byte[] byteArr, byte index)
        {
            UInt32 result = 0;
            for (sbyte offset = 3; offset >= 0; offset--)
            {
                result <<= 8;
                result += (byte)byteArr[index + offset];
            }

            return result;
        }

        internal static void SetUInt32(UInt32 number, byte[] byteArr, byte index)
        {
            for (byte offset = 0; offset <= 3; offset++)
            {
                byteArr[index + offset] = (byte)(number % 0x100);
                number >>= 8;
            };
        }

        internal void ThrowError(byte error)
        {
            if (error == 0x20)
            {
                throw new NXTException("Pending communication transaction in progress.", error);
            }
            if (error == 0x40)
            {
                throw new NXTException("Specified mailbox queue is empty.", error);
            }
            if (error == 0xBD)
            {
                throw new NXTException("Request failed.", error);
            }
            if (error == 0xBE)
            {
                throw new NXTException("Unknown command opcode.", error);
            }
            if (error == 0xBF)
            {
                throw new NXTException("Insane packet.", error);
            }
            if (error == 0xC0)
            {
                throw new NXTDataOutOfRange();
            }
            if (error == 0xDD)
            {
                throw new NXTException("Communication bus error.", error);
            }
            if (error == 0xDE)
            {
                throw new NXTException("No free memory in communication buffer.", error);
            }
            if (error == 0xDF)
            {
                throw new NXTException("Specified channel/connection is not valid.", error);
            }
            if (error == 0xE0)
            {
                throw new NXTException("Specified channel/connection is not configured or busy.", error);
            }
            if (error == 0xEC)
            {
                throw new NXTNoActiveProgram();
            }
            if (error == 0xED)
            {
                throw new NXTException("Illegal size specified.", error);
            }
            if (error == 0xEE)
            {
                throw new NXTException("Illegal mailbox queue ID specified.", error);
            }
            if (error == 0xEF)
            {
                throw new NXTException("Attempted to access invalid field of a structure.", error);
            }
            if (error == 0xF0)
            {
                throw new NXTException("Bad input or output specified.", error);
            }
            if (error == 0xFB)
            {
                throw new NXTInsufficientMemory();
            }
            if (error == 0xFF)
            {
                throw new NXTException("Bad arguments.", error);
            }

            //Message Commands
            if (error == 0x81)
            {
                throw new NXTException("No more handles.", error);
            }
            if (error == 0x82)
            {
                throw new NXTException("No space.", error);
            }
            if (error == 0x83)
            {
                throw new NXTNoMoreFiles();
            }
            if (error == 0x84)
            {
                throw new NXTException("End of file expected.", error);
            }
            if (error == 0x85)
            {
                throw new NXTEndOfFile();
            }
            if (error == 0x86)
            {
                throw new NXTException("Not a linear file.", error);
            }
            if (error == 0x87)
            {
                throw new NXTFileNotFound();
            }
            if (error == 0x88)
            {
                throw new NXTHandleAlreadyClosed();
            }
            if (error == 0x89)
            {
                throw new NXTException("Not linear space.", error);
            }
            if (error == 0x8A)
            {
                throw new NXTException("Undefined error.", error);
            }
            if (error == 0x8B)
            {
                throw new NXTFileBusy();
            }
            if (error == 0x8C)
            {
                throw new NXTException("No write buffers.", error);
            }
            if (error == 0x8D)
            {
                throw new NXTException("Append not possible.", error);
            }
            if (error == 0x8E)
            {
                throw new NXTException("File is full.", error);
            }
            if (error == 0x8F)
            {
                throw new NXTFileExists();
            }
            if (error == 0x90)
            {
                throw new NXTException("Module not found.", error);
            }
            if (error == 0x91)
            {
                throw new NXTException("Out of boundary.", error);
            }
            if (error == 0x92)
            {
                throw new NXTException("Illegal file name.", error);
            }
            if (error == 0x93)
            {
                throw new NXTException("Illegal handle.", error);
            }
            throw new NXTException("Unspecified internal NXT error.", error);
        }

#endregion

    }
}
