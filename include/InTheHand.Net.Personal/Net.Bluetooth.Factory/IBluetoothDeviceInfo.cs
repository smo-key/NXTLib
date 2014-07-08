using System;
namespace NXTLib.BluetoothWrapper.Bluetooth.Factory
{
    /// <exclude/>
    public interface IBluetoothDeviceInfo
    {
#pragma warning disable 1591
        void Merge(IBluetoothDeviceInfo other);
        void SetDiscoveryTime(DateTime dt);
        //
        bool Authenticated { get; }
        NXTLib.BluetoothWrapper.Bluetooth.ClassOfDevice ClassOfDevice { get; }
        bool Connected { get; }
        BluetoothAddress DeviceAddress { get; }
        string DeviceName { get; set; }
        byte[][] GetServiceRecordsUnparsed(Guid service);
        NXTLib.BluetoothWrapper.Bluetooth.ServiceRecord[] GetServiceRecords(Guid service);
#if !V1
        IAsyncResult BeginGetServiceRecords(Guid service, AsyncCallback callback, object state);
        NXTLib.BluetoothWrapper.Bluetooth.ServiceRecord[] EndGetServiceRecords(IAsyncResult asyncResult);
#endif
        Guid[] InstalledServices { get; }
        DateTime LastSeen { get; }
        DateTime LastUsed { get; }
        void Refresh();
        bool Remembered { get; }
        int Rssi { get;}
        void SetServiceState(Guid service, bool state, bool throwOnError);
        void SetServiceState(Guid service, bool state);
        void ShowDialog();
        void Update();
        RadioVersions GetVersions();
    }
}
