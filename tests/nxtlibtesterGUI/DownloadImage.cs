using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NXTLib;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using Ionic.Zip;

namespace NXTLibTesterGUI
{
    public partial class DownloadImage : BaseForm
    {
        public string returnerror { get; private set; }
        
        private Thread downloadthread;
        private Brick mybrick;

        public DownloadImage(Brick brick)
        {
            InitializeComponent();
            returnerror = null;
            mybrick = brick;

            downloadthread = new Thread(Download);
            downloadthread.Name = "DownloadThread";
            downloadthread.IsBackground = true;
            downloadthread.SetApartmentState(ApartmentState.STA);
            downloadthread.Start();
        }

        private void Download()
        {
            //open a save file dialog
            DialogResult result = saveDialog.ShowDialog();
            if (result != DialogResult.OK) { CloseOnError("Failed to save the image."); return; }
            String image = saveDialog.FileName;

            //connect to brick
            try
            {
                if (!mybrick.Connect()) { throw new NXTNotConnected(); }
            }
            catch (SocketException)
            { CloseOnError("Brick is busy!  Perform a soft reset by turning the brick off for a few seconds.");  return; }
            catch (Win32Exception)
            { CloseOnError("Brick is busy!  Perform a soft reset by turning the brick off for a few seconds.");  return; }
            catch (NullReferenceException)
            { CloseOnError("Not connected to a brick!"); return; }
            catch (Exception ex)
            { CloseOnError(ex.Message); return; }

            try
            {
                //prepare file system
                string temp = "tmp";
                if (Directory.Exists(temp)) { Directory.Delete(temp); }
                Directory.CreateDirectory(temp);

                //create zip
                ZipFile zip = new ZipFile(image);

                //get file list
                SetStatus("Getting file list...");
                mybrick.link.KeepAlive();
                string[] files = mybrick.FindFiles("*.*");
                Progress.Invoke(new MethodInvoker(delegate{ Progress.Maximum = files.Length + 2; }));

                //download files
                foreach (string file in files)
	            {
                    AddProgress(1);
                    SetStatus("Downloading " + file + "...");
                    mybrick.DownloadFile(file, temp + "/" + file);
                    SetStatus("Adding " + file + "...");
                    zip.AddFile(temp + "/" + file);
	            }

                SetStatus("Saving image...");
                AddProgress(1);
                zip.Save();
                Directory.Delete(temp);
            }
            catch (Exception ex)
            { CloseOnError(ex.Message); return; }

            //return successfully
            CloseOnError(null);
        }

        private void CloseOnError(string error)
        {
            returnerror = error;
            this.Invoke(new MethodInvoker(delegate { this.Close(); }));
        }

        private void SetStatus(string status)
        {
            Status.Invoke(new MethodInvoker(delegate { Status.Text = status; }));
        }

        private void AddProgress(int count)
        {
            Progress.Invoke(new MethodInvoker(delegate { Progress.Value += count; }));
        }

        private void SetProgress(int value)
        {
            Progress.Invoke(new MethodInvoker(delegate { Progress.Value = value; }));
        }
    }
}
