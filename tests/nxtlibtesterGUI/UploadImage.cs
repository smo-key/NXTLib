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
        }

        private void StartUpload()
        {
            if (!Programs.Checked && !Images.Checked && !Textfiles.Checked && !Flags.Checked && !Wipe.Checked)
            {
                CloseOnError("No options selected!");
            }
            StartPanel.Visible = false;
            this.Height = this.Height * 145 / 289;
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
                if (!Programs.Checked && !Images.Checked && !Textfiles.Checked && !Flags.Checked && Wipe.Checked)
                {
                    //if only wipe checked
                    WipeBrick();
                    return;
                }

                //create temporary directory
                string temp = "tmp";
                if (Directory.Exists(temp)) { Directory.Delete(temp); }
                Directory.CreateDirectory(temp);

                //load zip
                ZipFile zip = new ZipFile(image);
                ZipEntry[] files = zip.ToArray();
                Progress.Invoke(new MethodInvoker(delegate { Progress.Maximum = files.Length + 2; }));

                //keep brick alive
                mybrick.link.KeepAlive();

                //remove all NXT user files
                if (Wipe.Checked)
                {
                    WipeBrick();
                    mybrick.link.KeepAlive();
                }

                //download files
                foreach (ZipEntry file in files)
	            {
                    AddProgress(1);
                    if (TestFile(file.FileName))
                    {
                        SetStatus("Extracting " + file.FileName + "...");
                        file.Extract(temp + "/", ExtractExistingFileAction.OverwriteSilently);
                        SetStatus("Uploading " + file + "...");
                        mybrick.UploadFile(temp + "/" + file.FileName, file.FileName);
                    }
	            }

                //clean up
                Directory.Delete(temp);
            }
            catch (Exception ex)
            { CloseOnError(ex.Message); return; }

            //return successfully
            CloseOnError(null);
        }

        private bool TestFile(string filename)
        {
            if (Programs.Checked && filename.Contains(Brick.FormFilename(".", Protocol.FileType.Program))) { return true; }
            if (Images.Checked &&
                (filename.Contains(Brick.FormFilename(".", Protocol.FileType.Image)) ||
                 filename.Contains(Brick.FormFilename(".", Protocol.FileType.Sound)))) { return true; }
            if (Textfiles.Checked &&
                (filename.Contains(Brick.FormFilename(".", Protocol.FileType.LogFile)) ||
                 filename.Contains(Brick.FormFilename(".", Protocol.FileType.TextFile)))) { return true; }
            if (Flags.Checked &&
                (filename.Contains(Brick.FormFilename(".", Protocol.FileType.OnBrickProgram)) ||
                 filename.Contains(Brick.FormFilename(".", Protocol.FileType.SensorCalibration)) ||
                 filename.Contains(Brick.FormFilename(".", Protocol.FileType.TryMe)))) { return true; }
            return false;
        }

        private void WipeBrick()
        {
            SetStatus("Wiping NXT...");
            mybrick.link.DeleteUserFlash(true);
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
