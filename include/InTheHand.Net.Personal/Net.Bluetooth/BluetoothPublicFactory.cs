using System;
using NXTLib.BluetoothWrapper.Sockets;
using NXTLib.BluetoothWrapper.Bluetooth.Factory;

namespace NXTLib.BluetoothWrapper.Bluetooth
{
    /// <summary>
    /// Provides the means to create Bluetooth classes on the one selected Bluetooth
    /// stack where multiple are loaded in the same process.
    /// </summary>
    /// -
    /// <remarks>when 
    /// <para>When calling <c>new BluetoothClient()</c>, <c>new BluetoothListener()</c>,
    /// etc when multiple Bluetooth stacks are loaded at the same time then the
    /// instance is created on the primary stack.  This class allows the application
    /// to select which stack the instance is created on.
    /// Access this class via property
    /// <see cref="P:NXTLib.BluetoothWrapper.Bluetooth.BluetoothRadio.StackFactory"/>.
    /// </para>
    /// </remarks>
    public sealed class BluetoothPublicFactory
    {
        BluetoothFactory m_factory;

        internal BluetoothPublicFactory(BluetoothFactory factory)
        {
            m_factory = factory;
            m_factory.ToString();//null check
        }

        #region Client
        /// <overloads>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.BluetoothClient"/>
        /// class, using the respective stack and/or radio.
        /// </overloads>
        /// -
        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.BluetoothClient"/>
        /// class, using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <returns>The new instance.
        /// </returns>
        public BluetoothClient CreateBluetoothClient()
        {
            return new BluetoothClient(m_factory);
        }
        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.BluetoothClient"/> class,
        /// with the specified local endpoint and
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <param name="localEP">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothClient.#ctor(NXTLib.BluetoothWrapper.BluetoothEndPoint)"/>.
        /// </param>
        /// -
        /// <returns>The new instance.
        /// </returns>
        public BluetoothClient CreateBluetoothClient(BluetoothEndPoint localEP)
        {
            return new BluetoothClient(m_factory, localEP);
        }
        #endregion

