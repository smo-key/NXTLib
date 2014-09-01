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
            this.Status = new System.Windows.Forms.Label();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.Border.SuspendLayout();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Border
            // 
            this.Border.Controls.Add(this.Progress);
            this.Border.Controls.Add(this.Status);
            this.Border.Size = new System.Drawing.Size(400, 144);
            this.Border.Controls.SetChildIndex(this.TopPanel, 0);
            this.Border.Controls.SetChildIndex(this.IconBox, 0);
            this.Border.Controls.SetChildIndex(this.Status, 0);
            this.Border.Controls.SetChildIndex(this.Progress, 0);
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
            this.CloseForm.Visible = false;
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
            this.Status.Location = new System.Drawing.Point(34, 54);
            this.Status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(133, 23);
            this.Status.TabIndex = 25;
            this.Status.Text = "Preparing NXT...";
            // 
            // Progress
            // 
            this.Progress.Location = new System.Drawing.Point(38, 95);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(325, 23);
            this.Progress.TabIndex = 26;
            // 
            // saveDialog
            // 
            this.saveDialog.DefaultExt = "rim";
            this.saveDialog.FileName = "nxt.rim";
            this.saveDialog.Filter = "Robot Image|*.rim";
            this.saveDialog.Title = "Save NXT Image";
            // 
            // DownloadImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 144);
            this.Name = "DownloadImage";
            this.Text = "DownloadImage";
            this.title = "Create NXT Image";
            this.Border.ResumeLayout(false);
            this.Border.PerformLayout();
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Label Status;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.SaveFileDialog saveDialog;

    }
}