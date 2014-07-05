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
        static void Main(string[] args)
        {
            try
            {
                string filename = "../../version.ric"; //filename on disk (locally)
                string filenameonbrick = "version.ric"; //filename on remote NXT

                //Prepare Connection
                Console.WriteLine("File Upload Test\r\n");
                Console.WriteLine("Connecting to brick...");
                Brick brick = new Brick(Brick.LinkType.USB, null); //Brick = top layer of code, contains the sensors and motors
                if (!brick.Connect()) { throw new Exception(brick.LastError); }
                Protocol protocol = brick.ProtocolLink; //Protocol = underlying layer of code, contains NXT communications

                //Test Connection
                if (!brick.IsConnected) { throw new Exception("Not connected to NXT!"); }

                //Upload File
                Console.WriteLine("Uploading file...");
                if (!brick.UploadFile(filename, filenameonbrick)) { throw new Exception(brick.LastError); }

                Console.WriteLine("Success!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                return;
            }
        }
    }
}
