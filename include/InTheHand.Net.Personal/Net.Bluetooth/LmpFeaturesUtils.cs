#if WinXP
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace NXTLib.BluetoothWrapper.Bluetooth
{
    internal static class LmpFeaturesUtils
    {
        internal static void FindUndefinedValues(LmpFeatures lmpFeatures)
        {
            Action<LmpFeatures> action = delegate(LmpFeatures bitMask) {
                var x = lmpFeatures & bitMask;
                if (x != 0) {
                    var name = Enum.GetName(typeof(LmpFeatures), x);
                    if (name == null) {
                        var msg = "Not defined: 0x" + ((UInt64)x).ToString("X16");
                        Debug.WriteLine(msg);
                    }
                }
            };
            ForEachBit(action);
        }

        internal static void FindUnsetValues(LmpFeatures lmpFeatures)
        {
            Action<LmpFeatures> action = delegate(LmpFeatures bitMask) {
                var x = lmpFeatures & bitMask;
                if (x == 0) {
                    var name = Enum.GetName(typeof(LmpFeatures), bitMask);
                    string msg;
                    if (name == null) {
                        //msg = "((Not exist: '" + bitMask + "' 0x" + ((UInt64)bitMask).ToString("X16") + "))";
                        //Debug.WriteLine(msg);
                    } else {
                        msg = "Not set: '" + bitMask + "' 0x" + ((UInt64)bitMask).ToString("X16");
                        Debug.WriteLine(msg);
                    }
                }
            };
            ForEachBit(action);
        }

        static void ForEachBit(Action<LmpFeatures> action)
        {
            UInt64 i = 1;
            while (true) {
                action((LmpFeatures)i);
                try {
                    checked { i *= 2; }
                } catch (OverflowException) {
                    break;
                }
            }//while
        }
    }
}
#endif