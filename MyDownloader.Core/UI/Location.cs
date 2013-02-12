using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MyDownloader.Core;
using MyDownloader.Core.UI;

namespace MyDownloader.App.UI
{
    public partial class Location : UserControl
    {
        private bool hasSet = false;

        public Location()
        {
            InitializeComponent();

            Clear();
        }

        public event EventHandler UrlChanged;

        public string UrlLabelTitle
        {
            get
            {
                return lblURL.Text;
            }
            set
            {
                lblURL.Text = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ResourceLocation[] ResourceLocation
        {
            get 
            {
                string[] Urls = txtURL.Text.Replace("\r","").Split('\n');
                ResourceLocation[] ret=new ResourceLocation[Urls.Length];
                int cnt = 0;
                foreach(string u in Urls)
                {
                    if (u.Trim() != "")
                    {
                        ResourceLocation rl = new ResourceLocation();
                        rl.Authenticate = chkLogin.Checked;
                        rl.Login = txtLogin.Text;
                        rl.Password = txtPass.Text;
                        rl.URL = u;
                        ret[cnt] = rl;
                        cnt++;
                   }
                }
                return ret;
            }
            set
            {
                hasSet = true;

                if (value != null)
                {
                    chkLogin.Checked = value[0].Authenticate;
                    txtLogin.Text = value[0].Login;
                    txtPass.Text = value[0].Password;
                    txtURL.Text = value[0].URL;
                }
                else
                {
                    chkLogin.Checked = false;
                    txtLogin.Text = String.Empty;
                    txtPass.Text = String.Empty;
                    txtURL.Text = String.Empty;
                }
            }
        }

		public void Clear()
		{
            txtURL.Text = string.Empty;
            chkLogin.Checked = false;
            txtPass.Text = string.Empty;
            txtLogin.Text = string.Empty;
            UpdateUI();
		}

        private void chkLogin_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            lblLogin.Enabled = chkLogin.Checked;
            lblPass.Enabled = chkLogin.Checked;
            txtLogin.Enabled = chkLogin.Checked;
            txtPass.Enabled = chkLogin.Checked;
        }

        private void txtURL_TextChanged(object sender, EventArgs e)
        {
            if (UrlChanged != null)
            {
                UrlChanged(this, EventArgs.Empty);
            }
        }

        private void Location_Load(object sender, EventArgs e)
        {
            if (! hasSet)
            {
                txtURL.Text = ClipboardHelper.GetURLOnClipboard();
            }
        }

        public string Username
        {
            get { return this.txtLogin.Text; }
            set { chkLogin.Checked = true; this.txtLogin.Text = value; }
        }

        public string Password
        {
            get { return this.txtPass.Text; }
            set { chkLogin.Checked = true; this.txtPass.Text = value; }
        }
    }
}
