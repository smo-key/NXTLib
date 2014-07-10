using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;

namespace NXTLib
{
    public static class Utils
    {
        public static string AddressByte2String(byte[] address, bool withcolons)
        {
            //BluetoothAddress adr = new BluetoothAddress(address);
            //string c = adr.ToString();

            string[] str = new string[6];
            for (int i = 0; i < 6; i++)
                str[i] = address[5 - i].ToString("x2");

            string sep = ":";
            if (!withcolons) { sep = ""; }
            string a = string.Join(sep, str);

            return a;
        }
        public static byte[] AddressString2Byte(string address, bool withcolons)
        {
            byte[] a = new byte[6];
            int sep = 3;
            if (!withcolons) { sep = 2; }
            for (int i = 0; i < 6; i++)
            {
                byte b = byte.Parse(address.Substring(i * sep, 2), System.Globalization.NumberStyles.HexNumber);
                a[5 - i] = b;
            }

            return a;
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
