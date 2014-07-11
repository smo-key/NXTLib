using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXTLib;
using System.IO;

namespace NXTLibTester
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
            Console.WriteLine("NXTLib File Upload Test\r\n");

            string filename = "version.ric"; //filename on disk (locally)
            string filenameonbrick = "version.ric"; //filename on remote NXT
            USB usbLink = new USB();
            Bluetooth blueLink = new Bluetooth();

            Brick brick;

            try
            {
                //Try connecting via USB
                Console.WriteLine("Searching for bricks via USB...");
                List<Brick> bricks = usbLink.Search();
                Console.WriteLine("Connecting to brick via USB...");
                usbLink.Connect(bricks[0]);

                brick = bricks[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Failed to connect via USB.");

                try
                {
                    //Try Connecting via Bluetooth
                    Console.WriteLine("Searching for bricks via Bluetooth...");
                    List<Brick> bricks = blueLink.Search();
                    Console.WriteLine("Connecting to brick via Bluetooth...");
                    bricks[0].Connect();

                    brick = bricks[0];
                }
                catch (NXTNoBricksFound)
                {
                    Error_NoBricks();
                    return;
                }
                catch (Exception)
                {
                    Console.WriteLine(ex.Message);
                    Error_NoBricks();
                    return;
                }
            }

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
