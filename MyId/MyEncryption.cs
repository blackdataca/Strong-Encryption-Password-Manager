using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MyId
{


    public class MyEncryption
    {
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

            byte[] iv16 =  MyEncryption.Md5(Encoding.ASCII.GetBytes(iv));


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
    }

    

}