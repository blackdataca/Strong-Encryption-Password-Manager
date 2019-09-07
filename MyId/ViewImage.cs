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
        public string PlainFileName;
        private void UxSaveAs_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = PlainFileName;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var crypto = new Crypto();
                string f = Path.GetFileName(EncFile);
                f = Path.Combine(KnownFolders.DataDir, f);
                Cursor.Current = Cursors.WaitCursor;
                if (crypto.DecryptFile(f, saveFileDialog1.FileName))
                    MessageBox.Show("File saved: " + saveFileDialog1.FileName, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
