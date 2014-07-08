﻿// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.Win32.Win32Error
// 
// Copyright (c) 2011 Alan J McFarlane, All rights reserved.
// Copyright (c) 2011 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

// Values from:
//   winerror.h --  error code definitions for the Win32 API functions   *
//   Copyright (c) Microsoft Corp.  All rights reserved.                 *

namespace NXTLib.Win32
{
    internal enum Win32Error
    {
        ERROR_SUCCESS = 0,
        ERROR_FILE_NOT_FOUND = 2,
        ERROR_INSUFFICIENT_BUFFER = 0x7A, //122
        ERROR_SERVICE_DOES_NOT_EXIST = 1060,
        ERROR_DEVICE_NOT_CONNECTED = 1167,
        ERROR_NOT_FOUND = 1168,
        ERROR_NOT_AUTHENTICATED = 1244,
    }
}
