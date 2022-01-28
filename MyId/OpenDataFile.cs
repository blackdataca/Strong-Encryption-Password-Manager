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
    public partial class OpenDataFile : Form
    {
        public OpenDataFile()
        {
            InitializeComponent();
        }

        private void uxPriviateKeyOn_CheckedChanged(object sender, EventArgs e)
        {
            ShowPrivateKey();
        }

        private void ShowPrivateKey()
        {
            uxBrowsePrivateKey.Enabled = uxPriviateKeyOn.Checked;
            uxPrivateKeyPath.Enabled = uxPriviateKeyOn.Checked;
        }

        private void OpenDataFile_Load(object sender, EventArgs e)
        {
            uxDataFileDir.Text= KnownFolders.DataDir;
            
            ShowPrivateKey();
        }

        private void uxOk_Click(object sender, EventArgs e)
        {
            if (uxPriviateKeyOn.Checked)
            {
                if (!System.IO.File.Exists(uxPrivateKeyPath.Text))
                {
                    MessageBox.Show("Unable to open private key file!");
                    return;
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
