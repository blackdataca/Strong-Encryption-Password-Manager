﻿using MyIdMobile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MyIdMobile.Services
{
    [Serializable]
    public class MyDataStore : IDataStore<Item>
    {
        private List<Item> _allItems = new List<Item>();

        public MyDataStore()
        {

        }

        public async Task SaveToDiskAsync(bool webSync = true)
        {
            byte[] pin = await MyEncryption.GetKeyIvAsync("Pin");
            var key = new Rfc2898DeriveBytes(pin, await MyEncryption.GetKeyIvAsync("Salt"), 50000);

            BinaryFormatter formatter = new BinaryFormatter();
            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                myRijndael.KeySize = 256;
                myRijndael.BlockSize = 128;
                myRijndael.Padding = PaddingMode.PKCS7;
                //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                myRijndael.Mode = CipherMode.CFB;


                myRijndael.Key = key.GetBytes(32);
                myRijndael.IV = await MyEncryption.GetKeyIvAsync("Iv2022");



                using (var ms = new MemoryStream())
                {
                    //version 2022
                    ms.WriteByte(0x20); //file version major
                    ms.WriteByte(0x22); //file version minor
                    using (var cryptoStream = new CryptoStream(ms, myRijndael.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        formatter.Serialize(cryptoStream, _allItems);
                    }
                    ms.Close();

                    string encData = MyEncryption.Bin2Hex(ms.ToArray());
                    await SecureStorage.SetAsync("Data", encData);
                }


            }
            //if (webSync)
            //    _ = WebSync();
        }


        public async Task<bool> LoadFromDiskAsync(string pPrivateKeyFile = null)
        {

            _allItems = null;

            bool success = false;

            try
            {
                //using (var fs = new FileStream(pDataFile, FileMode.Open, FileAccess.Read))
                using (var ms = new MemoryStream())
                {
                    //string encData = MyEncryption.Bin2Hex(ms.ToArray());
                    string encString = await SecureStorage.GetAsync("Data");
                    byte[] endData = MyEncryption.Hex2Bin(encString);
                    ms.Write(endData, 0, endData.Length);
                    ms.Seek(0, SeekOrigin.Begin);

                    int version = 0;
                    if (ms.ReadByte() == 0x20 && ms.ReadByte() == 0x22)
                        version = 2022;
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Load Data", "Wrong data version", "OK");
                        return false;
                    }
                        // Set the stream position to the beginning of the file.
                        //ms.Seek(0, SeekOrigin.Begin);

                    BinaryFormatter formatter = new BinaryFormatter();
                    using (RijndaelManaged myRijndael = new RijndaelManaged())
                    {
                        myRijndael.KeySize = 256;
                        myRijndael.BlockSize = 128;
                        myRijndael.Padding = PaddingMode.PKCS7;
                        //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                        myRijndael.Mode = CipherMode.CFB;


                        if (pPrivateKeyFile != null)
                        {
                            if (!await LoadPrivateKeyAsync(pPrivateKeyFile))
                            {
                                //MessageBox.Show("Unable load private key!");
                                await App.Current.MainPage.DisplayAlert("Load Data", "Unable load private key!", "OK");
                                return false;
                            }
                        }
                        if (version == 2022)
                        {
                            byte[] pin = await MyEncryption.GetKeyIvAsync("Pin");
                            byte[] salt = await MyEncryption.GetKeyIvAsync("Salt");
                            var key = new Rfc2898DeriveBytes(pin, salt, 50000);
                            myRijndael.Key = key.GetBytes(32);
                            myRijndael.IV = await MyEncryption.GetKeyIvAsync("Iv2022");
                        }
                        else
                        {  //Old verion
                            await App.Current.MainPage.DisplayAlert("Load Data", "Unknown data file version!", "OK");
                            return false;
                        }

                        using (var cryptoStream = new CryptoStream(ms, myRijndael.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            try
                            {
                                _allItems = (List<Item>)formatter.Deserialize(cryptoStream);

                            }
                            catch (System.Security.Cryptography.CryptographicException)
                            {
                                return false;
                            }
                        }
                    }
                }

                int uniqIdUpdate = 0;
                foreach (var item in _allItems)
                {
                    if (item.UniqId == null)
                    {
                        item.UniqId = MyEncryption.UniqId("", true);
                        uniqIdUpdate++;
                    }
                }
                if (uniqIdUpdate > 0)
                    await SaveToDiskAsync(false);

                //foreach (var idItem in _idList)
                //{
                //    if (ShowDeleted(idItem.Deleted))
                //        AddListItem(idItem);
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
            catch (System.Security.Cryptography.CryptographicException cex)
            {
                //MessageBox.Show("Failed to decrypt data. Invalid PIN.", "LoadFromDisk", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await App.Current.MainPage.DisplayAlert(cex.Message, "Please try again with correct Master Password" , "OK");
                return false;
            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Load Data", "Access denied: " + ex.ToString(), "OK");
                return false;
            }

            //ShowNumberOfItems();
            return success;
        }

        private async Task<bool> LoadPrivateKeyAsync(string privateKeyFile)
        {
            string bufferS = File.ReadAllText(privateKeyFile);

            byte[] buffer = MyEncryption.ToByteArray(bufferS.Replace(",", "").Trim());


            if (buffer[0] == 0x20 && buffer[1] == 0x22) //new version
            {
                int pos = 2;

                pos += 16;
                byte[] salt = new byte[32];
                Array.Copy(buffer, pos, salt, 0, 32);
                await MyEncryption.SaveKeyIvAsync("Salt", salt);

                pos += 32;
                byte[] riIv = new byte[16];
                Array.Copy(buffer, pos, riIv, 0, 16);
                await MyEncryption.SaveKeyIvAsync("Iv2022", riIv);

                return true;

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("LoadPrivateKey", "Invalid key file", "OK");
            }
            return false;
        }


        public async Task<bool> AddItemAsync(Item item)
        {
            _allItems.Add(item);
            await SaveToDiskAsync();
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = _allItems.Where((Item arg) => arg.UniqId == item.UniqId).FirstOrDefault();
            _allItems.Remove(oldItem);
            _allItems.Add(item);
            await SaveToDiskAsync();
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = _allItems.Where((Item arg) => arg.UniqId == id).FirstOrDefault();
            oldItem.Deleted = true;
            //items.Remove(oldItem);
            await SaveToDiskAsync();
            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(_allItems.FirstOrDefault(s => s.UniqId == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            await LoadFromDiskAsync();
            return await Task.FromResult(_allItems);
        }

        
    }
}