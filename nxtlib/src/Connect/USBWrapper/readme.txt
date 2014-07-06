= Read me =


The WinUsbWrapper library is adapted from Jan Axelson's WinUsbDemo:
http://www.lvr.com/winusb.htm

I make absolutely no claim on the copyright for this part of the code.

Please read Jan Axelson's own readme. It is included with the project.


= Changes =

WinUsbWrapper contains the following 5 files from WinUsbDemo:

DeviceManagement.cs
DeviceManagementApi.cs
FileIOApi.cs
WinUsbDevice.cs
WinUsbDeviceApi.cs

A 6th file has been added by me; basically a facade for the WinUsb-methods, encapsulating them in the UsbCommunication class.

Only the methods for bulk transfer has been implemented since this is what is needed for communicating with the MINDSTORM's set.

The WinUsbDemo is a Windows.Form program. I have removed all the Forms-stuff in order to make it into a library that can be used in other contexts e.g. from WPF and of cause as part of MindSqualls.

The namespace was changed from "WinUsbDemo" to "WinUsbWrapper".

Finally, I have made minor editing to get rid of some of the compiler warnings.


Leg Godt,
Niels K. Handest