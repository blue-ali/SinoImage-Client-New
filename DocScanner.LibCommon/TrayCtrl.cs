using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.LibCommon
{
    public class TrayCtrl
    {
        // Fields
        private NotifyIcon _notifyicon;
        private Form _parent;
        public bool Enabled { get; set; }

        // Methods
        public TrayCtrl(Form parent, ContextMenu iconmenu = null)
        {
            this._parent = parent;
            this.Enabled = true;
            this._parent.SizeChanged += new EventHandler(this._parent_SizeChanged);
            this._notifyicon = new NotifyIcon();
            this._notifyicon.Icon = this._parent.Icon;
            this._notifyicon.MouseDoubleClick += new MouseEventHandler(this._notifyicon_MouseDoubleClick);
            this._notifyicon.ContextMenu = iconmenu ?? this.DefaultContextMenu;
            this._notifyicon.BalloonTipText = Process.GetCurrentProcess().ProcessName;
        }

        private void _notifyicon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this._parent.Visible = true;
            this._parent.WindowState = FormWindowState.Normal;
            this._parent.ShowInTaskbar = true;
            this._notifyicon.Visible = false;
        }

        private void _parent_SizeChanged(object sender, EventArgs e)
        {
            if ((this._parent.WindowState == FormWindowState.Minimized) && this.Enabled)
            {
                this._parent.ShowInTaskbar = false;
                this._parent.Hide();
                this._notifyicon.Visible = true;
            }
        }

        private void itemExit_Click(object sender, EventArgs e)
        {
            this._notifyicon.Visible = false;
            Process.GetCurrentProcess().Kill();
        }

        private void itemShow_Click(object sender, EventArgs e)
        {
            this._notifyicon_MouseDoubleClick(sender, null);
        }

        // Properties
        private ContextMenu DefaultContextMenu
        {
            get
            {
                ContextMenu menu = new ContextMenu();
                MenuItem item = new MenuItem
                {
                    Text = "退出"
                };
                item.Click += new EventHandler(this.itemExit_Click);
                MenuItem item2 = new MenuItem
                {
                    Text = "显示"
                };
                item2.Click += new EventHandler(this.itemShow_Click);
                menu.MenuItems.Add(item2);
                menu.MenuItems.Add(item);
                return menu;
            }
        }

        
    }

}
