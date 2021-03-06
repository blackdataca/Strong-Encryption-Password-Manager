﻿// MIT License - Copyright (c) 2019 Black Data
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using System.Drawing.Printing;

namespace MyId
{
    public partial class MainForm : Form, IMessageFilter
    {
        private List<IdItem> _idList = new List<IdItem>();
        private ListViewColumnSorter lvwColumnSorter;
        private DateTime _wentIdle = DateTime.Now;

        public MainForm()
        {
            InitializeComponent();
            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwColumnSorter = new ListViewColumnSorter();
            this.uxList.ListViewItemSorter = lvwColumnSorter;

            // watch for idle events and any message that might break idle
            //Application.Idle += new EventHandler(Application_OnIdle);
            Application.AddMessageFilter(this);
        }

        //private void Application_OnIdle(object sender, EventArgs e)
        //{
        //    // keep track of the last time we went idle
        //    _wentIdle = DateTime.Now;
        //}

        private byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            for (int i = 0; i < 10; i++)
            {
                // Fille the buffer with the generated data
                rng.GetBytes(data);
            }

            return data;
        }

        private MemoryStream DecryptFileStream(string encFile)
        {
            MemoryStream ms = null;

            string encFileNameOnly = Path.GetFileName(encFile);
            encFile = Path.Combine(KnownFolders.DataDir, encFileNameOnly);

            if (File.Exists(encFile))
            {
                ms = new MemoryStream();

                using (RijndaelManaged myRijndael = new RijndaelManaged())
                {
                    myRijndael.KeySize = 256;
                    myRijndael.BlockSize = 128;
                    myRijndael.Padding = PaddingMode.PKCS7;
                    //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                    myRijndael.Mode = CipherMode.CFB;

                    //http://stackoverflow.com/questions/2659214/why-do-i-need-to-use-the-rfc2898derivebytes-class-in-net-instead-of-directly
                    //"What it does is repeatedly hash the user password along with the salt." High iteration counts.
                    //var key = new Rfc2898DeriveBytes(GetKeyIv("Key"), GetKeyIv("IV"), 50000);
                    myRijndael.Key = GetKeyIv("RiKey");// key.GetBytes(myRijndael.KeySize / 8);
                    myRijndael.IV = GetKeyIv("RiIv");// key.GetBytes(myRijndael.BlockSize / 8);

                    using (var fsCrypt = new FileStream(encFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {

                        byte[] salt = new byte[32];
                        fsCrypt.Read(salt, 0, salt.Length);

                        using (var cryptoStream = new CryptoStream(fsCrypt, myRijndael.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            //create a buffer (1mb) so only this amount will allocate in the memory and not the whole file
                            byte[] buffer = new byte[1048576];
                            int read;
                            while ((read =
                                cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, read);
                                Thread.Sleep(1);
                            }

                            cryptoStream.Close();

                        }
                        fsCrypt.Close();
                    }
                }

                ms.Seek(0, SeekOrigin.Begin);

#if DEBUG
                /*
                //for debug
                string debugFile = Path.Combine(p, string.Format("MyId{0}dec.{1}", Path.DirectorySeparatorChar, encFileNameOnly));

                using (var fs = new FileStream(debugFile, FileMode.Create, FileAccess.Write))
                {                   
                    ms.WriteTo(fs);
                }
                */
#endif
            }
            return ms;
        }

        private string EncryptFile(string imageFile)
        {
            if (!File.Exists(imageFile))
                return null;

            string ext = Path.GetExtension(imageFile);
            string encFileNameOnly = string.Format("enc.{0}{1}", Guid.NewGuid(), ext);
            string encFile = Path.Combine(KnownFolders.DataDir, encFileNameOnly);

            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                myRijndael.KeySize = 256;
                myRijndael.BlockSize = 128;
                myRijndael.Padding = PaddingMode.PKCS7;
                //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                myRijndael.Mode = CipherMode.CFB;

                //http://stackoverflow.com/questions/2659214/why-do-i-need-to-use-the-rfc2898derivebytes-class-in-net-instead-of-directly
                //"What it does is repeatedly hash the user password along with the salt." High iteration counts.
                //var key = new Rfc2898DeriveBytes(GetKeyIv("Key"), GetKeyIv("IV"), 50000);
                myRijndael.Key = GetKeyIv("RiKey");// key.GetBytes(myRijndael.KeySize / 8);
                myRijndael.IV = GetKeyIv("RiIv");// key.GetBytes(myRijndael.BlockSize / 8);

                using (var fsPlain = new FileStream(imageFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var fsCrypt = new FileStream(encFile, FileMode.Create, FileAccess.Write))
                    {

                        //generate random salt
                        byte[] salt = GenerateRandomSalt();
                        fsCrypt.Write(salt, 0, salt.Length);

                        using (var cryptoStream = new CryptoStream(fsCrypt, myRijndael.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            //create a buffer (1mb) so only this amount will allocate in the memory and not the whole file
                            byte[] buffer = new byte[1048576];
                            int read;
                            while ((read = fsPlain.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                cryptoStream.Write(buffer, 0, read);
                                Thread.Sleep(1);
                            }

                            cryptoStream.Close();

                        }
                        fsCrypt.Close();
                    }
                    fsPlain.Close();
                }
            }
            //for (int i = 0; i < 5; i++)
            //{
            try
            {
                File.Delete(imageFile);
                //break;
            }
            catch (IOException ex)
            {
                MessageBox.Show(string.Format("Error delete file {0}: {1}", imageFile, ex.Message), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Thread.Sleep(1000);
            //}
            return encFileNameOnly;
        }

        private void UxEdit_Click(object sender, EventArgs e)
        {
            if (uxList.SelectedItems.Count > 0)
            {
                using (Edit edit = new Edit())
                {
                    ListViewItem li = uxList.SelectedItems[0];
                    IdItem aItem = GetAItem((string)li.Tag);
                    edit.AIdItem = aItem;
                    string oldPass = aItem.Password;

                    //var t = new Thread(() => { 
                    //    Thread.CurrentThread.IsBackground = true;
                    if (aItem.Images != null)
                    {

                        Cursor.Current = Cursors.WaitCursor;
                        foreach (var encFile in aItem.Images)
                        {
                            var st = DecryptFileStream(encFile.Key);
                            if (st != null)
                            {
                                Image img = null;
                                try
                                {
                                    img = Image.FromStream(st);
                                }
                                catch (Exception ex)
                                {
                                    string f = Path.Combine(KnownFolders.DataDir, encFile.Key);

                                    img = WindowsThumbnailProvider.GetThumbnail(f, 64, 64, ThumbnailOptions.None);
                                }


                                if (img != null)
                                {
                                    string idx = edit.imageList1.Images.Count.ToString();

                                    edit.imageList1.Images.Add(idx, img);

                                    edit.uxImages.Items.Add(encFile.Value, idx);

                                    edit.TempImages.Add(encFile.Key, img);
                                }
                            }

                        }
                        Cursor.Current = Cursors.Default;

                    }
                    //});
                    //t.Start();
                    //t.Join();


                    if (edit.ShowDialog(this) == DialogResult.OK)
                    {

                        if (aItem.Images != null)
                        {

                            Cursor.Current = Cursors.WaitCursor;

                            for (int i = 0; i < edit.TempImages.Count; i++)
                            {
                                var imgFile = edit.TempImages.ElementAt(i).Key;
                                if (!imgFile.StartsWith("enc."))
                                {
                                    //new file
                                    string encImg = EncryptFile(imgFile);
                                    edit.TempImages.Remove(edit.TempImages.ElementAt(i).Key);
                                    edit.TempImages.Add(encImg, null);

                                    aItem.Images.Add(encImg, Path.GetFileName(imgFile));
                                }
                            }
                            for (int i = aItem.Images.Count - 1; i >= 0; i--)
                            //foreach (var encFile in aItem.Images.Keys)
                            {
                                var encFile = aItem.Images.ElementAt(i).Key;
                                //string encFile = aItem.Images[i];
                                if (!edit.TempImages.Keys.Contains(encFile))
                                {

                                    string pf = Path.Combine(KnownFolders.DataDir, encFile);
                                    if (File.Exists(pf))
                                    {
                                        File.Delete(pf);
                                    }
                                    aItem.Images.Remove(encFile);
                                }
                            }
                            Cursor.Current = Cursors.Default;
                        }

                        if (oldPass != aItem.Password)
                            aItem.Changed = DateTime.Now;

                        li.Text = aItem.Site;
                        li.SubItems[1].Text = aItem.User;
                        li.SubItems[2].Text = ShowHint(li, aItem);
                        li.SubItems[3].Text = aItem.ChangedHuman;
                        li.SubItems[4].Text = aItem.Memo;
                        SaveToDisk();
                    }
                }
            }
        }
        private void UxNew_Click(object sender, EventArgs e)
        {
            using (Edit edit = new Edit())
            {
                if (edit.ShowDialog(this) == DialogResult.OK)
                {

                    Cursor.Current = Cursors.WaitCursor;
                    foreach (var img in edit.TempImages.Keys)
                    {
                        string encFile = EncryptFile(img);

                        edit.AIdItem.Images.Add(encFile, Path.GetFileName(img));

                    }
                    Cursor.Current = Cursors.Default;

                    AddListItem(edit.AIdItem);
                    _idList.Add(edit.AIdItem);
                    SaveToDisk();
                    ShowNumberOfItems();
                }
            }
        }

        private void AddListItem(IdItem idItem)
        {

            var item = new ListViewItem(new string[] { idItem.Site, idItem.User, "*****", idItem.ChangedHuman, idItem.Memo });
            ListViewItem li = uxList.Items.Add(item);
            li.Tag = idItem.Uid.ToString();
            li.SubItems[2].Text = ShowHint(li, idItem);
        }

        private string ShowHint(ListViewItem li, IdItem idItem)
        {
            if (!string.IsNullOrEmpty(idItem.PasswordTip))
            {
                li.ForeColor = Color.Red;
                li.ToolTipText = idItem.PasswordTip;
                return idItem.PasswordTip;
            }
            else
            {
                li.ForeColor = default;
                li.ToolTipText = "";
                return "*****";
            }
        }
        private IdItem GetAItem(string uid)
        {
            foreach (var item in _idList)
            {
                if (item.Uid.ToString() == uid)
                    return item;
            }
            return null;
        }
        private void UxList_DoubleClick(object sender, EventArgs e)
        {
            uxEdit.PerformClick();
        }


        private void CreateNewFile()
        {
            if (!Directory.Exists(Path.GetDirectoryName(IdFile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(IdFile));
            }
            if (File.Exists(IdFile))
            {
                MessageBox.Show("Data file already exists: " + IdFile, "Data file already exists", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
            }
            var si = new CreateNewMaster();
            if (si.ShowDialog() == DialogResult.OK)
            {
                string masterPass = si.uxMasterPassword.Text;


                using (RijndaelManaged myRijndael = new RijndaelManaged())
                {
                    myRijndael.GenerateIV();
                    SaveKeyIv("IV", myRijndael.IV);

                    //SaveKeyIv("IV", GenerateRandomSalt());
                    SaveKeyIv("Key", Encoding.Unicode.GetBytes(masterPass));

                    var key = new Rfc2898DeriveBytes(GetKeyIv("Key"), GenerateRandomSalt(), 50000);
                    SaveKeyIv("RiKey", key.GetBytes(myRijndael.KeySize / 8));
                    SaveKeyIv("RiIv", key.GetBytes(myRijndael.BlockSize / 8));

                    //myRijndael.Key = GetKeyIv("RiKey");// key.GetBytes(myRijndael.KeySize / 8);
                    //myRijndael.IV = GetKeyIv("RiIv");// key.GetBytes(myRijndael.BlockSize / 8);

                }




                SaveToDisk();
            }
            else
            {
                Application.Exit();
            }
        }

        private void SaveToDisk()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                myRijndael.KeySize = 256;
                myRijndael.BlockSize = 128;
                myRijndael.Padding = PaddingMode.PKCS7;
                //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                myRijndael.Mode = CipherMode.CFB;

                // var key = new Rfc2898DeriveBytes(GetKeyIv("Key"), GetKeyIv("IV"), 50000);
                //myRijndael.Key = key.GetBytes(myRijndael.KeySize / 8);
                //myRijndael.IV = key.GetBytes(myRijndael.BlockSize / 8);

                myRijndael.Key = GetKeyIv("RiKey");// key.GetBytes(myRijndael.KeySize / 8);
                myRijndael.IV = GetKeyIv("RiIv");// key.GetBytes(myRijndael.BlockSize / 8);

                //myRijndael.Key = GetKeyIv("Key");
                //myRijndael.IV = GetKeyIv("IV");

                using (var fs = new FileStream(IdFile, FileMode.Create, FileAccess.Write))
                {

                    using (var cryptoStream = new CryptoStream(fs, myRijndael.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        formatter.Serialize(cryptoStream, _idList);
                    }
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tryKey">if tryKey is not null, do a dry run to test the key</param>
        /// <returns></returns>
        private bool LoadFromDisk(string tryKey = null)
        {
            if (tryKey == null)
                uxList.Items.Clear();
            bool success = false;

            try
            {
                using (var fs = new FileStream(IdFile, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (RijndaelManaged myRijndael = new RijndaelManaged())
                    {
                        myRijndael.KeySize = 256;
                        myRijndael.BlockSize = 128;
                        myRijndael.Padding = PaddingMode.PKCS7;
                        //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                        myRijndael.Mode = CipherMode.CFB;

                        //myRijndael.IV = GetKeyIv("IV");
                        byte[] keyBytes;
                        if (tryKey == null)
                            keyBytes = GetKeyIv("Key");
                        else
                        {
                            using (SHA256 mySHA256 = SHA256.Create())
                            {
                                byte[] keyB = Encoding.Unicode.GetBytes(tryKey);
                                keyBytes = mySHA256.ComputeHash(keyB);
                            }
                            byte[] savedKey = GetKeyIv("Key");
                            if (!keyBytes.SequenceEqual(savedKey))
                            {
                                MessageBox.Show("Invalid password!");
                                return false;
                            }
                        }

                        if (GetKeyIv("RiKey") == null || GetKeyIv("RiIv") == null)
                        {
                            var key = new Rfc2898DeriveBytes(keyBytes, GetKeyIv("IV"), 50000);
                            myRijndael.Key = key.GetBytes(32); // myRijndael.KeySize / 8);
                            myRijndael.IV = key.GetBytes(16); // myRijndael.BlockSize / 8);

                            SaveKeyIv("RiKey", myRijndael.Key);
                            SaveKeyIv("RiIv", myRijndael.IV);

                        }
                        else
                        {
                            //var key = new Rfc2898DeriveBytes(keyBytes, GetKeyIv("IV"), 50000);
                            myRijndael.Key = GetKeyIv("RiKey");// key.GetBytes(myRijndael.KeySize / 8);
                            myRijndael.IV = GetKeyIv("RiIv");// key.GetBytes(myRijndael.BlockSize / 8);
                        }

                        using (var cryptoStream = new CryptoStream(fs, myRijndael.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            if (tryKey == null)
                                _idList = (List<IdItem>)formatter.Deserialize(cryptoStream);
                            else
                                formatter.Deserialize(cryptoStream);
                        }
                    }
                }
                if (tryKey == null)
                {
                    foreach (var idItem in _idList)
                    {
                        AddListItem(idItem);
                    }
                    int col = (int)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "SortColumn", -1);
                    if (col != -1)
                    {
                        lvwColumnSorter.SortColumn = (int)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "SortColumn", lvwColumnSorter.SortColumn);
                        int or = (int)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "SortOrder", (int)lvwColumnSorter.Order);
                        lvwColumnSorter.Order = (SortOrder)or;
                        // Perform the sort with these new sort options.
                        uxList.Sort();
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("Access denied: " + ex.ToString(), "Unlock MyId", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#else
                MessageBox.Show("Access denied", "Unlock MyId", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
            }
            if (tryKey == null)
            {
                ShowNumberOfItems();
            }
            return success;
        }

        private void ShowNumberOfItems()
        {
            if (uxList.Items.Count < 2)
                uxItemCountStatus.Text = string.Format("{0} item", uxList.Items.Count);
            else
                uxItemCountStatus.Text = string.Format("{0} items", uxList.Items.Count);
        }

        private string IdFile
        {
            get
            {

                string p = Path.Combine(KnownFolders.DataDir, "myid_secret.data");
                return p;
            }
        }

        private bool ValidatePassword(string pass)
        {
            if (GetKeyIv("RiKey") == null || GetKeyIv("RiIv") == null)
            {
                SaveKeyIv("Key", Encoding.Unicode.GetBytes(pass));
            }
            else
            {
                byte[] keyBytes;
                using (SHA256 mySHA256 = SHA256.Create())
                {
                    byte[] keyB = Encoding.Unicode.GetBytes(pass);
                    keyBytes = mySHA256.ComputeHash(keyB);
                }
                byte[] savedKey = GetKeyIv("Key");
                if (!keyBytes.SequenceEqual(savedKey))
                {
                    MessageBox.Show("Invalid password!");
                    return false;
                }

            }
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            uxExport.Visible = true;
            uxVersion.Text = Application.ProductVersion;
            if (File.Exists(IdFile))
            {

                bool success = false;
                for (int i = 0; i < 3; i++)
                {
                    SignIn si = new SignIn();

                    var result = si.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        byte[] iv = GetKeyIv("IV");
                        if (iv == null)
                        {
                            if (MessageBox.Show("Try re-import private key", "Something wrong!", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                            {
                                ImportPrivateKey();
                                //importPrivateKeyToolStripMenuItem.PerformClick();
                            }
                            else
                            {
                                //Application.Exit();
                                System.Environment.Exit(1);
                                return;
                            }
                        }

                        //byte[] mp = GetKeyIv("MasterPass");
                        //if (Encoding.Unicode.GetString(mp) == si.uxPassword.Text)
                        //SaveKeyIv("Key", Encoding.Unicode.GetBytes(si.uxPassword.Text));

                        if (ValidatePassword(si.uxPassword.Text))
                        {
                            if (LoadFromDisk())
                            {
                                //password match
                                success = true;
                                timer1.Enabled = true;
                                break;
                            }
                            else
                            {
                                if (MessageBox.Show("Try importing private key?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    ImportPrivateKey();
                                }
                            }
                        }
                        else
                            MessageBox.Show("Access denied", "Unlock MyID", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    }
                    else if (result == DialogResult.Yes) //Create new data file
                    {

                        CreateNewFile();
                        success = true;
                        break;
                    }
                    else if (result == DialogResult.No) //Open data file
                    {
                        success = OpenDataFile();
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                if (!success)
                {
                    //Application.Exit();
                    System.Environment.Exit(1);
                }
            }
            else
            {
                CreateNewFile();
            }
        }


        private byte[] GetKeyIv(string type)
        {
            switch (type)
            {
                case "IV": //16
                    {
                        byte[] iv32 = (byte[])Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "iv", null);
                        if (iv32 == null)
                            return null;
                        byte[] iv16 = new byte[16];
                        Array.Copy(iv32, iv16, 16);
                        return iv16;
                    }
                case "RiKey": //32
                case "RiIv": //16
                    {
                        byte[] data = (byte[])Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", type, null);
                        if (data == null)
                            return null;
                        return data;
                    }
                case "Key":
                    {
                        byte[] iv32 = GetKeyIv("IV");// (byte[])Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "iv", null);
                        byte[] ciphertext = (byte[])Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "key", null);

                        byte[] plaintext = ProtectedData.Unprotect(ciphertext, iv32, DataProtectionScope.CurrentUser);

                        return plaintext;

                        //using (SHA256 mySHA256 = SHA256.Create())
                        //{
                        //    return mySHA256.ComputeHash(plaintext);
                        //}
                    }
                default:
                    throw new Exception("error 191");
            }
        }
        private void SaveKeyIv(string type, byte[] value)
        {

            switch (type)
            {
                case "IV":
                    byte[] random = new byte[32];

                    //RNGCryptoServiceProvider is an implementation of a random number generator.
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    rng.GetBytes(random);
                    Array.Copy(value, random, 16);
                    Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "iv", random);
                    break;
                case "RiKey":
                case "RiIv":
                    Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", type, value);
                    break;
                case "Key":
                    byte[] keyBytes;
                    using (SHA256 mySHA256 = SHA256.Create())
                    {
                        byte[] keyB = value;
                        keyBytes = mySHA256.ComputeHash(keyB);
                    }
                    byte[] iv32 = GetKeyIv("IV");// (byte[])Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "iv", null);
                    byte[] ciphertext = ProtectedData.Protect(keyBytes, iv32, DataProtectionScope.CurrentUser);
                    Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "key", ciphertext);
                    break;
                default:
                    throw new Exception("error 237");

            }
        }

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string downloadsPath = KnownFolders.GetPath(KnownFolder.Downloads);
            downloadsPath = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "ImportPath", downloadsPath);

            var fd = new OpenFileDialog()
            {
                CheckFileExists = true,
                Filter = "Comma-separated values (.csv)|*.csv",
                Title = "Open import file each line has - site, uid, pwd, updated, notes",
                ReadOnlyChecked = true,
                InitialDirectory = downloadsPath
            };
            if (fd.ShowDialog() == DialogResult.OK)
            {

                Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "ImportPath", Path.GetDirectoryName(fd.FileName));
                var csvFile = fd.FileName;
                int noOfLines = 0;
                using (TextFieldParser parser = new TextFieldParser(csvFile))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        //dry run to count records                            
                        string[] fields = parser.ReadFields();
                        if (fields.Length > 4)
                        {
                            DateTime dummy;
                            if (noOfLines == 0 || fields[3] == "" || DateTime.TryParse(fields[3], out dummy))
                                noOfLines++;
                        }
                    }
                }
                if (noOfLines == 0)
                {
                    MessageBox.Show("Cannot find any record to import.", csvFile, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (MessageBox.Show(string.Format("Import {0} records?", noOfLines), csvFile, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    using (TextFieldParser parser = new TextFieldParser(csvFile))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        noOfLines = 0;
                        while (!parser.EndOfData)
                        {
                            //Process row
                            string[] fields = parser.ReadFields();
                            if (fields.Length > 4)
                            {
                                var item = new IdItem()
                                {
                                    Site = fields[0],
                                    User = fields[1],
                                    Password = fields[2]

                                };
                                if (noOfLines != 0 && fields[3] != "")
                                {
                                    item.Changed = DateTime.Parse(fields[3]);
                                }
                                for (int i = 4; i < fields.Length; i++)
                                {
                                    if (i != 4)
                                        item.Memo = item.Memo + Environment.NewLine;
                                    item.Memo = item.Memo + fields[i];
                                }
                                _idList.Add(item);
                                noOfLines++;
                            }
                        }
                        if (noOfLines > 0)
                        {
                            SaveToDisk();
                            UxSearchBox_TextChanged();
                        }
                    }

                    File.Delete(csvFile);

                    Cursor.Current = Cursors.Default;
                }
            }

        }



        private void UxList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void UxDelete_Click(object sender, EventArgs e)
        {
            List<string> removeList = new List<string>();

            foreach (ListViewItem eachItem in uxList.SelectedItems)
            {
                removeList.Add(eachItem.Tag.ToString());
            }

            if (MessageBox.Show(string.Format("Remove {0} items?", removeList.Count), "Delete record", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                int noOfItems = 0;
                for (int i = _idList.Count; i > 0; i--)
                {
                    IdItem eachItem = _idList[i - 1];
                    if (removeList.Contains(eachItem.Uid.ToString()))
                    {
                        _idList.Remove(eachItem);
                        noOfItems++;
                    }
                }
                if (noOfItems > 0)
                {
                    SaveToDisk();
                    UxSearchBox_TextChanged();
                    ShowNumberOfItems();
                }

            }
        }

        private void UxContextNew_Click(object sender, EventArgs e)
        {
            uxNew.PerformClick();
        }

        private void UxList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                uxEdit.PerformClick();
            }
            else if (e.KeyCode == Keys.A && e.Control)
            {
                foreach (ListViewItem item in uxList.Items)
                {
                    item.Selected = true;
                }
            }
        }

        private void UxList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "SortColumn", lvwColumnSorter.SortColumn);
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "SortOrder", (int)lvwColumnSorter.Order);
            // Perform the sort with these new sort options.
            uxList.Sort();
        }
        private void UxSearchBox_TextChanged()
        {
            string searchTerm = uxSearchBox.Text.Trim();
            if (searchTerm.Length == 0)
            {
                LoadFromDisk();
            }
            else
            {
                uxList.Items.Clear();
                foreach (var idItem in _idList)
                {
                    if (idItem.Site.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                        idItem.User.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                        idItem.Memo.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) >= 0
                        )
                    {
                        AddListItem(idItem);
                    }
                }
                uxItemCountStatus.Text = string.Format("{0} items found", uxList.Items.Count);
            }
        }

        private void UxSearchBox_TextChanged(object sender, EventArgs e)
        {
            UxSearchBox_TextChanged();
        }

        private void UxClear_Click(object sender, EventArgs e)
        {
            uxSearchBox.Text = "";
        }

        private void UxSearchBox_Click(object sender, EventArgs e)
        {

        }

        private void UxExport_Click(object sender, EventArgs e)
        {
            string downloadsPath = KnownFolders.GetPath(KnownFolder.Downloads);
            downloadsPath = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "ExportPath", downloadsPath);

            var fd = new SaveFileDialog()
            {
                FileName = "MyID Export.csv",
                Filter = "Comma-separated values .csv file|*.csv",
                Title = "Export all passwords - site, uid, pwd, updated, notes",
                InitialDirectory = downloadsPath
            };
            if (fd.ShowDialog() == DialogResult.OK)
            {
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "ExportPath", Path.GetDirectoryName(fd.FileName));

                if (File.Exists(fd.FileName))
                {
                    File.Delete(fd.FileName);
                }
                string fn = Path.GetFileNameWithoutExtension(fd.FileName);
                fn = Path.Combine(Path.GetDirectoryName(fd.FileName), fn + ".csv");
                if (File.Exists(fn))
                {
                    File.Delete(fn);
                }

                var sb = new StringBuilder();
                foreach (var item in _idList)
                {
                    sb.Append(CsvString(item.Site)).Append(",");
                    sb.Append(CsvString(item.User)).Append(",");
                    sb.Append(CsvString(item.Password)).Append(",");
                    sb.Append(CsvString(item.Changed.ToString("yyyy-MM-dd HH:mm:ss"))).Append(",");
                    sb.Append(CsvString(item.Memo));
                    sb.AppendLine();
                }
                File.WriteAllText(fn, sb.ToString());

                MessageBox.Show(string.Format("{0} records exported.", _idList.Count), fd.FileName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string CsvString(string data)
        {
            if (data.Contains("\""))
            {
                data = data.Replace("\"", "\"\"");
            }

            if (data.Contains(","))
            {
                data = String.Format("\"{0}\"", data);
            }

            else if (data.Contains("\n"))
            {
                data = String.Format("\"{0}\"", data);
            }
            return data;
        }

        private void UxExportPrivateKey_Click(object sender, EventArgs e)
        {
            string downloadsPath = KnownFolders.GetPath(KnownFolder.Downloads);
            downloadsPath = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "ExportKeyPath", downloadsPath);

            var fd = new SaveFileDialog()
            {
                FileName = "MyID Export.key",
                Filter = "Private .key file|*.key",
                Title = "Export private key (store this file separately)",
                InitialDirectory = downloadsPath
            };
            if (fd.ShowDialog() == DialogResult.OK)
            {
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "ExportKeyPath", Path.GetDirectoryName(fd.FileName));

                byte[] buffer = new byte[2 + 16 + 32 + 16];

                buffer[0] = 0x01; //major version #
                buffer[1] = 0x02; //minor version #

                GetKeyIv("IV").CopyTo(buffer, 2); //length 16
                GetKeyIv("RiKey").CopyTo(buffer, 16 + 2); //length 32
                GetKeyIv("RiIv").CopyTo(buffer, 48 + 2); //length 16
                try
                {
                    File.WriteAllText(fd.FileName, BitConverter.ToString(buffer).Replace("-", ","));
                    MessageBox.Show("Private key exported");
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "Failed to save private key", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void ImportPrivateKey()
        {

            string downloadsPath = KnownFolders.GetPath(KnownFolder.Downloads);
            downloadsPath = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "ImportKeyPath", downloadsPath);

            var fd = new OpenFileDialog()
            {
                CheckFileExists = true,
                Filter = "Private .key file|*.key",
                Title = "Import private key",
                ReadOnlyChecked = true,
                InitialDirectory = downloadsPath
            };
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(fd.FileName))
                {
                    Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "ImportKeyPath", Path.GetDirectoryName(fd.FileName));
                    string bufferS = File.ReadAllText(fd.FileName);

                    byte[] buffer = ToByteArray(bufferS.Replace(",", "").Trim());

                    if (buffer[0] == 0x01 && buffer[1] == 0x02)
                    {
                        SignIn si = new SignIn();

                        si.Text = "Restore Private Key";
                        si.label1.Text = "Enter new master password to encrypt restored private key:";

                        if (si.ShowDialog() != DialogResult.OK)
                            return;

                        byte[] iv = new byte[16];
                        Array.Copy(buffer, 2, iv, 0, 16);
                        SaveKeyIv("IV", iv);

                        byte[] riKey = new byte[32];
                        Array.Copy(buffer, 2 + 16, riKey, 0, 32);
                        SaveKeyIv("RiKey", riKey);

                        byte[] riIv = new byte[16];
                        Array.Copy(buffer, 2 + 48, riIv, 0, 16);
                        SaveKeyIv("RiIv", riIv);

                        SaveKeyIv("Key", Encoding.Unicode.GetBytes(si.uxPassword.Text));

                        MessageBox.Show("Private key imported");

                    }
                    else
                    {
                        MessageBox.Show("Unknown key file!", "Failed to restore key file", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
                else
                    MessageBox.Show("Key file not found: " + fd.FileName);
            }
        }
        private void ImportPrivateKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportPrivateKey();
            UxSearchBox_TextChanged();
        }
        private byte[] ToByteArray(String HexString)
        {
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }
            return bytes;
        }

        private void UxExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void UxToolDelete_Click(object sender, EventArgs e)
        {
            uxDelete.PerformClick();
        }

        private void UxToolEdit_Click(object sender, EventArgs e)
        {
            uxEdit.PerformClick();
        }

        private void UxChangeMasterPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var si = new SignIn();
            if (si.ShowDialog() == DialogResult.OK)
            {
                if (ValidatePassword(si.uxPassword.Text))
                {
                    //password match
                    if (CreateNewPass())
                        MessageBox.Show("Master password changed", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }
        private bool CreateNewPass()
        {
            var np = new CreateNewMaster();
            if (np.ShowDialog() == DialogResult.OK)
            {
                SaveKeyIv("Key", Encoding.Unicode.GetBytes(np.uxMasterPassword.Text));

                //SaveToDisk();

                return true;
            }
            else
                return false;
        }

        private void UxAbout_Click(object sender, EventArgs e)
        {
            var ab = new AboutBox1();
            ab.ShowDialog();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            // uint idle = GetIdleTimeMs();
            TimeSpan diff = DateTime.Now - _wentIdle;
            int timo = (int)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "IdleTimeout", 300);

            uxTimeout.Text = string.Format("Idle timeout {0}s", timo - (int)diff.TotalSeconds);
            if (diff.TotalSeconds > timo)
                Application.Exit();//close after 30s
        }



        private bool isUserInput(Message m)
        {
            // look for any message that was the result of user input
            if (m.Msg == 0x200) { return true; } // WM_MOUSEMOVE
            if (m.Msg == 0x020A) { return true; } // WM_MOUSEWHEEL
            if (m.Msg == 0x100) { return true; } // WM_KEYDOWN
            if (m.Msg == 0x101) { return true; } // WM_KEYUP

            // ... etc

            return false;
        }
        public bool PreFilterMessage(ref Message m)
        {
            // reset our last idle time if the message was user input
            if (isUserInput(m))
            {
                _wentIdle = DateTime.Now;
                //_wentIdle = DateTime.MaxValue;
                //_idleTicks = 0;

                //_statusLbl.Text = "We Are NOT idle!";
            }

            return false;
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uxNew.PerformClick();
        }

        //private void UxBackup_Click(object sender, EventArgs e)
        //{
        //    var backupForm = new BackupForm();
        //    if (backupForm.ShowDialog()== DialogResult.OK)
        //    {
        //        if (!Directory.Exists(backupForm.uxNewLocation.Text))
        //        {
        //            MessageBox.Show("Path not found: " + backupForm.uxNewLocation.Text);
        //            return;
        //        }

        //        if (KnownFolders.DataDir == backupForm.uxNewLocation.Text)
        //        {
        //            MessageBox.Show("Target path is the same as source");
        //            return;
        //        }

        //        Directory.Delete(backupForm.uxNewLocation.Text);
        //        Directory.Move(KnownFolders.DataDir, backupForm.uxNewLocation.Text);
        //        KnownFolders.DataDir = backupForm.uxNewLocation.Text;
        //        MessageBox.Show("Data moved to new directory: " + KnownFolders.DataDir);
        //    }

        //}

        private bool OpenDataFile()
        {
            folderBrowserDialog1.SelectedPath = KnownFolders.DataDir;
            folderBrowserDialog1.Description = "Current Data Folder: " + KnownFolders.DataDir;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                KnownFolders.DataDir = folderBrowserDialog1.SelectedPath;
                if (LoadFromDisk())
                {
                    CreateNewPass();
                    return true;
                }
                else
                {
                    MessageBox.Show("Failed to load data folder", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            return false;
        }
        private void UxOpenData_Click(object sender, EventArgs e)
        {
            OpenDataFile();
        }

        private void UxExplorer_Click(object sender, EventArgs e)
        {
            Process.Start(KnownFolders.DataDir);
        }

        private void MnuDelete_Click(object sender, EventArgs e)
        {
            uxDelete.PerformClick();
        }

        private void MnuEdit_Click(object sender, EventArgs e)
        {
            uxEdit.PerformClick();
        }

        private void uxNewData_Click(object sender, EventArgs e)
        {
            CreateNewFile();
        }

        private int printLineNo;
        private int page = 0;
        private void uxPrint_Click(object sender, EventArgs e)
        {
            //printDocument1.DefaultPageSettings.Landscape = true;

            if ( printDialog1.ShowDialog() == DialogResult.OK)
            {
                // this is were you take the printersettings from the printDialog

                printDocument1.PrinterSettings = printDialog1.PrinterSettings;
                printDocument1.Print();

            }
        }

        private string GetItemField(int itemIndex, int fieldIndex)
        {
            switch (fieldIndex)
            {
                case 0:
                    return (itemIndex + 1).ToString() ;
                case 1:
                    return _idList[itemIndex].Site;
                case 2:
                    return _idList[itemIndex].User;
                case 3:
                    return _idList[itemIndex].Password;
                case 4:
                    return _idList[itemIndex].Changed.ToString("yyyy-MM-dd HH:mm:ss");
                case 5:
                    return _idList[itemIndex].Memo;
                default:
                    throw new Exception("Unknown item");

            }
        }
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs ev)
        {
            //float linesPerPage = 0;
            
            
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            float bottomMargin = topMargin + ev.MarginBounds.Height;
            var printFont = new Font("Arial", 8);

            float fontHeight = printFont.GetHeight(ev.Graphics);


            ev.Graphics.DrawString(string.Format("Printed {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), printFont, Brushes.Black, leftMargin, topMargin - fontHeight);



            var recfs = new RectangleF[6];
            var fields = new string[] { "#", "Site","User","Password" ,"Changed","Memo" };

            float xPos = 0;
            for (int i = 0;i<6; i++)
            {
                float width;
                if (i == 0)
                {
                    var size = ev.Graphics.MeasureString(_idList.Count.ToString(), printFont);
                    width = size.Width;
                }
                //else if (i == 4)
                //{
                //    var size = ev.Graphics.MeasureString("2020-09-24 00:00:00", printFont);
                //    width = size.Width;
                //}
                else
                    width = (float)ev.MarginBounds.Width * 0.2f;
                recfs[i] = new RectangleF(leftMargin + xPos, topMargin, width , fontHeight);
                xPos += width;
                ev.Graphics.FillRectangle(Brushes.DarkGray, recfs[i]);
                ev.Graphics.DrawString(fields[i], printFont, Brushes.White, recfs[i]);
            }

            
            float yPos = fontHeight; //max. height of a line or bottom of last line
            float maxHeight = fontHeight;
            
            // Iterate over the file, printing each line.
            while ( printLineNo < _idList.Count)
            {
                
                var item = _idList[printLineNo];

                for (int i =0;i <6;i++)
                {
                    recfs[i].Offset(0, maxHeight);
                   
                }

                maxHeight = fontHeight;
                var sizefs = new SizeF[6];
                for (int i = 0; i < 6; i++)
                {
                    sizefs[i] = ev.Graphics.MeasureString(GetItemField(printLineNo,i), printFont, (int)recfs[i].Width);
                    if (maxHeight < sizefs[i].Height)
                        maxHeight = sizefs[i].Height;
                }
                
                if (yPos + topMargin + maxHeight > bottomMargin)
                    break;

                for (int i = 0; i < 6; i++)
                {
                    recfs[i].Height = maxHeight;
                    ev.Graphics.DrawRectangle(Pens.DarkGray, Rectangle.Round(recfs[i]));
                    ev.Graphics.DrawString(GetItemField(printLineNo, i), printFont, Brushes.Black, recfs[i]);
                }


                yPos += maxHeight;


                //sb.Append(CsvString(item.Site)).Append(",");
                //sb.Append(CsvString(item.User)).Append(",");
                //sb.Append(CsvString(item.Password)).Append(",");
                //sb.Append(CsvString(item.Changed.ToString("yyyy-MM-dd HH:mm:ss"))).Append(",");
                //sb.Append(CsvString(item.Memo));
                //sb.AppendLine();
                //string line = sb.ToString();

                //ev.Graphics.DrawString(line, printFont, Brushes.Black,
                //   leftMargin, yPos, new StringFormat());
                printLineNo++;
            }

            page++;
            ev.Graphics.DrawString(string.Format("Page {0}", page), printFont, Brushes.Black, leftMargin, bottomMargin + fontHeight);

            // If more lines exist, print another page.
            if (printLineNo < _idList.Count)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }

        private void printDocument1_BeginPrint(object sender, PrintEventArgs e)
        {
            printLineNo = 0;
            page = 0;
        }
    }

    
}
