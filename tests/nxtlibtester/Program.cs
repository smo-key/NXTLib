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
                Console.WriteLine("File Upload Test\r\n");

                string filename = "version.ric"; //filename on disk (locally)
                string filenameonbrick = "version.ric"; //filename on remote NXT
                Brick brick = new Brick(Brick.LinkType.Null);

                //Try Connecting via USB
                Console.WriteLine("Connecting to brick via USB...");
                try
                {
                    //Brick = top layer of code, contains the sensors and motors
                    brick = new Brick(Brick.LinkType.USB);
                    List<Protocol.BrickInfo> bricks = brick.Search();
                    if (bricks == null) { throw new Exception(brick.LastError); }
                    brick.Connect(bricks[0]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                    Console.WriteLine("Failed to connect via USB.");

                    //Try Connecting via Bluetooth
                    Console.WriteLine("Connecting to brick via Bluetooth...");
                    try
                    {
                        //Brick = top layer of code, contains the sensors and motors
                        brick = new Brick(Brick.LinkType.Bluetooth);
                        List<Protocol.BrickInfo> bricks = brick.Search();
                        if (bricks == null) { throw new Exception(brick.LastError); }
                        if (!brick.Connect(bricks[0])) { throw new Exception(brick.LastError); }
                        if (!brick.IsConnected) { throw new Exception("Failed to connect via Bluetooth!"); }
                    }
                    catch (Exception exc)
                    {
                        throw new Exception(exc.Message);
                    }
                }

                //Connect to Protocol
                Protocol protocol = brick.ProtocolLink;

                //Upload File
                Console.WriteLine("Uploading file...");
                if (!brick.UploadFile(filename, filenameonbrick)) { throw new Exception(brick.LastError); }

                //Disconnect
                if (!brick.Disconnect()) { throw new Exception(brick.LastError); }

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
