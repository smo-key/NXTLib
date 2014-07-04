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
            Brick brick = new Brick(Brick.LinkType.USB, null);
            brick.Connect();
            Protocol protocol = brick.ProtocolLink;

            if (!brick.IsConnected) { writeError("Not connected to NXT!"); return; }
            if (!protocol.StopProgram())
            { writeError(protocol.LastError); return; }

            Console.WriteLine("Success!");
            Console.WriteLine("Press ENTER...");
            Console.Read();
            return;
        }
    }
}
