// MIT License - Copyright (c) 2019 Black Data
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyId
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }
        public string IdFile;
        private void UxOk_Click(object sender, EventArgs e)
        {
            if (uxMasterPin.Text == "" )
            {
                MessageBox.Show("Please create a PIN", "Create data file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (uxSavePrivateKeyTo.Checked && string.IsNullOrWhiteSpace(uxPrivateKeyPath.Text))
            {
                MessageBox.Show("Please specify private key file", "Create data file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (uxMasterPin.Text == uxVerify.Text)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            return;
        }

        private void CreateNewMaster_Load(object sender, EventArgs e)
        {
            this.Text += Application.ProductVersion;
            uxDataFilePath.Text = System.IO.Path.GetDirectoryName(IdFile);
            ShowPrivateLocation();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void uxSavePrivateKeyTo_CheckedChanged(object sender, EventArgs e)
        {
            ShowPrivateLocation();
        }

        private void ShowPrivateLocation()
        {
            uxPrivateKeyPath.Enabled = uxSavePrivateKeyTo.Checked;
            uxBrowsePrivateKey.Enabled = uxSavePrivateKeyTo.Checked;
        }

        private void uxOther_Click(object sender, EventArgs e)
        {
            OpenDataFile of = new OpenDataFile();
            of.ShowDialog();
        }

        private void uxBrowseDataFile_Click(object sender, EventArgs e)
        {
            uxDataFileFolderBrowser.SelectedPath = uxDataFilePath.Text;
            uxDataFileFolderBrowser.Description = "Create data files in folder";

            if (uxDataFileFolderBrowser.ShowDialog() == DialogResult.OK)
            {
                uxDataFilePath.Text = uxDataFileFolderBrowser.SelectedPath;
            }
        }

        private void uxBrowsePrivateKey_Click(object sender, EventArgs e)
        {
            string initDir = "";
            if (!string.IsNullOrWhiteSpace(uxPrivateKeyPath.Text))
                initDir = System.IO.Path.GetDirectoryName(uxPrivateKeyPath.Text);
            if (!System.IO.Directory.Exists(initDir))
                initDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            uxSavePrivateKeyFile.InitialDirectory = initDir;
            

            if (uxSavePrivateKeyFile.ShowDialog() == DialogResult.OK)
            {
                uxPrivateKeyPath.Text = uxSavePrivateKeyFile.FileName;
            }
        }
    }
}
