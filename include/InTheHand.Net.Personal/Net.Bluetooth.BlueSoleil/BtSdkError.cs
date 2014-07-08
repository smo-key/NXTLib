﻿// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.Bluetooth.Widcomm.WidcommBluetoothFactoryBase
// 
// Copyright (c) 2010 Alan J McFarlane, All rights reserved.
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Collections.Generic;
using System.Text;

namespace NXTLib.BluetoothWrapper.Bluetooth.BlueSoleil
{
    enum BtSdkError : int
    {
        /* Error Code List */
        OK = 0x0000,

        /* SDP error */
        SDP_INDEX = 0x00C0,
        SERVER_IS_ACTIVE = (SDP_INDEX + 0x0000),
        /// <summary>
        /// &#x201C;No service record with the specified search pattern is found on the remote device.&#x201D;
        /// </summary>
        NO_SERVICE = (SDP_INDEX + 0x0001),
        /// <summary>
        /// &#x201C;The specified service record does not exist on the remote device..&#x201D;
        /// </summary>
        SERVICE_RECORD_NOT_EXIST = (SDP_INDEX + 0x0002),

        /* General Error */
        GENERAL_INDEX = 0x0300,
        HANDLE_NOT_EXIST = (GENERAL_INDEX + 0x0001),
        OPERATION_FAILURE = (GENERAL_INDEX + 0x0002),
        SDK_UNINIT = (GENERAL_INDEX + 0x0003),
        INVALID_PARAMETER = (GENERAL_INDEX + 0x0004),
        NULL_POINTER = (GENERAL_INDEX + 0x0005),
        NO_MEMORY = (GENERAL_INDEX + 0x0006),
        BUFFER_NOT_ENOUGH = (GENERAL_INDEX + 0x0007),
        FUNCTION_NOTSUPPORT = (GENERAL_INDEX + 0x0008),
        NO_FIXED_PIN_CODE = (GENERAL_INDEX + 0x0009),
        CONNECTION_EXIST = (GENERAL_INDEX + 0x000A),
        OPERATION_CONFLICT = (GENERAL_INDEX + 0x000B),
        NO_MORE_CONNECTION_ALLOWED = (GENERAL_INDEX + 0x000C),
        ITEM_EXIST = (GENERAL_INDEX + 0x000D),
        ITEM_INUSE = (GENERAL_INDEX + 0x000E),
        DEVICE_UNPAIRED = (GENERAL_INDEX + 0x000F),

        /* HCI Error */
        HCI_INDEX = 0x0400,
        UNKNOWN_HCI_COMMAND = (HCI_INDEX + 0x0001),
        NO_CONNECTION = (HCI_INDEX + 0x0002),
        HARDWARE_FAILURE = (HCI_INDEX + 0x0003),
        /// <summary>
        /// &#x201C;HCI error &#x201C;Page Timeout (0X04)&#x201D; is received.&#x201D;
        /// </summary>
        PAGE_TIMEOUT = (HCI_INDEX + 0x0004),
        AUTHENTICATION_FAILURE = (HCI_INDEX + 0x0005),
        KEY_MISSING = (HCI_INDEX + 0x0006),
        MEMORY_FULL = (HCI_INDEX + 0x0007),
        CONNECTION_TIMEOUT = (HCI_INDEX + 0x0008),
        MAX_NUMBER_OF_CONNECTIONS = (HCI_INDEX + 0x0009),
        MAX_NUMBER_OF_SCO_CONNECTIONS = (HCI_INDEX + 0x000A),
        ACL_CONNECTION_ALREADY_EXISTS = (HCI_INDEX + 0x000B),
        COMMAND_DISALLOWED = (HCI_INDEX + 0x000C),
        HOST_REJECTED_LIMITED_RESOURCES = (HCI_INDEX + 0x000D),
        HOST_REJECTED_SECURITY_REASONS = (HCI_INDEX + 0x000E),
        HOST_REJECTED_PERSONAL_DEVICE = (HCI_INDEX + 0x000F),
        HOST_TIMEOUT = (HCI_INDEX + 0x0010),
        UNSUPPORTED_FEATURE = (HCI_INDEX + 0x0011),
        INVALID_HCI_COMMAND_PARAMETERS = (HCI_INDEX + 0x0012),
        PEER_DISCONNECTION_USER_END = (HCI_INDEX + 0x0013),
        PEER_DISCONNECTION_LOW_RESOURCES = (HCI_INDEX + 0x0014),
        PEER_DISCONNECTION_TO_POWER_OFF = (HCI_INDEX + 0x0015),
        LOCAL_DISCONNECTION = (HCI_INDEX + 0x0016),
        REPEATED_ATTEMPTS = (HCI_INDEX + 0x0017),
        PAIRING_NOT_ALLOWED = (HCI_INDEX + 0x0018),
        UNKNOWN_LMP_PDU = (HCI_INDEX + 0x0019),
        UNSUPPORTED_REMOTE_FEATURE = (HCI_INDEX + 0x001A),
        SCO_OFFSET_REJECTED = (HCI_INDEX + 0x001B),
        SCO_INTERVAL_REJECTED = (HCI_INDEX + 0x001C),
        SCO_AIR_MODE_REJECTED = (HCI_INDEX + 0x001D),
        INVALID_LMP_PARAMETERS = (HCI_INDEX + 0x001E),
        UNSPECIFIED_ERROR = (HCI_INDEX + 0x001F),
        UNSUPPORTED_LMP_PARAMETER_VALUE = (HCI_INDEX + 0x0020),
        ROLE_CHANGE_NOT_ALLOWED = (HCI_INDEX + 0x0021),
        LMP_RESPONSE_TIMEOUT = (HCI_INDEX + 0x0022),
        LMP_ERROR_TRANSACTION_COLLISION = (HCI_INDEX + 0x0023),
        LMP_PDU_NOT_ALLOWED = (HCI_INDEX + 0x0024),
        ENCRYPTION_MODE_NOT_ACCEPTABLE = (HCI_INDEX + 0x0025),
        UNIT_KEY_USED = (HCI_INDEX + 0x0026),
        QOS_IS_NOT_SUPPORTED = (HCI_INDEX + 0x0027),
        INSTANT_PASSED = (HCI_INDEX + 0x0028),
        PAIRING_WITH_UNIT_KEY_NOT_SUPPORTED = (HCI_INDEX + 0x0029),
        DIFFERENT_TRANSACTION_COLLISION = (HCI_INDEX + 0x002A),
        QOS_UNACCEPTABLE_PARAMETER = (HCI_INDEX + 0x002C),
        QOS_REJECTED = (HCI_INDEX + 0x002D),
        CHANNEL_CLASS_NOT_SUPPORTED = (HCI_INDEX + 0x002E),
        INSUFFICIENT_SECURITY = (HCI_INDEX + 0x002F),
        PARAMETER_OUT_OF_RANGE = (HCI_INDEX + 0x0030),
        ROLE_SWITCH_PENDING = (HCI_INDEX + 0x0032),
        RESERVED_SLOT_VIOLATION = (HCI_INDEX + 0x0034),
        ROLE_SWITCH_FAILED = (HCI_INDEX + 0x0035),

