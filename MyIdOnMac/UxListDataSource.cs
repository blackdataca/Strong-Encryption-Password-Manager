using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AppKit;
using Foundation;
//using JavaScriptCore;
//using Microsoft.Win32;
using Xamarin.Essentials;

namespace MyIdOnMac
{
	public class UxListDataSource: NSTableViewDataSource
    {
        public NSWindow wndHandle;
        private List<IdItem> _idList = new List<IdItem>();

        public UxListDataSource()
		{
		}

        public void Add(IdItem idItem)
        {
            _idList.Add(idItem);
        }

        public IdItem Get(nint index)
        {
            return _idList[(int)index];
        }

        public void Set(IdItem idItem)
        {
            for (int i = 0;i < _idList.Count; i++)
            {
                var item = _idList[i];
                if (item.Uid == idItem.Uid)
                    item = idItem;
            }
            
        }

        //public IdItem GetAItem(nint index)
        //{
        //    return _idList[(int)index];

        //    //TODO
        //    //foreach (var item in _idList)
        //    //{
        //    //    if (item.Uid.ToString() == uid)
        //    //        return item;
        //    //}
        //    //return null;
        //}

        public override nint GetRowCount(NSTableView tableView)
        {
            return _idList.Count;
        }

        public void Clear()
        {
            _idList.Clear();
        }

        public nint Search(string searchString)
        {
            //nint row = 0;
            for (nint row =0; row <_idList.Count; row++)// IdItem item in DataSource.Items)
            {
                IdItem item = _idList[(int)row];

                if (item.Site.Contains(searchString)
                    ||
                    item.User.Contains(searchString)
                    ||
                    item.Password.Contains(searchString)
                    ||
                    item.Memo.Contains(searchString)
                    )
                    return row;

                // Increment row counter
                //++row;
            }

            // If not found select the first row
            return 0;
        }

        public void Sort(string key, bool ascending)
        {

            // Take action based on key
            switch (key)
            {
                case "Site":
                    if (ascending)
                    {
                        _idList.Sort((x, y) => x.Site.CompareTo(y.Site));
                    }
                    else
                    {
                        _idList.Sort((x, y) => -1 * x.Site.CompareTo(y.Site));
                    }
                    break;
                case "User":
                    if (ascending)
                    {
                        _idList.Sort((x, y) => x.User.CompareTo(y.User));
                    }
                    else
                    {
                        _idList.Sort((x, y) => -1 * x.User.CompareTo(y.User));
                    }
                    break;
                
            }

        }

        public override void SortDescriptorsChanged(NSTableView tableView, NSSortDescriptor[] oldDescriptors)
        {
            // Sort the data
            if (oldDescriptors.Length > 0)
            {
                // Update sort
                Sort(oldDescriptors[0].Key, oldDescriptors[0].Ascending);
            }
            else
            {
                // Grab current descriptors and update sort
                NSSortDescriptor[] tbSort = tableView.SortDescriptors;
                Sort(tbSort[0].Key, tbSort[0].Ascending);
            }

            // Refresh table
            tableView.ReloadData();
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



        private byte[] GenerateRandomBytes(int size)
        {
            byte[] data = new byte[size];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            for (int i = 0; i < 10; i++)
            {
                // Fille the buffer with the generated data
                rng.GetBytes(data);
            }

            return data;
        }

        private void CreateNewKey(byte[] masterPin)
        {
            byte[] salt = GenerateRandomBytes(32);
            SaveKeyIv("Salt", salt);
            var key = new Rfc2898DeriveBytes(masterPin, salt, 50000);

            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                myRijndael.KeySize = 256;
                myRijndael.BlockSize = 128;
                myRijndael.Padding = PaddingMode.PKCS7;
                //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                myRijndael.Mode = CipherMode.CFB;

                myRijndael.Key = key.GetBytes(32);//256 bits = 32 bytes

                myRijndael.GenerateIV();

                SaveKeyIv("Iv2022", myRijndael.IV); //128 blocksize / 8 = 16

            }
        }
        public bool SaveToDisk(byte[] masterPin = null)
        {
            if (masterPin != null)
            {
                SaveKeyIv("Pin", masterPin);
                CreateNewKey(masterPin);
            }

            byte[] pin = GetKeyIv("Pin");
            if (pin == null)
            {
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Critical,
                    InformativeText = "No encrypted key found",
                    MessageText = "Unable to save data...",
                };
                alert.RunModal();
                return false;
            }
            var key = new Rfc2898DeriveBytes(pin, GetKeyIv("Salt"), 50000);

            BinaryFormatter formatter = new BinaryFormatter();
            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                myRijndael.KeySize = 256;
                myRijndael.BlockSize = 128;
                myRijndael.Padding = PaddingMode.PKCS7;
                //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                myRijndael.Mode = CipherMode.CFB;



