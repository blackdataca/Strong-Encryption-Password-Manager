// MIT License - Copyright (c) 2019 Black Data
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private void UxOk_Click(object sender, EventArgs e)
        {
            if (uxMasterPin.Text == "" )
            {
                MessageBox.Show("Please create a PIN", "Create data file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //if (uxSavePrivateKeyTo.Checked && string.IsNullOrWhiteSpace(uxPrivateKeyFile.Text))
            //{
            //    MessageBox.Show("Please specify private key file", "Backup key file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}

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
            //uxDataFilePath.Text = System.IO.Path.GetDirectoryName(IdFile);
            //ShowPrivateLocation();
            uxDataFile.Text = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), uxSaveDataFileDialog.FileName);
        }



        //private void uxSavePrivateKeyTo_CheckedChanged(object sender, EventArgs e)
        //{
        //    ShowPrivateLocation();
        //    if (!uxPrivateKeyFile.Enabled)
        //        uxPrivateKeyFile.Text = "";
        //    else if (string.IsNullOrWhiteSpace(uxPrivateKeyFile.Text))
        //    {
        //        uxPrivateKeyFile.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "myid_private.key"); 
        //    }
        //}

        //private void ShowPrivateLocation()
        //{
        //    uxPrivateKeyFile.Enabled = uxSavePrivateKeyTo.Checked;
        //    uxBrowsePrivateKey.Enabled = uxSavePrivateKeyTo.Checked;
        //}

        private void uxOther_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        //private void uxBrowsePrivateKey_Click(object sender, EventArgs e)
        //{
        //    string initFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "myid_private.key");
        //    if (!string.IsNullOrWhiteSpace(uxPrivateKeyFile.Text))
        //        initFile = uxPrivateKeyFile.Text;
        //    //if (!System.IO.Directory.Exists(initDir))
        //    //    initDir = KnownFolders.DataDir; // Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //    uxSavePrivateKeyFile.InitialDirectory = Path.GetDirectoryName(initFile);


        //    if (uxSavePrivateKeyFile.ShowDialog() == DialogResult.OK)
        //    {
        //        uxPrivateKeyFile.Text = uxSavePrivateKeyFile.FileName;
        //    }
        //}

        private void uxBrowseDataFile_Click(object sender, EventArgs e)
        {
            string initFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), uxSaveDataFileDialog.FileName);
            if (!string.IsNullOrWhiteSpace(uxDataFile.Text))
                initFile = uxDataFile.Text;

            uxSaveDataFileDialog.InitialDirectory = Path.GetDirectoryName(initFile);


            if (uxSaveDataFileDialog.ShowDialog() == DialogResult.OK)
            {
                uxDataFile.Text = uxSaveDataFileDialog.FileName;
            }
        }
    }
}
