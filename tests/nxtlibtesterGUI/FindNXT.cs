﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using NXTLib;
using System.Net.Sockets;

namespace NXTLibTesterGUI
{
    public partial class FindNXT : BaseForm
    {
        private Brick myBrick = new Brick();
        private LinkType myLinkType = LinkType.Null;
        Thread searchthread;

        public FindNXT() : base()
        {
            InitializeComponent();
            List.Controls.Clear();
            CheckCompat();
            SearchVia.SelectedIndex = 0;
            Time.SelectedIndex = 2;
        }

        private void CheckCompat()
        {
            Bluetooth blue = new Bluetooth();
            if (!blue.IsSupported) { SearchVia.Items.RemoveAt(1); }
        }

        private void UpdateStart()
        {
            Disconnect.Enabled = false;
            Search.Enabled = false;
            CloseForm.Enabled = false;

            Thread thread = new Thread(UpdateBrick);
            thread.Start();
        }

        private void UpdateBrick()
        {
            try
            {
                bool success = true;
                WriteMessage("Reconnecting to brick...");
                try
                {
                    myBrick.Connect();
                }
                catch (SocketException)
                {
                    WriteMessage("Error while connecting to brick:");
                    WriteMessage("Brick is busy!  Perform a soft reset by turning the brick off for a few seconds.");
                    this.Invoke(new MethodInvoker(delegate
                    {
                        Disconnect_Click(null, null);
                    }));
                    return;
                }
                catch (Win32Exception)
                {
                    WriteMessage("Error while connecting to brick:");
                    WriteMessage("Brick is busy!  Perform a soft reset by turning the brick off for a few seconds.");
                    this.Invoke(new MethodInvoker(delegate
                    {
                        Disconnect_Click(null, null);
                    }));
                    return;
                }
                catch (Exception ex)
                {
                    WriteMessage("Error while connecting to brick:");
                    WriteMessage(ex.Message);
                    this.Invoke(new MethodInvoker(delegate
                    {
                        Disconnect_Click(null, null);
                    }));
                    return;
                }

                WriteMessage("Uploading file to brick...");
                try
                {
                    myBrick.link.KeepAlive();
                    myBrick.UploadFile("version.ric", "version.ric");
                }
                catch (Exception ex)
                {
                    WriteMessage("Error while writing to brick:");
                    WriteMessage(ex.Message);
                    success = false;
                }

                WriteMessage("Disconnecting from brick...");
                try
                {
                    myBrick.Disconnect();
                }
                catch (Exception ex)
                {
                    WriteMessage("Error while disconnecting from brick:");
                    WriteMessage(ex.Message);
                    this.Invoke(new MethodInvoker(delegate
                    {
                        Disconnect_Click(null, null);
                    }));
                    return;
                }

                if (success) { WriteMessage("Update successful!"); }
                else { WriteMessage("Update failed!"); }

                this.Invoke(new MethodInvoker(delegate
                {
                    Disconnect.Enabled = true;
                    Search.Enabled = true;
                    CloseForm.Enabled = true;
                }));
            }
            catch (InvalidOperationException) //Result when form closed while running
            {
                try
                {
                    myBrick.Disconnect();
                }
                catch (Exception)
                {
                    return;
                }
                return;
            }
        }
        private void StartSearch()
        {
            Console.Items.Clear();
            List.Controls.Clear();
            SearchVia.Enabled = false;
            Search.Enabled = false;
            Time.Enabled = false;
            List.Enabled = false;
            Disconnect.Enabled = true;
            Disconnect.Text = " Stop Now";
            Disconnect.Width = 90;

            searchthread = new Thread(SearchForNXT);
            searchthread.IsBackground = true;
            searchthread.Start();
        }

        private void SearchForNXT()
        {
            
            List<Brick> usbbricks = new List<Brick>();
            List<Brick> bluebricks = new List<Brick>();
            USB usb = new USB();
            Bluetooth blue = new Bluetooth();

            try
            {
                WriteMessage("Searching for bricks via USB...");
                List<Brick> list = usb.Search();
                usbbricks = list;
                WriteMessage("Brick found via USB!");
                foreach (Brick item in usbbricks)
                {
                    AddItem(item, LinkType.USB);
                }
            }
            catch (NXTNoBricksFound) { WriteMessage("No bricks found via USB."); }
            catch (Exception ex)
            {
                WriteMessage("Error while searching via USB:\r\n");
                WriteMessage(ex.Message);
            }

            int index = 0;
            SearchVia.Invoke(new MethodInvoker(delegate { index = SearchVia.SelectedIndex; }));
            int timeindex = 0;
            Time.Invoke(new MethodInvoker(delegate { timeindex = Time.SelectedIndex; }));
            
            if (index == 1)
            {
                try
                {
                    WriteMessage("Searching for bricks via Bluetooth...");
                    blue.Initialize();
                    blue.wirelessTimeout = new TimeSpan(0, 0, 5 * (timeindex + 1));
                    List<Brick> list2 = blue.Search();
                    bluebricks = list2;
                    WriteMessage("Bricks found via Bluetooth!");
                    foreach (Brick item in bluebricks)
                    {
                        AddItem(item, LinkType.Bluetooth);
                    }
                }
                catch (NXTNoBricksFound) { WriteMessage("No bricks found via Bluetooth."); }
                catch (Exception ex)
                {
                    WriteMessage("Error while searching via Bluetooth:");
                    WriteMessage(ex.Message);
                }
            }

            if ((usbbricks.Count == 0) && (bluebricks.Count == 0)) { WriteMessage("No bricks found!"); }

            this.Invoke(new MethodInvoker(delegate {
                SearchVia.Enabled = true;
                Search.Enabled = true;
                Time.Enabled = true;
                List.Enabled = true;
                Disconnect.Enabled = false;
                Disconnect.Text = " Disconnect";
                Disconnect.Width = 93;
            }));
        }

