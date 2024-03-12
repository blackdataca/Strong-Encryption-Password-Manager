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
    public partial class ViewImage : Form
    {
        public ViewImage()
        {
            InitializeComponent();
        }

        public string EncFile;
        public string PlainImage64String;
        private void UxSaveAs_Click(object sender, EventArgs e)
        {
            string onlyFileName = Path.GetFileName(EncFile);
            if (onlyFileName.StartsWith("enc."))
                onlyFileName = onlyFileName.Substring(4);
            saveFileDialog1.FileName = onlyFileName;
            saveFileDialog1.InitialDirectory = Path.GetDirectoryName(EncFile);
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //string f = Path.GetFileName(plainFileName);
                //f = Path.Combine(KnownFolders.DataDir, f);
                Cursor.Current = Cursors.WaitCursor;
                File.WriteAllBytes(saveFileDialog1.FileName, Convert.FromBase64String(PlainImage64String));

                //if (Crypto.DecryptFile(f, saveFileDialog1.FileName))
                    MessageBox.Show("File saved: " + saveFileDialog1.FileName, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cursor.Current = Cursors.Default;
            }
        }

        private void ViewImage_Load(object sender, EventArgs e)
        {

        }
    }
}
