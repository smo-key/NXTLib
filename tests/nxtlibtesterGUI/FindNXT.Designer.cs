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
            this.Border.SuspendLayout();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Border
            // 
            this.Border.Controls.Add(this.Search);
            this.Border.Controls.SetChildIndex(this.TopPanel, 0);
            this.Border.Controls.SetChildIndex(this.IconBox, 0);
            this.Border.Controls.SetChildIndex(this.Search, 0);
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
            this.ToolTips.SetToolTip(this.CloseForm, "Close");
            // 
            // Minimize
            // 
            this.Minimize.FlatAppearance.BorderSize = 0;
            this.Minimize.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Minimize.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.Minimize.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
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
            this.ToolTips.SetToolTip(this.Search, "Minimize");
            this.Search.UseVisualStyleBackColor = false;
            this.Search.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button_MouseDown);
            this.Search.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button_MouseUp);
            // 
            // FindNXT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 400);
            this.Name = "FindNXT";
            this.Text = "FindNXT";
            this.title = "Connect to NXT";
            this.Border.ResumeLayout(false);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Button Search;
    }
}