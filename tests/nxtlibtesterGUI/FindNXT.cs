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

namespace NXTLibTesterGUI
{
    public partial class FindNXT : BaseForm
    {
        private Brick myBrick = new Brick();
        private LinkType myLinkType = LinkType.Null;
        private bool connected = false;

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

        private void UpdateBrick()
        {
            Disconnect.Enabled = false;
            Search.Enabled = false;
            bool success = true;

            WriteMessage("Reconnecting to brick...");
            try
            {
                myBrick.Connect();
            }
            catch (Exception ex)
            {
                WriteMessage("Error while connecting to brick:");
                WriteMessage(ex.Message);
                return;
            }

            WriteMessage("Uploading file to brick...");
            try
            {
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
                return;
            }

            if (success) { WriteMessage("Update successful!"); }
            else { WriteMessage("Update failed!"); }

            Disconnect.Enabled = true;
            Search.Enabled = true;
        }

        private void SearchForNXT()
        {
            Console.Items.Clear();
            List.Controls.Clear();
            SearchVia.Enabled = false;
            Search.Enabled = false;
            Time.Enabled = false;
            USB usb = new USB();
            List<Brick> usbbricks = new List<Brick>();
            Bluetooth blue = new Bluetooth();
            List<Brick> bluebricks = new List<Brick>();

            try
            {
                WriteMessage("Searching for bricks via USB...");
                List<Brick> list = usb.Search();
                usbbricks = list;
                WriteMessage("Brick found via USB!");
            }
            catch (NXTNoBricksFound) { WriteMessage("No bricks found via USB."); }
            catch (Exception ex)
            {
                WriteMessage("Error while searching via USB:\r\n");
                WriteMessage(ex.Message);
            }
            
            if (SearchVia.SelectedIndex == 1)
            {
                try
                {
                    WriteMessage("Searching for bricks via Bluetooth...");
                    blue.wirelessTimeout = new TimeSpan(0, 0, 5 * (Time.SelectedIndex + 1));
                    List<Brick> list = blue.Search();
                    bluebricks = list;
                    WriteMessage("Bricks found via Bluetooth!");
                }
                catch (NXTNoBricksFound) { WriteMessage("No bricks found via Bluetooth."); }
                catch (Exception ex)
                {
                    WriteMessage("Error while searching via Bluetooth:");
                    WriteMessage(ex.Message);
                }
            }

            if ((usbbricks.Count == 0) && (bluebricks.Count == 0)) { WriteMessage("No bricks found!"); }
            else
            {
                foreach (Brick item in usbbricks)
                {
                    AddItem(item, LinkType.USB);
                }
                foreach (Brick item in bluebricks)
                {
                    AddItem(item, LinkType.Bluetooth);
                }
            }

            SearchVia.Enabled = true;
            Search.Enabled = true;
            Time.Enabled = true;
        }

        private void WriteMessage(string message)
        {
            Console.Items.Add(message);
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
                info.address = Utils.AddressString2Byte(s.Name.Substring(3));
                info.name = s.Text.Trim();
                brick = new Brick(blue, info);
                myLinkType = LinkType.Bluetooth;
            }
            try
            {
                brick.Connect();
                brick.Disconnect();
            }
            catch (Exception ex)
            {
                WriteMessage("Error while connecting to brick:");
                WriteMessage(ex.Message);
                return;
            }
            myBrick = brick;
            
            connected = true;
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
            if (myLinkType != LinkType.USB) { NXTAdd.Text = "Address: " + Utils.AddressByte2String(myBrick.brickinfo.address); }
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
            button.Image = global::NXTLibTesterGUI.Properties.Resources.field_16xLG;
            button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            button.Location = new System.Drawing.Point(3, 0);
            if (linktype == LinkType.USB) { button.Name = "USB"; }
            if (linktype == LinkType.Bluetooth) { button.Name = "BLU" + Utils.AddressByte2String(connection.brickinfo.address); }
            button.Size = new System.Drawing.Size(259, 20);
            button.TabIndex = 1;
            button.Text = "       " + connection.brickinfo.name;
            button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            button.MouseDown += Button_MouseDown;
            button.MouseClick += Item_Click;
            button.MouseUp += Button_MouseUp;
            List.Controls.Add(button);
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
            if (s.Text == " Search for NXT") { SearchForNXT(); }
            if (s.Text == " Update Version Info") { UpdateBrick(); }
        }

        private void Disconnect_Click(object sender, EventArgs e)
        {
            myBrick = new Brick();
            connected = false;
            Disconnect.Enabled = false;
            List.Visible = true;
            SearchVia.Enabled = true;
            Time.Enabled = true;
            Search.Enabled = true;
            NXTPanel.Visible = false;
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
