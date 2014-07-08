// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Widcomm.WidcommSocketExceptions
// 
// Copyright (c) 2008-2009 In The Hand Ltd, All rights reserved.
// Copyright (c) 2008-2009 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
#if !NETCF
using System.Runtime.Serialization;
using System.Security.Permissions;
#else
using NXTLib.BluetoothWrapper.Sockets;
#endif

namespace NXTLib.BluetoothWrapper.Bluetooth.Widcomm
{
    internal static class WidcommSocketExceptions
    {
        internal static SocketException Create(REM_DEV_INFO_RETURN_CODE err, string location)
        {
            int errorCode = 10000;
            return new REM_DEV_INFO_RETURN_CODE_WidcommSocketException(errorCode, err, location);
        }

        internal static SocketException Create(DISCOVERY_RESULT result, string location)
        {
            return new DISCOVERY_RESULT_WidcommSocketException(SocketError_StartDiscovery_Failed, result, location);
        }

        internal static SocketException Create(PORT_RETURN_CODE result, string location)
        {
            return new PORT_RETURN_CODE_WidcommSocketException(SocketError_StartDiscovery_Failed, result, location);
        }

        internal static SocketException Create_SDP_RETURN_CODE(SdpService.SDP_RETURN_CODE ret, string location)
        {
            return new SDP_RETURN_CODE_WidcommSocketException(
                SocketError_Listener_SdpError, ret, location);
        }

        //-- 
        internal static SocketException Create_NoResultCode(int errorCode, string location)
        {
            return new NoResultCodeWidcommSocketException(errorCode, location);
        }

        internal static SocketException Create_StartInquiry(string location)
        {
            return Create_NoResultCode(SocketError_StartInquiry_Failed, location);
        }

        internal static SocketException CreateConnectFailed(string location, int? socketErrorCode)
        {
            int errorCode = socketErrorCode ?? (int)SocketError.ConnectionRefused;// SocketError_ConnectFailed;
            return Create_NoResultCode(errorCode, location);
        }

        internal static SocketException ConnectionIsPeerClosed()
        {
            return Create_NoResultCode((int)SocketError.NotConnected, "RfcommStream_Closed");
        }

        internal static SocketException Create_StartDiscovery(WBtRc ee)
        {
            return Create_NoResultCode(SocketError_StartDiscovery_Failed, "StartDiscoverySDP"
                + ((ee == unchecked((WBtRc)(-1)) /*|| ee == WBtRc.WBT_SUCCESS*/) ? string.Empty
                    : string.Format(System.Globalization.CultureInfo.InvariantCulture, ", {0} = 0x{1:X}", ee, (uint)ee)));
        }
        //--------------------------------------------------------------
#if WinXP
        //static SocketError ___err;
#endif
        //const int SocketError_ConnectFailed = 10061; //ConnectionRefused = 10061,
        //
        //const int SocketError_SystemNotReady10091 = 10091;
        //const int SocketError_VersionNotSupported10092 = 10092;
        //const int SocketError_Fault10014 = 10014;
        //
        internal const int SocketError_StartInquiry_Failed = (int)SocketError.SystemNotReady; //SocketError_SystemNotReady10091;
        internal const int SocketError_SetSecurityLevel_Client_Fail = -1;
        internal const int SocketError_StartDiscovery_Failed = (int)SocketError.VersionNotSupported; //SocketError_VersionNotSupported10092;
        internal const int SocketError_NoSuchService = (int)SocketError.AddressNotAvailable; //10049;
        internal const int SocketError_ServiceNoneRfcommScn = (int)SocketError.HostDown; //10064;
        //
        internal const int SocketError_ConnectionClosed = (int)SocketError.NotConnected; //10057;
        //
        const int SocketError_Listener_SdpError = (int)SocketError.Fault; //SocketError_Fault10014;

    }

