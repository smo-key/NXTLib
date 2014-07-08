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
using System.IO;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NXTLib.BluetoothWrapper.Bluetooth.Widcomm
{
    sealed class WidcommRfcommInterface : IDisposable
    {
        IRfCommIf m_RfCommIf;


        internal WidcommRfcommInterface(IRfCommIf rfCommIf)
        {
            m_RfCommIf = rfCommIf;
        }

        //--------------------------------------------------------------
        internal void SetScnForPeerServer(Guid serviceGuid, int scn)
        {
            bool success = m_RfCommIf.ClientAssignScnValue(serviceGuid, scn);
            if (!success)
                throw new IOException(WidcommRfcommStreamBase.WrappingIOExceptionMessage, WidcommSocketExceptions.Create_NoResultCode(
                    WidcommSocketExceptions.SocketError_SetSecurityLevel_Client_Fail, "SetScnForPeerServer"));
        }

        internal void SetSecurityLevelClient(BTM_SEC securityLevel)
        {
            const bool isServerFalse = false;
            bool success = m_RfCommIf.SetSecurityLevel(WidcommUtils.SetSecurityLevel_Client_ServiceName, securityLevel, isServerFalse);
            if (!success)
                throw new IOException(WidcommRfcommStreamBase.WrappingIOExceptionMessage, WidcommSocketExceptions.Create_NoResultCode(
                    WidcommSocketExceptions.SocketError_SetSecurityLevel_Client_Fail, "SetSecurityLevel"));
        }

        //--------------------------------------------------------------
        internal int SetScnForLocalServer(Guid serviceGuid, int scn)
        {
            bool success = m_RfCommIf.ClientAssignScnValue(serviceGuid, scn);
            if (!success)
                throw new IOException(WidcommRfcommStreamBase.WrappingIOExceptionMessage, WidcommSocketExceptions.Create_NoResultCode(
                    WidcommSocketExceptions.SocketError_SetSecurityLevel_Client_Fail, "SetScnForLocalServer"));
            int scnAssigned = m_RfCommIf.GetScn();
            Utils.MiscUtils.Trace_WriteLine("Server GetScn returned port: {0}", scnAssigned);
            Debug.Assert(scnAssigned != 0);
            return scnAssigned;
        }

        internal void SetSecurityLevelServer(BTM_SEC securityLevel, byte[] serviceName)
        {
            const bool isServerTrue = true;
            bool success = m_RfCommIf.SetSecurityLevel(serviceName, securityLevel, isServerTrue);
            if (!success)
                throw new IOException(WidcommRfcommStreamBase.WrappingIOExceptionMessage, WidcommSocketExceptions.Create_NoResultCode(
                    WidcommSocketExceptions.SocketError_SetSecurityLevel_Client_Fail, "SetSecurityLevel"));
        }

        //--------------------------------------------------------------

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            m_RfCommIf.Destroy(disposing);
        }

    }
}
