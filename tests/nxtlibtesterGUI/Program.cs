using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;   //GuidAttribute
using System.Reflection;                //Assembly
using System.Threading;                 //Mutex
using System.Security.AccessControl;    //MutexAccessRule
using System.Security.Principal;        //SecurityIdentifier
using System.IO.Pipes;
using System.IO;

namespace NXTLibTesterGUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            // get application GUID as defined in AssemblyInfo.cs
            string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();

            // unique id for global mutex - Global prefix means it is global to the machine
            string mutexId = string.Format("Global\\{{{0}}}", appGuid);

            using (var mutex = new Mutex(false, mutexId))
            {
                // edited by Jeremy Wiebe to add example of setting up security for multi-user usage
                // edited by 'Marc' to work also on localized systems (don't use just "Everyone") 
                var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
                var securitySettings = new MutexSecurity();
                securitySettings.AddAccessRule(allowEveryoneRule);
                mutex.SetAccessControl(securitySettings);

                // edited by acidzombie24
                var hasHandle = false;
                try
                {
                    try
                    {
                        // note, you may want to time out here instead of waiting forever
                        // edited by acidzombie24
                        // mutex.WaitOne(Timeout.Infinite, false);
                        hasHandle = mutex.WaitOne(500, false);
                        if (hasHandle == false)
                        {
                            if (args.Count() == 1)
                            {
                                if (args[0] == "update")
                                {
                                    //TODO: send message to pipe
                                    NamedPipeClientStream pipe = new NamedPipeClientStream(".", "NXTLibTesterGUI_forceupdatepipe", PipeDirection.Out);
                                    StreamWriter sw = new StreamWriter(pipe);
                                    pipe.Connect();
                                    sw.AutoFlush = true;
                                    sw.WriteLine("Force Update Now!");
                                    pipe.WaitForPipeDrain();
                                    pipe.Dispose();
                                }
                                return;
                            }

                            MessageBox.Show("Application already running!  Close the other instance and try again.",
                                   "Application Running", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    catch (AbandonedMutexException)
                    {
                        // Log the fact the mutex was abandoned in another process, it will still get aquired
                        hasHandle = true;
                    }

                    FindNXT.updatewaiting = false;
                    if (args.Count() == 1)
                    {
                        if (args[0] == "update")
                        {
                            FindNXT.updatewaiting = true;
                        }
                    }

                    // Perform your work here.
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FindNXT());
                }
                finally
                {
                    // edited by acidzombie24, added if statemnet
                    if (hasHandle)
                        mutex.ReleaseMutex();
                }
            }

            
        }
    }
}
