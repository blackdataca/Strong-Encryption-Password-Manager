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
    static class Crypto
    {
        public static byte[] SaveKeyIv(string type, byte[] value)
        {

            switch (type)
            {
                //case "IV":
                //    byte[] random = new byte[32];

                //    //RNGCryptoServiceProvider is an implementation of a random number generator.
                //    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                //    rng.GetBytes(random);
                //    Array.Copy(value, random, 16);
                //    Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "iv", random);
                //    break;
                case "Iv2022": //16
                //case "RiKey":
                //case "RiIv": //16
                case "Salt": //32
                    Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", type, value);
                    break;
                //case "Key":
                //    byte[] keyBytes;
                //    using (SHA256 mySHA256 = SHA256.Create())
                //    {
                //        byte[] keyB = value;
                //        keyBytes = mySHA256.ComputeHash(keyB);
                //    }
                //    byte[] iv32 = GetKeyIv("IV");// PIN is only accessible on this computer
                //    byte[] ciphertext = ProtectedData.Protect(keyBytes, iv32, DataProtectionScope.CurrentUser);
                //    Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", "key", ciphertext);
                //    break;
                case "Pin":
                    return ProtectedData.Protect(value, null, DataProtectionScope.CurrentUser); //_pinEnc = 
                default:
                    throw new Exception("error 237");

            }

            return null;
        }
        public static byte[] GetKeyIv(string type, byte[] pin = null)
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
                case "Iv2022": //16
                case "RiKey": //32
                case "RiIv": //16
                case "Salt":
                    {
                        byte[] data = (byte[])Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", type, null);
                        return data;
                    }
                case "Key":
                    {
                        byte[] iv32 = GetKeyIv("IV");
                        byte[] ciphertext = (byte[])Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "key", null);
                        if (ciphertext == null)
                            return null;
                        byte[] plaintext = ProtectedData.Unprotect(ciphertext, iv32, DataProtectionScope.CurrentUser);

                        return plaintext;


                    }
                case "Pin":
                    //return ProtectedData.Unprotect(_pinEnc, null, DataProtectionScope.CurrentUser);
                    return ProtectedData.Unprotect(pin, null, DataProtectionScope.CurrentUser); //_pinEnc

                default:
                    throw new Exception("error 191");
            }
        }

        //private static byte[] GetKeyIv(string type)
        //{

        //    byte[] iv32 = (byte[])Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "iv", null);
        //    switch (type)
        //    {
        //        case "IV": //iv16
        //            //byte[] iv32 = (byte[]) Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "iv", random);
        //            if (iv32 == null)
        //                return null;
        //            byte[] iv16 = new byte[16];
        //            Array.Copy(iv32, iv16, 16);
        //            return iv16;
        //        case "Key":
        //            {

        //                byte[] ciphertext = (byte[])Registry.GetValue("HKEY_CURRENT_USER\\Software\\MyId", "key", null);

        //                byte[] plaintext = ProtectedData.Unprotect(ciphertext, iv32, DataProtectionScope.CurrentUser);

        //                using (SHA256 mySHA256 = SHA256.Create())
        //                {
        //                    return mySHA256.ComputeHash(plaintext);
        //                }
        //            }
        //        default:
        //            throw new Exception("error 191");
        //    }
        //}
        public static bool DecryptFile(string encFile, string decFile)
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

        public static MemoryStream DecryptFileStream(string encFile)
        {
            MemoryStream ms = null;
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

                        //using (var fs = new FileStream(decFile, FileMode.Create))
                        //{
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
                           // fs.Close();
                        //}
                        fsCrypt.Close();
                    }
                }
                //return true;
            }
            else
            {
                MessageBox.Show("File not found: " + encFile, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return ms;
        }

        public static string EncryptFileStream(string targetFile, MemoryStream ms, byte[] pinEnc)
        {

            string targetFileName = Path.GetFileName(targetFile);
            string encFileNameOnly = $"enc.{targetFileName}";
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
                byte[] pin = GetKeyIv("Pin", pinEnc);
                var key = new Rfc2898DeriveBytes(pin, GetKeyIv("Salt"), 50000);
                myRijndael.Key = key.GetBytes(32);
                myRijndael.IV = GetKeyIv("Iv2022"); // GetKeyIv("RiIv"); // GetKeyIv("RiIv");// key.GetBytes(myRijndael.BlockSize / 8);

                using (var fsPlain = new BinaryReader(ms))
                {
                    using (var fsCrypt = new FileStream(encFile, FileMode.Create, FileAccess.Write))
                    {
                        //version 2022
                        fsCrypt.WriteByte(0x20); //file version major
                        fsCrypt.WriteByte(0x22); //file version minor

                        //generate random salt
                        byte[] salt = GenerateRandomBytes(32);
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

            return encFileNameOnly;
        }

        public static byte[] GenerateRandomBytes(int size)
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

        public static string EncryptFile(string imageFile)
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
                byte[] pin = GetKeyIv("Pin");
                var key = new Rfc2898DeriveBytes(pin, GetKeyIv("Salt"), 50000);
                myRijndael.Key = key.GetBytes(32);
                myRijndael.IV = GetKeyIv("Iv2022"); // GetKeyIv("RiIv"); // GetKeyIv("RiIv");// key.GetBytes(myRijndael.BlockSize / 8);

                using (var fsPlain = new FileStream(imageFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var fsCrypt = new FileStream(encFile, FileMode.Create, FileAccess.Write))
                    {
                        //version 2022
                        fsCrypt.WriteByte(0x20); //file version major
                        fsCrypt.WriteByte(0x22); //file version minor

                        //generate random salt
                        byte[] salt = GenerateRandomBytes(32);
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

            return encFileNameOnly;
        }
    }
}
