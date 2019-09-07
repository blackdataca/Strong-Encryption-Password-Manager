// MIT License - Copyright (c) 2019 Black Data
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MyId
{
    class Crypto
    {

        private byte[] GetKeyIv(string type)
        {

            byte[] iv32 = (byte[])Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "iv", null);
            switch (type)
            {
                case "IV": //iv16
                    //byte[] iv32 = (byte[]) Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "iv", random);
                    if (iv32 == null)
                        return null;
                    byte[] iv16 = new byte[16];
                    Array.Copy(iv32, iv16, 16);
                    return iv16;
                case "Key":
                    {

                        byte[] ciphertext = (byte[])Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "key", null);

                        byte[] plaintext = ProtectedData.Unprotect(ciphertext, iv32, DataProtectionScope.CurrentUser);

                        using (SHA256 mySHA256 = SHA256.Create())
                        {
                            return mySHA256.ComputeHash(plaintext);
                        }
                    }
                default:
                    throw new Exception("error 191");
            }
        }
        public bool DecryptFile(string encFile, string decFile)
        {
            if (File.Exists(encFile))
            {

                using (RijndaelManaged myRijndael = new RijndaelManaged())
                {
                    myRijndael.KeySize = 256;
                    myRijndael.BlockSize = 128;
                    myRijndael.Padding = PaddingMode.PKCS7;
                    //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                    myRijndael.Mode = CipherMode.CFB;

                    //http://stackoverflow.com/questions/2659214/why-do-i-need-to-use-the-rfc2898derivebytes-class-in-net-instead-of-directly
                    //"What it does is repeatedly hash the user password along with the salt." High iteration counts.
                    var key = new Rfc2898DeriveBytes(GetKeyIv("Key"), GetKeyIv("IV"), 50000);
                    myRijndael.Key = key.GetBytes(myRijndael.KeySize / 8);
                    myRijndael.IV = key.GetBytes(myRijndael.BlockSize / 8);

                    using (var fsCrypt = new FileStream(encFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {

                        byte[] salt = new byte[32];
                        fsCrypt.Read(salt, 0, salt.Length);

                        using (var fs = new FileStream(decFile, FileMode.Create))
                        {
                            using (var cryptoStream = new CryptoStream(fsCrypt, myRijndael.CreateDecryptor(), CryptoStreamMode.Read))
                            {
                                //create a buffer (1mb) so only this amount will allocate in the memory and not the whole file
                                byte[] buffer = new byte[1048576];
                                int read;
                                while ((read =
                                    cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    fs.Write(buffer, 0, read);
                                    Thread.Sleep(1);
                                }

                                cryptoStream.Close();

                            }
                            fs.Close();
                        }
                        fsCrypt.Close();
                    }
                }
                return true;
            }
            else
            {
                MessageBox.Show("File not found: " + encFile, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return false;
        }
    }
}
