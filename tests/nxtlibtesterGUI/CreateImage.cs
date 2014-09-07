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
    public partial class CreateImage : BaseForm
    {
        public string returnerror { get; private set; }
        public bool returnwarning { get; private set; }
        
        private Brick mybrick;
        private String[] filelist;
        private string tempdir = "tmp";
        private string vaultdir = "vault/";
        private string image;
        private string password = null;

        public CreateImage(Brick brick)
        {
            InitializeComponent();
            returnerror = null;
            mybrick = brick;
            ProgressPanel.Visible = false;
            PasswordPanel.Visible = true;
        }

        private void StartDownloading()
        {
            Thread downloadthread = new Thread(PrepareDownload);
            downloadthread.Name = "PrepareDownloadThread";
            downloadthread.IsBackground = true;
            downloadthread.SetApartmentState(ApartmentState.STA);
            downloadthread.Start();
        }

        private void ConnectToBrick()
        {
            //connect to brick
            try
            {
                if (!mybrick.Connect()) { throw new NXTNotConnected(); }
            }
            catch (SocketException)
            { CloseOnError("Brick is busy!  Perform a soft reset by turning the brick off for a few seconds."); return; }
            catch (Win32Exception)
            { CloseOnError("Brick is busy!  Perform a soft reset by turning the brick off for a few seconds."); return; }
            catch (NullReferenceException)
            { CloseOnError("Not connected to a brick!"); return; }
            catch (Exception ex)
            { CloseOnError(ex.Message); return; }
        }

        private void PrepareDownload()
        {
            //connect to brick
            ConnectToBrick();

            //prepare web vault
            SetStatus("Connecting to Vault...");
            if (!IsConnected()) { CloseOnError("Not connected to the Vault!"); return; }

            try
            {
                //prepare file system
                if (Directory.Exists(tempdir)) { Directory.Delete(tempdir, true); }
                Directory.CreateDirectory(tempdir);

                //get file list
                SetStatus("Getting file list...");
                mybrick.link.KeepAlive();
                string[] files = mybrick.FindFiles(Brick.FormFilename("*", Protocol.FileType.Program));
                files = files.Concat(mybrick.FindFiles(Brick.FormFilename("*", Protocol.FileType.TryMe))).ToArray();
                files = files.Concat(mybrick.FindFiles(Brick.FormFilename("*", Protocol.FileType.TextFile))).ToArray();
                files = files.Concat(mybrick.FindFiles(Brick.FormFilename("*", Protocol.FileType.Image))).ToArray();
                filelist = files;

            }
            catch (Exception ex)
            { 
                CloseOnError(ex.Message);
                return; 
            }

            //create UI elements
            this.Invoke(new MethodInvoker(delegate { CreateList(); }));

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

        private void CreateList()
        {
            this.Height = this.Height * 464 / 145;
            this.FileList.Controls.Clear();
            FileList.Visible = true;
            MakeImage.Visible = true;
            ProgressPanel.Visible = false;

            foreach (string item in filelist)
            {
                AddItem(item);
            }
        }

        private void AddItem(string file)
        {
            CheckBox item = new System.Windows.Forms.CheckBox();
            item.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            item.Location = new System.Drawing.Point(3, 3);
            item.Name = file;
            item.Size = new System.Drawing.Size(180, 24);
            item.TabIndex = 37;
            item.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            item.UseVisualStyleBackColor = true;

            Bitmap image = global::NXTLibTesterGUI.Properties.Resources.StatusAnnotations_Play_16xLG_color;
            item.Checked = true;

            if (file.Contains(Brick.FormFilename("", Protocol.FileType.TryMe)))
            {
                image = global::NXTLibTesterGUI.Properties.Resources.StatusAnnotations_Play_16xLG;
                item.Checked = false;
            }
            if (file.Contains(Brick.FormFilename("", Protocol.FileType.Image)))
            {
                image = global::NXTLibTesterGUI.Properties.Resources.resource_16xLG;
                item.Checked = false;
            }
            if (file.Contains(Brick.FormFilename("", Protocol.FileType.TextFile)))
            {
                image = global::NXTLibTesterGUI.Properties.Resources.pencil_005_16xLG;
                item.Checked = false;
            }

            item.Text = file;
            item.Image = image;
            this.FileList.Controls.Add(item);
        }

        private void Download()
        {
            try 
            {
                //create zip
                if (!Directory.Exists(vaultdir)) { Directory.CreateDirectory(vaultdir); }
                if (File.Exists(vaultdir + image)) { File.Delete(vaultdir + image); }
                ZipFile zip = new ZipFile(vaultdir + image);

                //load UI elements
                this.Invoke(new MethodInvoker(delegate { LoadList(); }));

                //prepare brick
                mybrick.link.KeepAlive();

                string[] files = filelist;
                Progress.Invoke(new MethodInvoker(delegate{ Progress.Maximum = files.Length + 2; }));

                //download files
                foreach (string file in files)
	            {
                    AddProgress(1);
                    SetStatus("Downloading " + file + "...");
                    mybrick.DownloadFile(file, tempdir + "/" + file);
	            }

                //close connection
                mybrick.Disconnect();

                //add files
                SetStatus("Saving image...");
                AddProgress(1);
                foreach (string file in files)
                {
                    zip.AddFile(tempdir + "/" + file, "");
                }

                zip.Save(vaultdir + image);
                Directory.Delete(tempdir, true);
            }
            catch (Exception ex)
            { CloseOnError(ex.Message); return; }

            if (UploadToVault())
            {
                //return successfully
                CloseOnError(null);         }

        }

        private bool UploadToVault()
        {
            System.Net.WebClient webs = new System.Net.WebClient();
            SetStatus("Uploading to Vault...");
            try
            {
                byte[] response = webs.UploadFile("http://ehsandev.com/robotics/upload.php?password=" + password, "POST", vaultdir + image);
                string str = System.Text.Encoding.Default.GetString(response);
                if (str != "true")
                {
                    CloseOnError("Vault error: " + str);
                    return false;
                }
            }
            catch (WebException)
            {
                CloseOnError("Cannot access the Vault."); return false;
            }
            return true;
        }

        private void LoadList()
        {
            List<String> files = new List<String>();
            foreach (CheckBox item in this.FileList.Controls)
            {
                if (item.Checked) { files.Add(item.Name); }
            }
            filelist = files.ToArray();
        }

        private void CloseOnError(string error)
        {
            try
            {
                returnerror = error;
                this.Invoke(new MethodInvoker(delegate { this.Close(); }));
            }
            catch { }
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

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        internal override void CloseForm_Click(object sender, EventArgs e)
        {
            //return cancelled error
            returnwarning = true;
            CloseOnError("Cancelled by user.");
        }

        private void CreateImage_Click(object sender, EventArgs e)
        {
            CloseForm.Visible = false;
            FileList.Visible = false;
            MakeImage.Visible = false;
            ProgressPanel.Visible = true;
            this.Height = this.Height * 145 / 464;

            Thread downloadthread = new Thread(Download);
            downloadthread.Name = "DownloadThread";
            downloadthread.IsBackground = true;
            downloadthread.SetApartmentState(ApartmentState.STA);
            downloadthread.Start();
        }

        private void Go_Click(object sender, EventArgs e)
        {
            if (Go.Name != "NameIt")
            {
                //password entry
                password = Entry.Text;
                Entry.Clear();
                Entry.ClearUndo();
                Entry.UseSystemPasswordChar = false;
                TextTitle.Text = "Enter image name.";
                Go.Name = "NameIt";
            }
            else
            {
                //name entry
                if (Entry.Text == null || Entry.Text.Trim() == "") { CloseOnError("No image name entered, you idiot!"); return; }
                image = Entry.Text + ".rim";
                PasswordPanel.Visible = false;
                ProgressPanel.Visible = true;
                StartDownloading();
            }
        }
    }
}