    /// <summary>
    /// Note that this exception will always be internal, just catch SocketException.
    /// </summary>
    [Serializable]
    abstract class WidcommSocketException
        : SocketException
    {
        //--------------------------------------------------------------
        readonly string m_location;

        protected WidcommSocketException(int errorCode, string location)
            : base(errorCode)
        {
            m_location = location;
        }

        public override string Message
        {
            get
            {
                return /*base.Message
                    + "; " +*/ ErrorCodeAndDescription
                    + (m_location == null ? null : ("; " + m_location));
            }
        }
        protected abstract string ErrorCodeAndDescription { get;}

        //----
        #region Serializable
#if !NETCF
        private const string SzName_location = "_location";

        protected WidcommSocketException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            m_location = info.GetString(SzName_location);
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(SzName_location, m_location);
        }
#endif
        #endregion
    }


    [Serializable]
    class NoResultCodeWidcommSocketException
        : WidcommSocketException
    {
        internal NoResultCodeWidcommSocketException(int errorCode, string location)
            : base(errorCode, location)
        {
        }

        protected override string ErrorCodeAndDescription
        {
            get { return null; }
        }

        //----
        #region Serializable
#if !NETCF
        protected NoResultCodeWidcommSocketException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
        #endregion
    }

    //----
    abstract class GenericReturnCodeWidcommSocketException<T>
        : WidcommSocketException
        where T : IConvertible // Really want a constraint of "enum", see SetEnum...
    {
        protected readonly Int32 m_ret;  // MUST call SetEnum after setting this.
        protected string m_retName;

        internal GenericReturnCodeWidcommSocketException(int errorCode, T ret, string location)
            : base(errorCode, location)
        {
            if (!typeof(T).IsEnum) { // Need to check the constraint at runtime. :-(
                throw new InvalidOperationException("Internal error -- The generic parameter must be an Enum type.");
            }
            //
            m_ret = ret.ToInt32(System.Globalization.CultureInfo.InvariantCulture);
            SetEnum();
        }

        protected void SetEnum()
        {
            // Would like to do: "m_retName = (T)m_ret;"  But that would need a 
            // constraint of "where T : enum" which isn't possible in C# (but is
            // in IL).  So have to do something else...
#if !NETCF
            m_retName = Enum.Format(typeof(T), m_ret, "G");
#else
            object ee = Enum.Parse(typeof(T), m_ret.ToString(), false);
            m_retName = ee.ToString();
#endif
        }

        protected override string ErrorCodeAndDescription
        {
            get
            {
                return string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    typeof(T).Name // e.g. PORT_RETURN_CODE
                    + "={0}=0x{1:X}", m_retName, m_ret);
            }
        }

        //----
        #region Serializable
#if !NETCF
        private const string SzName_ret = "_ret";
        protected GenericReturnCodeWidcommSocketException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            m_ret = info.GetInt32(SzName_ret);
            SetEnum();
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(SzName_ret, m_ret);
        }
#endif
        #endregion
    }

    /**************
    // REPLACE XX four times
    [Serializable]
    class XX_WidcommSocketException
        : GenericReturnCodeWidcommSocketException<XX>
    {
        internal XX_WidcommSocketException(int errorCode, XX ret, string location)
            : base(errorCode, ret, location)
        {
        }

        #region Serializable
#if !NETCF
        protected XX_WidcommSocketException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
        #endregion
    }
    ****************/

    
    //----
    [Serializable]
    class REM_DEV_INFO_RETURN_CODE_WidcommSocketException
        : GenericReturnCodeWidcommSocketException<REM_DEV_INFO_RETURN_CODE>
    {
        internal REM_DEV_INFO_RETURN_CODE_WidcommSocketException(int errorCode, REM_DEV_INFO_RETURN_CODE ret, string location)
            : base(errorCode, ret, location)
        {
        }

        #region Serializable
#if !NETCF
        protected REM_DEV_INFO_RETURN_CODE_WidcommSocketException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
        #endregion
    }


    [Serializable]
    class PORT_RETURN_CODE_WidcommSocketException
        : GenericReturnCodeWidcommSocketException<PORT_RETURN_CODE>
    {
        internal PORT_RETURN_CODE_WidcommSocketException(int errorCode, PORT_RETURN_CODE ret, string location)
            : base(errorCode, ret, location)
        {
        }

        #region Serializable
#if !NETCF
        protected PORT_RETURN_CODE_WidcommSocketException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
        #endregion
    }


    [Serializable]
    class DISCOVERY_RESULT_WidcommSocketException
        : GenericReturnCodeWidcommSocketException<DISCOVERY_RESULT>
    {
        internal DISCOVERY_RESULT_WidcommSocketException(int errorCode, DISCOVERY_RESULT ret, string location)
            : base(errorCode, ret, location)
        {
        }

        #region Serializable
#if !NETCF
        protected DISCOVERY_RESULT_WidcommSocketException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
        #endregion
    }


    [Serializable]
    class SDP_RETURN_CODE_WidcommSocketException
        : GenericReturnCodeWidcommSocketException<SdpService.SDP_RETURN_CODE>
    {
        internal SDP_RETURN_CODE_WidcommSocketException(int errorCode, SdpService.SDP_RETURN_CODE ret, string location)
            : base(errorCode, ret, location)
        {
        }

        #region Serializable
#if !NETCF
        protected SDP_RETURN_CODE_WidcommSocketException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
        #endregion
    }

}
