using System;


namespace NXTLib.BluetoothWrapper.Bluetooth
{

    /// <summary>
    /// Configures what type of element will be added by the <see cref="T:NXTLib.BluetoothWrapper.Bluetooth.ServiceRecordBuilder"/>
    /// for the <see cref="F:NXTLib.BluetoothWrapper.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList"/> 
    /// attribute.
    /// </summary>
    /// -
    /// <remarks><para>Used with the <see cref="P:NXTLib.BluetoothWrapper.Bluetooth.ServiceRecordBuilder.ProtocolType"/>
    /// property.
    /// </para>
    /// </remarks>
    public enum BluetoothProtocolDescriptorType
    {
        /// <summary>
        /// No PDL attribute will be added.
        /// </summary>
        None,
        /// <summary>
        /// A standard L2CAP element will be added.
        /// </summary>
        L2Cap,
        /// <summary>
        /// A standard RFCOMM element will be added.
        /// </summary>
        Rfcomm,
        /// <summary>
        /// A standard GOEP (OBEX) element will be added.
        /// </summary>
        GeneralObex
    }

}