namespace NXTLibTesterGUI
{
    partial class DownloadImage
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
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.Status = new System.Windows.Forms.Label();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.ProgressPanel = new System.Windows.Forms.Panel();
            this.FileList = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.CreateImage = new System.Windows.Forms.Button();
            this.Border.SuspendLayout();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).BeginInit();
            this.ProgressPanel.SuspendLayout();
            this.FileList.SuspendLayout();
            this.SuspendLayout();
            // 
            // Border
            // 
            this.Border.Controls.Add(this.ProgressPanel);
            this.Border.Controls.Add(this.CreateImage);
            this.Border.Controls.Add(this.FileList);
            this.Border.Size = new System.Drawing.Size(400, 145);
            this.Border.Controls.SetChildIndex(this.FileList, 0);
            this.Border.Controls.SetChildIndex(this.CreateImage, 0);
            this.Border.Controls.SetChildIndex(this.ProgressPanel, 0);
            this.Border.Controls.SetChildIndex(this.TopPanel, 0);
            this.Border.Controls.SetChildIndex(this.IconBox, 0);
            // 
            // Title
            // 
            this.Title.Size = new System.Drawing.Size(150, 23);
            this.Title.Text = "Create NXT Image";
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
            // saveDialog
            // 
            this.saveDialog.DefaultExt = "rim";
            this.saveDialog.FileName = "nxt.rim";
            this.saveDialog.Filter = "Robot Image|*.rim";
            this.saveDialog.Title = "Save NXT Image";
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
            this.ProgressPanel.Location = new System.Drawing.Point(8, 44);
            this.ProgressPanel.Name = "ProgressPanel";
            this.ProgressPanel.Size = new System.Drawing.Size(379, 91);
            this.ProgressPanel.TabIndex = 32;
            // 
            // FileList
            // 
            this.FileList.AutoScroll = true;
            this.FileList.Controls.Add(this.checkBox1);
            this.FileList.Controls.Add(this.checkBox2);
            this.FileList.Controls.Add(this.checkBox4);
            this.FileList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.FileList.Location = new System.Drawing.Point(8, 101);
            this.FileList.Name = "FileList";
            this.FileList.Size = new System.Drawing.Size(379, 353);
            this.FileList.TabIndex = 36;
            this.FileList.Visible = false;
            this.FileList.WrapContents = false;
            // 
            // checkBox1
            // 
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Image = global::NXTLibTesterGUI.Properties.Resources.StatusAnnotations_Play_16xLG;
            this.checkBox1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBox1.Location = new System.Drawing.Point(3, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(352, 24);
            this.checkBox1.TabIndex = 37;
            this.checkBox1.Text = "Program: test";
            this.checkBox1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Image = global::NXTLibTesterGUI.Properties.Resources.resource_16xLG;
            this.checkBox2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBox2.Location = new System.Drawing.Point(3, 33);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(352, 24);
            this.checkBox2.TabIndex = 38;
            this.checkBox2.Text = "Image: test";
            this.checkBox2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Image = global::NXTLibTesterGUI.Properties.Resources.pencil_005_16xLG;
            this.checkBox4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBox4.Location = new System.Drawing.Point(3, 63);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(352, 24);
            this.checkBox4.TabIndex = 40;
            this.checkBox4.Text = "Text File: test";
            this.checkBox4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // CreateImage
            // 
            this.CreateImage.AutoEllipsis = true;
            this.CreateImage.BackColor = System.Drawing.Color.Transparent;
            this.CreateImage.FlatAppearance.BorderSize = 0;
            this.CreateImage.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CreateImage.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.CreateImage.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CreateImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CreateImage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateImage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CreateImage.Image = global::NXTLibTesterGUI.Properties.Resources.Warning_yellow_7231_31x32;
            this.CreateImage.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.CreateImage.Location = new System.Drawing.Point(9, 44);
            this.CreateImage.Margin = new System.Windows.Forms.Padding(4);
            this.CreateImage.Name = "CreateImage";
            this.CreateImage.Size = new System.Drawing.Size(381, 50);
            this.CreateImage.TabIndex = 37;
            this.CreateImage.TabStop = false;
            this.CreateImage.Text = " Create Image Now";
            this.CreateImage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CreateImage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.CreateImage.UseVisualStyleBackColor = false;
            this.CreateImage.Visible = false;
            this.CreateImage.Click += new System.EventHandler(this.CreateImage_Click);
            // 
            // DownloadImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 145);
            this.Name = "DownloadImage";
            this.Text = "DownloadImage";
            this.title = "Create NXT Image";
            this.Border.ResumeLayout(false);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).EndInit();
            this.ProgressPanel.ResumeLayout(false);
            this.ProgressPanel.PerformLayout();
            this.FileList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveDialog;
        private System.Windows.Forms.Panel ProgressPanel;
        private System.Windows.Forms.ProgressBar Progress;
        protected System.Windows.Forms.Label Status;
        private System.Windows.Forms.FlowLayoutPanel FileList;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox4;
        protected System.Windows.Forms.Button CreateImage;

    }
}