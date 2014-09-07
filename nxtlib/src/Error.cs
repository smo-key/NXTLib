using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NXTLib
{
    #region Native
    public class NXTException : Exception
    {
        public NXTException() : base() { ErrorByte = 0; }
        public NXTException(string message) : base(message) { ErrorByte = 0; }
        public NXTException(string message, byte errorbyte) : base(message) { ErrorByte = errorbyte; }
        public virtual byte ErrorByte { get; private set; }
    }
    public class NXTDataOutOfRange : NXTException
    {
        public override string Message
        { get { return "Data consists of out-of-range values."; } }
        public override byte ErrorByte { get { return 0xC0; } }
    }
    public class NXTNoActiveProgram : NXTException
    {
        public override string Message
        { get { return "No active program."; } }
        public override byte ErrorByte { get { return 0xEC; } }
    }
    public class NXTInsufficientMemory : NXTException
    {
        public override string Message
        { get { return "Insufficient memory available."; } }
        public override byte ErrorByte { get { return 0xFB; } }
    }
    public class NXTNoMoreFiles : NXTException
    {
        public override string Message
        { get { return "No more files."; } }
        public override byte ErrorByte { get { return 0x83; } }
    }
    public class NXTEndOfFile : NXTException
    {
        public override string Message
        { get { return "End of file."; } }
        public override byte ErrorByte { get { return 0x85; } }
    }
    public class NXTFileNotFound : NXTException
    {
        public override string Message
        { get { return "End of file."; } }
        public override byte ErrorByte { get { return 0x87; } }
    }
    public class NXTHandleAlreadyClosed : NXTException
    {
        public override string Message
        { get { return "Handle already closed."; } }
        public override byte ErrorByte { get { return 0x88; } }
    }
    public class NXTNoHandles : NXTException
    {
        public override string Message
        { get { return "No more handles.  Please turn off and on the brick."; } }
        public override byte ErrorByte { get { return 0x81; } }
    }
    public class NXTFileBusy : NXTException
    {
        public override string Message
        { get { return "File is busy."; } }
        public override byte ErrorByte { get { return 0x8B; } }
    }
    public class NXTFileExists : NXTException
    {
        public override string Message
        { get { return "File exists."; } }
        public override byte ErrorByte { get { return 0x8F; } }
    }
    public class NXTReplyIncorrect : NXTException
    {
        public override string Message
        { get { return "There was a problem with the reply."; } }
        public override byte ErrorByte { get { return 0; } }
    }
    #endregion

    #region Communication
    public class NXTCommException : NXTException
    {
        public NXTCommException() : base() { }
        public NXTCommException(string message) : base(message) { }
        public override byte ErrorByte { get { return 0; } }
    }
    public class NXTNotConnected : NXTCommException
    {
        public override string Message
        { get { return "Not connected to an NXT!"; } }
    }
    public class NXTNoBricksFound : NXTCommException
    {
        public override string Message
        { get { return "Not NXTs found!"; } }
    }
    public class NXTNoReply : NXTCommException
    {
        public override string Message
        { get { return "Not reply recieved!"; } }
    }
    public class NXTLinkNotSupported : NXTCommException
    {
        public override string Message
        { get { return "Link type not supported on this machine!"; } }
    }
    public class NXTLinkNotInitialized : NXTCommException
    {
        public override string Message
        { get { return "Link not initialized!  Run Initialize() before any other function!"; } }
    }
    #endregion
    
}