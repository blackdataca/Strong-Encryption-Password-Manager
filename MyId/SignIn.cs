using System;
using System.Windows.Forms;

namespace MyId;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public partial class SignIn : Form
{
    public SignIn()
    {
        InitializeComponent();
    }

    private void UxPassword_TextChanged(object sender, EventArgs e)
    {

    }

    private void uxCancel_Click(object sender, EventArgs e)
    {

    }

    private void uxNew_Click(object sender, EventArgs e)
    {

    }

    private void newDataFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Yes;
        Close();
    }

    private void openDataFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.No;
        Close();
    }

    private void SignIn_Load(object sender, EventArgs e)
    {

    }
}
