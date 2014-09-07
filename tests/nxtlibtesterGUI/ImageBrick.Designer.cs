namespace NXTLibTesterGUI
{
    partial class ImageBrick
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
            this.Status = new System.Windows.Forms.Label();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.ProgressPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.StartPanel = new System.Windows.Forms.Panel();
            this.ImageList = new System.Windows.Forms.ComboBox();
            this.ImageNow = new System.Windows.Forms.Button();
            this.openDialog = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.Border.SuspendLayout();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).BeginInit();
            this.ProgressPanel.SuspendLayout();
            this.StartPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Border
            // 
            this.Border.Controls.Add(this.ProgressPanel);
            this.Border.Controls.Add(this.StartPanel);
            this.Border.Size = new System.Drawing.Size(400, 169);
            this.Border.Controls.SetChildIndex(this.StartPanel, 0);
            this.Border.Controls.SetChildIndex(this.ProgressPanel, 0);
            this.Border.Controls.SetChildIndex(this.TopPanel, 0);
            this.Border.Controls.SetChildIndex(this.IconBox, 0);
            // 
            // Title
            // 
            this.Title.Size = new System.Drawing.Size(198, 23);
            this.Title.Text = "Restore NXT from Image";
            // 
            // CloseForm
            // 
            this.CloseForm.FlatAppearance.BorderSize = 0;
            this.CloseForm.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CloseForm.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.CloseForm.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ToolTips.SetToolTip(this.CloseForm, "Close");
            // 
            // Minimize
            // 
            this.Minimize.FlatAppearance.BorderSize = 0;
            this.Minimize.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Minimize.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.Minimize.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Minimize.Visible = false;
            // 
            // IconBox
            // 
            this.IconBox.Image = global::NXTLibTesterGUI.Properties.Resources.build_Solution_16xLG;
            this.IconBox.Size = new System.Drawing.Size(21, 22);
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.BackColor = System.Drawing.Color.Transparent;
            this.Status.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Status.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Status.Location = new System.Drawing.Point(26, 11);
            this.Status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(133, 23);
            this.Status.TabIndex = 25;
            this.Status.Text = "Preparing NXT...";
            // 
            // Progress
            // 
            this.Progress.Location = new System.Drawing.Point(30, 52);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(325, 23);
            this.Progress.TabIndex = 26;
            // 
            // ProgressPanel
            // 
            this.ProgressPanel.Controls.Add(this.Progress);
            this.ProgressPanel.Controls.Add(this.Status);
            this.ProgressPanel.Location = new System.Drawing.Point(10, 55);
            this.ProgressPanel.Name = "ProgressPanel";
            this.ProgressPanel.Size = new System.Drawing.Size(379, 91);
            this.ProgressPanel.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(50, 65);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 19);
            this.label1.TabIndex = 29;
            this.label1.Text = "ALL USER DATA WILL BE OBLITERATED!";
            // 
            // StartPanel
            // 
            this.StartPanel.Controls.Add(this.label2);
            this.StartPanel.Controls.Add(this.ImageList);
            this.StartPanel.Controls.Add(this.ImageNow);
            this.StartPanel.Controls.Add(this.label1);
            this.StartPanel.Location = new System.Drawing.Point(-1, 36);
            this.StartPanel.Name = "StartPanel";
            this.StartPanel.Size = new System.Drawing.Size(400, 131);
            this.StartPanel.TabIndex = 36;
            this.StartPanel.Visible = false;
            // 
            // ImageList
            // 
            this.ImageList.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ImageList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ImageList.FormattingEnabled = true;
            this.ImageList.Items.AddRange(new object[] {
            "Select a Version..."});
            this.ImageList.Location = new System.Drawing.Point(113, 88);
            this.ImageList.Margin = new System.Windows.Forms.Padding(4);
            this.ImageList.Name = "ImageList";
            this.ImageList.Size = new System.Drawing.Size(251, 24);
            this.ImageList.TabIndex = 37;
            this.ImageList.TabStop = false;
            // 
            // ImageNow
            // 
            this.ImageNow.AutoEllipsis = true;
            this.ImageNow.BackColor = System.Drawing.Color.Transparent;
            this.ImageNow.FlatAppearance.BorderSize = 0;
            this.ImageNow.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ImageNow.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.ImageNow.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ImageNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImageNow.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImageNow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ImageNow.Image = global::NXTLibTesterGUI.Properties.Resources.Warning_yellow_7231_31x32;
            this.ImageNow.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ImageNow.Location = new System.Drawing.Point(9, 8);
            this.ImageNow.Margin = new System.Windows.Forms.Padding(4);
            this.ImageNow.Name = "ImageNow";
            this.ImageNow.Size = new System.Drawing.Size(381, 50);
            this.ImageNow.TabIndex = 28;
            this.ImageNow.TabStop = false;
            this.ImageNow.Text = " Image NXT Now";
            this.ImageNow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ImageNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ImageNow.UseVisualStyleBackColor = false;
            this.ImageNow.Click += new System.EventHandler(this.ImageNow_Click);
            // 
            // openDialog
            // 
            this.openDialog.DefaultExt = "rim";
            this.openDialog.FileName = ".rim";
            this.openDialog.Filter = "Robot Images|*.rim";
            this.openDialog.Title = "Open NXT Image";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(50, 90);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 19);
            this.label2.TabIndex = 38;
            this.label2.Text = "Version";
            // 
            // ImageBrick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 169);
            this.Name = "ImageBrick";
            this.Text = "DownloadImage";
            this.title = "Restore NXT from Image";
            this.Border.ResumeLayout(false);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).EndInit();
            this.ProgressPanel.ResumeLayout(false);
            this.ProgressPanel.PerformLayout();
            this.StartPanel.ResumeLayout(false);
            this.StartPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Label Status;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.Panel ProgressPanel;
        protected System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel StartPanel;
        private System.Windows.Forms.OpenFileDialog openDialog;
        protected System.Windows.Forms.Button ImageNow;
        private System.Windows.Forms.ComboBox ImageList;
        protected System.Windows.Forms.Label label2;

    }
}