        private void WriteMessage(string message)
        {
            Console.Invoke(new MethodInvoker(delegate { Console.Items.Add(message); }));
        }

        private void Item_Click(object sender, EventArgs e)
        {
            Button s = (Button)sender;
            s.Enabled = false;
            Brick brick = new Brick();
            if (s.Name.StartsWith("USB"))
            {
                USB usb = new USB();
                Protocol.BrickInfo info = new Protocol.BrickInfo();
                info.address = new byte[] { 0, 0, 0, 0, 0, 0 };
                info.name = s.Text.Trim();
                brick = new Brick(usb, info);
                myLinkType = LinkType.USB;
            }
            if (s.Name.StartsWith("BLU"))
            {
                Bluetooth blue = new Bluetooth();
                Protocol.BrickInfo info = new Protocol.BrickInfo();
                info.address = Utils.AddressString2Byte(s.Name.Substring(3), true);
                info.name = s.Text.Trim();
                brick = new Brick(blue, info);
                myLinkType = LinkType.Bluetooth;
            }
            /*try
            {
                brick.Connect();
                brick.Disconnect();
            }
            catch (Exception ex)
            {
                WriteMessage("Error while connecting to brick:");
                WriteMessage(ex.Message);
                Disconnect_Click(null, null);
                return;
            }*/
            myBrick = brick;
            
            Disconnect.Enabled = true;
            s.Enabled = true;
            List.Controls.Clear();
            List.Visible = false;
            SearchVia.Enabled = false;
            Search.Enabled = true;
            Time.Enabled = false;
            Search.Image = global::NXTLibTesterGUI.Properties.Resources.StatusAnnotations_Complete_and_ok_32xLG_color;

            NXTPanel.Visible = true;
            NXT.Text = "       Connected to " + myBrick.brickinfo.name;
            NXTConn.Text = "Connection Type: " + myLinkType.ToString();
            if (myLinkType != LinkType.USB) { NXTAdd.Text = "Address: " + Utils.AddressByte2String(myBrick.brickinfo.address, true); }
            else { NXTAdd.Text = ""; }

            Search.Text = " Update Version Info";
            WriteMessage("Connection successful!");
        }

        private void AddItem(Brick connection, LinkType linktype)
        {
            Button button = new Button();
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = SystemColors.Control;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = SystemColors.ControlLightLight;
            button.FlatAppearance.MouseDownBackColor = SystemColors.HotTrack;
            button.AutoEllipsis = false;
            if (linktype == LinkType.USB) { button.Image = global::NXTLibTesterGUI.Properties.Resources.usb2; }
            if (linktype == LinkType.Bluetooth) { button.Image = global::NXTLibTesterGUI.Properties.Resources.bluetooth; }
            button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            button.Location = new System.Drawing.Point(3, 0);
            if (linktype == LinkType.USB) { button.Name = "USB"; }
            if (linktype == LinkType.Bluetooth) { button.Name = "BLU" + Utils.AddressByte2String(connection.brickinfo.address, true); }
            button.Size = new System.Drawing.Size(259, 20);
            button.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            button.TabIndex = 1;
            button.Text = "       " + connection.brickinfo.name;
            button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            button.MouseDown += Button_MouseDown;
            button.MouseClick += Item_Click;
            button.MouseUp += Button_MouseUp;
            List.Invoke(new MethodInvoker(delegate { List.Controls.Add(button); }));
        }

        private void Item_MouseDown(object sender, MouseEventArgs e)
        {
            Control s = (Control)sender;
            s.ForeColor = Color.White;
        }
        private void Item_MouseUp(object sender, MouseEventArgs e)
        {
            Control s = (Control)sender;
            s.ForeColor = Color.FromArgb(64, 64, 64);
        }
        new private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            Button s = (Button)sender;
            s.ForeColor = Color.White;
            s.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.HotTrack;
        }
        new private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            Button s = (Button)sender;
            s.ForeColor = Color.FromArgb(64, 64, 64);
            s.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            Button s = (Button)sender;
            if (s.Text == " Search for NXT") { StartSearch(); }
            if (s.Text == " Update Version Info") { UpdateStart(); }
        }

        private void Disconnect_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Button s = (Button)sender;
                if (s.Text == " Stop Now")
                {
                    searchthread.Abort();
                    s.Width = 93;
                    s.Enabled = false;
                    s.Text = " Disconnect";
                    SearchVia.Enabled = true;
                    Search.Enabled = true;
                    Time.Enabled = true;
                    List.Enabled = true;
                    WriteMessage("Aborted search!");
                    return;
                }
            }
            myBrick = new Brick();
            Disconnect.Enabled = false;
            List.Visible = true;
            SearchVia.Enabled = true;
            Time.Enabled = true;
            Search.Enabled = true;
            NXTPanel.Visible = false;
            CloseForm.Enabled = true;
            Search.Image = global::NXTLibTesterGUI.Properties.Resources.StatusAnnotations_Play_32xLG_color;
            Search.Text = " Search for NXT";
            WriteMessage("Disconnected successfully!");
        }

        private void ClearLog_Click(object sender, EventArgs e)
        {
            Console.Items.Clear();
        }

    }
}
