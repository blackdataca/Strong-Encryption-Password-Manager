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
            ShowPrivateKey();
        }
    }
}
