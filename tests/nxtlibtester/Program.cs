using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXTLib;

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
            string filename = "version.ric"; //filename on disk (locally)
            string filenameonbrick = "version.ric"; //filename on remote NXT
            uint filesize = 0;
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
            System.IO.StreamReader localfile =
               new System.IO.StreamReader(filename);
            string localcontents = localfile.ReadToEnd();
            byte[] localarray = Encoding.ASCII.GetBytes(localcontents);
            localfile.Close();

            //Find Length of Local File
            filesize = (uint)localcontents.Length;

            //Open New File for Reading
            filehandle = protocol.OpenWrite(filenameonbrick, filesize);
            if (!filehandle.HasValue) { writeError(protocol.LastError); return; }

            //Copy Local to Remote
            int? reply = protocol.Write(filehandle.Value, localarray);
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
