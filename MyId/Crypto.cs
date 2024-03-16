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

        public static byte[] SaveKeyIv(string type, byte[] value)
        {
            switch (type)
            {
                case "Iv2022": //16

                case "Salt": //32
                    Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", type, value);
                    break;

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

        
        

        public static MemoryStream DecryptFileStream(KeyValuePair<string, string> encFile, byte[] pinEnc)
        {
            MemoryStream ms = null;
            string fileName = encFile.Key;
            string encFileNameOnly = Path.GetFileName(fileName);
            fileName = Path.Combine(KnownFolders.DataDir, encFileNameOnly);

            if (File.Exists(fileName))
            {
                ms = new MemoryStream();

                using (var fsCrypt = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    int version = 0;
                    if (fsCrypt.ReadByte() == 0x20)
                    {
                        if (fsCrypt.ReadByte() == 0x24)
                            version = 2024;
                        else
                            version = 2022;
                    }

                    byte[] pin = Crypto.GetKeyIv("Pin", pinEnc);

                    if (version == 2024)
                    {
                        using var aes = Aes.Create();
                        aes.Key = SHA256.Create().ComputeHash(pin);
                        aes.IV = Crypto.GetKeyIv("Iv2022");

                        using var cryptoStream = new CryptoStream(fsCrypt, aes.CreateDecryptor(), CryptoStreamMode.Read);
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

                        fsCrypt.Close();
                    }
                    else
                    {
                        using (RijndaelManaged myRijndael = new RijndaelManaged())
                        {
                            myRijndael.KeySize = 256;
                            myRijndael.BlockSize = 128;
                            myRijndael.Padding = PaddingMode.PKCS7;
                            //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                            myRijndael.Mode = CipherMode.CFB;

                            if (version == 2022)
                            { //version 2022

                                var key = new Rfc2898DeriveBytes(pin, Crypto.GetKeyIv("Salt"), 50000);
                                myRijndael.Key = key.GetBytes(32);
                                myRijndael.IV = Crypto.GetKeyIv("Iv2022");
                            }
                            else
                            {
                                fsCrypt.Seek(0, SeekOrigin.Begin);
                                if (Crypto.GetKeyIv("RiKey") == null || Crypto.GetKeyIv("RiIv") == null)
                                {
                                    return null;
                                }
                                myRijndael.Key = Crypto.GetKeyIv("RiKey");
                                myRijndael.IV = Crypto.GetKeyIv("RiIv");

                            }

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
                }

                ms.Seek(0, SeekOrigin.Begin);

            }
            return ms;
        }



        public static string EncryptFileStream(string targetFile, MemoryStream ms, byte[] pinEnc)
        {
            string targetFileName = Path.GetFileName(targetFile);
            string encFileNameOnly = $"enc.{targetFileName}";
            string encFile = Path.Combine(KnownFolders.DataDir, encFileNameOnly);

            using var myRijndael = Aes.Create();

            byte[] pin = GetKeyIv("Pin", pinEnc);
            myRijndael.Key = SHA256.Create().ComputeHash(pin);
            myRijndael.IV = GetKeyIv("Iv2022"); // GetKeyIv("RiIv"); // GetKeyIv("RiIv");// key.GetBytes(myRijndael.BlockSize / 8);

            using var fsPlain = new BinaryReader(ms);
            using var fsCrypt = new FileStream(encFile, FileMode.Create, FileAccess.Write);
            //version 2024
            fsCrypt.WriteByte(0x20); //file version major
            fsCrypt.WriteByte(0x24); //file version minor

            //generate random salt
            byte[] salt = GenerateRandomBytes(32);
            fsCrypt.Write(salt, 0, salt.Length);

            using var cryptoStream = new CryptoStream(fsCrypt, myRijndael.CreateEncryptor(), CryptoStreamMode.Write);
            //create a buffer (1mb) so only this amount will allocate in the memory and not the whole file
            byte[] buffer = new byte[1048576];
            int read;
            while ((read = fsPlain.Read(buffer, 0, buffer.Length)) > 0)
            {
                cryptoStream.Write(buffer, 0, read);
                Thread.Sleep(1);
            }

            cryptoStream.Close();

            fsCrypt.Close();
            fsPlain.Close();

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

        
    }
}
