﻿using Microsoft.Win32;
using System.Security.Cryptography;
using System.Text;

namespace MyIdLibrary.Models;

public class Crypto
{

    /// <summary>
    /// Encrypt - AES
    /// </summary>
    /// <param name="plain"></param>
    /// <param name="key">256-bit/8, 32 bytes</param>
    /// <param name="iv">IV size is BlockSize / 8 = 128 / 8 = 16 bytes</param>
    /// <returns></returns>
    public static string SymmetricEncrypt(string plain, byte[] key, byte[] iv)
    {
        string crypt = "";
        var aes = Aes.Create();
        ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);

        // Create the streams used for encryption.
        using (MemoryStream msEncrypt = new MemoryStream())
        {
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write(plain);
                }
                crypt = Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
        return crypt;
    }

    /// <summary>
    /// Decrypt - AES
    /// </summary>
    /// <param name="plain"></param>
    /// <param name="key">256-bit/8, 32 bytes</param>
    /// <param name="iv">IV size is BlockSize / 8 = 128 / 8 = 16 bytes</param>
    /// <returns></returns>
    public static string SymmetricDecrypt(string crypt, byte[] key, byte[] iv)
    {
        string plain = "";
        var aes = Aes.Create();
        ICryptoTransform cryptor = aes.CreateDecryptor(key, iv);
        byte[] cryptBytes = Convert.FromBase64String(crypt);
        // Create the streams used for encryption.
        using (MemoryStream ms = new MemoryStream(cryptBytes))
        {
            using (CryptoStream cs = new CryptoStream(ms, cryptor, CryptoStreamMode.Read))
            {
                using (var sCrypt = new StreamReader(cs))
                {
                    plain = sCrypt.ReadToEnd();
                }
            }
        }
        return plain;
    }

    /// <summary>
    /// Encrypt - RSA
    /// </summary>
    /// <param name="byteData"></param>
    /// <param name="pubKey"></param>
    /// <returns></returns>
    public static byte[] AsymetricEncrypt(byte[] byteData, byte[] pubKey)
    {
        using (var rsa = RSA.Create())
        {
            rsa.ImportRSAPublicKey(pubKey, out int bytesRead);
            var encryptedData = rsa.Encrypt(byteData, RSAEncryptionPadding.OaepSHA256);
            return encryptedData;
        }
    }

    /// <summary>
    /// Decrypt - RSA
    /// </summary>
    /// <param name="byteData"></param>
    /// <param name="pubKey"></param>
    /// <returns></returns>
    public static byte[] AsymetricDecrypt(byte[] byteData, byte[] priKey)
    {
        using (var rsa = RSA.Create())
        {
            rsa.ImportRSAPrivateKey(priKey, out int bytesRead);
            var decryptedData = rsa.Decrypt(byteData, RSAEncryptionPadding.OaepSHA256);
            return decryptedData;
        }
    }

    public static string EncryptString(string plainText, string key, string iv)
    {
        // Instantiate a new Aes object to perform string symmetric encryption
        Aes encryptor = Aes.Create();

        encryptor.Mode = CipherMode.CBC;
        //encryptor.KeySize = 256;
        //encryptor.BlockSize = 128;
        //encryptor.Padding = PaddingMode.Zeros;
        byte[] key32;
        using (var shaManaged = new SHA256Managed())
        {
            key32 = shaManaged.ComputeHash(Encoding.ASCII.GetBytes(key));
        }

        // Set key and IV
        encryptor.Key = key32;

        byte[] iv16 = Md5(Encoding.ASCII.GetBytes(iv));


        encryptor.IV = iv16;

        // Instantiate a new MemoryStream object to contain the encrypted bytes
        MemoryStream memoryStream = new MemoryStream();

        // Instantiate a new encryptor from our Aes object
        ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

        // Instantiate a new CryptoStream object to process the data and write it to the 
        // memory stream
        CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

        // Convert the plainText string into a byte array
        byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

        // Encrypt the input plaintext string
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);

        // Complete the encryption process
        cryptoStream.FlushFinalBlock();

        // Convert the encrypted data from a MemoryStream to a byte array
        byte[] cipherBytes = memoryStream.ToArray();

        // Close both the MemoryStream and the CryptoStream
        memoryStream.Close();
        cryptoStream.Close();

        // Convert the encrypted byte array to a base64 encoded string
        string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

        // Return the encrypted data as a string
        return cipherText;
    }

    public static string DecryptString(string cipherText, string key, string iv)
    {
        // Instantiate a new Aes object to perform string symmetric encryption
        Aes encryptor = Aes.Create();

        encryptor.Mode = CipherMode.CBC;
        //encryptor.KeySize = 256;
        //encryptor.BlockSize = 128;
        //encryptor.Padding = PaddingMode.Zeros;
        byte[] key32;
        using (var shaManaged = new SHA256Managed())
        {
            key32 = shaManaged.ComputeHash(Encoding.ASCII.GetBytes(key));
        }

        // Set key and IV
        encryptor.Key = key32;

        byte[] iv16 = Md5(Encoding.ASCII.GetBytes(iv));


        encryptor.IV = iv16;

        // Instantiate a new MemoryStream object to contain the encrypted bytes
        MemoryStream memoryStream = new MemoryStream();

        // Instantiate a new encryptor from our Aes object
        ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

        // Instantiate a new CryptoStream object to process the data and write it to the 
        // memory stream
        CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

        // Will contain decrypted plaintext
        string plainText = String.Empty;

        try
        {
            // Convert the ciphertext string into a byte array
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            // Decrypt the input ciphertext string
            cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

            // Complete the decryption process
            cryptoStream.FlushFinalBlock();

            // Convert the decrypted data from a MemoryStream to a byte array
            byte[] plainBytes = memoryStream.ToArray();

            // Convert the decrypted byte array to string
            plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
        }
        finally
        {
            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();
        }

        // Return the decrypted data as a string
        return plainText;
    }

    public static string UniqId(string prefix, bool more_entropy)
    {
        if (string.IsNullOrEmpty(prefix))
            prefix = string.Empty;

        if (!more_entropy)
        {
            return (prefix + System.Guid.NewGuid().ToString().Replace("-", "")).Substring(0, 13);
        }
        else
        {
            return (prefix + System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 14)) + "." + System.Guid.NewGuid().ToString().Substring(0, 8);
        }
    }


    public static string MyHash(string input)
    {
        using (SHA512 shaManaged = new SHA512Managed())
        {
            byte[] hash = shaManaged.ComputeHash(Encoding.UTF8.GetBytes(input));
            string hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return hashString;
        }
    }

    public static byte[] Md5(byte[] inputBytes)
    {
        // Use input string to calculate MD5 hash
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return hashBytes;

            //return Convert.ToHexString(hashBytes); // .NET 5 +

            // Convert the byte array to hexadecimal string prior to .NET 5
            // StringBuilder sb = new System.Text.StringBuilder();
            // for (int i = 0; i < hashBytes.Length; i++)
            // {
            //     sb.Append(hashBytes[i].ToString("X2"));
            // }
            // return sb.ToString();
        }
    }

    public static byte[] MyHash(byte[] input)
    {
        using (SHA512 shaManaged = new SHA512Managed())
        {
            byte[] hash = shaManaged.ComputeHash(input);
            return hash;
        }
    }

    public string Bin2Hex(byte[] input)
    {
        return BitConverter.ToString(input).Replace("-", "").ToLower();
    }

    public void HexToBin(string hexData, out byte[] binData)
    {
        binData = new byte[hexData.Length / 2];
        for (int i = 0; i < hexData.Length; i += 2)
        {
            binData[i / 2] = Convert.ToByte(hexData.Substring(i, 2), 16);
        }
    }

    public static string UcFirst(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        char firstChar = input[0];
        string restOfTheString = input.Substring(1);

        // Convert the first character to upper case and concatenate with the rest of the string.
        return char.ToUpper(firstChar) + restOfTheString;
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

                byte[] pin = GetKeyIv("Pin", pinEnc);

                if (version == 2024)
                {
                    using (var aes = Aes.Create())
                    {
                        aes.Key = SHA256.Create().ComputeHash(pin);
                        aes.IV = GetKeyIv("Iv2022");

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

                            var key = new Rfc2898DeriveBytes(pin, GetKeyIv("Salt"), 50000);
                            myRijndael.Key = key.GetBytes(32);
                            myRijndael.IV = GetKeyIv("Iv2022");
                        }
                        else
                        {
                            fsCrypt.Seek(0, SeekOrigin.Begin);
                            if (GetKeyIv("RiKey") == null || GetKeyIv("RiIv") == null)
                            {
                                return null;
                            }
                            myRijndael.Key = GetKeyIv("RiKey");
                            myRijndael.IV = GetKeyIv("RiIv");

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

        using var aes = Aes.Create();

        byte[] pin = GetKeyIv("Pin", pinEnc);
        aes.Key = SHA256.Create().ComputeHash(pin);
        aes.IV = GetKeyIv("Iv2022"); // GetKeyIv("RiIv"); // GetKeyIv("RiIv");// key.GetBytes(myRijndael.BlockSize / 8);

        using var fsPlain = new BinaryReader(ms);
        using var fsCrypt = new FileStream(encFile, FileMode.Create, FileAccess.Write);
        //version 2024
        fsCrypt.WriteByte(0x20); //file version major
        fsCrypt.WriteByte(0x24); //file version minor

        //generate random salt
        byte[] salt = GenerateRandomBytes(32);
        fsCrypt.Write(salt, 0, salt.Length);

        using var cryptoStream = new CryptoStream(fsCrypt, aes.CreateEncryptor(), CryptoStreamMode.Write);
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
