using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyId
{
    public partial class CreateNewMaster : Form
    {
        public CreateNewMaster()
        {
            InitializeComponent();
        }

        private void UxOk_Click(object sender, EventArgs e)
        {
            if (uxMasterPassword.Text == "" )
            {
                MessageBox.Show("Please enter a new password", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (uxMasterPassword.Text == uxVerify.Text)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            return;
        }
    }
}
