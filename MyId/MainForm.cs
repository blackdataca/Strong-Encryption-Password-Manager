// MIT License - Copyright (c) 2019 Black Data
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using MyIdLibrary.Models;

namespace MyId;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public partial class MainForm : Form, IMessageFilter
{
    private List<IdItem> _idList = [];
    public List<IdItem> IdList { get => _idList; set => _idList = value; } //For Unit Test

    private ListViewColumnSorter lvwColumnSorter;
    private DateTime _wentIdle = DateTime.Now;

    /// <summary>
    /// Recovers this instance of the form.
    /// </summary>
    public void RestoreFromTray()
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(RestoreFromTray));
            return;
        }
        this.Visible = true;
        this.WindowState = FormWindowState.Normal;
        this.Activate();
    }

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

#if DEBUG
        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
#endif
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
                        Image img = null;
                        var st = Crypto.DecryptFileStream(encFile, _pinEnc);
                        if (st is null)
                        { //new format
                            var bytes = Convert.FromBase64String(encFile.Value);
                            st = new MemoryStream(bytes);
                            try
                            {
                                img = Image.FromStream(st);
                            }
                            catch
                            {
                                //bytes is not image type, create encrypted file on disk with "enc." + file name
                                string outputFileName = Crypto.EncryptFileStream(encFile.Key, st, _pinEnc);
                                string f = Path.Combine(KnownFolders.DataDir, outputFileName);
                                img = WindowsThumbnailProvider.GetThumbnail(f, 64, 64, ThumbnailOptions.None);
                            }
                            if (img != null)
                            {
                                string idx = edit.imageList1.Images.Count.ToString();

                                edit.imageList1.Images.Add(idx, img);

                                edit.uxImages.Items.Add(encFile.Key, idx);

                                edit.TempFiles.Add(encFile.Key, encFile.Value);
                            }
                        }
                        else
                        { //Old format

                            try
                            {
                                img = Image.FromStream(st);
                            }
                            catch
                            {
                                string f = Path.Combine(KnownFolders.DataDir, encFile.Key);

                                img = WindowsThumbnailProvider.GetThumbnail(f, 64, 64, ThumbnailOptions.None);
                            }


                            if (img != null)
                            {
                                string idx = edit.imageList1.Images.Count.ToString();

                                edit.imageList1.Images.Add(idx, img);


                                edit.uxImages.Items.Add(encFile.Value, idx);

                                using (var m = new MemoryStream())
                                {
                                    img.Save(m, img.RawFormat);
                                    byte[] imgBytes = m.ToArray();

                                    edit.TempFiles.Add(encFile.Key, Convert.ToBase64String(imgBytes));
                                }
                            }
                        }

                    }
                    Cursor.Current = Cursors.Default;

                }

                if (edit.ShowDialog(this) == DialogResult.OK)
                {
                    aItem.Images = edit.TempFiles;

                    if (oldPass != aItem.Password || li.Text != aItem.Site || li.SubItems[1].Text != aItem.User || li.SubItems[4].Text != aItem.Memo1Line)
                    {
                        aItem.Changed = DateTime.UtcNow;

                        li.Text = aItem.Site;
                        li.SubItems[1].Text = aItem.User;
                        li.SubItems[2].Text = ShowHint(li, aItem);
                        li.SubItems[3].Text = aItem.ChangedHuman;
                        li.SubItems[4].Text = aItem.Memo1Line;

                        SaveToDisk();
                    }
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

                //Cursor.Current = Cursors.WaitCursor;
                foreach (var img in edit.TempFiles.Keys)
                {
                    //string encFile = Crypto.EncryptFile(img);

                    //edit.AIdItem.Images.Add(encFile, Path.GetFileName(img));
                    edit.AIdItem.Images.Add(img, edit.TempFiles[img]);
                }
                //Cursor.Current = Cursors.Default;

                AddListItem(edit.AIdItem);
                _idList.Add(edit.AIdItem);
                SaveToDisk();
                ShowNumberOfItems();
            }
        }
    }

    private void AddListItem(IdItem idItem)
    {

        var item = new ListViewItem(new string[] { idItem.Site, idItem.User, "*****", idItem.ChangedHuman, idItem.Memo1Line });
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

    private IdItem GetAItemByRecId(string recId)
    {
        foreach (var item in _idList)
        {
            if (item.UniqId == recId)
                return item;
        }
        return null;
    }
    private void UxList_DoubleClick(object sender, EventArgs e)
    {
        uxEdit.PerformClick();
    }


    private bool CreateNewFile()
    {
        if (KnownFolders.DataFile != "")
        {
            if (MessageBox.Show("You will lose access to existing data file if private key is not backed up. Only click Yes if you are 100% sure private key has been backed up or you no longer need existing data file. Otherwise click No.", "Backup private key", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return false;
        }
        var si = new Welcome();
        for (var i = 0; i < 100; i++)
        {

            switch (si.ShowDialog())
            {
                case DialogResult.OK:
                    if (File.Exists(si.uxDataFile.Text))
                    {
                        MessageBox.Show($"Data file already exists: {si.uxDataFile.Text}", "Data file already exists", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    IdFile = si.uxDataFile.Text;
                    byte[] masterPin = Encoding.Unicode.GetBytes(si.uxMasterPin.Text);
                    _pinEnc = Crypto.SaveKeyIv("Pin", masterPin);
                    CreateNewKey(masterPin);
                    SaveToDisk();
                    return true;
                case DialogResult.Retry:
                    if (OpenDataFile())
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
            }
        }
        return false;
    }

    private void CreateNewKey(byte[] masterPin)
    {
        byte[] salt = Crypto.GenerateRandomBytes(32);
        Crypto.SaveKeyIv("Salt", salt);
        //var key = new Rfc2898DeriveBytes(masterPin, salt, 50000);

        //using (RijndaelManaged myRijndael = new RijndaelManaged())
        //{
        //    myRijndael.KeySize = 256;
        //    myRijndael.BlockSize = 128;
        //    myRijndael.Padding = PaddingMode.PKCS7;
        //    //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
        //    myRijndael.Mode = CipherMode.CFB;

        //    myRijndael.Key = key.GetBytes(32);//256 bits = 32 bytes

        //    myRijndael.GenerateIV();

        //    Crypto.SaveKeyIv("Iv2022", myRijndael.IV); //128 blocksize / 8 = 16

        //    //SaveKeyIv("Key", Encoding.Unicode.GetBytes(masterPin));
        //}

        using var aes = Aes.Create();
        aes.Key = SHA256.Create().ComputeHash(masterPin);
        aes.GenerateIV();
        Crypto.SaveKeyIv("Iv2022", aes.IV);

    }

    private void SaveToDisk(bool webSync = true)
    {
        var plain = JsonConvert.SerializeObject(_idList);
        using (var aes = Aes.Create())
        {
            byte[] pin = Crypto.GetKeyIv("Pin", _pinEnc);
            aes.Key = SHA256.Create().ComputeHash(pin); // key.GetBytes(32);
            aes.IV = Crypto.GetKeyIv("Iv2022");

            using (var fs = new FileStream(IdFile, FileMode.Create, FileAccess.Write))
            {
                //version 2024
                fs.WriteByte(0x20); //file version major
                fs.WriteByte(0x24); //file version minor
                using (var cryptoStream = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (var sWriter = new StreamWriter(cryptoStream))
                {

                    sWriter.Write(plain);
                }
                fs.Close();
            }
        }


        if (webSync)
            _ = WebSync(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    private bool LoadFromDisk(string pDataFile, string pPrivateKeyFile)
    {
        uxList.Items.Clear();

        bool success = false;

        try
        {
            using (var fs = new FileStream(pDataFile, FileMode.Open, FileAccess.Read))
            {
                int version = 0;
                if (fs.ReadByte() == 0x20)
                {
                    if (fs.ReadByte() == 0x24)
                        version = 2024;
                    else
                        version = 2022;
                }
                else
                    // Set the stream position to the beginning of the file.
                    fs.Seek(0, SeekOrigin.Begin);

                if (version == 2024)
                {
                    if (pPrivateKeyFile != null)
                    {
                        if (!LoadPrivateKey(pPrivateKeyFile))
                        {
                            MessageBox.Show("Unable load private key!");
                            return false;
                        }
                    }
                    using (var aes = Aes.Create())
                    {
                        byte[] pin = Crypto.GetKeyIv("Pin", _pinEnc);
                        aes.Key = SHA256.Create().ComputeHash(pin);  //key.GetBytes(32);
                        aes.IV = Crypto.GetKeyIv("Iv2022");
                        using (var cryptoStream = new CryptoStream(fs, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        using (var sReader = new StreamReader(cryptoStream))
                        {
                            try
                            {
                                string plain = sReader.ReadToEnd();
                                _idList = JsonConvert.DeserializeObject<List<IdItem>>(plain);

                            }
                            catch (System.Security.Cryptography.CryptographicException)
                            {
                                return false;
                            }

                        }
                    }
                }
                else
                {
#pragma warning disable SYSLIB0011
                    var formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011

#pragma warning disable SYSLIB0022
                    using (RijndaelManaged myRijndael = new RijndaelManaged())
#pragma warning restore SYSLIB0022
                    {

                        myRijndael.KeySize = 256;
                        myRijndael.BlockSize = 128;
                        myRijndael.Padding = PaddingMode.PKCS7;
                        //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                        myRijndael.Mode = CipherMode.CFB;


                        if (pPrivateKeyFile != null)
                        {
                            if (!LoadPrivateKey(pPrivateKeyFile))
                            {
                                MessageBox.Show("Unable load private key!");
                                return false;
                            }
                        }
                        if (version == 2022)
                        {
                            byte[] pin = Crypto.GetKeyIv("Pin", _pinEnc);
                            byte[] salt = Crypto.GetKeyIv("Salt");
#pragma warning disable SYSLIB0041 // Type or member is obsolete
                            var key = new Rfc2898DeriveBytes(pin, salt, 50000);
#pragma warning restore SYSLIB0041 // Type or member is obsolete
                            myRijndael.Key = key.GetBytes(32);
                            myRijndael.IV = Crypto.GetKeyIv("Iv2022");
                        }
                        else
                        {  //Old verion
                            byte[] keyBytes;
                            keyBytes = Crypto.GetKeyIv("Key");

                            if (Crypto.GetKeyIv("RiKey") == null || Crypto.GetKeyIv("RiIv") == null)
                            {
                                MessageBox.Show("Missing private key");
                            }
                            else
                            {

                                myRijndael.Key = Crypto.GetKeyIv("RiKey");
                                myRijndael.IV = Crypto.GetKeyIv("RiIv");
                            }
                        }

                        using (var cryptoStream = new CryptoStream(fs, myRijndael.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            try
                            {
                                _idList = (List<IdItem>)formatter.Deserialize(cryptoStream);

                            }
                            catch (System.Security.Cryptography.CryptographicException)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            //Upgrade or update data
            int uniqIdUpdate = 0;
            foreach (var item in _idList)
            {
                if (item.UniqId == null)
                {
                    item.UniqId = Crypto.UniqId("");
                    uniqIdUpdate++;
                }

                foreach (var encFile in item.Images.ToList())
                {
                    if (encFile.Key.StartsWith("enc."))
                    {
                        var st = Crypto.DecryptFileStream(encFile, _pinEnc);
                        if (st is null)
                        {
                            continue;
                        }
                        byte[] imageArray = st.ToArray();


                        string newKey = encFile.Value;
                        for (int i = 0; i < 100; i++)
                        {
                            if (item.Images.ContainsKey(newKey))
                            {
                                string ext = Path.GetExtension(newKey);
                                string nameNoExt = Path.GetFileNameWithoutExtension(newKey);
                                newKey = $"{nameNoExt}(1){ext}";
                            }
                            else
                                break;
                        }
                        if (!item.Images.ContainsKey(newKey))
                        {
                            item.Images.Remove(encFile.Key);

                            item.Images.Add(newKey, Convert.ToBase64String(imageArray));
                        }
                    }
                }
            }

            if (uniqIdUpdate > 0)
                SaveToDisk(false);



            foreach (var idItem in _idList)
            {
                if (ShowDeleted(idItem.Deleted))
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
            success = true;
        }
        catch (System.Security.Cryptography.CryptographicException)
        {
            MessageBox.Show("Failed to decrypt data. Invalid PIN.", "LoadFromDisk", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        catch (Exception ex)
        {
#if DEBUG
            MessageBox.Show("Access denied: " + ex.ToString(), "Unlock MyId", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#else
            MessageBox.Show("Access denied", "Unlock MyId", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
        }

        ShowNumberOfItems();
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
            if (KnownFolders.DataFile != "")
                return KnownFolders.DataFile;
            else
                return Path.Combine(KnownFolders.DataDir, "myid_secret.data");
        }
        set
        {
            KnownFolders.DataFile = value;
        }
    }

 

    private bool ValidatePassword(string pass)
    {
        byte[] masterPin = Encoding.Unicode.GetBytes(pass);
        _pinEnc = Crypto.SaveKeyIv("Pin", masterPin);
        if (Crypto.GetKeyIv("Salt") == null)
            CreateNewKey(masterPin);
        if (Crypto.GetKeyIv("Key") != null)
        {  //old version
            byte[] keyBytes;
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] keyB = Encoding.Unicode.GetBytes(pass);
                keyBytes = mySHA256.ComputeHash(keyB);
            }
            byte[] savedKey = Crypto.GetKeyIv("Key");
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
        toolStripStatusLabel2.Text = "©2019-" + DateTime.Now.Year.ToString() + " BlackData";
        uxExport.Visible = true;
        uxVersion.Text = Application.ProductVersion;
        if (File.Exists(IdFile))
        {

            bool success = false;
            for (int i = 0; i < 100; i++)
            {
                SignIn si = new SignIn();

                var result = si.ShowDialog();
                if (result == DialogResult.OK)
                {

                    if (ValidatePassword(si.uxPassword.Text))
                    {
                        if (LoadFromDisk(IdFile, null))
                        {
                            //password match
                            success = true;
                            timer1.Enabled = true;
                            _ = WebSync(false);
                            break;
                        }
                        else
                        {
                            MessageBox.Show("Access denied", "Unlock MyID", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        MessageBox.Show("Invalid PIN", "Unlock MyID", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                else if (result == DialogResult.Yes) //Create new data file
                {

                    if (CreateNewFile())
                    {
                        success = true;
                        break;
                    }
                }
                else if (result == DialogResult.No) //Open data file
                {
                    if (OpenDataFile())
                    {
                        success = true;
                        break;
                    }
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
            if (!CreateNewFile())
                System.Environment.Exit(1);  //First time app run
        }

        string webSyncSuccessSaved = Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "WebSyncSuccess", 0).ToString();
        uxWebSync.Enabled = webSyncSuccessSaved != "1";
    }

    private byte[] _pinEnc;






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
                    eachItem.Deleted = true;
                    eachItem.Changed = DateTime.UtcNow;
                    //_idList.Remove(eachItem);
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
            LoadFromDisk(IdFile, null);
        }
        else
        {
            uxList.Items.Clear();
            foreach (var idItem in _idList)
            {
                if ((
                    (idItem.Site != null && idItem.Site.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                    (idItem.User != null && idItem.User.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                    (idItem.Memo != null && idItem.Memo.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    ) && ShowDeleted(idItem.Deleted))
                {
                    AddListItem(idItem);
                }
            }
            uxItemCountStatus.Text = string.Format("{0} items found", uxList.Items.Count);
        }
    }

    private bool ShowDeleted(bool deleted)
    {
        return !deleted;
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
            FileName = "MyID Private Key Backup.key",
            Filter = "Private .key file|*.key",
            Title = "Export private key (store this file separately)",
            InitialDirectory = downloadsPath
        };
        if (fd.ShowDialog() == DialogResult.OK)
        {
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "ExportKeyPath", Path.GetDirectoryName(fd.FileName));
            SavePrivateKey(fd.FileName);

        }
    }

    private void SavePrivateKey(string fileName)
    {
        byte[] buffer = new byte[2 + 16 + 32 + 16];

        buffer[0] = 0x20; //major version #
        buffer[1] = 0x22; //minor version #


        byte[] random = new byte[16];
        //RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(random);
        random.CopyTo(buffer, 2);

        //GetKeyIv("IV").CopyTo(buffer, 2); //length 16
        Crypto.GetKeyIv("Salt").CopyTo(buffer, 16 + 2); //length 32
        Crypto.GetKeyIv("Iv2022").CopyTo(buffer, 18 + 32); //length 16
        try
        {
            File.WriteAllText(fileName, BitConverter.ToString(buffer).Replace("-", ","));
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "ImportKeyPath", Path.GetDirectoryName(fileName));
            MessageBox.Show("Private key exported");
        }
        catch (IOException ex)
        {
            MessageBox.Show(ex.Message, "Failed to save private key", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                SignIn si = new SignIn();

                si.Text = "Restore Private Key";
                si.label1.Text = "Enter new master password to encrypt restored private key:";

                if (si.ShowDialog() != DialogResult.OK)
                    return;


                Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "ImportKeyPath", Path.GetDirectoryName(fd.FileName));

                if (LoadPrivateKey(fd.FileName))
                {
                    _pinEnc = Crypto.SaveKeyIv("Pin", Encoding.Unicode.GetBytes(si.uxPassword.Text));
                    MessageBox.Show("Private key imported");

                }
                else
                    MessageBox.Show("Unknown key file!", "Failed to restore key file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Key file not found: " + fd.FileName);
        }
    }

    private bool LoadPrivateKey(string privateKeyFile)
    {
        string bufferS = File.ReadAllText(privateKeyFile);

        byte[] buffer = ToByteArray(bufferS.Replace(",", "").Trim());


        if (buffer[0] == 0x20 && buffer[1] == 0x22) //new version
        {
            int pos = 2;
            //byte[] iv = new byte[16];
            //Array.Copy(buffer, 2, iv, 0, 16);
            //SaveKeyIv("IV", iv);

            pos += 16;
            byte[] salt = new byte[32];
            Array.Copy(buffer, pos, salt, 0, 32);
            Crypto.SaveKeyIv("Salt", salt);

            pos += 32;
            byte[] riIv = new byte[16];
            Array.Copy(buffer, pos, riIv, 0, 16);
            Crypto.SaveKeyIv("Iv2022", riIv);

            return true;

        }
        else
        {
            MessageBox.Show("Invalid key file");
        }
        return false;
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
        var np = new Welcome();
        if (np.ShowDialog() == DialogResult.OK)
        {
            _pinEnc = Crypto.SaveKeyIv("Pin", Encoding.Unicode.GetBytes(np.uxMasterPin.Text));

            //SaveToDisk();

            return true;
        }
        else
            return false;
    }

    private void UxAbout_Click(object sender, EventArgs e)
    {
        using var ab = new AboutBox1();
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

        }

        return false;
    }

    private void NewToolStripMenuItem_Click(object sender, EventArgs e)
    {
        uxNew.PerformClick();
    }

    private bool OpenDataFile()
    {
        if (KnownFolders.DataFile != "")
        {
            if (MessageBox.Show("You will lose access to existing data file if private key is not backed up. Only click Yes if you are 100% sure private key has been backed up or you no longer need existing data file. Otherwise click No.", "Backup private key", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return false;
        }
        var openDataFileBox = new OpenDataFile();
        if (openDataFileBox.ShowDialog() == DialogResult.OK)
        {
            string privateKeyFile = null;
            if (openDataFileBox.uxPriviateKeyOn.Checked)
                privateKeyFile = openDataFileBox.uxPrivateKeyFile.Text;
            SignIn si = new SignIn();

            var result = si.ShowDialog();
            if (result == DialogResult.OK)
            {

                if (ValidatePassword(si.uxPassword.Text))
                {

                    if (LoadFromDisk(openDataFileBox.uxDataFile.Text, privateKeyFile))
                    {
                        KnownFolders.DataFile = openDataFileBox.uxDataFile.Text;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Failed to load data file", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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
        string dir = KnownFolders.DataDir;
        Process.Start("explorer.exe", dir);
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

        if (printDialog1.ShowDialog() == DialogResult.OK)
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
                return (itemIndex + 1).ToString();
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
        var fields = new string[] { "#", "Site", "User", "Password", "Changed", "Memo" };

        float xPos = 0;
        for (int i = 0; i < 6; i++)
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
            recfs[i] = new RectangleF(leftMargin + xPos, topMargin, width, fontHeight);
            xPos += width;
            ev.Graphics.FillRectangle(Brushes.DarkGray, recfs[i]);
            ev.Graphics.DrawString(fields[i], printFont, Brushes.White, recfs[i]);
        }


        float yPos = fontHeight; //max. height of a line or bottom of last line
        float maxHeight = fontHeight;

        // Iterate over the file, printing each line.
        while (printLineNo < _idList.Count)
        {

            var item = _idList[printLineNo];

            for (int i = 0; i < 6; i++)
            {
                recfs[i].Offset(0, maxHeight);

            }

            maxHeight = fontHeight;
            var sizefs = new SizeF[6];
            for (int i = 0; i < 6; i++)
            {
                sizefs[i] = ev.Graphics.MeasureString(GetItemField(printLineNo, i), printFont, (int)recfs[i].Width);
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

    private void ToolSyncVisual(int syncState)
    {
        switch (syncState)
        {
            case 0: //syncing
                Cursor.Current = Cursors.WaitCursor;
                uxWebSync.Enabled = false;
                uxWebSync.Text = "Syncing...";
                break;
            case 1: //Synced
                Cursor.Current = Cursors.Default;
                uxWebSync.Enabled = false;
                uxWebSync.Text = "Synced";
                break;
            default: //not synced
                Cursor.Current = Cursors.Default;
                uxWebSync.Enabled = true;
                uxWebSync.Text = "Sync";
                break;
        }
    }

    private async void uxToolSync_Click(object sender, EventArgs e)
    {
        ToolSyncVisual(0);
        string webSyncSuccessSaved = Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "WebSyncSuccess", 0).ToString();
        bool webSyncSuccess = (webSyncSuccessSaved == "1");
        while (true)
        {
            string userName = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "WebSyncUser", "");
            userName = userName.ToLower();

            string userPass = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "WebSyncPass", "");


            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPass) || !webSyncSuccess)
            {
                WebSync frm = new WebSync();
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    ToolSyncVisual(2);

                    return;
                }
                else
                    webSyncSuccess = true;
            }
            else
                break;
        }

        await WebSync(true);
    }

    private async Task WebSync(bool fromMemu)
    {
        ToolSyncVisual(0);
        string err = "";
        try
        {
            err = await SendWebSyncData(fromMemu);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "[WebSync]", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        ToolSyncVisual(err == "0" ? 1 : 2);
    }

 

    private async Task<string> SendSyncData(HttpClient client, MemoryStream compressedData, bool fromMenu)
    {
        string response = null;

        uxItemCountStatus.Text = $"Uploading {compressedData.Length:N0} bytes";
        Debug.WriteLine(uxItemCountStatus.Text);

        var start = new Stopwatch();
        start.Start();
        // Prepare the content to send in the request
        HttpContent httpContent = new StreamContent(compressedData);
        // Set the content type (replace "application/octet-stream" with the appropriate content type for your binary data)
        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        httpContent.Headers.ContentEncoding.Add("gzip");
        client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip");
        client.DefaultRequestHeaders
          .Accept
          .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

        // Send the POST request using PostAsync method
        HttpResponseMessage res = await client.PutAsync("https://localhost:7283/Sync", httpContent);

        // Check if the request was successful
        if (res.IsSuccessStatusCode)
        {
            // Read the response content as a string
            response = await res.Content.ReadAsStringAsync();

            uxItemCountStatus.Text = $"Received {response.Length:N0} bytes {start.ElapsedMilliseconds:N0} seconds: {response}";

        }
        else
        {
            // Handle unsuccessful response
            if (fromMenu)
                MessageBox.Show($"Error: {res.StatusCode} \n {res.Content.ReadAsStringAsync().Result}", "WebSync", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                uxItemCountStatus.Text = $"WebSync: {res.StatusCode}";
#if DEBUG
                File.WriteAllText("websync_error_dump.html", res.Content.ReadAsStringAsync().Result);
#endif
            }
        }
        return response;
    }

    private async Task<string> SendWebSyncData(bool fromMenu)
    {
        string userName = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "WebSyncUser", "");
        string userPass = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "WebSyncPass", "");
        string err = "";
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPass))
            return err;

        userName = userName.ToLower();

        try
        {
            using (var client = new HttpClient())
            {
                // Step 1: Authenticate
                string token = await GetTokenAsync(client, userName, userPass, fromMenu);
                if (string.IsNullOrWhiteSpace(token))
                    return "Not authorized";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Step 2: Send light weight client items
                string dataString = PrepareSyncData(null);
                string response = null;
                uxItemCountStatus.Text = $"Comprssing {dataString.Length:N0} bytes...";
                using (var compressedData = Crypto.CompressString(dataString))
                {
                    response = await SendSyncData(client, compressedData, fromMenu);
                }
                if (response == null)
                    return "No response";

                // Step 3: Update or Add newer items from server
                var newrClientItems = ParseSyncResponse(fromMenu, response);

                if (newrClientItems.Count == 0)
                    return "All update-to-date";

                // Step 4: Send full payload for newer client items 
                dataString = PrepareSyncData(newrClientItems);
                response = null;
                uxItemCountStatus.Text = $"Comprssing {dataString.Length:N0} bytes";
                using (var compressedData = Crypto.CompressString(dataString))
                {
                    response = await SendSyncData(client, compressedData, fromMenu);
                }
                if (response == null)
                    return "No response";

                ParseSyncResponse(fromMenu, response);

            }
        }
        catch (Exception ex)
        {
            if (fromMenu)
                MessageBox.Show(ex.Message, "WebSync", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                uxItemCountStatus.Text = ex.Message;
#if DEBUG
            File.WriteAllText("websync_exception_dump.html", ex.ToString());
#endif
        }
        finally
        {
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "WebSyncSuccess", err == "0" ? 1 : 0);
        }

        return err;
    }

    public string PrepareSyncData(List<string> newerClientItems)
    {
        var payloads = new List<object>();
        uxItemCountStatus.Text = $"Preparing sync data payload: {newerClientItems?.Count}...";

        foreach (var item in _idList)
        {
            var recId = item.UniqId;

            string json = "";

            if (newerClientItems != null)
            {
                if (newerClientItems.Contains(recId))
                    json = JsonConvert.SerializeObject(item, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                else
                    continue;
            }
            DateTime uTime = item.Changed;
            string sTime = uTime.ToString("yyyy-MM-dd HH:mm:ss");

            var rec = new { RecId = recId, LastUpdate = sTime, Payload = json };

            payloads.Add(rec);
        }
        uxItemCountStatus.Text = $"Found {payloads.Count:N0} record{(payloads.Count > 1 ? 's' : ' ')}...";

        var vm = new { payloads.Count, Payloads = payloads };

        var dataString = JsonConvert.SerializeObject(vm);
        return dataString;
    }

    private List<string> ParseSyncResponse(bool fromMenu, string response)
    {
        var ret = new List<string>();
        JObject joResponse = JObject.Parse(response);
        var err = joResponse["Error"]?.ToString();
        if (err == "0")
        {
            ProcessReturn(joResponse);
            ret = ProcessNewerClientItems(joResponse);
        }
        else
        {
            if (fromMenu)
                MessageBox.Show(err ?? "Invalid json", "WebSync", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                uxItemCountStatus.Text = "WebSync: " + err ?? "Invalid json";
        }
        Debug.WriteLine( $"Newer client items {ret.Count:N0}");
        return ret;
    }
    private List<string> ProcessNewerClientItems(JObject joResponse)
    {
        var ret = new List<string>();
        if (joResponse["Request"] != null)
        {
            foreach (var row in joResponse["Request"])
            {
                ret.Add(row["RecId"].ToString());
            }
        }
        return ret;
    }

    private void ProcessReturn(JObject joResponse)
    {
        int recNew = 0;
        int recUpdated = 0;
        if (joResponse["Return"] != null)
        {
            foreach (var row in joResponse["Return"])
            {
                var recId = row["RecordId"].ToString();
                string payload = row["Payload"].ToString();
                var item = JsonConvert.DeserializeObject<IdItem>(payload);

                IdItem aItem = GetAItemByRecId(recId);
                if (aItem == null)
                {   //new record from server
                    aItem = new IdItem();
                    aItem.UniqId = recId;
                    aItem.Uid = Guid.Parse(row["Id"].ToString());

                    _idList.Add(aItem);
                    recNew++;
                }
                else
                {  //updated records from app, upload files

                    recUpdated++;
                }
                aItem.Uid = Guid.Parse(row["Id"].ToString());
                aItem.User = item.User;
                aItem.Password = item.Password;
                aItem.Site = item.Site;
                aItem.Memo = item.Memo;
                aItem.Images = item.Images;
                aItem.Deleted = bool.Parse(row["Deleted"].ToString());
                string sTime = row["Modified"].ToString();
                DateTime dTime = DateTime.Parse(sTime);
                aItem.Changed = DateTime.SpecifyKind(dTime, DateTimeKind.Utc);

            }
            if (recNew > 0 || recUpdated > 0)
            {
                SaveToDisk(false);
                ShowNumberOfItems();
                UxSearchBox_TextChanged();
            }
        }
        uxItemCountStatus.Text = $"Added {recNew:N0} record{(recNew > 1 ? 's' : ' ')} Updated {recUpdated:N0} record{(recUpdated > 1 ? 's' : ' ')} ";
    }

    private async Task<string> GetTokenAsync(HttpClient client, string uid, string pwd, bool fromMemu)
    {
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var form = new Dictionary<string, string>()
        {
            { "email" , uid },
            { "password" , pwd }
        };
        var json = JsonConvert.SerializeObject(form);
        try
        {
            var body = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage tokenResponse = await client.PostAsync("https://localhost:7283/login", body);
            if (tokenResponse.IsSuccessStatusCode)
            {
                var jsonContent = await tokenResponse.Content.ReadAsStringAsync();
                var jobj = JsonConvert.DeserializeObject<JObject>(jsonContent);
                return jobj["accessToken"].ToString();
            }
            else
            {
                string msg = $"Token request failed: {tokenResponse.StatusCode} \n {tokenResponse.Content.ReadAsStringAsync().Result}";
                if (fromMemu)
                    MessageBox.Show(msg, "GetToken", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    uxItemCountStatus.Text = msg;


            }
        }
        catch (Exception ex)
        {
            if (fromMemu)
                MessageBox.Show(ex.Message, "GetToken", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                uxItemCountStatus.Text = "GetToken " + ex.Message;

        }
        return null;
    }
}
