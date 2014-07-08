// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.Factory.BluetoothFactoryConfg
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.IO;
using System.Xml;
using NXTLib.BluetoothWrapper.Bluetooth.Msft;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NXTLib.BluetoothWrapper.Bluetooth
{
    static class BluetoothFactoryConfig
    {
        internal static readonly Type MsftFactoryType = typeof(NXTLib.BluetoothWrapper.Bluetooth.SocketsBluetoothFactory);
        internal static readonly Type WidcommFactoryType = typeof(NXTLib.BluetoothWrapper.Bluetooth.Widcomm.WidcommBluetoothFactory);
#if !NETCF
        internal static readonly Type BlueSoleilFactoryType = typeof(NXTLib.BluetoothWrapper.Bluetooth.BlueSoleil.BluesoleilFactory);
#if BlueZ
        internal static readonly Type BlueZFactoryType = typeof(NXTLib.BluetoothWrapper.Bluetooth.BlueZ.BluezFactory);
#endif
#else
        internal static readonly Type StoneStreetFactoryType = typeof(NXTLib.BluetoothWrapper.Bluetooth.StonestreetOne.BluetopiaFactory);
#endif
        //internal static readonly Type StoneStreetFactoryFakeType = typeof(NXTLib.BluetoothWrapper.Bluetooth.StonestreetOne.BluetopiaFakeFactory);

        internal static readonly string[] s_knownStacks = {
#if NETCF
            // Eeech this has to go first as the MSFT stack API is often
            // also present on such devices...
            StoneStreetFactoryType.FullName,
#else
            //StoneStreetFactoryFakeType.FullName,
#endif
            MsftFactoryType.FullName,
#if !NETCF && BlueZ
            BlueZFactoryType.FullName,
#endif
            WidcommFactoryType.FullName,
#if !NETCF
            BlueSoleilFactoryType.FullName,
#endif
            //--typeof(NXTLib.BluetoothWrapper.Bluetooth.NullBluetoothFactory).FullName,
        };


        internal const bool Default_oneStackOnly = true;
        internal const bool Default_reportAllErrors = false;

#if NETCF
        internal static bool s_loaded;
        internal static bool s_oneStackOnly = Default_oneStackOnly;
        internal static bool s_reportAllErrors = Default_reportAllErrors;
#endif

        //----
        internal static string[] KnownStacks
        {
            get
            {
#if !NETCF
                return BluetoothFactorySection.GetInstance().StackList2;
#else
                LoadManuallyOnce();
                return s_knownStacks;
#endif
            }
        }

        internal static bool ReportAllErrors
        {
            get
            {
#if !NETCF
                return BluetoothFactorySection.GetInstance().ReportAllErrors;
#else
                LoadManuallyOnce();
                return s_reportAllErrors;
#endif
            }
        }

        internal static bool OneStackOnly
        {
            get
            {
#if !NETCF
                return BluetoothFactorySection.GetInstance().OneStackOnly;
#else
                LoadManuallyOnce();
                return s_oneStackOnly;
#endif
            }
        }

        internal static bool WidcommICheckIgnorePlatform
        {
            get
            {
#if !NETCF
                return BluetoothFactorySection.GetInstance().WidcommICheckIgnorePlatform;
#else
                throw new NotSupportedException();
#endif
            }
        }

        //--------

        // Returns the full path to the running executable on CE (Equivalent to Assembly.GetEntryAssembly() on desktop).
        // Overcomes issue if 32feet .dll is in a different folder to the application (e.g. GAC).
        internal static string GetEntryAssemblyPath()
        {
#if NETCF
            if (Environment.OSVersion.Platform != PlatformID.WinCE) {
                return null;
            }
            System.Text.StringBuilder buffer = new System.Text.StringBuilder(NativeMethods.MAX_PATH);
            int chars = NativeMethods.GetModuleFileName(IntPtr.Zero, buffer, NativeMethods.MAX_PATH);
            System.Diagnostics.Debug.WriteLine(buffer.ToString());
            return buffer.ToString();
#else
            var ea = System.Reflection.Assembly.GetEntryAssembly();
            if (ea == null) return null;
            var cb = ea.CodeBase;
            var u = new Uri(cb);
            return u.LocalPath;
#endif
        }


#if NETCF
        static void LoadManuallyOnce()
        {
            if (s_loaded) {
                return;
            }
            s_loaded = true;
            Values vs = new Values();
            bool success = false;

            string exePath = GetEntryAssemblyPath();
            if (exePath == null) { // null if we're under unit-test or on desktop...
                Debug.Fail("Running NETCF assembly on desktop??");
                return;
            }
            string path = exePath + ".config"; // e.g. "\Program Files\MyApp\MyApp.exe.config"
            bool pathExists = File.Exists(path);

            if (pathExists)
            {

                using (TextReader rdr = File.OpenText(path))
                {
                    success = LoadManually(rdr, vs);
                }

                System.Diagnostics.Debug.WriteLine(string.Format("Successfully read from {0} = {1}", path, success));
            }

            if (!success | !pathExists)
            {
                //use legacy path
                path = Path.Combine(Path.GetDirectoryName(exePath), "32feet.config");

                if (File.Exists(path))
                {
                    using (TextReader rdr = File.OpenText(path))
                    {
                        success = LoadManually(rdr, vs);
                    }

                    System.Diagnostics.Debug.WriteLine(string.Format("Successfully read from {0} = {1}", path, success));
                }
            }

            //if one of the reads was successful set the values retrieved
            if (success)
            {
                if (vs.oneStackOnly != null)
                    s_oneStackOnly = (bool)vs.oneStackOnly;
                if (vs.reportAllErrors != null)
                    s_reportAllErrors = (bool)vs.reportAllErrors;
            }//if
        }
#endif

#if true || WinCE
        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
            Justification = "Is used on NETCF.")]
        sealed internal class Values
        {
            public bool? oneStackOnly;
            public bool? reportAllErrors;
        }

        static readonly string[] ElementNames = { "configuration", "NXTLib.BluetoothWrapper.Personal", "BluetoothFactory" };

        internal static bool LoadManually(TextReader src, Values v)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load(src);
            return LoadManually(xd, v);
        }

        static bool LoadManually(XmlDocument xd, Values v)
        {
            XmlNode cur = xd;
            for (int depth = 0; depth < ElementNames.Length; ++depth) {
                XmlNode prev = cur;
                cur = cur[ElementNames[depth]];
                if (cur == null) {
                    return false;
                }
            }
            //--
            XmlElement elem = (XmlElement)cur;
            bool found = GetBoolOptionalAttribute(elem, "oneStackOnly", ref v.oneStackOnly);
            found |= GetBoolOptionalAttribute(elem, "reportAllErrors", ref v.reportAllErrors);

            //return true if one or both settings were found
            return found;
        }

        static bool GetBoolOptionalAttribute(XmlElement elem, string name, ref bool? var)
        {
            string str = elem.GetAttribute(name);
            if (!StringUtilities.IsNullOrEmpty(str)) {
                var = XmlConvert.ToBoolean(str);
                return true;
            }
            //attribute was not found
            return false;
        }
#endif

    }//class
}
