using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            UxBrowsePrivateKey.Enabled = uxPriviateKeyOn.Checked;
            uxPrivateKeyPath.Enabled = uxPriviateKeyOn.Checked;
        }

        private void OpenDataFile_Load(object sender, EventArgs e)
        {
            uxDataFileDir.Text= Path.Combine(KnownFolders.DataDir, "myid_secret.data");
            
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


        private void UxBrowsePrivateKey_Click(object sender, EventArgs e)
        {
            string p = uxPrivateKeyPath.Text;
            if (string.IsNullOrWhiteSpace(p))
                p = uxDataFileDir.Text;

            openFileDialog1.InitialDirectory = Path.GetDirectoryName(p);
            openFileDialog1.FileName = p;
            openFileDialog1.Filter = "Key file|*.key|All files|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
            }
        }

        private void UxBrowseDataFile_Click(object sender, EventArgs e)
        {
            string p = uxDataFileDir.Text;
            if (string.IsNullOrWhiteSpace(p))
                p = Path.Combine(KnownFolders.DataDir, "myid_secret.data"); 

            openFileDialog1.InitialDirectory = Path.GetDirectoryName(p);
            openFileDialog1.FileName = p;
            openFileDialog1.Filter = "MyId data file|*.data|All files|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

            }
        }
    }
}