                myRijndael.Key = key.GetBytes(32); 
                myRijndael.IV = GetKeyIv("Iv2022");

                using (var fs = new FileStream(IdFile, FileMode.Create, FileAccess.Write))
                {
                    //version 2022
                    fs.WriteByte(0x20); //file version major
                    fs.WriteByte(0x22); //file version minor
                    using (var cryptoStream = new CryptoStream(fs, myRijndael.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        formatter.Serialize(cryptoStream, _idList);
                    }
                    fs.Close();
                }
            }
            return true;
        }

        //private byte[] _pinEnc;

        private static byte[] Hex2Bin(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private static string Bin2Hex(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static byte[] GetKeyIv(string type)
        {
            switch (type)
            {
                case "IV": //16
                    //{
                    //    string hexString = Xamarin.Essentials.Preferences.Get("iv", "");
                    //    byte[] iv32 = Hex2Bin(hexString); // (byte[])Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "iv", null);
                    //    if (iv32 == null)
                    //        return null;
                    //    byte[] iv16 = new byte[16];
                    //    Array.Copy(iv32, iv16, 16);
                    //    return iv16;
                    //}
                case "Iv2022": //16
                case "RiKey": //32
                case "RiIv": //16
                case "Salt":
                    //{
                    //    string hexString = Xamarin.Essentials.Preferences.Get(type, "");
                    //    byte[] data = Hex2Bin(hexString);

                    //    //byte[] data = (byte[])Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", type, null);
                    //    return data;
                    //}
                case "Key":
                    //{
                    //    byte[] iv32 = GetKeyIv("IV");
                    //    string hexString = Xamarin.Essentials.Preferences.Get("key", "");
                    //    byte[] ciphertext = Hex2Bin(hexString);

                    //    //byte[] ciphertext = (byte[])Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "key", null);
                    //    if (ciphertext == null)
                    //        return null;
                    //    byte[] plaintext = Unprotect(ciphertext, iv32);

                    //    return plaintext;


                    //}
                case "Pin":
                    //if (_pinEnc == null)
                    //    return null;
                    byte[] ret = null;
                    try
                    {
                        ret = Unprotect(type);
                    }
                    catch
                    {
                    }

                    return ret;

                default:
                    throw new Exception($"Unknown type: {type}");
            }
        }

        private static void Protect(string key, byte[] userData)
        {
            string value = Bin2Hex(userData);
            SecureStorage.SetAsync(key, value).Wait();
           

            //byte[] encryptedData = userData;
            //return encryptedData;
        }

        private static byte[] Unprotect(string key)
        {
            string value = SecureStorage.GetAsync(key).Result;
            byte[] userData = Hex2Bin(value);
            return userData;
        }

        public static void SaveKeyIv(string type, byte[] value)
        {

            switch (type)
            {

                case "Iv2022": //16

                case "Salt": //32
                    //Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", type, value);
                    //string valueHex = Bin2Hex(value); // BitConverter.ToString(value).Replace("-", "");
                    //Xamarin.Essentials.Preferences.Set(type, valueHex);
                    
                    //break;

                case "Pin":
                    Protect(type, value);
                    break;
                default:
                    throw new Exception($"Unknown type: {type}");

            }
        }

        public bool LoadFromDisk(string pDataFile, string pPrivateKeyFile)
        {

            _idList.Clear();
            bool success = false;

            try
            {
                using (var fs = new FileStream(pDataFile, FileMode.Open, FileAccess.Read))
                {
                    int version = 0;
                    if (fs.ReadByte() == 0x20 && fs.ReadByte() == 0x22)
                        version = 2022;
                    else
                        // Set the stream position to the beginning of the file.
                        fs.Seek(0, SeekOrigin.Begin);

                    BinaryFormatter formatter = new BinaryFormatter();
                    using (RijndaelManaged myRijndael = new RijndaelManaged())
                    {
                        myRijndael.KeySize = 256;
                        myRijndael.BlockSize = 128;
                        myRijndael.Padding = PaddingMode.PKCS7;
                        //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                        myRijndael.Mode = CipherMode.CFB;

                        if (pPrivateKeyFile != null && pPrivateKeyFile != "<null>")
                        {
                            if (!LoadPrivateKey(pPrivateKeyFile))
                            {
                                var alert = new NSAlert()
                                {
                                    AlertStyle = NSAlertStyle.Warning,
                                    InformativeText = "Please choose the correct private key file and try again",
                                    MessageText = "Unable load private key file",
                                };
                                alert.BeginSheet(wndHandle);
                                
                                return false;
                            }
                        }
                        if (version == 2022)
                        {
                            byte[] pin = UxListDataSource.GetKeyIv("Pin");
                            byte[] salt = UxListDataSource.GetKeyIv("Salt");
                            var key = new Rfc2898DeriveBytes(pin, salt, 50000);
                            myRijndael.Key = key.GetBytes(32); // GetKeyIv("RiKey");// key.GetBytes(myRijndael.KeySize / 8);
                            myRijndael.IV = UxListDataSource.GetKeyIv("Iv2022");// key.GetBytes(myRijndael.BlockSize / 8);
                        }
                        else
                        {  //Old verion
                            byte[] keyBytes;
                            keyBytes = UxListDataSource.GetKeyIv("Key");

                            if (UxListDataSource.GetKeyIv("RiKey") == null || UxListDataSource.GetKeyIv("RiIv") == null)
                            {
                                var alert = new NSAlert()
                                {
                                    AlertStyle = NSAlertStyle.Warning,
                                    InformativeText = "Please import private key and try again",
                                    MessageText = "Missing private key",
                                };
                                alert.BeginSheet(wndHandle);
                            }
                            else
                            {
                                //var key = new Rfc2898DeriveBytes(keyBytes, GetKeyIv("IV"), 50000);
                                myRijndael.Key = UxListDataSource.GetKeyIv("RiKey");// key.GetBytes(myRijndael.KeySize / 8);
                                myRijndael.IV = UxListDataSource.GetKeyIv("RiIv");// key.GetBytes(myRijndael.BlockSize / 8);
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

                //foreach (var idItem in _idList)
                //{
                //    AddListItem(idItem);
                //}
                //int col = (int)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "SortColumn", -1);
                //if (col != -1)
                //{
                //    lvwColumnSorter.SortColumn = (int)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "SortColumn", lvwColumnSorter.SortColumn);
                //    int or = (int)Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "SortOrder", (int)lvwColumnSorter.Order);
                //    lvwColumnSorter.Order = (SortOrder)or;
                //    // Perform the sort with these new sort options.
                //    uxList.Sort();
                //}
                success = true;
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Warning,
                    InformativeText = "Failed to decrypt data. Invalid PIN.",
                    MessageText = "LoadFromDisk",
                };
                alert.BeginSheet(wndHandle);

            }
            catch (Exception ex)
            {
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Warning,
                    InformativeText = "Access denied: " + ex.ToString(),
                    MessageText = "Unlock MyId",
                };
                alert.BeginSheet(wndHandle);

            }

            //ShowNumberOfItems();
            return success;
        }

        private bool LoadPrivateKey(string privateKeyFile)
        {
            //string bufferS = File.ReadAllText(privateKeyFile);

            //byte[] buffer = ToByteArray(bufferS.Replace(",", "").Trim());


            //if (buffer[0] == 0x20 && buffer[1] == 0x22) //new version
            //{
            //    int pos = 2;
            //    //byte[] iv = new byte[16];
            //    //Array.Copy(buffer, 2, iv, 0, 16);
            //    //SaveKeyIv("IV", iv);

            //    pos += 16;
            //    byte[] salt = new byte[32];
            //    Array.Copy(buffer, pos, salt, 0, 32);
            //    SaveKeyIv("Salt", salt);

            //    pos += 32;
            //    byte[] riIv = new byte[16];
            //    Array.Copy(buffer, pos, riIv, 0, 16);
            //    SaveKeyIv("Iv2022", riIv);

            //    return true;

            //}
            //else
            //{
            //    MessageBox.Show("Invalid key file");
            //}
            return false;
        }

        public bool ValidatePassword(string pass)
        {
            //if (GetKeyIv("Salt") != null)
            byte[] masterPin = Encoding.Unicode.GetBytes(pass);
            SaveKeyIv("Pin", masterPin);
            if (GetKeyIv("Salt") == null)
                CreateNewKey(masterPin);
            //if (GetKeyIv("RiKey") == null || GetKeyIv("RiIv") == null)
            //{
            //    SaveKeyIv("Key", Encoding.Unicode.GetBytes(pass));

            //}
            if (GetKeyIv("Key") != null)
            {  //old version
                byte[] keyBytes;
                using (SHA256 mySHA256 = SHA256.Create())
                {
                    byte[] keyB = Encoding.Unicode.GetBytes(pass);
                    keyBytes = mySHA256.ComputeHash(keyB);
                }
                byte[] savedKey = GetKeyIv("Key");
                if (!keyBytes.SequenceEqual(savedKey))
                {
                    var alert = new NSAlert()
                    {
                        AlertStyle = NSAlertStyle.Warning,
                        InformativeText = "Please enter current Master PIN",
                        MessageText = "Invalid PIN",
                    };
                    alert.BeginSheet(wndHandle);
                    return false;
                }

            }
            return true;
        }


    }
}

