﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NXTLibTesterGUI
{
    public partial class BaseForm : Form
    {
        public BaseForm() : base()
        {
            InitializeComponent();
            ResizeBase();
            EnableActivation();
        }

        public bool activated { get; private set; }
        public string title
        {
            get
            {
                return Title.Text;
            }
            set
            {
                Title.Text = value;
            }
        }

        internal virtual void Control_Resize(object sender, EventArgs e)
        {
            ResizeBase();
        }

        private void ResizeBase()
        {
            TopPanel.Width = this.Width;
            Border.Size = this.Size;
            int curx = TopPanel.Width - 4;
            
            SortedDictionary<int, Button> sorted = new SortedDictionary<int, Button>();
            foreach (Button item in TopPanel.Controls.OfType<Button>())
            {
                sorted.Add(item.Location.X, item);
            }
            for (int i = sorted.Count() - 1; i > -1; i--)
            {
                int x = curx - (sorted.Values.ElementAt(i).Width);
                curx = x;
                Point p = new Point(x, sorted.Values.ElementAt(i).Location.Y);
                sorted.Values.ElementAt(i).Location = p;
            } 
            //Resizer.BringToFront();
        }

        internal virtual void CloseForm_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #region Drag
        private int locx = 0;
        private int locy = 0;
        private bool drag = false;
        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            //locx and locy = the mouse's position ON THE FORM
            locx = e.X;
            locy = e.Y;
            drag = true;
            BringToFront();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag == true)
            {
                if (e.Button == MouseButtons.Left)
                {
                    int l = e.X + this.Left - locx;
                    int t = e.Y + this.Top - locy;
                    this.Left = l;
                    this.Top = t;
                }
                else
                {
                    drag = false;
                }
            }
        }
        #endregion

        #region Activation
        internal void Activate_MouseDown(object sender, MouseEventArgs e)
        {
            ActivateForm();
        }
        public void EnableActivation()
        {
            Application.DoEvents();
            this.MouseDown += Activate_MouseDown;
            List<Control> controls = GetAll(this).ToList();
            foreach (Control item in controls)
            {
                item.MouseDown += Activate_MouseDown;
            }
        }
        private IEnumerable<Control> GetAll(Control control)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl))
                                      .Concat(controls);
        }
        public virtual void ActivateForm()
        {
            this.BringToFront();
            activated = true;
            ResizeBase();
        }
        public virtual void DeactivateForm()
        {
            activated = false;
            ResizeBase();
        }
        #endregion

        private void PopupBase_Activated(object sender, EventArgs e)
        {
            ActivateForm();
        }

        private void PopupBase_Deactivate(object sender, EventArgs e)
        {
            DeactivateForm();
        }

        public void Button_MouseDown(object sender, MouseEventArgs e)
        {
            Button s = (Button)sender;
            s.ForeColor = Color.White;
            s.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.HotTrack;
        }
        public void Button_MouseUp(object sender, MouseEventArgs e)
        {
            Button s = (Button)sender;
            s.ForeColor = Color.FromArgb(64, 64, 64);
            s.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
        }

        private void Minimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

    }
}
