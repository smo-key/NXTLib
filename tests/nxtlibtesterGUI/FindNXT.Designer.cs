namespace NXTLibTesterGUI
{
    partial class FindNXT
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Search = new System.Windows.Forms.Button();
            this.List = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SearchVia = new System.Windows.Forms.ComboBox();
            this.Time = new System.Windows.Forms.ComboBox();
            this.Console = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Disconnect = new System.Windows.Forms.Button();
            this.ClearLog = new System.Windows.Forms.Button();
            this.NXTPanel = new System.Windows.Forms.Panel();
            this.NXTAdd = new System.Windows.Forms.Label();
            this.NXTConn = new System.Windows.Forms.Label();
            this.NXT = new System.Windows.Forms.Label();
            this.Border.SuspendLayout();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).BeginInit();
            this.List.SuspendLayout();
            this.NXTPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Border
            // 
            this.Border.Controls.Add(this.NXTPanel);
            this.Border.Controls.Add(this.ClearLog);
            this.Border.Controls.Add(this.Disconnect);
            this.Border.Controls.Add(this.label2);
            this.Border.Controls.Add(this.Time);
            this.Border.Controls.Add(this.Console);
            this.Border.Controls.Add(this.SearchVia);
            this.Border.Controls.Add(this.List);
            this.Border.Controls.Add(this.Search);
            this.Border.Size = new System.Drawing.Size(656, 400);
            this.Border.Controls.SetChildIndex(this.Search, 0);
            this.Border.Controls.SetChildIndex(this.List, 0);
            this.Border.Controls.SetChildIndex(this.SearchVia, 0);
            this.Border.Controls.SetChildIndex(this.Console, 0);
            this.Border.Controls.SetChildIndex(this.Time, 0);
            this.Border.Controls.SetChildIndex(this.TopPanel, 0);
            this.Border.Controls.SetChildIndex(this.IconBox, 0);
            this.Border.Controls.SetChildIndex(this.label2, 0);
            this.Border.Controls.SetChildIndex(this.Disconnect, 0);
            this.Border.Controls.SetChildIndex(this.ClearLog, 0);
            this.Border.Controls.SetChildIndex(this.NXTPanel, 0);
            // 
            // TopPanel
            // 
            this.TopPanel.Size = new System.Drawing.Size(656, 30);
            // 
            // Title
            // 
            this.Title.Size = new System.Drawing.Size(100, 17);
            this.Title.Text = "Connect to NXT";
            // 
            // CloseForm
            // 
            this.CloseForm.FlatAppearance.BorderSize = 0;
            this.CloseForm.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CloseForm.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.CloseForm.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CloseForm.Location = new System.Drawing.Point(628, 3);
            this.ToolTips.SetToolTip(this.CloseForm, "Close");
            // 
            // Minimize
            // 
            this.Minimize.FlatAppearance.BorderSize = 0;
            this.Minimize.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Minimize.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.Minimize.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Minimize.Location = new System.Drawing.Point(604, 3);
            // 
            // IconBox
            // 
            this.IconBox.Image = global::NXTLibTesterGUI.Properties.Resources.magnifier_16xLG;
            // 
            // Search
            // 
            this.Search.AutoEllipsis = true;
            this.Search.BackColor = System.Drawing.Color.Transparent;
            this.Search.FlatAppearance.BorderSize = 0;
            this.Search.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Search.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.Search.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Search.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Search.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Search.Image = global::NXTLibTesterGUI.Properties.Resources.StatusAnnotations_Play_32xLG_color;
            this.Search.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Search.Location = new System.Drawing.Point(6, 35);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(286, 41);
            this.Search.TabIndex = 25;
            this.Search.TabStop = false;
            this.Search.Text = " Search for NXT";
            this.Search.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Search.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Search.UseVisualStyleBackColor = false;
            this.Search.Click += new System.EventHandler(this.Search_Click);
            this.Search.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button_MouseDown);
            this.Search.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button_MouseUp);
            // 
            // List
            // 
            this.List.AutoScroll = true;
            this.List.BackColor = System.Drawing.SystemColors.Control;
            this.List.Controls.Add(this.label1);
            this.List.Controls.Add(this.label5);
            this.List.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.List.Location = new System.Drawing.Point(6, 115);
            this.List.Margin = new System.Windows.Forms.Padding(0);
            this.List.Name = "List";
            this.List.Size = new System.Drawing.Size(286, 277);
            this.List.TabIndex = 27;
            this.List.WrapContents = false;
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Image = global::NXTLibTesterGUI.Properties.Resources.field_16xLG;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(259, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "       NXT Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ToolTips.SetToolTip(this.label1, "Test");
            this.label1.Click += new System.EventHandler(this.Item_Click);
            // 
            // label5
            // 
            this.label5.AutoEllipsis = true;
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Image = global::NXTLibTesterGUI.Properties.Resources.field_16xLG;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Location = new System.Drawing.Point(3, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(259, 18);
            this.label5.TabIndex = 3;
            this.label5.Text = "       Mecabot";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SearchVia
            // 
            this.SearchVia.BackColor = System.Drawing.SystemColors.ControlLight;
            this.SearchVia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SearchVia.FormattingEnabled = true;
            this.SearchVia.Items.AddRange(new object[] {
            "Search via USB",
            "Search via USB & Bluetooth"});
            this.SearchVia.Location = new System.Drawing.Point(12, 84);
            this.SearchVia.Name = "SearchVia";
            this.SearchVia.Size = new System.Drawing.Size(163, 21);
            this.SearchVia.TabIndex = 28;
            this.SearchVia.TabStop = false;
            // 
            // Time
            // 
            this.Time.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Time.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Time.FormattingEnabled = true;
            this.Time.Items.AddRange(new object[] {
            "5 second search",
            "10 second search",
            "15 second search",
            "20 second search",
            "25 second search",
            "30 second search"});
            this.Time.Location = new System.Drawing.Point(181, 84);
            this.Time.Name = "Time";
            this.Time.Size = new System.Drawing.Size(111, 21);
            this.Time.TabIndex = 30;
            this.Time.TabStop = false;
            // 
            // Console
            // 
            this.Console.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Console.FormattingEnabled = true;
            this.Console.HorizontalScrollbar = true;
            this.Console.Location = new System.Drawing.Point(307, 76);
            this.Console.Name = "Console";
            this.Console.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.Console.Size = new System.Drawing.Size(340, 316);
            this.Console.TabIndex = 29;
            this.Console.TabStop = false;
            this.Console.UseTabStops = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(308, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Console";
            // 
            // Disconnect
            // 
            this.Disconnect.BackColor = System.Drawing.Color.Transparent;
            this.Disconnect.Enabled = false;
            this.Disconnect.FlatAppearance.BorderSize = 0;
            this.Disconnect.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Disconnect.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.Disconnect.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Disconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Disconnect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Disconnect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Disconnect.Image = global::NXTLibTesterGUI.Properties.Resources.StatusAnnotations_Critical_16xLG_color;
            this.Disconnect.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Disconnect.Location = new System.Drawing.Point(551, 44);
            this.Disconnect.Name = "Disconnect";
            this.Disconnect.Size = new System.Drawing.Size(93, 26);
            this.Disconnect.TabIndex = 32;
            this.Disconnect.TabStop = false;
            this.Disconnect.Text = " Disconnect";
            this.Disconnect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Disconnect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Disconnect.UseVisualStyleBackColor = false;
            this.Disconnect.Click += new System.EventHandler(this.Disconnect_Click);
            this.Disconnect.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button_MouseDown);
            this.Disconnect.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button_MouseUp);
            // 
            // ClearLog
            // 
            this.ClearLog.BackColor = System.Drawing.Color.Transparent;
            this.ClearLog.FlatAppearance.BorderSize = 0;
            this.ClearLog.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClearLog.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.ClearLog.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClearLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClearLog.Image = global::NXTLibTesterGUI.Properties.Resources.document_16xLG;
            this.ClearLog.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ClearLog.Location = new System.Drawing.Point(463, 44);
            this.ClearLog.Name = "ClearLog";
            this.ClearLog.Size = new System.Drawing.Size(85, 26);
            this.ClearLog.TabIndex = 33;
            this.ClearLog.TabStop = false;
            this.ClearLog.Text = " Clear Log";
            this.ClearLog.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ClearLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ClearLog.UseVisualStyleBackColor = false;
            this.ClearLog.Click += new System.EventHandler(this.ClearLog_Click);
            // 
            // NXTPanel
            // 
            this.NXTPanel.Controls.Add(this.NXTAdd);
            this.NXTPanel.Controls.Add(this.NXTConn);
            this.NXTPanel.Controls.Add(this.NXT);
            this.NXTPanel.Location = new System.Drawing.Point(12, 116);
            this.NXTPanel.Name = "NXTPanel";
            this.NXTPanel.Size = new System.Drawing.Size(283, 274);
            this.NXTPanel.TabIndex = 34;
            this.NXTPanel.Visible = false;
            // 
            // NXTAdd
            // 
            this.NXTAdd.AutoEllipsis = true;
            this.NXTAdd.BackColor = System.Drawing.SystemColors.Control;
            this.NXTAdd.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.NXTAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.NXTAdd.Location = new System.Drawing.Point(21, 47);
            this.NXTAdd.Name = "NXTAdd";
            this.NXTAdd.Size = new System.Drawing.Size(259, 16);
            this.NXTAdd.TabIndex = 4;
            this.NXTAdd.Text = "Address: 00:00:00:00:00:00";
            this.NXTAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NXTConn
            // 
            this.NXTConn.AutoEllipsis = true;
            this.NXTConn.BackColor = System.Drawing.SystemColors.Control;
            this.NXTConn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.NXTConn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.NXTConn.Location = new System.Drawing.Point(21, 31);
            this.NXTConn.Name = "NXTConn";
            this.NXTConn.Size = new System.Drawing.Size(259, 16);
            this.NXTConn.TabIndex = 3;
            this.NXTConn.Text = "Connection Type: USB";
            this.NXTConn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NXT
            // 
            this.NXT.AutoEllipsis = true;
            this.NXT.BackColor = System.Drawing.SystemColors.Control;
            this.NXT.ForeColor = System.Drawing.SystemColors.ControlText;
            this.NXT.Image = global::NXTLibTesterGUI.Properties.Resources.field_16xLG;
            this.NXT.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.NXT.Location = new System.Drawing.Point(0, 0);
            this.NXT.Name = "NXT";
            this.NXT.Size = new System.Drawing.Size(283, 27);
            this.NXT.TabIndex = 2;
            this.NXT.Text = "       Connected to NXT Name";
            this.NXT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FindNXT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 400);
            this.Name = "FindNXT";
            this.Text = "FindNXT";
            this.title = "Connect to NXT";
            this.Border.ResumeLayout(false);
            this.Border.PerformLayout();
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).EndInit();
            this.List.ResumeLayout(false);
            this.NXTPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Button Search;
        private System.Windows.Forms.FlowLayoutPanel List;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox SearchVia;
        private System.Windows.Forms.ComboBox Time;
        private System.Windows.Forms.ListBox Console;
        private System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Button Disconnect;
        protected System.Windows.Forms.Button ClearLog;
        private System.Windows.Forms.Panel NXTPanel;
        private System.Windows.Forms.Label NXTAdd;
        private System.Windows.Forms.Label NXTConn;
        private System.Windows.Forms.Label NXT;
    }
}