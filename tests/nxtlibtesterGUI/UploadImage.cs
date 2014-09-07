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
    public partial class UploadImage : BaseForm
    {
        public string returnerror { get; private set; }
        
        private Thread uploadthread;
        private Brick mybrick;

        public UploadImage(Brick brick)
        {
            InitializeComponent();
            returnerror = null;
            mybrick = brick;
            ResizeRedraw = true;
            ProgressPanel.Visible = false;
            StartPanel.Visible = true;
        }

        private void StartUpload()
        {
            StartPanel.Visible = false;
            ProgressPanel.Visible = true;
            CloseForm.Visible = false;

            uploadthread = new Thread(Upload);
            uploadthread.Name = "UploadThread";
            uploadthread.IsBackground = true;
            uploadthread.SetApartmentState(ApartmentState.STA);
            uploadthread.Start();
        }

        private void Upload()
        {
            //open an open file dialog
            DialogResult result = openDialog.ShowDialog();
            if (result != DialogResult.OK) { CloseOnError("Failed to find an image."); return; }
            String image = openDialog.FileName;

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
                //create temporary directory
                string temp = "tmp";
                if (Directory.Exists(temp)) { Directory.Delete(temp, true); }
                Directory.CreateDirectory(temp);

                //load zip
                ZipFile zip = new ZipFile(image);
                ZipEntry[] files = zip.ToArray();
                Progress.Invoke(new MethodInvoker(delegate { Progress.Maximum = files.Length + 2; }));

                //keep brick alive
                mybrick.link.KeepAlive();

                //remove all NXT user files
                WipeBrick();

                //download files
                foreach (ZipEntry file in files)
	            {
                    AddProgress(1);
                    SetStatus("Extracting " + file.FileName + "...");
                    file.Extract(temp + "/", ExtractExistingFileAction.OverwriteSilently);
                    SetStatus("Uploading " + file.FileName + "...");
                    mybrick.UploadFile(temp + "/" + file.FileName, file.FileName);
	            }

                //clean up
                Directory.Delete(temp, true);
            }
            catch (Exception ex)
            { 
                CloseOnError(ex.Message);
                return;
            }

            //return successfully
            CloseOnError(null);
        }

        private void WipeBrick()
        {
            SetStatus("Wiping NXT...");
            String[] list = mybrick.FindFiles(Brick.FormFilename("*", Protocol.FileType.Program));
            list = list.Concat(mybrick.FindFiles(Brick.FormFilename("*", Protocol.FileType.Image))).ToArray();
            list = list.Concat(mybrick.FindFiles(Brick.FormFilename("*", Protocol.FileType.TextFile))).ToArray();
            foreach (string file in list)
            {
                mybrick.link.Delete(file);
            }
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

        private void ImageNow_Click(object sender, EventArgs e)
        {
            StartUpload();
        }
    }
}
