using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace MyId
{
    public partial class Edit : Form
    {
        public IdItem AIdItem = null;
        public Edit()
        {
            InitializeComponent();
        }

        private void Edit_Load(object sender, EventArgs e)
        {
            uxHint.Text = "";
            if (AIdItem != null)
            {
                uxSite.Text = AIdItem.Site;
                uxUser.Text = AIdItem.User;
                uxPassword.Text = AIdItem.Password;
                uxMemo.Text = AIdItem.Memo;
                uxHint.Text = AIdItem.PasswordTip;
                if (uxHint.Text != "")
                    uxHint.ForeColor = Color.Red;
            }
            else
                AIdItem = new IdItem();
        }

        private void UxOK_Click(object sender, EventArgs e)
        {
            AIdItem.Site = uxSite.Text;
            AIdItem.User = uxUser.Text;
            AIdItem.Password = uxPassword.Text;
            AIdItem.Memo = uxMemo.Text;
        }

        private void UxCopyPassword_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(uxPassword.Text);
        }

        private void UxCopyUser_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(uxUser.Text);
        }

        private void UxGenerate_Click(object sender, EventArgs e)
        {
            uxPassword.Text = GenerateRandomPassword();
            uxHint.Text = "New password generated";
            uxHint.ForeColor = Color.Green;

        }

        /// <summary>
        /// Generates a Random Password
        /// respecting the given strength requirements.
        /// </summary>
        /// <param name="opts">A valid PasswordOptions object
        /// containing the password strength requirements.</param>
        /// <returns>A random password</returns>
        public static string GenerateRandomPassword()
        {


            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$?_-"                        // non-alphanumeric
        };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            chars.Insert(rand.Next(0, chars.Count),
                randomChars[0][rand.Next(0, randomChars[0].Length)]);

            chars.Insert(rand.Next(0, chars.Count),
                randomChars[1][rand.Next(0, randomChars[1].Length)]);

            chars.Insert(rand.Next(0, chars.Count),
                randomChars[2][rand.Next(0, randomChars[2].Length)]);

            chars.Insert(rand.Next(0, chars.Count),
                randomChars[3][rand.Next(0, randomChars[3].Length)]);

            int len = rand.Next(10, 20);
            for (int i = chars.Count; i < len; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        public Dictionary<string, string> TempFiles = new Dictionary<string, string>();

        private void UxImageAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    foreach (var inFile in openFileDialog1.FileNames)
                    {
                        //TODO add file

                        Image img;
                        try
                        {
                            img = Image.FromFile(inFile);
                        }
                        catch
                        {

                            img = WindowsThumbnailProvider.GetThumbnail(inFile, 64, 64, ThumbnailOptions.None);
                        }

                        if (img != null)
                        {
                            string idx = imageList1.Images.Count.ToString();
                            imageList1.Images.Add(idx, img);
                            TempFiles.Add(inFile, null);

                            uxImages.Items.Add(Path.GetFileName(inFile), idx);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UxImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            uxDelete.Enabled = uxImages.SelectedIndices.Count > 0;
            uxView.Enabled = uxDelete.Enabled;
        }

        private void UxDelete_Click(object sender, EventArgs e)
        {

            //TODO check this
            for (int i = uxImages.SelectedIndices.Count -1; i>=0;i--)
            {
                int idx = uxImages.SelectedIndices[i];
                TempFiles.Remove(TempFiles.ElementAt(idx).Key);
                uxImages.Items.RemoveAt(idx);
                imageList1.Images.RemoveAt(idx);
            }
        }

        private void UxView_Click(object sender, EventArgs e)
        {
            if (uxImages.SelectedIndices.Count >0)
            {
                var viewer = new ViewImage();
                var image = TempFiles.ElementAt(uxImages.SelectedIndices[0]).Value;
                string f = TempFiles.ElementAt(uxImages.SelectedIndices[0]).Key;
                Image img = null;
                if (image != null)
                {
                    try
                    {
                        img = Image.FromStream(new MemoryStream(Convert.FromBase64String(image)));
                        //image = Image.FromFile(f);
                    }
                    catch 
                    {
                        //image is not image type
                         f = Path.Combine(KnownFolders.DataDir, "enc." + f);
                        img = WindowsThumbnailProvider.GetThumbnail(  f, 64, 64, ThumbnailOptions.None);
                        
                    }
                }
                if (img != null)
                {
                    viewer.pictureBox1.Image = img;
                    viewer.EncFile = f;
                    viewer.PlainImage64String = image; // uxImages.SelectedItems[0].Text;
                    viewer.ShowDialog();
                }
                //image.Dispose();
                
            }
        }

        private void UxImages_DoubleClick(object sender, EventArgs e)
        {
            uxView.PerformClick();
        }

        //private void Edit_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    foreach (var item in TempFiles)
        //    {
        //        if (item.Value != null)
        //            item.Value.Dispose();
        //    }
        //}

        private void UxViewPass_Click(object sender, EventArgs e)
        {
            //uxPassword.PasswordChar = '\0';
            //uxPassword.PasswordChar = default;
            //uxPassword.UseSystemPasswordChar = !uxPassword.UseSystemPasswordChar;
        }

        private void UxViewPass_MouseDown(object sender, MouseEventArgs e)
        {
            uxPassword.UseSystemPasswordChar = false;
        }

        private void UxViewPass_MouseUp(object sender, MouseEventArgs e)
        {
            uxPassword.UseSystemPasswordChar = true;
        }

        private void UxViewPass_KeyDown(object sender, KeyEventArgs e)
        {
            uxPassword.UseSystemPasswordChar = false;
        }

        private void UxViewPass_KeyUp(object sender, KeyEventArgs e)
        {
            uxPassword.UseSystemPasswordChar = true;
        }

        private void UxGo_Click(object sender, EventArgs e)
        {
            string url = uxSite.Text;
            if (!url.StartsWith("http"))
                url = string.Format("http://{0}", url);
            System.Diagnostics.Process.Start(url);
        }
    }
}