        /* OBEX error */
        OBEX_INDEX = 0x0600,
        CONTINUE = (OBEX_INDEX + 0x0090),
        SUCCESS__obex = (OBEX_INDEX + 0x00A0),
        CREATED = (OBEX_INDEX + 0x00A1),
        ACCEPTED = (OBEX_INDEX + 0x00A2),
        NON_AUTH_INFO = (OBEX_INDEX + 0x00A3),
        NO_CONTENT = (OBEX_INDEX + 0x00A4),
        RESET_CONTENT = (OBEX_INDEX + 0x00A5),
        PARTIAL_CONTENT = (OBEX_INDEX + 0x00A6),
        MULT_CHOICES = (OBEX_INDEX + 0x00B0),
        MOVE_PERM = (OBEX_INDEX + 0x00B1),
        MOVE_TEMP = (OBEX_INDEX + 0x00B2),
        SEE_OTHER = (OBEX_INDEX + 0x00B3),
        NOT_MODIFIED = (OBEX_INDEX + 0x00B4),
        USE_PROXY = (OBEX_INDEX + 0x00B5),
        BAD_REQUEST = (OBEX_INDEX + 0x00C0),
        UNAUTHORIZED = (OBEX_INDEX + 0x00C1),
        PAY_REQ = (OBEX_INDEX + 0x00C2),
        FORBIDDEN = (OBEX_INDEX + 0x00C3),
        NOTFOUND = (OBEX_INDEX + 0x00C4),
        METHOD_NOT_ALLOWED = (OBEX_INDEX + 0x00C5),
        NOT_ACCEPTABLE = (OBEX_INDEX + 0x00C6),
        PROXY_AUTH_REQ = (OBEX_INDEX + 0x00C7),
        REQUEST_TIMEOUT = (OBEX_INDEX + 0x00C8),
        CONFLICT = (OBEX_INDEX + 0x00C9),
        GONE = (OBEX_INDEX + 0x00CA),
        LEN_REQ = (OBEX_INDEX + 0x00CB),
        PREC_FAIL = (OBEX_INDEX + 0x00CC),
        REQ_ENTITY_TOO_LARGE = (OBEX_INDEX + 0x00CD),
        URL_TOO_LARGE = (OBEX_INDEX + 0x00CE),
        UNSUPPORTED_MEDIA_TYPE = (OBEX_INDEX + 0x00CF),
        SVR_ERR = (OBEX_INDEX + 0x00D0),
        NOTIMPLEMENTED = (OBEX_INDEX + 0x00D1),
        BAD_GATEWAY = (OBEX_INDEX + 0x00D2),
        SERVICE_UNAVAILABLE = (OBEX_INDEX + 0x00D3),
        GATEWAY_TIMEOUT = (OBEX_INDEX + 0x00D4),
        HTTP_NOTSUPPORT = (OBEX_INDEX + 0x00D5),
        DATABASE_FULL = (OBEX_INDEX + 0x00E0),
        DATABASE_LOCK = (OBEX_INDEX + 0x00E1),

        // "start bluetooth error extend"
        //BTSDK_ER_FAIL_INITIALIZE_BTSDK = (BTSDK_ER_APPEXTEND_INDEX + 0x0006),
    }
}
