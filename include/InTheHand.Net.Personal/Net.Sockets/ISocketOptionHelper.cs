using System;

namespace NXTLib.BluetoothWrapper.Sockets
{
    internal interface ISocketOptionHelper
    {
        bool Authenticate { get; set; }
        bool Encrypt { get; set; }
        void SetPin(BluetoothAddress device, string pin);
    }

}
