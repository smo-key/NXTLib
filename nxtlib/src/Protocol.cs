using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace NXTLib
{
    public abstract class Protocol
    {
        //TODO: Complete the missing portions of the library after phase 3!
        //Ensure comments are written for each method!

#region Base
        public Protocol()
            : base()
        { }

        public abstract bool Connect();
        internal abstract bool Send(byte[] request);
        public abstract bool Disconnect();
        internal abstract byte[] RecieveReply();
        private string error;
        public virtual string LastError { get { return error; } }
        internal SerialPort link { get; set; }
        public abstract bool IsConnected { get; }

#endregion

#region Misc
        /// <summary> 
        /// [Native] Starts a program (.rxe) on the NXT brick.
        /// </summary>
        /// <param name="pattern">The name of the program you wish to run.</param>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool StartProgram(string fileName)
        {
            try
            {
                string n = ValidateFilename(fileName, FileType.Program, true);
                if (n == null)
                {
                    return false;
                }
                fileName = n;
                byte[] fileNameByteArr = Encoding.ASCII.GetBytes(fileName);

                byte[] request = new byte[22];
                request[0] = (byte)(0x00);
                request[1] = (byte)(DirectCommand.StartProgram);
                fileNameByteArr.CopyTo(request, 2);

                return CompleteRequest(request);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary> 
        /// [Native] Stops the currently running program on the NXT brick.
        /// </summary>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool StopProgram()
        {
            try
            {
                byte[] request = new byte[22];
                request[0] = (byte)(0x00);
                request[1] = (byte)(DirectCommand.StopProgram);

                return CompleteRequest(request);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary> 
        /// [Native] Plays a beep tone on the NXT brick.
        /// </summary>
        /// <param name="frequency">The frequency, in Hz, where frequency is between 200 and 14000 Hz.</param>
        /// <param name="durarion">The duration of the tone, in milliseconds.</param>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool PlayTone(UInt16 frequency, UInt16 duration)
        {
            try
            {
                if (frequency < 200) frequency = 200;
                if (frequency > 14000) frequency = 14000;

                byte[] request = new byte[6];
                request[0] = (byte)(0x00);
                request[1] = (byte)(DirectCommand.PlayTone);
                SetUInt16(frequency, request, 2);
                SetUInt16(duration, request, 4);

                return CompleteRequest(request);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary> 
        /// [Native] Plays a sound file from the NXT.
        /// </summary>
        /// <param name="loop">Set to true to loop the sound until stopped.</param>
        /// <param name="pattern">The name of the sound file (.rso) on the NXT.</param>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool PlaySoundFile(bool loop, string fileName)
        {
            try
            {
                string n = ValidateFilename(fileName, FileType.Sound, true);
                if (n == null)
                {
                    return false;
                }
                fileName = n;

                byte[] request = new byte[23];
                request[0] = (byte)(0x00);
                request[1] = (byte)(DirectCommand.PlaySoundFile);
                request[2] = (byte)(loop ? 0xFF : 0x00);
                Encoding.ASCII.GetBytes(fileName).CopyTo(request, 3);

                return CompleteRequest(request);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary> 
        /// [Native] Returns the voltage of the NXT battery, in millivolts.
        /// </summary>
        /// <returns>The battery level, in millivolts.</returns>
        public int? GetBatteryLevel()
        {
            try
            {
                byte[] request = new byte[] {
                0x00,
                (byte) DirectCommand.GetBatteryLevel
                };
                
                Send(request);
                byte[] reply = RecieveReply();

                if (reply == null) return null;

                UInt16 voltage = GetUInt16(reply, 3);
                return Convert.ToInt32(voltage);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary> 
        /// [Native] Stops sound playback on the NXT.
        /// </summary>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool StopSoundPlayback()
        {
            try
            {
                byte[] request = new byte[] {
                0x00,
                (byte) DirectCommand.StopSoundPlayback
                };

                return CompleteRequest(request);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary> 
        /// [Native] Keeps the NXT on and
        ///  returns a value saying how long until the NXT falls asleep again, in milliseconds.
        /// </summary>
        /// <returns>The time until the NXT falls asleep again, in milliseconds.</returns>
        public int? KeepAlive()
        {
            try
            {
                byte[] request = new byte[] {
                0x00,
                (byte) DirectCommand.KeepAlive
                };

                Send(request);
                byte[] reply = RecieveReply();

                if (reply == null) return null;

                UInt32 sleeplimit = GetUInt32(reply, 3);
                return Convert.ToInt32(sleeplimit);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary> 
        /// [Native] Gets the name of the currently running program on the NXT brick.
        /// </summary>
        /// <returns>The name of the currently running program, as a string.</returns>
        public string GetCurrentProgramName()
        {
            try
            {
                byte[] request = new byte[] {
                (byte) (0x00),
                (byte) (DirectCommand.GetCurrentProgramName),
                };

                if (Send(request) == false)
                {
                    return null;
                }
                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return null;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return null;
                }
                string fileName = Encoding.ASCII.GetString(reply, 3, 20).TrimEnd('\0');
                return fileName;
            }
            catch (Exception ex)
            {
                error = ex.Message;
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
        public GetFirmwareVersionReply? GetFirmwareVersion()
        {
            try
            {
                byte[] request = new byte[] {
                0x01,
                (byte) MessageCommand.GetFirmwareVersion
                };

                if (Send(request) == false)
                {
                    return null;
                }
                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return null;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return null;
                }
                GetFirmwareVersionReply result;
                result.protocolVersion = new Version(reply[4], reply[3]);
                result.firmwareVersion = new Version(reply[6], reply[5]);
                return result;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary> 
        /// [Native] Boot the NXT.  THIS COMMAND MAY ONLY BE ACCEPTED BY USB!
        /// </summary>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool Boot()
        {
            try
            {
                byte[] request = new byte[21];
                request[0] = 0x01;
                request[1] = (byte)MessageCommand.Boot;
                Encoding.ASCII.GetBytes("Let's dance: SAMBA").CopyTo(request, 2);

                if (Send(request) == false)
                {
                    return false;
                }
                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return false;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return false;
                }
                string result = Encoding.ASCII.GetString(reply, 3, 4).TrimEnd('\0');
                if (result != "Yes")
                {
                    throw new Exception("[NXTLib] The reply was incorrect.");
                }
                return true;

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary> 
        /// [Native] Sets the name of the NXT.
        /// </summary>
        /// <param name="newname">The NXT's new name.</param>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool SetBrickName(string newname)
        {
            try
            {
                if (newname.Length > 15)
                    newname = newname.Substring(0, 15);

                byte[] request = new byte[18];
                request[0] = 0x01;
                request[1] = (byte)MessageCommand.SetBrickName;
                Encoding.ASCII.GetBytes(newname).CopyTo(request, 2);

                return CompleteRequest(request);

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary> 
        /// [Native] Returns the NXT's information.
        /// </summary>
        /// <returns>The reply as a nullable structure, GetDeviceInfoReply.</returns>
        public GetDeviceInfoReply? GetDeviceInfo()
        {
            try
            {
                byte[] request = new byte[] {
                0x01,
                (byte) MessageCommand.GetDeviceInfo
                };

                if (Send(request) == false)
                {
                    return null;
                }
                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return null;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return null;
                }

                GetDeviceInfoReply result;
                result.Name = Encoding.ASCII.GetString(reply, 3, 15).TrimEnd('\0');
                result.Address = new byte[7];
                Array.Copy(reply, 18, result.Address, 0, 7);
                result.SignalStrength = GetUInt32(reply, 25);
                result.freeUserFlash = GetUInt32(reply, 29);

                return result;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary> 
        /// [Native] Completely wipes the NXT's user-created data.  USE WITH CARE!
        /// </summary>
        /// <param name="waitfor">Set to true to wait until the NXT has completed its memory wipe, false otherwise.</param>
        public void DeleteUserFlash(bool waitfor)
        {
            try
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
            catch
            {

            }
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
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool SetSensorMode(SensorPort port,
            SensorType type, SensorMode mode)
        {
            try
            {
                byte[] request = new byte[] {
                (byte) (0x00),
                (byte) (DirectCommand.SetInputMode),
                (byte) port,
                (byte) type,
                (byte) mode
                };

                return CompleteRequest(request);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// [Native] Get a sensor's current state.
        /// </summary>
        /// <param name="port">The port of the attached sensor.</param>
        /// <returns>Returns a SensorInput package.</returns>
        public SensorInput? GetSensorValues(SensorPort port)
        {
            try
            {
                byte[] request = new byte[] {
                0x00,
                (byte) DirectCommand.GetInputValues,
                (byte) port
                };

                if (Send(request) == false)
                {
                    return null;
                }

                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return null;
                }
                if (reply[3] != (byte)port)
                {
                    error = "[NXTLib] The reply was incorrect.";
                    return null;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return null;
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
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// [Native] Reset's the selected sensor's scaled value.
        /// </summary>
        /// <param name="port">The port of the attached sensor.</param>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool ResetSensorScale(SensorPort port)
        {
            try
            {
                byte[] request = new byte[] {
                (byte) (0x00),
                (byte) (DirectCommand.ResetInputScaledValue),
                (byte) port
                };

                return CompleteRequest(request);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// [Native] Reads the number of bytes readable in a lowpeed sensor port.
        /// </summary>
        /// <param name="port">The port of the attached sensor.</param>
        /// <returns>Count of available bytes to read.</returns>
        public byte? LowspeedGetStatus(SensorPort port)
        {
            try
            {
                byte[] request = new byte[] {
                (byte) (0x00),
                (byte) (DirectCommand.LSGetStatus),
                (byte) port
                };

                if (Send(request) == false)
                {
                    return null;
                }

                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return null;
                }                
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return null;
                }
                return reply[3];
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// [Native] Writes an array of bytes to a lowspeed sensor port.
        /// </summary>
        /// <param name="port">The port of the attached sensor.</param>
        /// <param name="txData">The tx Data to be written.</param>
        /// <param name="port">The rx Data Length. </param>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool LowspeedWrite(SensorPort port, byte[] txData, byte rxDataLength)
        {
            try
            {
                byte txDataLength = (byte)txData.Length;
                if (txDataLength == 0)
                    throw new ArgumentException("[NXTLib] No data to send.");

                if (txDataLength > 16)
                    throw new ArgumentException("[NXTLib] Tx data may not exceed 16 bytes.");

                if (rxDataLength < 0 || 16 < rxDataLength)
                    throw new ArgumentException("[NXTLib] Rx data length should be in the interval 0-16.");

                byte[] request = new byte[5 + txDataLength];
                request[0] = (byte)(0x00);
                request[1] = (byte)DirectCommand.LSWrite;
                request[2] = (byte)port;
                request[3] = txDataLength;
                request[4] = rxDataLength;
                txData.CopyTo(request, 5);

                return CompleteRequest(request);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// [Native] Reads the number of bytes readable in a lowpeed sensor port.
        /// </summary>
        /// <param name="port">The port of the attached sensor.</param>
        /// <returns>The data from the lowspeed sensor, as a byte array.</returns>
        public byte[] LowspeedRead(SensorPort port)
        {
            try
            {
                byte[] request = new byte[] {
                0x00,
                (byte) DirectCommand.LSRead,
                (byte) port
                };

                if (Send(request) == false)
                {
                    return null;
                }

                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return null;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return null;
                }

                byte bytesRead = reply[3];
                byte[] rxData = new byte[bytesRead];
                Array.Copy(reply, 4, rxData, 0, bytesRead);

                return rxData;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
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
        ///  <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool ResetMotorPosition(MotorPortSingle port, bool relative)
        {
            try
            {
                byte[] request = new byte[] {
                (byte) (0x00),
                (byte) (DirectCommand.ResetMotorPosition),
                (byte) port,
                (byte) (relative ? 0xFF : 0x00)
                };

                return CompleteRequest(request);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary> 
        /// [Native] Get the state of an NXT motor port.
        /// </summary>
        /// <param name="port">The port of the selected motor.</param>
        /// <returns>Returns a MotorStateOut package.</returns>
        public MotorStateOut? GetMotorState(MotorPortSingle port)
        {
            try
            {
                byte[] request = new byte[] {
            0x00,
            (byte) DirectCommand.GetOutputState,
            (byte) port
                };

                if (Send(request) == false)
                {
                    return null;
                }

                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return null;
                }
                byte motorportout = reply[3];
                if (reply[3] != (byte)port)
                {
                    error = "[NXTLib] The reply was incorrect.";
                    return null;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return null;
                }
                MotorStateOut result = new MotorStateOut();
                result.power = reply[4];
                result.tachocount = GetInt32(reply, 13);
                result.blocktachocount = GetInt32(reply, 17);
                result.rotationcount = GetInt32(reply, 21);
                return result;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }

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
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool MoveMotors(MotorPort port, int power)
        {
            try
            {
                MotorMode mode = MotorMode.On_Regulated;
                if ((power < 75) && (power > -75)) { mode = MotorMode.MotorOn; }
                if (port != MotorPort.AB && port != MotorPort.AC && port != MotorPort.BC)
                {
                    if (SetOutputState((Motor)((byte)port), (sbyte)power, mode,
                        MotorReg.Speed, 0, MotorState.Running, 0) == false)
                    {
                        return false;
                    }
                }
                else
                {
                    Motor[] motors = new Motor[] { };
                    if (port == MotorPort.AB) { motors = new Motor[] { Motor.A, Motor.B }; }
                    if (port == MotorPort.AC) { motors = new Motor[] { Motor.A, Motor.C }; }
                    if (port == MotorPort.BC) { motors = new Motor[] { Motor.B, Motor.C }; }
                    if (SetOutputState(motors[0], (sbyte)power, mode,
                        MotorReg.Sync, 0, MotorState.Running, (uint)0) == false)
                    {
                        return false;
                    }
                    if (SetOutputState(motors[1], (sbyte)power, mode,
                        MotorReg.Sync, 0, MotorState.Running, (uint)0) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary> 
        /// [Native] Move NXT motors, specifying whether to brake or coast.
        /// </summary>
        /// <param name="port">The ports of the selected motors.</param>
        /// <param name="power">The power at which to move the motors, between -100 and 100.</param>
        /// <param name="stop">Whether to brake or coast during operation.  Braking uses far more power.</param>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool MoveMotors(MotorPort port, int power, MotorStop stop)
        {
            try
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
                    if (SetOutputState((Motor)((byte)port), (sbyte)power, mode,
                        MotorReg.Speed, 0, MotorState.Running, 0) == false)
                    {
                        return false;
                    }
                }
                else
                {
                    Motor[] motors = new Motor[] { };
                    if (port == MotorPort.AB) { motors = new Motor[] { Motor.A, Motor.B }; }
                    if (port == MotorPort.AC) { motors = new Motor[] { Motor.A, Motor.C }; }
                    if (port == MotorPort.BC) { motors = new Motor[] { Motor.B, Motor.C }; }
                    if (SetOutputState(motors[0], (sbyte)power, mode,
                        MotorReg.Sync, 0, MotorState.Running, (uint)0) == false)
                    {
                        return false;
                    }
                    if (SetOutputState(motors[1], (sbyte)power, mode,
                        MotorReg.Sync, 0, MotorState.Running, (uint)0) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
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
        ///  <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool MoveMotors(MotorPort port, int power, MotorStop stop, MotorSmooth smooth, int degrees)
        {
            try
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
                    if (SetOutputState((Motor)((byte)port), (sbyte)power, mode,
                        MotorReg.Speed, 0, MotorState.Running, 0) == false)
                    {
                        return false;
                    }
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
                    if (SetOutputState(motors[0], (sbyte)power, mode,
                        MotorReg.Sync, 0, state, (uint)degrees) == false)
                    {
                        return false;
                    }
                    if (SetOutputState(motors[1], (sbyte)power, mode,
                        MotorReg.Sync, 0, state, (uint)degrees) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary> 
        /// [Native] Sync two motors as driving motors, specifying whether to brake or coast and the turn ratio.
        /// </summary>
        /// <param name="port">The ports of the selected motors.  Exactly two motors must be selected.</param>
        /// <param name="power">The power at which to move the motors, between -100 and 100.</param>
        /// <param name="stop">Whether to brake or coast during operation.  Braking uses far more power.</param>
        /// <param name="turnratio">The turn ratio of the two motors, between -100 and 100.  A ratio of zero will make both motors move straight.
        ///   A negative ratio will move the left motor more and a positive ratio would move the right motor more.</param>
        ///   <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool MoveMotors(MotorPortSyncable port, int power, MotorStop stop, int turnratio)
        {
            try
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
                if (SetOutputState(motors[0], (sbyte)pow[0], mode,
                    MotorReg.Speed, (sbyte)turnratio, MotorState.Running, (uint)0) == false)
                {
                    return false;
                }
                if (SetOutputState(motors[1], (sbyte)pow[1], mode,
                    MotorReg.Speed, (sbyte)turnratio, MotorState.Running, (uint)0) == false)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
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
            internal bool SetOutputState(Motor motorPort
                , sbyte power, MotorMode mode
                , MotorReg regulationMode, sbyte turnRatio
                , MotorState runState, UInt32 tachoLimit)
            {
                try
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

                    if (CompleteRequest(request)) { return true; }
                    else { return false; }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    return false;
                }
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

        /// <summary>
        /// [Native] Open a file for reading.  When finished reading, the handle MUST be closed with 
        /// the Close() command.  If an error occurs, this handle will be closed automatically.
        /// </summary>
        /// <param name="filename">The name of the file to read, as a string.
        ///   The extension must be included.  The possible file extensions are: 
        /// Program (.rxe), Graphic (.ric), Sound (.rso), and Datalog (.rdt).</param>
        /// <returns>The reply as struct NXTOpenReadReply.</returns>
        public NXTOpenReadReply? OpenRead(string filename)
        {
            try
            {
                if (ValidateFilename(filename, new string[] { ".rxe", ".ric", ".rso", ".rdt" }) == false)
                {
                    return null;
                }

                byte[] request = new byte[22];
                request[0] = 0x01;
                request[1] = (byte)MessageCommand.OpenRead;
                Encoding.ASCII.GetBytes(filename).CopyTo(request, 2);

                if (Send(request) == false)
                {
                    return null;
                }

                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return null;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return null;
                }
                NXTOpenReadReply result = new NXTOpenReadReply();
                result.handle = reply[3];
                result.fileSize = GetUInt32(reply, 4);
                return result;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// [Native] Open a file for writing.  When finished writing, the handle MUST be closed with 
        /// the Close() command.  If an error occurs, this handle will be closed automatically.
        /// </summary>
        /// <param name="filename">The name of the file to read, as a string.
        ///   The extension must be included.  The possible file extensions are: 
        /// Firmware (.rfw), Program (.rxe), OnBrick Program (.rpg), TryMe Program (.rtm),
        ///  Sound (.rso), and Graphic (.ric).</param>
        /// <param name="filesize">The size of the file to be written.</param>
        /// <returns>The handle used in the write session.  Use this handle with the Close() command.</returns>
        public byte? OpenWrite(string filename, UInt32 filesize)
        {
            try
            {
                if (ValidateFilename(filename, new string[] { ".rfw", ".rxe", ".ric",
                    ".rso", ".rtm", ".rpg" }) == false)
                {
                    return null;
                }

                byte[] request = new byte[26];
                request[0] = 0x01;
                request[1] = (byte)MessageCommand.OpenWrite;
                Encoding.ASCII.GetBytes(filename).CopyTo(request, 2);
                SetUInt32(filesize, request, 22);

                if (Send(request) == false)
                {
                    return null;
                }

                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return null;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return null;
                }
                return reply[3];
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// [Native] Reads data from the NXT.
        /// </summary>
        /// <param name="handle">The handle, located in the OpenRead command.</param>
        /// <param name="bytesToRead">The number of bytes to be read.</param>
        /// <returns>The requested data from NXT flash memory.</returns>
        public byte[] Read(byte handle, UInt16 bytesToRead)
        {
            try
            {
                byte[] request = new byte[5];
                request[0] = 0x01;
                request[1] = (byte)MessageCommand.Read;
                request[2] = handle;
                SetUInt16(bytesToRead, request, 3);

                if (Send(request) == false)
                {
                    return null;
                }
                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return null;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return null;
                }
                if (reply[3] != handle)
                {
                    error = "[NXTLib] There was a problem with the reply.";
                    return null;
                }

                UInt16 bytesRead = GetUInt16(reply, 4);
                byte[] response = new byte[bytesRead];
                Array.Copy(reply, 6, response, 0, bytesRead);

                return response;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// [Native] Writes data to an NXT.
        /// </summary>
        /// <param name="handle">The handle, located in the OpenWrite command.</param>
        /// <param name="data">The data to be written to flash memory.</param>
        /// <returns>The number of bytes written.</returns>
        public int? Write(byte handle, byte[] data)
        {
            try
            {
                byte[] request = new byte[3 + data.Length];
                request[0] = 0x01;
                request[1] = (byte)MessageCommand.Write;
                request[2] = handle;
                data.CopyTo(request, 3);
                if (Send(request) == false)
                {
                    return null;
                }
                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return null;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return null;
                }
                byte handleOut = reply[3];
                if (handleOut != handle)
                {
                    error = "[NXTLib] There was a problem with the reply.";
                    return null;
                }

                UInt16 bytesWritten = GetUInt16(reply, 4);

                return Convert.ToInt32(bytesWritten);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// [Native] Close and dispose of a handle.
        /// </summary>
        /// <param name="buffer">The buffer number.  Either 0x00 (Poll) or 0x01 (High Speed).</param>
        /// <returns>The number of bytes for the command ready in the buffer (0 = no command ready).</returns>
        public bool Close(byte handle)
        {
            try
            {

                byte[] request = new byte[] {
                0x01,
                (byte) MessageCommand.Close,
                handle
                };
                return CompleteRequest(request);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }


        /// <summary>
        /// [Native] Delete a file from the NXT.
        /// </summary>
        /// <param name="filename">The name of the file to read, as a string.
        ///   The extension must be included.  The possible file extensions are: 
        /// Program (.rxe), OnBrick Program (.rpg), TryMe Program (.rtm),
        ///  Sound (.rso), Datalog (.rdt), and Graphic (.ric).</param>
        /// <param name="filesize">The size of the file to be written.</param>
        /// <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool Delete(string filename)
        {
            try
            {
                if (ValidateFilename(filename, new string[] { ".rso", ".ric", ".rxe", ".rpg",
                ".rtm", ".rdt"}) == false)
                {
                    return false;
                }

                byte[] request = new byte[22];
                request[0] = 0x01;
                request[1] = (byte)MessageCommand.Delete;
                Encoding.ASCII.GetBytes(filename).CopyTo(request, 2);

                if (Send(request) == false)
                {
                    return false;
                }

                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return false;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return false;
                }

                string fileNameOut = Encoding.ASCII.GetString(reply, 3, 20);
                if (fileNameOut != filename)
                {
                    throw new Exception(string.Format(
                        "[NXTLib] The file reported as deleted, '{0}', was different from the file requested, '{1}'."
                        , fileNameOut, filename));
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
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
            public string fileName;

            /// <summary>
            /// <para>The filesize.</para>
            /// </summary>
            public UInt32 fileSize;
        }

        /// <summary>
        /// [Native] Find the first file that matches a pattern.
        /// </summary>
        /// <param name="pattern">The pattern to search against.  Allows the following wildcards:
        ///  Filename.Extension, Filename.*, *.Extension, and *.*</param>
        /// <returns>The nullable structure FileTypeReply.  Returns null if an error occurs.</returns>
        public FindFileReply? FindFirst(string pattern)
        {
            try
            {
                ValidateFilename(pattern);

                byte[] request = new byte[22];
                request[0] = 0x01;
                request[1] = (byte)MessageCommand.FindFirst;
                Encoding.ASCII.GetBytes(pattern).CopyTo(request, 2);

                return Parse_FindFile(request);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// [Native] Find the next file that matches the pattern stored in a handle.
        /// </summary>
        /// <param name="handle">The handle found in FindFirst.</param>
        /// <returns>The nullable structure FileTypeReply.  Returns null if an error occurs.</returns>
        public FindFileReply? FindNext(byte handle)
        {
            byte[] request = new byte[] {
                0x01,
                (byte) MessageCommand.FindNext,
                handle
            };

            return Parse_FindFile(request);
        }

        private FindFileReply? Parse_FindFile(byte[] request)
        {
            byte[] reply;
            FindFileReply result;

            try
            {
                if (Send(request) == false)
                {
                    return null;
                }

                reply = RecieveReply();

                if (reply == null)
                {
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "[NXT Bluetooth] File not found.")
                {
                    result.fileFound = false;
                    result.handle = 0;
                    result.fileName = "";
                    result.fileSize = 0;
                    return result;
                }

                // Rethrow if not a FileNotFound error.
                error = ex.Message;
                return null;
            }

            result.fileFound = true;
            result.handle = reply[3];
            result.fileName = Encoding.ASCII.GetString(reply, 4, 20).TrimEnd('\0');
            result.fileSize = GetUInt32(reply, 24);
            return result;
        }

        /// <summary>
        /// [Native] Poll the length of a command in a specific buffer.
        /// </summary>
        /// <param name="buffer">The buffer number.  Either 0x00 (Poll) or 0x01 (High Speed).</param>
        /// <returns>The number of bytes for the command ready in the buffer (0 = no command ready).</returns>
        public byte? PollCommandLength(byte buffer)
        {
            try
            {
                byte[] request = new byte[] {
                0x01,
                (byte) MessageCommand.PollCommandLength,
                buffer
                };

                if (Send(request) == false)
                {
                    return null;
                }
                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return null;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return null;
                }
                byte bufferNoOut = reply[3];
                if (bufferNoOut != buffer)
                    throw new Exception("[NXTLib] There was a problem with the reply.");

                byte bytesReady = reply[4];
                return bytesReady;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
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
        ///   <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool MessageWrite(Mailbox box, string message)
        {
            try
            {
                if (!message.EndsWith("\0"))
                    message += '\0';

                int messageSize = message.Length;
                if (messageSize > 59)
                {
                    error = "[NXTLib] Message may not exceed 57 characters.";
                    return false;
                }

                byte[] request = new byte[4 + messageSize];
                request[0] = (byte)(0x00);
                request[1] = (byte)DirectCommand.MessageWrite;
                request[2] = (byte)box;
                request[3] = (byte)messageSize;
                Encoding.ASCII.GetBytes(message).CopyTo(request, 4);

                return CompleteRequest(request);                
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary> 
        /// [Native] Write a message to an incoming box on the NXT.
        /// </summary>
        /// <param name="box">The mailbox to send the message to.</param>
        /// <param name="message">The message to send, as a byte array.
        ///   The array must not be longer than 59 characters and must conclude with '/0'.</param>
        ///   <returns>Returns true if operation was a success, false otherwise.  If false, check LastError.</returns>
        public bool MessageWrite(Mailbox box, byte[] message)
        {
            try
            {
                int messageSize = message.Length + 1;  // Add 1 for the 0-byte at the end.
                if (messageSize > 59)
                {
                    error = "[NXTLib] Message may not exceed 59 characters.";
                    return false;
                }

                byte[] request = new byte[4 + messageSize];
                request[0] = (byte)(0x00);
                request[1] = (byte)DirectCommand.MessageWrite;
                request[2] = (byte)box;
                request[3] = (byte)messageSize;
                message.CopyTo(request, 4);
                request[request.Length - 1] = 0;

                return CompleteRequest(request);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
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
            try
            {
                byte[] request = new byte[] {
                0x00,
                (byte) DirectCommand.MessageRead,
                (byte) remoteInboxNo,
                (byte) localInboxNo,
                (byte) (remove ? 0xFF : 0x00)
                };

                if (Send(request) == false)
                {
                    return null;
                }
                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return null;
                }
                if (reply[3] != (byte)localInboxNo)
                {
                    error = "[NXTLib] The reply was incorrect.";
                    return null;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return null;
                }
                byte localInboxNoOut = reply[3];

                byte messageSize = reply[4];

                string message = Encoding.ASCII.GetString(reply, 5, messageSize).TrimEnd('\0');
                return message;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
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
            OpenWriteLinear = 0x89, OpenWriteLinear_Internal = 0x8A,
            OpenWriteData = 0x8B, OpenAppendData = 0x8C, Boot = 0x97,
            SetBrickName = 0x98, GetDeviceInfo = 0x9B, DeleteUserFlash = 0xA0,
            PollCommandLength = 0xA1, Poll = 0xA2, BluetoothFactoryReset = 0xA4
            //BluetoothFactoryReset and Boot may only be accepted by USB
        }

        internal bool CompleteRequest(byte[] request)
        {
            try
            {
                if (Send(request) == false)
                {
                    return false;
                }
                byte[] reply = RecieveReply();
                if (reply == null)
                {
                    return false;
                }
                if (reply[2] != 0x00)
                {
                    string error = LookupError(reply[2]);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        internal bool ValidateFilename(string fileName)
        {
            if (fileName.Length > 19)
            {
                error = "Filename cannot be larger than 19 characters.";
                return false;
            }
            return true;
        }

        internal bool ValidateFilename(string fileName, string[] possible_extensions)
        {
            if (fileName.Length > 19)
            {
                error = "[NXTLib] Filename cannot be larger than 19 characters.";
                return false;
            }
            if (fileName.Contains(".") == false)
            {
                error = "[NXTLib] Filename MUST include a valid extension.  The valid extensions are: ";
                for (int i = 0; i < possible_extensions.Length; i++)
                {
                    error += possible_extensions[i] + " ";
                }
                return false;
            }
            bool valid = false;
            for (int i = 0; i < possible_extensions.Length; i++)
			{
			    if (fileName.EndsWith(possible_extensions[i]))
                {
                    valid = true;
                }
			}
            if (valid == false)
            {
                error = "[NXTLib] Filename MUST include a valid extension.  The valid extensions are: ";
                for (int i = 0; i < possible_extensions.Length; i++)
                {
                    error += possible_extensions[i] + " ";
                }
            }
            return valid;
        }

        internal enum FileType { Program = 0, Internal_Program = 1, TryMe = 2, Image = 3, Sound = 4, System = 5, Sensor = 6, TXT = 7, LOG = 8, Firmware = 9 };

        internal string ValidateFilename(string fileName, FileType filetype, bool appendifmissing)
        {
            string name = fileName;
            String[] FileTypes = new String[] { ".rxe", ".rpg", ".rtm", ".ric", ".rso", ".sys", ".cal", ".txt", ".log", ".rfw" };
            string type = FileTypes[(int)(filetype)];
            if (fileName.Length > 19)
            {
                error = "[NXTLib] Filename cannot be larger than 19 characters.";
                return null;
            }
            if (appendifmissing == false)
            {
                if (fileName.Contains(type) == false)
                {
                    error = "[NXTLib] Filename must end with '" + type + "' extension.";
                    return null;
                }
            }
            else
            {
                if ((fileName.Contains(type) == false) && (fileName.Contains(".")))
                {
                    error = "[NXTLib] Filename must end with '" + type + "' extension.";
                    return null;
                }
                else
                {
                    if ((fileName.Contains(type) == true))
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

        internal string LookupError(byte error)
        {
            if (error == 0x20)
            {
                return "[NXT Bluetooth] Pending communication transaction in progress.";
            }
            if (error == 0x40)
            {
                return "[NXT Bluetooth] Specified mailbox queue is empty.";
            }
            if (error == 0xBD)
            {
                return "[NXT Bluetooth] Request failed.";
            }
            if (error == 0xBE)
            {
                return "[NXT Bluetooth] Unknown command opcode.";
            }
            if (error == 0xBF)
            {
                return "[NXT Bluetooth] Insane packet.";
            }
            if (error == 0xC0)
            {
                return "[NXT Bluetooth] Data contains out-of-range values.";
            }
            if (error == 0xDD)
            {
                return "[NXT Bluetooth] Communication bus error.";
            }
            if (error == 0xDE)
            {
                return "[NXT Bluetooth] No free memory in communtication buffer.";
            }
            if (error == 0xDF)
            {
                return "[NXT Bluetooth] Specified channel/connection is not valid.";
            }
            if (error == 0xE0)
            {
                return "[NXT Bluetooth] Specified channel/connection is not configured or busy.";
            }
            if (error == 0xEC)
            {
                return "[NXT Bluetooth] No active program.";
            }
            if (error == 0xED)
            {
                return "[NXT Bluetooth] Illegal size specified.";
            }
            if (error == 0xEE)
            {
                return "[NXT Bluetooth] Illegal mailbox queue ID specified.";
            }
            if (error == 0xEF)
            {
                return "[NXT Bluetooth] Attempted to access invalid field of a structure.";
            }
            if (error == 0xF0)
            {
                return "[NXT Bluetooth] Bad input or output specified.";
            }
            if (error == 0xFB)
            {
                return "[NXT Bluetooth] Insufficient memory available.";
            }
            if (error == 0xFF)
            {
                return "[NXT Bluetooth] Bad arguments.";
            }

            //Message Commands
            if (error == 0x81)
            {
                return "[NXT Bluetooth] No more handles.";
            }
            if (error == 0x82)
            {
                return "[NXT Bluetooth] No space.";
            }
            if (error == 0x83)
            {
                return "[NXT Bluetooth] No more files.";
            }
            if (error == 0x84)
            {
                return "[NXT Bluetooth] End of file expected.";
            }
            if (error == 0x85)
            {
                return "[NXT Bluetooth] End of file.";
            }
            if (error == 0x86)
            {
                return "[NXT Bluetooth] Not a linear file.";
            }
            if (error == 0x87)
            {
                return "[NXT Bluetooth] File not found.";
            }
            if (error == 0x88)
            {
                return "[NXT Bluetooth] Handle already closed.";
            }
            if (error == 0x89)
            {
                return "[NXT Bluetooth] Not linear space.";
            }
            if (error == 0x8A)
            {
                return "[NXT Bluetooth] Undefined error.";
            }
            if (error == 0x8B)
            {
                return "[NXT Bluetooth] File is busy.";
            }
            if (error == 0x8C)
            {
                return "[NXT Bluetooth] No write buffers.";
            }
            if (error == 0x8D)
            {
                return "[NXT Bluetooth] Append not possible.";
            }
            if (error == 0x8E)
            {
                return "[NXT Bluetooth] File is full.";
            }
            if (error == 0x8F)
            {
                return "[NXT Bluetooth] File exists.";
            }
            if (error == 0x90)
            {
                return "[NXT Bluetooth] Module not found.";
            }
            if (error == 0x91)
            {
                return "[NXT Bluetooth] Out of boundary.";
            }
            if (error == 0x92)
            {
                return "[NXT Bluetooth] Illegal file name.";
            }
            if (error == 0x93)
            {
                return "[NXT Bluetooth] Illegal handle.";
            }
            return "Unspecified internal NXT error.";
        }

#endregion

    }
}
