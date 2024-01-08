using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyId
{
    public partial class WebSync : Form
    {
        public WebSync()
        {
            InitializeComponent();
        }

        private void WebSync_Load(object sender, EventArgs e)
        {
            
            uxEmail.Text = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "WebSyncUser", "");
            if (!string.IsNullOrEmpty(uxEmail.Text))
                uxPassword.Select();
        }

        private void uxOk_Click(object sender, EventArgs e)
        {
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "WebSyncUser", uxEmail.Text);

            //var userPassmd5 = MyEncryption.MyHash(uxPassword.Text);

            Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "WebSyncPass", uxPassword.Text);
        }
    }
}
