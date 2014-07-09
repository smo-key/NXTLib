using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NXTLib.BluetoothWrapper;
using NXTLib.BluetoothWrapper.Sockets;
using NXTLib.BluetoothWrapper.Bluetooth;

namespace NXTLib
{
    public static class Utils
    {
        public static string AddressByte2String(byte[] address)
        {
            BluetoothAddress adr = new BluetoothAddress(address);
            return adr.ToInt64().ToString();
        }
        public static byte[] AddressString2Byte(string address)
        {
            BluetoothAddress adr = new BluetoothAddress(GetValue(address));
            return adr.ToByteArray();
        }
        public static List<Brick> Combine(List<Brick> bricks1, List<Brick> bricks2)
        {
            List<Brick> bricks = new List<Brick>();
            bricks.AddRange(bricks1);
            bricks.AddRange(bricks2);
            return bricks;
        }
        private static Int64 GetValue(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }
            Int64 output;
            // TryParse returns a boolean to indicate whether or not
            // the parse succeeded. If it didn't, you may want to take
            // some action here.
            Int64.TryParse(value, out output);
            return output;
        }
    }
}
