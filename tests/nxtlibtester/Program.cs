using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXTLib;
using System.IO;

namespace nxtlibtester
{
    class Program
    {
        static void writeError(string error)
        {
            Console.WriteLine("Error: {0}", error);
            Console.WriteLine("Press ENTER...");
            Console.Read();
            return;
        }

        static void Main(string[] args)
        {
            string filename = "lasa.ric"; //filename on disk (locally)
            string filenameonbrick = "lasa.ric"; //filename on remote NXT
            UInt32 filesize = 0;
            byte? filehandle;

            //Prepare Connection
            Brick brick = new Brick(Brick.LinkType.USB, null); //Brick = top layer of code, contains the sensors and motors
            brick.Connect();
            Protocol protocol = brick.ProtocolLink; //Protocol = underlying layer of code, contains NXT communications

            //Test Connection
            if (!brick.IsConnected) { writeError("Not connected to NXT!"); return; }
            
            //Delete File, if Exists
            if (protocol.DoesExist(filenameonbrick))
            {
                if (!protocol.Delete(filenameonbrick)) 
                { 
                    writeError(protocol.LastError); return; 
                }
            }

            //Read Local File
            byte[] localcontents;
            using (var stream = File.OpenRead(filename))
            {
                localcontents = new byte[(int)stream.Length];
                int offset = 0;
                while (offset < localcontents.Length)
                {
                    int chunk = stream.Read(localcontents, offset, localcontents.Length - offset);
                    if (chunk == 0)
                    {
                        // Or handle this some other way
                        throw new IOException("File has shrunk while reading!");
                    }
                    offset += chunk;
                }
                stream.Close();
            }            

            //Find Length of Local File
            filesize = (UInt32)localcontents.Length;

            //Open New File for Reading
            filehandle = protocol.OpenWrite(filenameonbrick, filesize);
            if (!filehandle.HasValue) { writeError(protocol.LastError); return; }

            //Copy Local to Remote
            int? reply = protocol.Write(filehandle.Value, localcontents);
            if (!reply.HasValue) { writeError(protocol.LastError); return; }
            if (reply.Value < filesize) { writeError("Not all bytes written!"); return; }

            //Close Remote Files
            if (!protocol.Close(filehandle.Value)) { writeError(protocol.LastError); return; }


            Console.WriteLine("Success!");
            Console.WriteLine("Press ENTER...");
            Console.Read();
            return;
        }
    }
}