        #region Listener
        /// <overloads>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.BluetoothListener"/>
        /// class, using the respective stack and/or radio.
        /// </overloads>
        /// -
        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.BluetoothListener"/>
        /// class,
        /// with the specified Service Class Id
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <param name="service">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(System.Guid)"/>.
        /// </param>
        /// -
        /// <returns>The new instance.
        /// </returns>
        public BluetoothListener CreateBluetoothListener(Guid service)
        {
            return new BluetoothListener(m_factory, service);
        }
        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.BluetoothListener"/>
        /// class,
        /// with the specified Service Class Id and local device address
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <param name="localAddress">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothAddress,System.Guid)"/>.
        /// </param>
        /// <param name="service">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothAddress,System.Guid)"/>.
        /// </param>
        /// -
        /// <returns>The new instance.
        /// </returns>
        public BluetoothListener CreateBluetoothListener(BluetoothAddress localAddress, Guid service)
        {
            return new BluetoothListener(m_factory, localAddress, service);
        }
        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.BluetoothListener"/>
        /// class,
        /// with the specified Service Class Id and local device address as a
        /// <see cref="T:NXTLib.BluetoothWrapper.BluetoothEndPoint"/>
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <param name="localEP">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothEndPoint)"/>.
        /// </param>
        /// -
        /// <returns>The new instance.
        /// </returns>
        public BluetoothListener CreateBluetoothListener(BluetoothEndPoint localEP)
        {
            return new BluetoothListener(m_factory, localEP);
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.BluetoothListener"/>
        /// class,
        /// with the specified Service Class Id and raw Service Record
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <param name="service">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(System.Guid,System.Byte[],System.Int32)"/>.
        /// </param>
        /// <param name="sdpRecord">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(System.Guid,System.Byte[],System.Int32)"/>.
        /// </param>
        /// <param name="channelOffset">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(System.Guid,System.Byte[],System.Int32)"/>.
        /// </param>
        /// -
        /// <returns>The new instance.
        /// </returns>
        public BluetoothListener CreateBluetoothListener(Guid service, byte[] sdpRecord, int channelOffset)
        {
            return new BluetoothListener(m_factory, service, sdpRecord, channelOffset);
        }
        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.BluetoothListener"/>
        /// class,
        /// with the specified Service Class Id, local device address and raw Service Record
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <param name="localAddress">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothAddress,System.Guid,System.Byte[],System.Int32)"/>.
        /// </param>
        /// <param name="service">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothAddress,System.Guid,System.Byte[],System.Int32)"/>.
        /// </param>
        /// <param name="sdpRecord">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothAddress,System.Guid,System.Byte[],System.Int32)"/>.
        /// </param>
        /// <param name="channelOffset">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothAddress,System.Guid,System.Byte[],System.Int32)"/>.
        /// </param>
        /// -
        /// <returns>The new instance.
        /// </returns>
        public BluetoothListener CreateBluetoothListener(BluetoothAddress localAddress, Guid service, byte[] sdpRecord, int channelOffset)
        {
            return new BluetoothListener(m_factory, localAddress, service, sdpRecord, channelOffset);
        }
        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.BluetoothListener"/>
        /// class,
        /// with the specified Service Class Id and local device address as a
        /// <see cref="T:NXTLib.BluetoothWrapper.BluetoothEndPoint"/> and raw Service Record
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <param name="localEP">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothEndPoint,System.Byte[],System.Int32)"/>.
        /// </param>
        /// <param name="sdpRecord">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothEndPoint,System.Byte[],System.Int32)"/>.
        /// </param>
        /// <param name="channelOffset">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothEndPoint,System.Byte[],System.Int32)"/>.
        /// </param>
        /// -
        /// <returns>The new instance.
        /// </returns>
        public BluetoothListener CreateBluetoothListener(BluetoothEndPoint localEP, byte[] sdpRecord, int channelOffset)
        {
            return new BluetoothListener(m_factory, localEP, sdpRecord, channelOffset);
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.BluetoothListener"/>
        /// class,
        /// with the specified Service Class Id and Service Record
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <param name="service">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(System.Guid,NXTLib.BluetoothWrapper.Bluetooth.ServiceRecord)"/>.
        /// </param>
        /// <param name="sdpRecord">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(System.Guid,NXTLib.BluetoothWrapper.Bluetooth.ServiceRecord)"/>.
        /// </param>
        /// -
        /// <returns>The new instance.
        /// </returns>
        public BluetoothListener CreateBluetoothListener(Guid service, ServiceRecord sdpRecord)
        {
            return new BluetoothListener(m_factory, service, sdpRecord);
        }
        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.BluetoothListener"/>
        /// class,
        /// with the specified Service Class Id, local device address and Service Record
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <param name="localAddress">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothAddress,System.Guid,NXTLib.BluetoothWrapper.Bluetooth.ServiceRecord)"/>.
        /// </param>
        /// <param name="service">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothAddress,System.Guid,NXTLib.BluetoothWrapper.Bluetooth.ServiceRecord)"/>.
        /// </param>
        /// <param name="sdpRecord">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothAddress,System.Guid,NXTLib.BluetoothWrapper.Bluetooth.ServiceRecord)"/>.
        /// </param>
        /// -
        /// <returns>The new instance.
        /// </returns>
        public BluetoothListener CreateBluetoothListener(BluetoothAddress localAddress, Guid service, ServiceRecord sdpRecord)
        {
            return new BluetoothListener(m_factory, localAddress, service, sdpRecord);
        }
        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.BluetoothListener"/>
        /// class,
        /// with the specified Service Class Id and local device address as a
        /// <see cref="T:NXTLib.BluetoothWrapper.BluetoothEndPoint"/> and Service Record
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <param name="localEP">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothEndPoint,NXTLib.BluetoothWrapper.Bluetooth.ServiceRecord)"/>.
        /// </param>
        /// <param name="sdpRecord">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothListener.#ctor(NXTLib.BluetoothWrapper.BluetoothEndPoint,NXTLib.BluetoothWrapper.Bluetooth.ServiceRecord)"/>.
        /// </param>
        /// -
        /// <returns>The new instance.
        /// </returns>
        public BluetoothListener CreateBluetoothListener(BluetoothEndPoint localEP, ServiceRecord sdpRecord)
        {
            return new BluetoothListener(m_factory, localEP, sdpRecord);
        }
        #endregion

        #region Security
        /// <summary>
        /// Gets the <see cref="T:NXTLib.BluetoothWrapper.Bluetooth.BluetoothSecurity"/>
        /// instance for the respective stack and/or radio.
        /// </summary>
        /// -
        /// <value>A <see cref="T:NXTLib.BluetoothWrapper.Bluetooth.BluetoothSecurity"/>
        /// as an <see cref="T:NXTLib.BluetoothWrapper.Bluetooth.Factory.IBluetoothSecurity"/>
        /// </value>
        public IBluetoothSecurity BluetoothSecurity
        {
            get { return m_factory.DoGetBluetoothSecurity(); }
        }
        #endregion

        #region Misc
        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.BluetoothDeviceInfo"/> class,
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <param name="addr">See <see cref="M:NXTLib.BluetoothWrapper.Sockets.BluetoothDeviceInfo.#ctor(NXTLib.BluetoothWrapper.BluetoothAddress)"/>.
        /// </param>
        /// -
        /// <returns>The new instance.
        /// </returns>
        public BluetoothDeviceInfo CreateBluetoothDeviceInfo(BluetoothAddress addr)
        {
            return new BluetoothDeviceInfo(m_factory.DoGetBluetoothDeviceInfo(addr));
        }
        #endregion

        #region OBEX
        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.ObexWebRequest"/> class,
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <returns>The new instance of <see cref="T:NXTLib.BluetoothWrapper.ObexWebRequest"/>.
        /// </returns>
        /// -
        /// <param name="requestUri">See <see cref="M:NXTLib.BluetoothWrapper.ObexWebRequest.#ctor(System.Uri)"/>.
        /// </param>
        public ObexWebRequest CreateObexWebRequest(Uri requestUri)
        {
            ObexWebRequest req = new ObexWebRequest(requestUri, this);
            return req;
        }

        /// <summary>
        /// Initialize an instance of this <see cref="T:NXTLib.BluetoothWrapper.ObexWebRequest"/> class,
        /// given a scheme, a Bluetooth Device Address, and a remote path name; 
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <param name="scheme">The Uri scheme. One of 
        /// <c>obex</c>, <c>obex-push</c>, <c>obex-ftp</c>, or <c>obex-sync</c>.
        /// </param>
        /// <param name="target">The Bluetooth Device Address of the OBEX server.
        /// </param>
        /// <param name="path">The path on the OBEX server.
        /// </param>
        /// -
        /// <returns>The new instance of <see cref="T:NXTLib.BluetoothWrapper.ObexWebRequest"/>.
        /// </returns>
        public ObexWebRequest CreateObexWebRequest(string scheme, BluetoothAddress target, string path)
        {
            ObexWebRequest req = new ObexWebRequest(scheme, target, path, this);
            return req;
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.ObexListener"/> class,
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <returns>The new instance of <see cref="T:NXTLib.BluetoothWrapper.ObexListener"/>.
        /// </returns>
        public ObexListener CreateObexListener()
        {
            ObexListener req = new ObexListener(this);
            return req;
        }
        #endregion

        #region L2CAP Listener
#if false
        /// <overloads>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.L2CapListener"/>
        /// class, using the respective stack and/or radio.
        /// </overloads>
        /// -
        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.L2CapListener"/>
        /// class,
        /// with the specified Service Class Id
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <returns>The new instance.
        /// </returns>
        public L2CapListener CreateL2CapListener(Guid service)
        {
            return new L2CapListener(m_factory, service);
        }
        /// <summary>
        /// Initialise a new instance of the <see cref="T:NXTLib.BluetoothWrapper.Sockets.L2CapListener"/>
        /// class,
        /// with the specified Service Class Id and local device address as a
        /// <see cref="T:NXTLib.BluetoothWrapper.BluetoothEndPoint"/>
        /// using the respective stack and/or radio.
        /// </summary>
        /// -
        /// <returns>The new instance.
        /// </returns>
        public L2CapListener CreateL2CapListener(BluetoothEndPoint localEP)
        {
            return new L2CapListener(m_factory, localEP);
        }
#endif
        #endregion
    } //class
}
