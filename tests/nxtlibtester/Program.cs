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
        static void Error_NoBricks()
        {
            Console.WriteLine("No bricks found!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            return;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("File Upload Test\r\n");

            string filename = "version.ric"; //filename on disk (locally)
            string filenameonbrick = "version.ric"; //filename on remote NXT
            Brick brick = new Brick(Brick.LinkType.Null);

            try
            {
                //Try connecting via USB
                Console.WriteLine("Searching for bricks via USB...");
                brick = new Brick(Brick.LinkType.USB);
                List<Protocol.BrickInfo> bricks = brick.link.Search();
                Console.WriteLine("Connecting to brick via USB...");
                brick.Connect(bricks[0]);
            }
            catch (NXTException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Failed to connect via USB.");

                try
                {
                    //Try Connecting via Bluetooth
                    Console.WriteLine("Searching for bricks via Bluetooth...");
                    brick = new Brick(Brick.LinkType.Bluetooth);
                    List<Protocol.BrickInfo> bricks = brick.Search();
                    Console.WriteLine("Connecting to brick via Bluetooth...");
                    brick.Connect(bricks[0]);
                }
                catch (NXTLinkNotSupported)
                {
                    Console.WriteLine("Bluetooth not supported on this machine!");
                    Error_NoBricks();
                    return;
                }
                catch (NXTNoBricksFound)
                {
                    Error_NoBricks();
                    return;
                }
                catch (NXTException)
                {
                    Console.WriteLine(ex.Message);
                    Error_NoBricks();
                    return;
                }
            }

            //Connect to underlying NXT Protocol
            Protocol protocol = brick.link;

            //Upload File
            Console.WriteLine("Uploading file...");
            brick.UploadFile(filename, filenameonbrick);

            //Disconnect
            Console.WriteLine("Disconnecting...");
            brick.Disconnect();

            Console.WriteLine("Success!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            return;
        }
    }
}
