// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.Msft.BLOB
// 
// Copyright (c) 2003-2007,2011 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Runtime.InteropServices;


namespace NXTLib.BluetoothWrapper.Bluetooth.Msft
{

    internal struct BLOB
    {
        public int cbSize;
        public IntPtr pBlobData;

        internal BLOB(int size, IntPtr data)
        {
            cbSize = size;
            pBlobData = data;
        }
    }


#if ! V1
    static
#endif
    class BlobOffsets
    {
        //struct _BLOB {
        //    ULONG cbSize;
        //    BYTE* pBlobData;
        //}

        // Presumably the pointer field is 8-aligned.
        public static readonly int Offset_cbSize_0 = 0;
        public static readonly int Offset_pBlobData_4 = 1 * IntPtr.Size;

        static bool s_doneAssert;

        [System.Diagnostics.Conditional("DEBUG")]
        public static void AssertCheckLayout()
        {
            if (s_doneAssert)
                return;
            s_doneAssert = true;
#if !NETCF // No OffsetOf on NETCF
            System.Diagnostics.Debug.Assert(BlobOffsets.Offset_cbSize_0
                == Marshal.OffsetOf(typeof(BLOB), "cbSize").ToInt64(), "offset cbSize");
            System.Diagnostics.Debug.Assert(BlobOffsets.Offset_pBlobData_4
                == Marshal.OffsetOf(typeof(BLOB), "pBlobData").ToInt64(), "offset pBlobData");
#endif
        }

    }//class

}
