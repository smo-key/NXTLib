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
using System.Net;

namespace NXTLibTesterGUI
{
    public partial class ImageBrick : BaseForm
    {
        public string returnerror { get; private set; }
        
        private Brick mybrick;
        string vaultdir = "vault/";
        string[] vaultfiles = null;

        public ImageBrick(Brick brick)
        {
            InitializeComponent();
            returnerror = null;
            mybrick = brick;
            ResizeRedraw = true;
            PrepareVault();
        }

        private void PrepareVault() 
        {
            Thread uploadthread = new Thread(SyncVault);
            uploadthread.Name = "PrepareVaultThread";
            uploadthread.IsBackground = true;
            uploadthread.SetApartmentState(ApartmentState.STA);
            uploadthread.Start();
        }

        private void StartUpload()
        {
            StartPanel.Visible = false;
            ProgressPanel.Visible = true;
            CloseForm.Visible = false;

            Thread uploadthread = new Thread(Upload);
            uploadthread.Name = "UploadThread";
            uploadthread.IsBackground = true;
            uploadthread.SetApartmentState(ApartmentState.STA);
            uploadthread.Start();
        }

        private void SyncVault()
        {
            System.Threading.Thread.Sleep(100);

            SetStatus("Syncing Vault...");
            System.Net.WebClient webs = new System.Net.WebClient();
            try
            {
                if (!IsConnected()) { throw new WebException(); }
                string list = webs.DownloadString("http://ehsandev.com/robotics/listversions.php");

                //get file list
                string[] files = list.Split(',');
                vaultfiles = files;
            } 
            catch (WebException)
            {
                vaultfiles = null;
            }

            this.Invoke(new MethodInvoker(delegate { UpdateUISync(); }));
        }

        private void UpdateUISync()
        {
            StartPanel.Visible = true;
            ProgressPanel.Visible = false;

            string[] files = new string[] { };

            //find files
            if (vaultfiles != null)
            {
                IconBox.BackColor = TopPanel.BackColor = Color.DodgerBlue;
                Title.ForeColor = Color.White;
                Title.Text = "Restore NXT from Vault";

                files = vaultfiles;
            }
            else
            {
                if (Directory.Exists(vaultdir))
                {
                    string[] unparsedfiles = Directory.GetFiles(vaultdir, "*.rim", SearchOption.TopDirectoryOnly);
                    List<string> filelist = new List<string>();
                    foreach (string item in unparsedfiles)
                    {
                        FileInfo file = new FileInfo(item);
                        filelist.Add(file.Name);
                    }
                    files = filelist.ToArray();
                }
            }

            //list all versions from vault
            foreach (string version in files)
            {
                if (version == null) { continue; }
                if (version.Trim() == "") { continue; }
                if (!version.EndsWith(".rim")) { continue; }

                string parsed = version.Substring(0, version.Length - 4);

                ImageList.Items.Add(parsed);
            }

            ImageList.SelectedIndex = 0;
            
        }

        private void Upload()
        {
            //get file name
            string image = null;
            this.Invoke(new MethodInvoker(delegate
            { 
                if (ImageList.SelectedIndex == 0) { CloseOnError("You need to select a version!"); return; }
                image = ImageList.Items[ImageList.SelectedIndex] + ".rim";
            }));
            
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

                //create vault directory
                if (!Directory.Exists(vaultdir)) { Directory.CreateDirectory(vaultdir); }

                //download file, if need to
                if (vaultfiles != null)
                {
                    System.Net.WebClient webs = new System.Net.WebClient();
                    webs.DownloadFile("http://ehsandev.com/robotics/" + image, vaultdir + image);
                }

                //load zip
                ZipFile zip = new ZipFile(vaultdir + image);
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

        private bool IsConnected()
        {
            try
            {
                WebClient client = new WebClient();
                using (var stream = client.OpenRead("http://ehsandev.com/"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void WipeBrick()
        {
            SetStatus("Wiping NXT...");
            String[] list = mybrick.FindFiles(Brick.FormFilename("*", Protocol.FileType.Program));
            list = list.Concat(mybrick.FindFiles(Brick.FormFilename("*", Protocol.FileType.TryMe))).ToArray();
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

        internal override void CloseForm_Click(object sender, EventArgs e)
        {
            //return cancelled error
            CloseOnError("Cancelled by user.");
        }

        private void ImageNow_Click(object sender, EventArgs e)
        {
            StartUpload();
        }
    }
}
