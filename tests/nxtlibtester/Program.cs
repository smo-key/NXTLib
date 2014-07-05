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
            string filename = "version.ric"; //filename on disk (locally)
            string filenameonbrick = "version.ric"; //filename on remote NXT
            UInt32 filesize = 0;
            byte? filehandle;

            //Prepare Connection
            Brick brick = new Brick(Brick.LinkType.USB, null); //Brick = top layer of code, contains the sensors and motors
            brick.Connect();
            Protocol protocol = brick.ProtocolLink; //Protocol = underlying layer of code, contains NXT communications

            //Upload File
            if (!brick.UploadFile(filename, filenameonbrick)) { writeError(brick.LastError); return; }

            Console.WriteLine("Success!");
            Console.WriteLine("Press ENTER...");
            Console.Read();
            return;
        }
    }
}
