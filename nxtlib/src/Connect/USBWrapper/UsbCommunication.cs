﻿using System;

namespace NXTLib.USBWrapper
{
    public class UsbCommunication
    {
        System.Guid winUsbGuid;

        private bool myDeviceDetected = false;
        private DeviceManagement myDeviceManagement = new DeviceManagement();
        internal string devicePathName { get; private set; }
        private WinUsbDevice myWinUsbDevice = new WinUsbDevice();

        public UsbCommunication(Guid winUsbGuid)
        {
            this.winUsbGuid = winUsbGuid;
        }

        public bool FindMyDevice()
        {
            if (!myDeviceDetected)
            {
                string devicePathName = "";

                // Fill an array with the device path names of all attached devices with matching GUIDs.
                bool deviceFound = myDeviceManagement.FindDeviceFromGuid(winUsbGuid, ref devicePathName);
                if (deviceFound)
                {
                    bool success = myWinUsbDevice.GetDeviceHandle(devicePathName);
                    if (success)
                    {
                        myDeviceDetected = true;

                        // Save DevicePathName so OnDeviceChange() knows which name is my device.
                        this.devicePathName = devicePathName;
                    }
                    else
                    {
                        // There was a problem in retrieving the information.
                        myDeviceDetected = false;
                        myWinUsbDevice.CloseDeviceHandle();
                    }
                }

                if (myDeviceDetected)
                    myWinUsbDevice.InitializeDevice();
            }

            return myDeviceDetected;
        }

        public void SendDataViaBulkTransfers(byte[] databuffer)
        {
            myDeviceDetected = FindMyDevice();
            if (myDeviceDetected)
            {
                UInt32 bytesToSend = Convert.ToUInt32(databuffer.Length);
                bool success = myWinUsbDevice.SendViaBulkTransfer(ref databuffer, bytesToSend);

                if (!success) myDeviceDetected = false;
            }
        }

        public byte[] ReadDataViaBulkTransfer()
        {
            myDeviceDetected = FindMyDevice();
            if (myDeviceDetected)
            {
                uint bytesToRead = 64;
                byte[] buffer = new byte[bytesToRead];
                uint bytesRead = 0;
                bool success = false;

                myWinUsbDevice.ReadViaBulkTransfer(myWinUsbDevice.myDevInfo.bulkInPipe, bytesToRead, ref buffer, ref bytesRead, ref success);

                if (!success)
                {
                    myDeviceDetected = false;
                    return null;
                }

                byte[] reply = new byte[bytesRead];
                Array.Copy(buffer, reply, bytesRead);
                return reply;
            }

            return null;
        }
    }
}
