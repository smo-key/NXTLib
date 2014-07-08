﻿// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Widcomm.WidcommSppSocketExceptions
// 
// Copyright (c) 2011 In The Hand Ltd, All rights reserved.
// Copyright (c) 2011 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Net.Sockets;
#if !NETCF
using System.Runtime.Serialization;
using System.Security.Permissions;
#endif

namespace NXTLib.BluetoothWrapper.Bluetooth.Widcomm
{
    static class WidcommSppSocketExceptions
    {
        const int SocketError_Fault10014 = 10014;
        internal const int SocketError_Misc = /*WidcommSocketExceptions.*/SocketError_Fault10014;

        //--------
        internal static SocketException Create(WidcommSppClient.SPP_STATE_CODE result, string location)
        {
            return new SPP_STATE_CODE_WidcommSocketException(SocketError_Misc, result, location);
        }

        internal static SocketException Create(WidcommSppClient.SPP_CLIENT_RETURN_CODE result, string location)
        {
            return new SPP_CLIENT_RETURN_CODE_WidcommSocketException(SocketError_Misc, result, location);
        }

    }

    [Serializable]
    class SPP_STATE_CODE_WidcommSocketException
        : GenericReturnCodeWidcommSocketException<WidcommSppClient.SPP_STATE_CODE>
    {
        internal SPP_STATE_CODE_WidcommSocketException(int errorCode, WidcommSppClient.SPP_STATE_CODE ret, string location)
            : base(errorCode, ret, location)
        {
        }

        #region Serializable
#if !NETCF
        protected SPP_STATE_CODE_WidcommSocketException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
        #endregion
    }

    [Serializable]
    class SPP_CLIENT_RETURN_CODE_WidcommSocketException
        : GenericReturnCodeWidcommSocketException<WidcommSppClient.SPP_CLIENT_RETURN_CODE>
    {
        internal SPP_CLIENT_RETURN_CODE_WidcommSocketException(int errorCode, WidcommSppClient.SPP_CLIENT_RETURN_CODE ret, string location)
            : base(errorCode, ret, location)
        {
        }

        #region Serializable
#if !NETCF
        protected SPP_CLIENT_RETURN_CODE_WidcommSocketException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
        #endregion
    }

}
