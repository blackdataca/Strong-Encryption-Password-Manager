﻿using System;
using System.IO;
using System.Windows.Forms;
using MyIdLibrary.Models;

namespace MyId;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
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
        uxPrivateKeyFile.Enabled = uxPriviateKeyOn.Checked;
    }

    private void OpenDataFile_Load(object sender, EventArgs e)
    {
        uxDataFile.Text= Path.Combine(KnownFolders.DataDir, "myid_secret.data");
        
        ShowPrivateKey();
    }

    private void uxOk_Click(object sender, EventArgs e)
    {
        if (uxPriviateKeyOn.Checked)
        {
            if (!System.IO.File.Exists(uxPrivateKeyFile.Text))
            {
                MessageBox.Show("Unable to open private key file!", "Open Data File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        if (!File.Exists(uxDataFile.Text))
        {
            MessageBox.Show("Data file not found: " + uxDataFile.Text, "Open Data File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        this.DialogResult = DialogResult.OK;
        this.Close();
    }


    private void UxBrowsePrivateKey_Click(object sender, EventArgs e)
    {
        string p = uxPrivateKeyFile.Text;
        if (string.IsNullOrWhiteSpace(p))
            p = uxDataFile.Text;

        openFileDialog1.InitialDirectory = Path.GetDirectoryName(p);
        openFileDialog1.FileName = p;
        openFileDialog1.Filter = "Key file|*.key|All files|*.*";
        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        {
            uxPrivateKeyFile.Text = openFileDialog1.FileName;
        }
    }

    private void UxBrowseDataFile_Click(object sender, EventArgs e)
    {
        string p = uxDataFile.Text;
        if (string.IsNullOrWhiteSpace(p))
            p = Path.Combine(KnownFolders.DataDir, "myid_secret.data"); 

        openFileDialog1.InitialDirectory = Path.GetDirectoryName(p);
        openFileDialog1.FileName = p;
        openFileDialog1.Filter = "MyId data file|*.data|All files|*.*";
        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        {
            uxDataFile.Text = openFileDialog1.FileName;
        }
    }
}
