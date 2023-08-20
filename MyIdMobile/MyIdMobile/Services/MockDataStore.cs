using MyIdMobile.Models;
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
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>()
            {
                //new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                //new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                //new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                //new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                //new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                //new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            };
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
                        formatter.Serialize(cryptoStream, items);
                    }
                    ms.Close();

                    string encData = MyEncryption.Bin2Hex(ms.ToArray());
                    await SecureStorage.SetAsync("Data", encData);
                }


            }
            //if (webSync)
            //    _ = WebSync();
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        
    }
}